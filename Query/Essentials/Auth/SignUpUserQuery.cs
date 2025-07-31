using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Zorro.Data;
using Zorro.Data.Interfaces;
using Zorro.Middlewares;

namespace Zorro.Query.Essentials.Auth;

public static class SignUpUserQuery
{
    // TODO: Consider moving these constants to a shared configuration file or class.
    public const int EMAIL_MAX_LENGTH = 80;
    public const int USERNAME_MAX_LENGTH = 45;
    public const int USERNAME_MIN_LENGTH = 4;
    public const int PHONENUMBER_MAX_LENGTH = 50;

    public interface IUserSignUpForm
    {
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string password { get; set; }
        [Required(ErrorMessage = "Confirm password is required"), DataType(DataType.Password)]
        public string confirmPassword { get; set; }
    }

    public struct PhoneAndUsernameSignUpForm : IUserSignUpForm
    {
        [Required(ErrorMessage = "Phone number is required")]
        [StringLength(PHONENUMBER_MAX_LENGTH, MinimumLength = 1)]
        public required string phoneNumber { get; set; }
        [Required(ErrorMessage = "Username is required")]
        [StringLength(USERNAME_MAX_LENGTH, MinimumLength = USERNAME_MIN_LENGTH)]
        public required string userName { get; set; }

        public required string password { get; set; }
        public required string confirmPassword { get; set; }
    }

    public struct EmailAndUsernameSignUpForm : IUserSignUpForm
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        [StringLength(EMAIL_MAX_LENGTH, MinimumLength = 1)]
        public required string email { get; set; }
        [Required(ErrorMessage = "Username is required")]
        [StringLength(USERNAME_MAX_LENGTH, MinimumLength = USERNAME_MIN_LENGTH)]
        public required string userName { get; set; }

        public required string password { get; set; }
        public required string confirmPassword { get; set; }
    }

    public struct UsernameSignUpForm : IUserSignUpForm
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(USERNAME_MAX_LENGTH, MinimumLength = USERNAME_MIN_LENGTH)]
        public required string userName { get; set; }

        public required string password { get; set; }
        public required string confirmPassword { get; set; }
    }

    public static ArgHttpQueryContext<TUser> SignUpUser<TUser, TKey>(this HttpQueryContext context, IUserSignUpForm signUpForm)
        where TUser : IdentityUser<TKey>, IEntity, new()
        where TKey : IEquatable<TKey>
    {
        // Clear last token from cookies

        var userManager = context.GetService<UserManager<TUser>>();

        string? phoneNumber = null;
        string? userName = null;
        string? email = null;
        string? password = signUpForm.password;
        string? confirmPassword = signUpForm.confirmPassword;

        if (signUpForm is PhoneAndUsernameSignUpForm phoneAndUsernameForm)
        {
            phoneNumber = phoneAndUsernameForm.phoneNumber;
            userName = phoneAndUsernameForm.userName;
        }
        else if (signUpForm is UsernameSignUpForm usernameForm)
        {
            userName = usernameForm.userName;
        }
        else if (signUpForm is EmailAndUsernameSignUpForm emailAndUsernameForm)
        {
            email = emailAndUsernameForm.email;
            userName = emailAndUsernameForm.userName;
        }

        if (password != confirmPassword)
        {
            throw new QueryException(fields: [(nameof(confirmPassword), ["Passwords do not match."])]);
        }

        if (phoneNumber is not null && userManager.Users.Any(u => u.PhoneNumber == phoneNumber))
        {
            throw new QueryException(fields: [(nameof(phoneNumber), ["Phone number already exists."])]);
        }

        if (userName is not null && userManager.Users.Any(u => u.UserName == userName))
        {
            throw new QueryException(fields: [(nameof(userName), ["User with such name already exists."])]);
        }

        TUser user = new TUser()
        {
            PhoneNumber = phoneNumber,
            UserName = userName,
            Email = email,
        };

        var createTask = userManager.CreateAsync(user, password).GetAwaiter().GetResult();
        if (createTask.Succeeded is false)
        {
            throw new QueryException(fields: ConvertToFieldErrors(createTask.Errors));
        }

        (string, string[])[] ConvertToFieldErrors(IEnumerable<IdentityError> identityErrors)
        {
            return identityErrors.Select(e => (e.Code, e.Description))
                .GroupBy(e => MapCodeToFieldName(e.Code))
                .Select(g => (g.Key, g.Select(e => e.Description).ToArray()))
                .ToArray();

            string MapCodeToFieldName(string code)
            {
                code = code.ToLower();
                if (code.Contains(nameof(password).ToLower()))
                    return nameof(password);
                else if (code.Contains(nameof(userName).ToLower()))
                    return nameof(userName);
                return "general";
            }
        }

        var userRepo = context.GetService<ModelRepository<TUser>>();
        user = userRepo.Find(u => u.Id.Equals(user.Id))!;

        context.TryLogElapsedTime(nameof(SignUpUserQuery));

        return context.PassArg(user);
    }
}