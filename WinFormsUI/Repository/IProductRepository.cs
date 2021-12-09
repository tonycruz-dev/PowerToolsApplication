using PowerTools.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsUI.Repository
{
    public interface IProductRepository
    {
        Task<Product> AddProductAsync(Product product);
        Task<Product> UpdateProductAsync(Product product);
        Task<Product> GetProductByIdAsync(int Id);
        Task<Product> DeleteProductAsync(int Id);
        Task<List<Product>> GetProductsAsyc();
        Task<Product> GetProductByProductCodeAsync(string productCode);
    }
    public interface IProductClientRepository
    {
        Task<Product> AddProductAsync(Product product);
        Task<Product> UpdateProductAsync(Product product);
        Task<Product> GetProductByIdAsync(int Id);
        Task<Product> DeleteProductAsync(int Id);
        Task<List<Product>> GetProductsAsyc();
        Task<Product> GetProductByProductCodeAsync(string productCode);
    }
}
