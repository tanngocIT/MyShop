﻿using MyShop.Data.Infrastructure;
using MyShop.Data.Repositories;
using MyShop.Model.Models;
using System.Collections.Generic;
using System;

namespace MyShop.Service
{
    public interface IProductCategoryService
    {
        ProductCategory Add(ProductCategory ProductCategory);

        void Update(ProductCategory ProductCategory);

        ProductCategory Delete(int id);

        IEnumerable<ProductCategory> GetAll();

        IEnumerable<ProductCategory> GetAll(string keyword);

         IEnumerable<ProductCategory> GetAllToUse();

        IEnumerable<ProductCategory> GetAllByParentId(int parentId);

        ProductCategory GetById(int id);

        void Save();
    }

    public class ProductCategoryService : IProductCategoryService
    {
        private IProductCategoryRepository _productCategoryRepository;
        private IUnitOfWork _unitOfWork;

        public ProductCategoryService(IProductCategoryRepository ProductCategoryRepository, IUnitOfWork unitOfWork)
        {
            this._productCategoryRepository = ProductCategoryRepository;
            this._unitOfWork = unitOfWork;
        }

        public ProductCategory Add(ProductCategory ProductCategory)
        {
            return _productCategoryRepository.Add(ProductCategory);
        }

        public ProductCategory Delete(int id)
        {
            return _productCategoryRepository.Delete(id);
        }

        public IEnumerable<ProductCategory> GetAll()
        {
            return _productCategoryRepository.GetAll();
        }

        public IEnumerable<ProductCategory> GetAllToUse()
        {
            return _productCategoryRepository.GetMulti(x=>x.Status==true);
        }

        public IEnumerable<ProductCategory> GetAll(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return _productCategoryRepository.GetAll();
            }
            else
            {
                return _productCategoryRepository.GetMulti(x => x.Name.Contains(keyword) || x.Description.Contains(keyword));
            }
            
        }

        public IEnumerable<ProductCategory> GetAllByParentId(int parentId)
        {
            return _productCategoryRepository.GetMulti(x => x.Status && x.ParentID == parentId);
        }

        public ProductCategory GetById(int id)
        {
            return _productCategoryRepository.GetSingleById(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(ProductCategory ProductCategory)
        {
            _productCategoryRepository.Update(ProductCategory);
        }
    }
}