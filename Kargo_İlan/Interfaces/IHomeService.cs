using Kargo_İlan.DTOs.Freight;
using Kargo_İlan.Models;

namespace Kargo_İlan.Interfaces
{
    public interface IHomeService
    {
        Task<List<CategoryType>> GetAllCategoriesAsync();
        Task<List<FreightIndexDto>> GetLastSixFreightsAsync();
    }
}
