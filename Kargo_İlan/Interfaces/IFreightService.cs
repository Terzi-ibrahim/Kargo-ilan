using Kargo_İlan.DTOs.Freight;
using Kargo_İlan.Models;
 
namespace Kargo_İlan.Interfaces
{
    public interface IFreightService
    {
 
        Task<List<FreightIndexDto>> GetAllFreightsAsync();
        Task<(List<int> UserOfferedFreightIds, List<int> AcceptedOfferFreightIds)> GetUserOfferStatusAsync(int userId);
        Task<List<FreightIndexDto>> GetFilteredFreightsAsync(
              string searchText,
              int? yukilId,
              int? yukilceId,
              int? varisilId,
              int? varisilceId,
              int? vehicleId,
              int? categoryId,
              int? cargoid);
        Task<FilterViewModel> GetFilterOptionsAsync();
        Task<FreightCreateViewDto> GetFreightCreateViewModelAsync();

        Task CreateFreightAsync(FreightCreate model, int userId);
        Task<List<FreightIndexDto>> MyFreight(int user);
        Task<FreightCreateViewDto> GetFreightEditViewModelAsync();
        Task<Freight> GetFreightByIdAsync(int id);
        Task<bool> EditFreightAsync(int id, FreightCreate model, int userId);

        Task<FreightIndexDto?> GetFreightDetailAsync(int id);

        Task<Freight?> GetFreightForDeleteAsync(int id, int userId);
        Task<bool> SoftDeleteFreightAsync(int id, int userId);
    }
}
