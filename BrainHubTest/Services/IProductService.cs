using BrainHubTest.Model;

namespace BrainHubTest.Services
{
    public interface IProductService
    {
        Task<ApiResponse<List<Product>>> GetAllAsync(string search, int page, int pageSize);
        Task<ApiResponse<Product>> AddProductAsync(ProductRequest request, List<IFormFile> images);
        Task<ApiResponse<Product>> EditProductAsync(int id, ProductRequest request);
        Task<ApiResponse<bool>> DeleteProductAsync(int id);
    }
}
