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
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        private readonly ApplicationDbContext _Db;
        public OrderDetailRepository(ApplicationDbContext Db) : base(Db)
        {
            _Db = Db;
        }
       

        public void Update(OrderDetail obj)
        {
            _Db.OrderDetails.Update(obj);
        }
    }
}
