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

    public static (QueryContext, TUser) SignInUser<TUser, TKey>(
        this ANY_TUPLE carriage,
        IUserSignInForm signInForm
    )
        where TUser : IdentityUser<TKey>, IEntity, new()
        where TKey : IEquatable<TKey>
    {
        var userRepo = carriage.context.http.RequestServices.GetRequiredService<ModelRepository<TUser>>();
        TUser? targetUser = null;

        if (signInForm is PhoneSingInForm phoneSingInForm)
        {
            targetUser = userRepo.Find(u => u.PhoneNumber == phoneSingInForm.phoneNumber);
        }
        else if (signInForm is EmailSignInForm emailSingInForm)
        {
            targetUser = userRepo.Find(u => u.Email == emailSingInForm.email);
        }
        else if (signInForm is UsernameSingInForm usernameSingInForm)
        {
            targetUser = userRepo.Find(u => u.UserName == usernameSingInForm.userName);
        }

        if (targetUser is null)
        {
            throw new QueryException(statusCode: StatusCodes.Status400BadRequest);
        }

        if (string.IsNullOrEmpty(signInForm.password))
        {
            throw new QueryException(statusCode: StatusCodes.Status400BadRequest);
        }

        var userManager = carriage.context.http.RequestServices.GetRequiredService<UserManager<TUser>>();
        var pwCheckStatus = userManager.CheckPasswordAsync(targetUser, signInForm.password).GetAwaiter().GetResult();
        if (pwCheckStatus is false)
        {
            throw new QueryException(statusCode: StatusCodes.Status400BadRequest);
        }

        if (DefaultAuthenticationMethod is null)
        {
            var signInManager = carriage.context.http.RequestServices.GetRequiredService<SignInManager<TUser>>();
            signInManager.SignInAsync(targetUser, signInForm.rememberMe ?? false).GetAwaiter().GetResult();
        }
        else
        {
            DefaultAuthenticationMethod(carriage.context, signInForm, targetUser);
        }

        return (carriage.context, targetUser);
    }
}