using JobBoard.Domain.Entities.DTOs;

namespace JobBoard.Application.Abstractions.IService
{
    public interface IAuthSevice
    {
        public Task<ResponseLogin> GenerateToken(RequestLogin request);
        public Task<string> CreateUser(UserDTO userDTO);
    }
}