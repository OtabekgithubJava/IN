using JobBoard.API.Attributes;
using JobBoard.Application.Abstractions.IService;
using JobBoard.Domain.Entities.DTOs;
using JobBoard.Domain.Entities.Enums;
using JobBoard.Domain.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }
        
        [HttpPost]
        [IdentityFilter(Permission.AddPost)]
        public async Task<string> AddPost(PostDTO model)
        {
            var result = await _postService.Add(model);

            return result;
        }
        [HttpGet]
        [IdentityFilter(Permission.GetByPostId)]
        public async Task<Post> GetByPostId(int id)
        {
            return await _postService.GetPostById(id);
        }
        
        [HttpGet]
        [IdentityFilter(Permission.GetJobsBySalaryRange)] 
        public async Task<List<Post>> GetJobsBySalaryRange(decimal minSalary, decimal maxSalary)
        {
            var result = await _postService.GetJobsBySalaryRange(minSalary, maxSalary);
            return result;
        }
        
        [HttpPost]
        [IdentityFilter(Permission.AddAttachment)] 
        public async Task<string> AddAttachment(IFormFile file)
        {
            return await _postService.AddAttachment(file);
        }
        
        [HttpGet]
        [IdentityFilter(Permission.GetAllPost)]
        public async Task<List<Post>> GetAllPost()
        {
            var result = await _postService.GetAllPost();

            return result;
        }

        [HttpPut]
        [IdentityFilter(Permission.UpdatePost)]
        public async Task<string> UpdatePost(int id, PostDTO postDto)
        {
            return await _postService.Update(id, postDto);
        }
        
        [HttpDelete]
        [IdentityFilter(Permission.DeletePost)]
        public async Task<string> DeletePost(int id)
        {
            return await _postService.Delete(id);
        }
        
        [HttpGet]
        [IdentityFilter(Permission.GetJobsPDF)]
        public async Task<IActionResult> GetJobsPDF()
        {
            var filePath = await _postService.GetJobsPDF();

            if (!System.IO.File.Exists(filePath))
                return NotFound("File not found");

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var contentType = "application/pdf";
            var fileExtension = Path.GetExtension(filePath).ToLowerInvariant();
            
            return File(fileBytes, contentType, Path.GetFileName(filePath));
        }
    }
}