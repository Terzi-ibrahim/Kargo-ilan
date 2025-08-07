using Kargo_İlan.DTOs.Account;
using System.Security.Claims;

namespace Kargo_İlan.Interfaces
{
    public interface IAccountService
    {
        Task<(bool Success, string? ErrorMessage, ClaimsPrincipal? Principal)> LoginAsync(Login model);
        Task<(bool Success, string? ErrorMessage)> RegisterAsync(Register model);
     
        Task<bool> HandleForgotPasswordAsync(string email);
        Task<bool> ValidateResetTokenAsync(string token);
        Task<bool> ChangePasswordAsync(ChangePassword model);
     
        Task<ProfileViewModel> GetProfileAsync(int userId);
        Task<bool> UpdateProfileAsync(int userId, Profile profileDto);
        Task<ProfileViewModel> BuildProfileViewModelAsync(int userId);
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
       

    }

}

