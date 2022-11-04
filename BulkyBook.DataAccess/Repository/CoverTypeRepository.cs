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
    public class CoverTypeRepository : Repository<CoverType>, ICoverTypeRepository
    {
        private readonly ApplicationDbContext _Db;
        public CoverTypeRepository(ApplicationDbContext Db) : base(Db)
        {
            _Db = Db;
        }
       

        public void Update(CoverType obj)
        {
            _Db.CoverTypes.Update(obj);
        }
    }
}
