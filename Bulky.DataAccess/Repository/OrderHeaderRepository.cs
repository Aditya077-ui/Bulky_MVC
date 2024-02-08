using Bulky.DataAccess.Data;
using Bulky.DataAccess.Migrations;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private ApplicationDBContext _db;
        public OrderHeaderRepository(ApplicationDBContext db) : base(db)
        {
            _db = db;
        }
        //public void Save()
        //{
          //  _db.SaveChanges();
        //}

        public void Update(OrderHeader obj)
        {
           _db.OrderHeaders.Update(obj);
        }

		public void UpdateStatus(int id, string orderstatus, string? paymentstatus = null)
		{
			var ordersfromDB = _db.OrderHeaders.FirstOrDefault(u => u.Id == id);
            if (ordersfromDB != null)
            {
                ordersfromDB.OrderStatus = orderstatus;
                if (!string.IsNullOrEmpty(paymentstatus))
                {
                    ordersfromDB.PaymentStatus = paymentstatus;
                }
            }
		}

		public void updateStripePaymentId(int id, string SessionId, string paymentIntendId)
		{
			var ordersfromDB = _db.OrderHeaders.FirstOrDefault(u => u.Id == id);
            if (!string.IsNullOrEmpty(SessionId))
            {
                ordersfromDB.SessionId = SessionId;
            }
            if (!string.IsNullOrEmpty (paymentIntendId))
            {
                ordersfromDB.PaymentIntendId = paymentIntendId;
                ordersfromDB.PaymentDate = DateTime.Now;
            }
		}
	}
}
