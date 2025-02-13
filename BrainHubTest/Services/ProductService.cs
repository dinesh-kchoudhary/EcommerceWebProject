using BrainHubTest.Data.Model;
using BrainHubTest.Data;
using BrainHubTest.Model;
using Microsoft.EntityFrameworkCore;

namespace BrainHubTest.Services
{
    public class ProductService :  IProductService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public ProductService(AppDbContext context, IWebHostEnvironment env) { _context = context; _env = env; }

        public async Task<ApiResponse<List<Product>>> GetAllAsync(string search, int page, int pageSize)
        {
            var query = _context.Products.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.Name.Contains(search));
            }
            var products = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return new ApiResponse<List<Product>>(true, "Products retrieved successfully", products);
        }

        public async Task<ApiResponse<Product>> AddProductAsync(ProductRequest request, List<IFormFile> images)
        {
            if (await _context.Products.AnyAsync(p => p.Name == request.Name))
            {
                throw new Exception("Product name already exists.");
            }
            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Quantity = request.Quantity,
                ImageUrls = new List<string>()
            };
            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            foreach (var image in images)
            {
                if (image.Length > 0)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                    string filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    product.ImageUrls.Add(filePath);
                }
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return new ApiResponse<Product>(true, "Product added successfully", product);
        }

        public async Task<ApiResponse<Product>> EditProductAsync(int id, ProductRequest request)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return null;
            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;
            product.Quantity = request.Quantity;
            await _context.SaveChangesAsync();
            return new ApiResponse<Product>(true, "Product Updated successfully", product);
        }
        public async Task<ApiResponse<bool>> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return new ApiResponse<bool>(false, "Product not found", false);
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return new ApiResponse<bool>(true, "Product deleted successfully", true);
        }
    }
}
