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
    public class OrderHeaderRepository : Repository<OrderHeader>,IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _Db;
        public OrderHeaderRepository(ApplicationDbContext Db) : base(Db)
        {
            _Db = Db;
        }
       

        public void Update(OrderHeader obj)
        {
            _Db.OrderHeaders.Update(obj);
        }

        public void UpdateStatus(int id, string ordreStatus, string? paymantStatus = null)
        {
            var orderFromDb = _Db.OrderHeaders.FirstOrDefault(u => u.Id == id);
            if(orderFromDb != null)
            {
                orderFromDb.OrderStatus = ordreStatus;
                if(paymantStatus != null)
                {
                    orderFromDb.PaymentStatus = paymantStatus;
                }
            }
        }
    }
}
