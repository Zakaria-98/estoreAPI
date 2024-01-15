using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using storeAPI.Dto;
using storeAPI.Models;

namespace storeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private ApplicationDbContext _context;
        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]

        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _context.Products.Include(m => m.Category).ToListAsync();
            return Ok(products);

        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetProductById(int id )
        {
            var product = await _context.Products.SingleOrDefaultAsync( m=> m.Id ==id);
            return Ok(product);

        }

        [HttpGet("{CategoryId}")]

        public async Task<IActionResult> GetProductsById(int id)
        {
            var products = await _context.Products.Where(m=>m.CategoryId == id).Include(m=>m.Category).ToListAsync();
            return Ok(products);

        }



        [HttpPost]
        public async Task<IActionResult> AddProduct([FromForm] CreateProductDto dto)
        {
            using var dataStream = new MemoryStream();
            await dto.image.CopyToAsync(dataStream);
            var product = new Product
            { Name = dto.Name,
                Description = dto.Description,
                image = dataStream.ToArray(),
                Price=dto.Price,
                rating=dto.rating,
                IsAvailable=dto.IsAvailable,
                CategoryId = dto.CategoryId,


            };
            _context.Products.AddAsync(product);
            _context.SaveChanges();
            return Ok(product);

        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] CreateProductDto dto)
        {
            using var dataStream = new MemoryStream();
            await dto.image.CopyToAsync(dataStream);
            var product = await _context.Products.SingleOrDefaultAsync(product => product.Id == id);

            if (product == null)
                return NotFound("Wrong Id:" + id);


                product.Name = dto.Name;
                product.Description = dto.Description;
                product.image = dataStream.ToArray();
                product.Price = dto.Price;
                product.rating = dto.rating;
                product.IsAvailable = dto.IsAvailable;
                product.CategoryId = dto.CategoryId;


           
            _context.SaveChanges();
            return Ok(product);

        }

        [HttpDelete]
        public async Task<IActionResult> UpdateProducts(int id)
        {
            var product = await _context.Products.SingleOrDefaultAsync(product => product.Id == id);

            if (product == null)
                return NotFound("Wrong Id:" + id);
            _context.Remove(product);
            _context.SaveChanges();
            return Ok(product);

        }





    }
}
