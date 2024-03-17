using Xunit;
using Moq;
using System.Threading.Tasks;
using JobBoard.Application.Abstractions.IService;
using JobBoard.Domain.Entities.DTOs;
using JobBoard.Domain.Entities.Models;
using System.Collections.Generic;
using System.Linq.Expressions;
using JobBoard.Application.Abstractions;
using JobBoard.Application.Services.UserService;

namespace JobBoard.UnitTests
{
    public class UserTests
    {
        [Fact]
        public async Task GetUserById_ReturnsUser()
        {
            int userId = 1;
            var mockUserRepository = new Mock<IUserRepository>();
            var expectedUser = new User { UserId = userId, FullName = "John Doe", Email = "john@example.com", Role = "User" };
            mockUserRepository.Setup(repo => repo.GetByAny(It.IsAny<Expression<Func<User, bool>>>()))
                              .ReturnsAsync(expectedUser);
            var userService = new UserService(mockUserRepository.Object, null);

            var result = await userService.GetById(userId);

            Assert.NotNull(result);
            Assert.Equal(expectedUser.UserId, result.UserId);
        }

        [Fact]
        public async Task UpdateUser_Success()
        {
            int userId = 1;
            var userDTO = new UserDTO { FullName = "Updated Name", Email = "updated@example.com" };
            var mockUserRepository = new Mock<IUserRepository>();
            var existingUser = new User { UserId = userId, FullName = "John Doe", Email = "john@example.com", Role = "User" };
            mockUserRepository.Setup(repo => repo.GetByAny(It.IsAny<Expression<Func<User, bool>>>()))
                              .ReturnsAsync(existingUser);
            var userService = new UserService(mockUserRepository.Object, null);

            var result = await userService.UpdateUser(userId, userDTO);

            Assert.Equal("Updated", result);
            Assert.Equal(userDTO.FullName, existingUser.FullName);
            Assert.Equal(userDTO.Email, existingUser.Email);
        }

        [Fact]
        public async Task DeleteUser_Success()
        {
            int userId = 1;
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(repo => repo.Delete(It.IsAny<Expression<Func<User, bool>>>()))
                              .ReturnsAsync(true);
            var userService = new UserService(mockUserRepository.Object, null);

            var result = await userService.DeleteUser(userId);
            
            Assert.Equal("Deleted", result);
        }
    }
}
