using SeaOfShops.Domain.Entities;
using SeaOfShops.Domain.Interfaces;
using SeaOfShops.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaOfShops.Service
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IList<Product>> GetAll()
        {
            return await _unitOfWork.Repository<Product>().GetAllAsync();
        }

        public async Task<Product> GetOne(int productId)
        {
            return await _unitOfWork.Repository<Product>().FindAsync(productId);
        }

        public async Task Update(Product productInput)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                var productRepos = _unitOfWork.Repository<Product>();
                var product = await productRepos.FindAsync(productInput.Id);
                if (product == null)
                    throw new KeyNotFoundException();

                product.ProductName = product.ProductName;

                await _unitOfWork.CommitTransaction();
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackTransaction();
                throw;
            }
        }

        public async Task Add(Product productInput)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                var productRepos = _unitOfWork.Repository<Product>();
                await productRepos.InsertAsync(productInput);

                await _unitOfWork.CommitTransaction();
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackTransaction();
                throw;
            }
        }

        public async Task Delete(int productId)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                var productRepos = _unitOfWork.Repository<Product>();
                var product = await productRepos.FindAsync(productId);
                if (product == null)
                    throw new KeyNotFoundException();

                await productRepos.DeleteAsync(product);

                await _unitOfWork.CommitTransaction();
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackTransaction();
                throw;
            }
        }
    }
}
