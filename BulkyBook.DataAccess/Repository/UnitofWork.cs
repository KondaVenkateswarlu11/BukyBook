using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Dataccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class UnitofWork : IUnitofWork
    {
        private readonly ApplicationDbContext _Db;
        public UnitofWork(ApplicationDbContext Db)
        {
            _Db = Db;
            Category = new CategoryRepository(_Db);
            CoverType = new CoverTypeRepository(_Db);
            Product = new ProductRepository(_Db);
        }

        //here Category is a CategoryRepository which is an implementation of ICategoryRepository
        //Same for all the models
        public ICategoryRepository Category { get; private set; }

        public ICoverTypeRepository CoverType { get; private set; }
        public IProductRepository Product { get; private set; }

        public void Save()
        {
            _Db.SaveChanges();
        }
    }
}
