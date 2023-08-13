using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }
        //POST:{apibaseUrl}/api/images
        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file,
            [FromForm] string fileName,[FromForm]string title)
        {
            ValidateFileUpload(file);
            if (ModelState.IsValid)
            {
                //File Upload
                //We have to create a DM Object
                var blogImage = new BlogImage
                {
                    FileExtension = Path.GetExtension(file.FileName).ToLower(),
                    Title = title,
                    DateCreated = DateTime.Now,
                    FileName=fileName
      
                };
                blogImage=await imageRepository.Upload(file, blogImage);
                //Convert DM to DTO
                var response = new BlogImageDto
                {
                    Id = blogImage.Id,
                    Title = blogImage.Title,
                    DateCreated = blogImage.DateCreated,
                    FileExtension = blogImage.FileExtension,
                    FileName = blogImage.FileName,
                    ImageUrl = blogImage.ImageUrl
                };
                return Ok(response);
            }
            return BadRequest(ModelState);
        }
        private void ValidateFileUpload(IFormFile file)
        {
            var allowedExtensions = new string[]
            {
                ".jpeg","jpg",".png"
            };

            if(!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower())) 
            {
                ModelState.AddModelError("file", "Unsupported File Format");
            }

            if(file.Length > 10485760)
            {
                ModelState.AddModelError("file", "File Size can't be more than 10 MB");
            }

        }

        //GET{apibaseUrl}/api/images
        [HttpGet]
        public async Task<IActionResult> GetAllmages()
        {
            //Call repository method to egt all images
            var images = await imageRepository.GetAll();
            var response= new List<BlogImageDto>();
            //convert DM to Dto
            foreach (var image in images)
            {
                response.Add(new BlogImageDto
                {
                    Id = image.Id,
                    Title = image.Title,
                    DateCreated = image.DateCreated,
                    FileExtension = image.FileExtension,
                    FileName = image.FileName,
                    ImageUrl = image.ImageUrl
                });
            }
            return Ok(response);
        }
    }
}
