using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceApp.Data;
using FinanceApp.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;


namespace FinanceApp.Controllers

{
    [EnableCors("AllowOrigin")]
    [Route("api/[controller]")]

    public class PaymentController : Controller
    {
        private readonly UserDbContext context;

        public PaymentController(UserDbContext userdbcontext)
        {
            context = userdbcontext;
        }

        [HttpPost("PaymentDetails")]
        public IActionResult AddPaymentDetails([FromBody] PaymentModel paymentObj)
        {
            if (paymentObj != null && context.ProductCustomerModels.Any(a => a.ProductCustomerId == paymentObj.ProductCustomerId))
            {
                context.PaymentModels.Add(paymentObj);
                context.SaveChanges();
                return Ok(paymentObj);
            }
            return BadRequest();
        }

        [HttpGet("AllpaymentDetails")]
        public IActionResult GetPayment()
        {
            var payment = context.PaymentModels.AsQueryable();
            return Ok(payment);
        }

        [HttpGet("getPaymentDetails")]
        public IActionResult GetPaymentDetails(int id)
        {
            var products = context.PaymentModels.Where(a => a.ProductCustomerId == id);
            return Ok(products);

        }

        [HttpPost("FilteredItems")]
        public IActionResult GetFilterDetails([FromBody] RequestModel requestModel)
        {
            var response = GetFilter(requestModel);
            return Ok(response);
        }

        private List<PaymentModel> GetFilter(RequestModel requestModel)
        {
            var response = context.PaymentModels.Where(a => a.PaymentDate.Date >= requestModel.FromDate.Date && a.PaymentDate.Date
            <= requestModel.ToDate.Date);
            return response.ToList();
        }

        [HttpGet("CustomerPayHistory")]
        public IActionResult CustomerDetailsForPay(int id)
        {
            var data = from c1 in context.ProductCustomerModels
                       join c in context.CustomerModels on c1.CustomerId equals c.CustomerId
                       join p in context.ProductModels on c1.ProductId equals p.ProductId
                       join p1 in context.PaymentModels on c1.ProductCustomerId equals p1.ProductCustomerId
                       where c1.ProductCustomerId  == id 
                       select new
                       {
                           p.ProductName,
                           c.CustomerName,
                           p1.PaymentId,
                           p1.PaymentDate,
                           p1.PaidAmount,
                           c1.ProductCustomerId
                       };
            return Ok(data);
        }

    }
}