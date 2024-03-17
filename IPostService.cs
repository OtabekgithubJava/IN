using JobBoard.Domain.Entities.DTOs;
using JobBoard.Domain.Entities.Models;
using Microsoft.AspNetCore.Http;

namespace JobBoard.Application.Abstractions.IService
{
    public interface IPostService
    {
        public Task<string> Add(PostDTO postDto);
        public Task<Post> GetPostById(int id);
        public Task<List<Post>> GetAllPost();
        public Task<string> Delete(int id);
        public Task<string> Update(int id, PostDTO postDto);
        Task<List<Post>> GetJobsBySalaryRange(decimal minSalary, decimal maxSalary);
        Task<string> AddAttachment(IFormFile file);
        Task<string> GetJobsPDF();
    }
}