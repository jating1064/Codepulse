using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementation
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly ApplicationDbContext dbContext;

        public BlogPostRepository(ApplicationDbContext dbContext) 
        {
            this.dbContext = dbContext;
        }
        public async Task<BlogPost> CreateAsync(BlogPost blogPost)
        {
            await dbContext.BlogPosts.AddAsync(blogPost);
            await dbContext.SaveChangesAsync();
            return blogPost;
        }

        public async Task<IEnumerable<BlogPost>> GetAllasync()
        {   
            return await dbContext.BlogPosts.Include(x=>x.Categories).ToListAsync();

        }

        public async Task<BlogPost?> GetByIdAsync(Guid id)
        {
            return await dbContext.BlogPosts.Include(x=>x.Categories).FirstOrDefaultAsync(x=>x.Id == id);

        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            var existingBlogPost= dbContext.BlogPosts.Include(x=>x.Categories).FirstOrDefault(x=>x.Id==blogPost.Id);
            if (existingBlogPost == null) 
            {
                return null;
            }

            //Update BP
            dbContext.Entry(existingBlogPost).CurrentValues.SetValues(blogPost);
            
            //Update Categories
            existingBlogPost.Categories=blogPost.Categories;
            await dbContext.SaveChangesAsync();
            return blogPost;
        }
    }
}
