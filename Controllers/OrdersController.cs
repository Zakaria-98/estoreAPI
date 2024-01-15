using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using storeAPI.Dto;
using storeAPI.Models;
namespace storeAPI.Controllers
{

//    [Authorize(Roles ="User")]
    [Route("api/[controller]")]
    [ApiController]

    
    public class OrdersController : ControllerBase
    {
        private ApplicationDbContext _context;
        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        [HttpGet]

        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _context.Orders
                .Join(
                _context.OrderProducts,
                order=>order.Id,
                orderproduct=>orderproduct.OrderId,
                (order, orderproduct) => new
                {
                    productid=orderproduct.ProductId,
                    quantity=orderproduct.ProductQty,
                    orderid=order.Id,
                    userid=order.ApplicationUserId,
                    
                }
                ).GroupBy(o => o.orderid)
                .Select(g => new
                {
                    OrderId = g.Key,
                    Contents = g.ToList()
                })
                 .ToListAsync();
                        return Ok(orders);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderbyId(int id)
        {
            var order = await _context.Orders.Where(order=>order.Id==id)
                .Join(
                _context.OrderProducts,
                order => order.Id,
                orderproduct => orderproduct.OrderId,
                (order, orderproduct) => new
                {
                    orderid = order.Id,
                    productid = orderproduct.ProductId,
                    quantity = orderproduct.ProductQty,
                    userid = order.ApplicationUserId,
                    price=order.Price
                })

                 .ToListAsync();
            return Ok(order);

        }

        [HttpGet("UserId")]
        public async Task<IActionResult> GetOrderbyUserId(string userid)
        {
            var orders = await _context.Orders
                 .Join(
                 _context.OrderProducts,
                 order => order.Id,
                 orderproduct => orderproduct.OrderId,
                 (order, orderproduct) => new
                 {
                     orderid = order.Id,
                     productid = orderproduct.ProductId,
                     quantity = orderproduct.ProductQty,
                     customername = order.CustomerName,
                     userid = order.ApplicationUserId,
                     
                 }
                 ).Where(o =>o.userid==userid).GroupBy(o => o.orderid)
                 .Select(g => new
                 {
                     OrderId = g.Key,
                     Contents = g.ToList()
                 })
                  .ToListAsync();
            return Ok(orders);



            

        }


        [HttpPost]
        public async Task<IActionResult> AddOrder([FromQuery] CreateOrderDto dto , [FromBody] List< CreateOrderProduct> dto2)
        {
            var order = new Order
            { CustomerName = dto.CustomerName,
                PhoneNumber=dto.PhoneNumber,
                Address=dto.Address,
                Price=0,
                Status=0,
             
                ApplicationUserId=dto.ApplicationUserId


            };

            order.Products = new List<OrderProduct>();

           
            foreach (var item in dto2) {
                var product = await _context.Products.SingleOrDefaultAsync(i => i.Id == item.ProductId);
                order.Price += product.Price;
                order.Products.Add(
                    new OrderProduct
                {
                    ProductId = item.ProductId,
                    Order = order,
                    ProductQty = item.ProductQty

                });
               
                if (product != null && product.ProductQty < item.ProductQty)
                {
                    return BadRequest("The quantity of product is less than your order!");
                }
            }


            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();


            return Ok(order);

        }




        [HttpDelete]
        public async Task<IActionResult> UpdatOrder(int id)
        {
            var order = await _context.Orders.SingleOrDefaultAsync(order => order.Id == id);

            if (order == null)
                return NotFound("Wrong Id:" + id);
            _context.Remove(order);
            _context.SaveChanges();
            return Ok(order);

        }


    }
}
