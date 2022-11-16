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
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext _Db;
        public CompanyRepository(ApplicationDbContext Db) : base(Db)
        {
            _Db = Db;
        }
       

        public void Update(Company obj)
        {
            _Db.Companies.Update(obj);
        }
    }
}
