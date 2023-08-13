using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementation
{
    public class ImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;

        //private readonly HttpContextAccessor httpContextAccessor;
        private readonly ApplicationDbContext appDbContext;

        public ImageRepository(IWebHostEnvironment webHostEnvironment, 
            IHttpContextAccessor httpContextAccessor, ApplicationDbContext appDbContext)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.appDbContext = appDbContext;
        }

        public async Task<IEnumerable<BlogImage>> GetAll()
        {
            return await appDbContext.BlogImages.ToListAsync();
        }

        public async Task<BlogImage> Upload(IFormFile file, BlogImage blogImage)
        {
            //
            var localPath = Path.Combine(webHostEnvironment.ContentRootPath, "Images",
                $"{blogImage.FileName}{blogImage.FileExtension}");
            using var stream = new FileStream(localPath, FileMode.Create);
            await file.CopyToAsync(stream);

            // we are going to construct this url and save it in DB along with other info

            var httpRequest = httpContextAccessor.HttpContext.Request;
            var urlPath = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}/Images/{blogImage.FileName}{blogImage.FileExtension}";
            //var urlPath = $"{httpContextAccessor.HttpContext.Request}";
            blogImage.ImageUrl = urlPath;
            await appDbContext.BlogImages.AddAsync(blogImage);
            await appDbContext.SaveChangesAsync();
            return blogImage;

        }
    }
}
