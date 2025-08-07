using Kargo_İlan.DTOs.Account;
using Kargo_İlan.DTOs.Company;
using Kargo_İlan.Models;

namespace Kargo_İlan.Interfaces
{
    public interface ICompanyService
    {
        Task<CompanyUserDto> GetCompanyUserByIdAsync(int companyUserId);
        Task<CompanyUserDetail> GetUserDetailAsync(int userCompanyId);
        Task<List<User>> GetCompanyUsersAsync();
        Task<(bool Success, string? ErrorMessage)> RegisterCompanyAsync(RegisterCompany model, int userId);       
        Task<bool> UpdateCompanyProfileAsync(int userId, CompanyProfile model);
        Task<(bool Success, string ErrorMessage)> AddCompanyUserAsync(AddCompanyUser model, int loggedInUserId);     
        Task<CompanyProfile?> GetCompanyProfileByUserIdAsync(int userId);
        Task<(bool success, string message)> DeleteCompanyAsync(int userId, int updatedByUserId);
        Task<bool> DeleteCompanyUserAsync(int userCompanyId, int companyUserId);

    }
}
