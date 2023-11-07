using AutoMapper;
using BlogWebApi.Models;
using BlogWebApi.Models.Domain;
using BlogWebApi.Models.ModelMapping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace BlogWebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        private readonly BlogDbwebapiContext _context;
        private readonly IMapper _mapper;

        public PostController(BlogDbwebapiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //get all posts
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllPosts()
        {
            //get the information of the currrent ID 
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var posts = await _context.Posts.Where(post => post.UserId == new Guid(userId)).ToListAsync();
            return Ok(posts);
        }


        //create new posts
        [Authorize(Roles = "admin, superadmin")]
        [HttpPost]
        public IActionResult CreatePost([FromBody] CreatePostModel model)
        {
            if (ModelState.IsValid)
            {
                //Find the categorey by the given name
                var category = _context.Categories.SingleOrDefault(c => c.Name.Contains(model.CategoryName));
                if (category == null) return BadRequest("Category not found");

                //get the information of the currrent ID 
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                //Create a new post based on the given model 
                var post = new Post
                {
                    CategoryId = category.CategoryId,
                    Title = model.Title,
                    Content = model.Content,
                    UserId = new Guid(userId),
                    Created = DateTime.Now,
                    LastModified = DateTime.Now
                };

                //save the changes
                _context.Posts.Add(post);
                _context.SaveChanges();

                return Ok("Posted successfully"); // Return success response
            }
            return BadRequest(ModelState); //Return bad request if the model state is invalide
        }

        //update an existing post 
        [Authorize(Roles = "admin, superadmin")]
        [HttpPut("{postId}")]
        public IActionResult UpdatePost(Guid postId, [FromBody] UpdatePostDTO model) 
        { 
            if (ModelState.IsValid)
            {
                //Find the post 
                var post = _context.Posts.SingleOrDefault(p => p.PostId == postId);

                if (post == null) return NotFound();

                //Check the if the user is authoried own this post
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if(new Guid(userId) == post.UserId)
                {
                    //Update its infromations
                    post.Title = model.Title;
                    post.Content = model.Content;
                    post.LastModified = DateTime.Now;
                }
                else return Forbid();

                //save the changes
                _context.SaveChanges();
                return Ok("Post has been updated");
            }
            
            return BadRequest(ModelState);
        }

        //deleting an existing post
        [Authorize(Roles = "admin, superadmin")]
        [HttpDelete("{postId}")]
        public IActionResult DeletePost(Guid postId)
        {
            //find and verify
            var post = _context.Posts.Find(postId);
            if (post == null) return NotFound();

            //Check the if the user is authoried own this post
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (new Guid(userId) == post.UserId)
            {
                //remove
                _context.Posts.Remove(post);

                //save changes
                _context.SaveChanges();
                return Ok("Deleted successfully");
            }
            else return Forbid();
        }


        //Dashboard for only superadmin
        [Authorize(Roles = "superadmin")]
        [HttpGet]
        [Route("Dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var posts = _context.Posts.Include(p => p.Category)
                                     .Include(p => p.User)
                                     .Select(p => _mapper.Map<PostDTO>(p));
            if (posts == null)
            {
                return Ok(posts);
            }
            else return NotFound("There is not posted yet");   
        }

        //get a single post by id only for superadmin
        [Authorize(Roles = "superadmin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPosById(Guid id)
        {
            //var post = await _context.Posts.Include(p => p.Category).FirstOrDefaultAsync(p => p.PostId == id);
            var post = _context.Posts.Include(p => p.Category)
                                     .Include(p => p.User)
                                     .Select(p => _mapper.Map<PostDTO>(p));
            if (post != null)
            {
                return Ok(post);
            }
            return NotFound();

        }
    }
}