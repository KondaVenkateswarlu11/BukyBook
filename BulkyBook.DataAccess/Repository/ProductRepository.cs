using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Dataccess;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _Db;
        public ProductRepository(ApplicationDbContext Db) : base(Db)
        {
            _Db = Db;
        }
       

        public void Update(Product obj)
        {
            var ObjFromDb = _Db.Products.FirstOrDefault(u => u.Id == obj.Id);
            if(ObjFromDb != null)
            {
                ObjFromDb.Title = obj.Title;
                ObjFromDb.Description = obj.Description;
                ObjFromDb.ISBN = obj.ISBN;
                ObjFromDb.Author = obj.Author;
                ObjFromDb.ListPrice = obj.ListPrice;
                ObjFromDb.Price = obj.Price;
                ObjFromDb.Price50 = obj.Price50;
                ObjFromDb.Price100 = obj.Price100;
                ObjFromDb.CategoryId = obj.CategoryId;
                ObjFromDb.CoverTypeId = obj.CoverTypeId;
                if(ObjFromDb.ImageUrl != null)
                {
                    ObjFromDb.ImageUrl = obj.ImageUrl;
                }
            }
        }
    }
}
