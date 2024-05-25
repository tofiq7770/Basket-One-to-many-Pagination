using Fiorello.Models;
using Fiorello.ViewModels;

namespace Fiorello.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<Product>> GetAllWithImagesAsync();
        Task<Product> GetByIdAsync(int id);
        Task<List<Product>> GetAllAsync();
        List<ProductVM> GetMappedDatas(List<Product> products);
        Task<List<Product>> GetAllPaginateAsync(int page, int take = 4);
        Task<int> GetCountAsync();
    }
}
