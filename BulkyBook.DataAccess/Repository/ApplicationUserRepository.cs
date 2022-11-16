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
    public class ApplicationRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDbContext _Db;
        public ApplicationRepository(ApplicationDbContext Db) : base(Db)
        {
            _Db = Db;
        }
       
    }
}
