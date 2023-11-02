using BlogWebApi.Models;
using BlogWebApi.Models.Domain;
using BlogWebApi.Models.ModelMapping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace BlogWebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        private readonly BlogDbwebapiContext _context;

        public PostController(BlogDbwebapiContext context)
        {
            _context = context;
        }

        //get all posts
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllPosts()
        {
            var posts = await _context.Posts.ToListAsync();
            return Ok(posts);
        }

        //get a single post by id
        [Authorize(Roles = "admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPosById(Guid id)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.PostId == id);
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }

        //create new posts
        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult CreatePost([FromBody] CreatePostModel model)
        {
            if (ModelState.IsValid)
            {
                //Find the categorey by the given name
                var category = _context.Categories.SingleOrDefault(c => c.Name.Contains(model.CategoryName));
                if (category == null) return BadRequest("Category not found");

                //Create a new post based on the given model 
                var post = new Post
                {
                    CategoryId = category.CategoryId,
                    Title = model.Title,
                    Content = model.Content,
                    //UserId = 2, //Tempo solution i will change it after implementing the authentification
                    Created = DateTime.Now,
                    LastModified = DateTime.Now
                };

                //save the changes
                _context.Posts.Add(post);
                _context.SaveChanges();

                return Ok(); // Return success response
            }
            return BadRequest(ModelState); //Return bad request if the model state is invalide
        }

        //update an existing post 
        [Authorize(Roles = "admin")]
        [HttpPut("{postId}")]
        public IActionResult UpdatePost(Guid postId, [FromBody] UpdatePostModel model) 
        { 
            if (ModelState.IsValid)
            {
                //Find the post 
                var post = _context.Posts.SingleOrDefault(p => p.PostId == postId);

                if (post == null) return NotFound();

                //Update its infromations
                post.Title = model.Title;
                post.Content = model.Content;
                post.LastModified = DateTime.Now;

                //save the changes
                _context.SaveChanges();
                return Ok();
            }
            return BadRequest(ModelState);
        }

        //deleting an existing post
        [Authorize(Roles = "admin")]
        [HttpDelete("{postId}")]
        public IActionResult DeletePost(int postId)
        {
            //find and verify
            var post = _context.Posts.Find(postId);
            if (post == null) return NotFound();

            //remove
            _context.Posts.Remove(post);    

            //save changes
            _context.SaveChanges();
            return Ok();
        }

        //Dashboard for only superadmin
        [Authorize(Roles = "superadmin")]
        [HttpGet]
        [Route("Dashboard")]
        public string GetDashboard()
        {
            return "Hello SuperAdmin \n You are in the Dashboard";
        } 
    }
}