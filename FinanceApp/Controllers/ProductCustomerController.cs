using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceApp.Data;
using FinanceApp.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("api/[controller]")]
    public class ProductCustomerController : Controller
    {
        private readonly UserDbContext context;

        public ProductCustomerController(UserDbContext userdbcontext)
        {
            context = userdbcontext;
        }

        [HttpPost("AddProductCustomerdetails")]
        public IActionResult AddProductCustometDetails([FromBody] ProductCustomerModel userObj)
        {
            try
            {
                var slotno = (from a in context.ProductCustomerModels where a.ProductId == userObj.ProductId select a.SlotNo).Max();
                userObj.SlotNo = slotno + 1;
                if (context.ProductModels.Any(a => a.ProductId == userObj.ProductId && a.NoOfCustomers >= userObj.SlotNo) &&
            context.CustomerModels.Any(a => a.CustomerId == userObj.CustomerId) &&
           !context.ProductCustomerModels.Any(a => a.ProductId == userObj.ProductId && a.SlotNo == userObj.SlotNo))
                {
                    context.ProductCustomerModels.Add(userObj);
                    context.SaveChanges();
                    return Ok(userObj);
                }
            }
            catch (InvalidOperationException)
            {
                userObj.SlotNo = 1;
                context.ProductCustomerModels.Add(userObj);
                context.SaveChanges();
                return Ok(userObj); ;
            }
            return BadRequest();
        }

        [HttpGet("AllproductCustomer")]
        public IActionResult GetAllProductCustomer()
        {
            bool IsActive = true;
            if (IsActive == true)
            {
                var productcustomer = context.ProductCustomerModels.Where(a => a.IsActive == IsActive);
                return Ok(productcustomer);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet("OrderByProduct")]
        public IActionResult GetById(int ProductId)
        {
            bool IsActive = true;
            var products = context.ProductCustomerModels.Where(a => a.ProductId == ProductId && a.IsActive == IsActive);
            if (products != null)
            {
                return Ok(products);
            }
            return NotFound();
        }

        [HttpGet("OrderBycustomer")]
        public IActionResult GetCustomer(int customerId)
        {
            bool IsActive = true;
            var products = context.ProductCustomerModels.Where(a => a.CustomerId == customerId && a.IsActive == IsActive);
            if (products != null)
            {
                return Ok(products);
            }
            return NotFound();
        }

        [HttpGet("FliterCustomerDetailsForProduct")]
        public IActionResult CustomerDetailsForPay(int id)
        {
            var data = from c1 in context.ProductCustomerModels
                       join c in context.CustomerModels on c1.CustomerId equals c.CustomerId
                       join p in context.ProductModels on c1.ProductId equals p.ProductId
                       where c1.ProductId == id
                       select new
                       {
                           c.CustomerName,
                           c1.SlotNo,
                           c.CustomerId,
                           p.ProductId,
                           c1.ProductCustomerId
                       };
            return Ok(data);
        }

        [HttpGet("FliterProductForCustomer")]
        public IActionResult ProductDetailsForPay(int id)
        {
            var data = from c1 in context.ProductCustomerModels
                       join c in context.CustomerModels on c1.CustomerId equals c.CustomerId
                       join p in context.ProductModels on c1.ProductId equals p.ProductId
                       where c1.CustomerId == id
                       select new
                       {
                           c.CustomerName,
                           c1.SlotNo,
                           p.ProductName,
                           c.CustomerId,
                           p.ProductId
                       };
            return Ok(data);
        }
    }
}

