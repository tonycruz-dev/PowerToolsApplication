using Microsoft.EntityFrameworkCore;
using PowerTools.Database;
using PowerTools.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsUI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _context;

        public ProductRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<Product> AddProductAsync(Product product)
        {
            //using (var _context = new DataContext())
            //{
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
            //}

        }

        public async Task<Product> DeleteProductAsync(int Id)
        {
            //using (var _context = new ModelProducts())
            //{
            var itemToDelete = await _context.Products
          .FirstOrDefaultAsync(cp => cp.Id == Id);
            if (itemToDelete != null)
            _context.Products.Remove(itemToDelete);
            await _context.SaveChangesAsync();
            return itemToDelete;
            //}
        }

        public async Task<List<Product>> GetProductsAsyc()
        {
            return await _context
            .Products.ToListAsync();
            //}
        }

        public async Task<Product> GetProductByIdAsync(int Id)
        {

            return await _context
             .Products.FirstOrDefaultAsync(bu => bu.Id == Id);

        }
        public async Task<Product> GetProductByProductCodeAsync(string productCode)
        {
            //using (var _context = new ModelProducts())
            //{
            return await _context
           .Products.FirstOrDefaultAsync(bu => bu.ProductCode == productCode);
            //}
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            //using (var _context = new ModelProducts())
            //{
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return product;
            // }
        }
    }
}