using Xunit;
using Moq;
using System.Threading.Tasks;
using JobBoard.Application.Abstractions.IService;
using JobBoard.Domain.Entities.DTOs;
using JobBoard.Domain.Entities.Models;
using System.Collections.Generic;
using System.Linq.Expressions;
using JobBoard.Application.Abstractions;
using JobBoard.Application.Services.BookService;

namespace JobBoard.UnitTests
{
    public class PostTests
    {
        [Fact]
        public async Task GetPostById_ReturnsPost()
        {
            int postId = 1;
            var mockPostRepository = new Mock<IPostRepository>();
            var expectedPost = new Post { ID = postId, Job = "Software Engineer", Salary = "80000" };
            mockPostRepository.Setup(repo => repo.GetByAny(It.IsAny<Expression<Func<Post, bool>>>()))
                .ReturnsAsync(expectedPost);
            var postService = new PostService(mockPostRepository.Object);

            var result = await postService.GetPostById(postId);

            Assert.NotNull(result);
            Assert.Equal(expectedPost.ID, result.ID);
            Assert.Equal(expectedPost.Salary, result.Salary); 
        }

        [Fact]
        public async Task UpdatePost_Success()
        {
            int postId = 1;
            var postDTO = new PostDTO { Job = "Updated Job", Salary = "100000" };
            var existingPost = new Post { ID = postId, Job = "Software Engineer", Salary = "80000" };
            var mockPostRepository = new Mock<IPostRepository>();
            mockPostRepository.Setup(repo => repo.GetByAny(It.IsAny<Expression<Func<Post, bool>>>()))
                              .ReturnsAsync(existingPost);
            var postService = new PostService(mockPostRepository.Object);

            var result = await postService.Update(postId, postDTO);

            Assert.Equal("Updated", result);
            Assert.Equal(postDTO.Job, existingPost.Job);
            Assert.Equal(postDTO.Salary, existingPost.Salary);
        }

        [Fact]
        public async Task DeletePost_Success()
        {
            int postId = 1;
            var mockPostRepository = new Mock<IPostRepository>();
            mockPostRepository.Setup(repo => repo.Delete(It.IsAny<Expression<Func<Post, bool>>>()))
                              .ReturnsAsync(true);
            var postService = new PostService(mockPostRepository.Object);

            var result = await postService.Delete(postId);

            Assert.Equal("Deleted", result);
        }
    }
}
