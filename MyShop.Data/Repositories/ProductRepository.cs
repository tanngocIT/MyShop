using MyShop.Data.Infrastructure;
using MyShop.Model.Models;

namespace MyShop.Data.Repositories
{
    public interface IProductRepository: IRepository<Product>
    {
    }

    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}