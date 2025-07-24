using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Zorro.Data;
using Zorro.Data.Interfaces;
using Zorro.Middlewares;

namespace Zorro.Query.Essentials.Auth;

public static class SignInUserQuery
{
    public interface IUserSignInForm
    {
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string password { get; set; }
        public bool? rememberMe { get; set; }
    }

    public struct PhoneSingInForm : IUserSignInForm
    {
        [Required(ErrorMessage = "Phone number is required")]
        [StringLength(SignUpUserQuery.PHONENUMBER_MAX_LENGTH, MinimumLength = 1)]
        public required string phoneNumber { get; set; }

        public required string password { get; set; }
        public bool? rememberMe { get; set; }
    }

    public struct EmailSignInForm : IUserSignInForm
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        [StringLength(SignUpUserQuery.EMAIL_MAX_LENGTH, MinimumLength = 1)]
        public required string email { get; set; }

        public required string password { get; set; }
        public bool? rememberMe { get; set; }
    }

    public struct UsernameSingInForm : IUserSignInForm
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(SignUpUserQuery.USERNAME_MAX_LENGTH, MinimumLength = SignUpUserQuery.USERNAME_MIN_LENGTH)]
        public required string userName { get; set; }

        public required string password { get; set; }
        public bool? rememberMe { get; set; }
    }

    public delegate void AuthenticationMethod(QueryContext context, IUserSignInForm signInForm, object user);
    public static AuthenticationMethod? DefaultAuthenticationMethod { get; set; } = null;

    public static ArgQueryContext<TUser> SignInUser<TUser, TKey>(
        this QueryContext context,
        IUserSignInForm signInForm
    )
        where TUser : IdentityUser<TKey>, IEntity, new()
        where TKey : IEquatable<TKey>
    {
        var userRepo = context.GetService<ModelRepository<TUser>>();
        TUser? user = null;

        if (signInForm is PhoneSingInForm phoneSingInForm)
        {
            user = userRepo.Find(u => u.PhoneNumber == phoneSingInForm.phoneNumber);
        }
        else if (signInForm is EmailSignInForm emailSingInForm)
        {
            user = userRepo.Find(u => u.Email == emailSingInForm.email);
        }
        else if (signInForm is UsernameSingInForm usernameSingInForm)
        {
            user = userRepo.Find(u => u.UserName == usernameSingInForm.userName);
        }

        if (user is null)
        {
            throw new QueryException(statusCode: StatusCodes.Status400BadRequest);
        }

        if (string.IsNullOrEmpty(signInForm.password))
        {
            throw new QueryException(statusCode: StatusCodes.Status400BadRequest);
        }

        var userManager = context.GetService<UserManager<TUser>>();
        var pwCheckStatus = userManager.CheckPasswordAsync(user, signInForm.password).GetAwaiter().GetResult();
        if (pwCheckStatus is false)
        {
            throw new QueryException(statusCode: StatusCodes.Status400BadRequest);
        }

        if (DefaultAuthenticationMethod is null)
        {
            var signInManager = context.GetService<SignInManager<TUser>>();
            signInManager.SignInAsync(user, signInForm.rememberMe ?? false).GetAwaiter().GetResult();
        }
        else
        {
            DefaultAuthenticationMethod(context, signInForm, user);
        }

        return context.PassArg(user);
    }
}