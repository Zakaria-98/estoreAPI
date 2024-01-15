using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using storeAPI.Dto;
using storeAPI.Models;

namespace storeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private ApplicationDbContext _context;
        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]

        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            return Ok(categories);

        }

        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody]CreateCategoryDto dto)
        {
            var category = new Category { Name = dto.Name };
            _context.Categories.AddAsync(category);
            _context.SaveChanges();
            return Ok(category);

        }

        [HttpPut ("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CreateCategoryDto dto)
        {
            var category = await _context.Categories.SingleOrDefaultAsync(category => category.Id == id);

            if (category == null)
                return NotFound("Wrong Id:" + id);

            category.Name = dto.Name;
            _context.SaveChanges();
            return Ok(category);

        }

        [HttpDelete]
        public async Task<IActionResult> UpdateCategory(int id)
        {
            var category = await _context.Categories.SingleOrDefaultAsync(category => category.Id == id);

            if (category == null)
                return NotFound("Wrong Id:" + id);
            _context.Remove(category);
            _context.SaveChanges();
            return Ok(category);

        }



    }
}
