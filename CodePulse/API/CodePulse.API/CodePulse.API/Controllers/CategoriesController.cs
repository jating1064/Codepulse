using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Writers;

namespace CodePulse.API.Controllers
{
    //https://localhost:xxxx/api/categories
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;

        //private readonly ApplicationDbContext dbContext;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
            //this.dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody]CreateCategoryRequestDto request)
        {
            //Map DTO to DM
            var category = new Category
            {
                Name = request.Name,
                UrlHandle = request.UrlHandle
            };

            //await dbContext.Categories.AddAsync(category);
            //await dbContext.SaveChangesAsync();

            await categoryRepository.CreateAsync(category);

            //Mao DM to DTO

            var response = new CategoryDto
            {
                Id = category.Id,
                Name = request.Name,
                UrlHandle = request.UrlHandle
            };
            
            return Ok(response);
        }

        //GET:/api/categories
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await categoryRepository.GetAllAsync();

            var response = new List<CategoryDto>();
            //Map DM To DTo
            foreach (var category in categories)
            {
                response.Add(new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    UrlHandle = category.UrlHandle
                });
            }
            return Ok(response);
        }

        //Get: https://localhost:7226/api/categories/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetCategoryById([FromRoute]Guid id)
        {
            var existingCategory= await categoryRepository.GetByIdAsync(id);
            if (existingCategory == null)
            {
                return NotFound();
            }
            //Map DM to DTo
            var response = new CategoryDto
            {
                Id = existingCategory.Id,
                Name = existingCategory.Name,
                UrlHandle = existingCategory.UrlHandle
            };
            return Ok(response);
        }

        //PUT: https://localhost:7226/api/categories/{id}

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> EditCategory([FromRoute]Guid id, UpdateCategoryRequestDto request)
        {
            //convert DTO to DM
            var category = new Category
            {
                Id = id,
                Name = request.Name,
                UrlHandle = request.UrlHandle
            };
            //RP to update category
            category= await categoryRepository.UpdateAsync(category);
            if(category == null)
            {
                return NotFound();
            }
            //Convert DM To DTO
            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };
            return Ok(response);

        }

        //DELETE https://localhost:7226/api/categories/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteCategory([FromRoute]Guid id)
        {
            var existingCategory = await categoryRepository.DeleteAsync(id);
            if(existingCategory == null)
            {
                return NotFound();
            }
            //Map DM to DTO
            var response = new CategoryDto
            {
                Id = existingCategory.Id,
                Name = existingCategory.Name,
                UrlHandle = existingCategory.UrlHandle
            };
            return Ok(response);
        }
    }
}
