using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IUnitofWork
    {
        public ICategoryRepository Category { get; }

        public ICoverTypeRepository CoverType { get; }

        public IProductRepository Product { get; }
        public ICompanyRepository Company { get; }
        public IApplicationUserRepository ApplicationUser { get; }
        public IShoppingCartRepository ShoppingCart { get; }
        public IOrderDetailRepository OrderDetail { get; }
        public IOrderHeaderRepository OrderHeader { get; }
        void Save();
    }
}
