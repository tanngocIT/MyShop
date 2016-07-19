using MyShop.Data.Infrastructure;
using MyShop.Data.Repositories;
using MyShop.Model.Models;
using System.Collections.Generic;

namespace MyShop.Service
{
    public interface IProductService
    {
        Product Add(Product Product);

        void Update(Product Product);

        Product Delete(int id);

        IEnumerable<Product> GetAll();

        IEnumerable<Product> GetAll(string keyword);

        IEnumerable<Product> GetAllByCategoryId(int parentId);

        Product GetById(int id);

        void Save();
    }

    public class ProductService : IProductService
    {
        private IProductRepository _productRepository;
        private IUnitOfWork _unitOfWork;

        public ProductService(IProductRepository ProductRepository, IUnitOfWork unitOfWork)
        {
            this._productRepository = ProductRepository;
            this._unitOfWork = unitOfWork;
        }

        public Product Add(Product Product)
        {
            return _productRepository.Add(Product);
        }

        public Product Delete(int id)
        {
            return _productRepository.Delete(id);
        }

        public IEnumerable<Product> GetAll()
        {
            return _productRepository.GetAll();
        }

        public IEnumerable<Product> GetAll(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return _productRepository.GetAll();
            }
            else
            {
                return _productRepository.GetMulti(x => x.Name.Contains(keyword) || x.Description.Contains(keyword));
            }
        }

        public IEnumerable<Product> GetAllByCategoryId(int cateId)
        {
            return _productRepository.GetMulti(x => x.Status && x.CategoryID == cateId);
        }

        public Product GetById(int id)
        {
            return _productRepository.GetSingleById(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(Product ProductCategory)
        {
            _productRepository.Update(ProductCategory);
        }
    }
}