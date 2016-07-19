using AutoMapper;
using MyShop.Model.Models;
using MyShop.Service;
using MyShop.Web.Infrastructure.Core;
using MyShop.Web.Infrastructure.Extensions;
using MyShop.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace MyShop.Web.Api
{
    [RoutePrefix("api/product")]
    public class ProductController : ApiControllerBase
    {


        IProductService _productService;
        IProductCategoryService _productCategoryService;

        #region Initial
        public ProductController(IErrorService errorService, IProductService productService, IProductCategoryService productCategoryService) : base(errorService)
        {
            this._productService = productService;
            this._productCategoryService = productCategoryService;
        }
        #endregion


        [Route("getallcategory")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _productCategoryService.GetAll();

                var responseData = Mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }



        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string keyword, int page, int pageSize = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var model = _productService.GetAll(keyword);

                totalRow = model.Count();
                var query = model.OrderByDescending(x => x.CreatedDate).Skip(page * pageSize).Take(pageSize);

                var responseData = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(query);

                var paginationSet = new PaginationSet<ProductViewModel>()
                {
                    Items = responseData,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
                };
                var response = request.CreateResponse(HttpStatusCode.OK, paginationSet);
                return response;
            });
        }

        [Route("getbyid/{id:int}")]
        [HttpGet]
        public HttpResponseMessage GetById(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _productService.GetById(id);

                var responseData = Mapper.Map<Product, ProductViewModel>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }


        [Route("create")]
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage Create(HttpRequestMessage request, ProductViewModel productVm)
        {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var newProduct = new Product();
                    newProduct.UpdateProduct(productVm);
                    newProduct.CreatedDate = DateTime.Now;
                    _productService.Add(newProduct);
                    _productService.Save();

                    var responseData = Mapper.Map<Product, ProductViewModel>(newProduct);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }
                return response;
            });
        }

        [Route("update")]
        [HttpPut]
        [AllowAnonymous]
        public HttpResponseMessage Update(HttpRequestMessage request, ProductViewModel productVm)
        {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var dbProduct= _productService.GetById(productVm.ID);
                    dbProduct.UpdateProduct(productVm);
                    dbProduct.UpdatedDate = DateTime.Now;
                    _productService.Update(dbProduct);
                    _productService.Save();

                    var responseData = Mapper.Map<Product, ProductViewModel>(dbProduct);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }
                return response;
            });
        }



        [Route("delete")]
        [HttpDelete]
        [AllowAnonymous]
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var oldProduct = _productService.Delete(id);
                    _productService.Save();

                    var responseData = Mapper.Map<Product, ProductViewModel>(oldProduct);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }
                return response;
            });
        }


        [Route("deletemulti")]
        [HttpDelete]
        [AllowAnonymous]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedProducts)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var listProduct = new JavaScriptSerializer().Deserialize<List<int>>(checkedProducts);
                    foreach (var item in listProduct)
                    {
                        _productService.Delete(item);
                    }

                    _productService.Save();

                    response = request.CreateResponse(HttpStatusCode.OK, listProduct.Count);
                }

                return response;
            });
        }


    }
}
