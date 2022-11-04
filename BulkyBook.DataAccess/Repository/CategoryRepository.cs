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
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _Db;
        public CategoryRepository(ApplicationDbContext Db) : base(Db)
        {
            _Db = Db;
        }
       

        public void Update(Category obj)
        {
            _Db.Categories.Update(obj);
        }
    }
}
