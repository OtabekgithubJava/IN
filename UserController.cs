using JobBoard.API.Attributes;
using JobBoard.Application.Abstractions.IService;
using JobBoard.Domain.Entities.DTOs;
using JobBoard.Domain.Entities.Enums;
using JobBoard.Domain.Entities.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [IdentityFilter(Permission.GetAll)]
        public async Task<List<UserViewModel>> GetAll()
        {
            return await _userService.GetAll();
        }
        [HttpGet]
        [IdentityFilter(Permission.GetByUserId)]
        public async Task<UserViewModel> GetByUserId(int id)
        {
            return await _userService.GetById(id);
        }
        
        [HttpGet]
        [IdentityFilter(Permission.GetByRole)]
        public async Task<List<UserViewModel>> GetByRole(string role)
        {
            return await _userService.GetByRole(role);
        }
        [HttpGet]
        [IdentityFilter(Permission.GetByUserFullName)]
        public async Task<List<UserViewModel>> GetByUserFullName(string fullname)
        {
            return await _userService.GetByName(fullname);
        }
        [HttpPut]
        [IdentityFilter(Permission.UpdateUser)]
        public async Task<string> UpdateUser(int id, UserDTO userDTO)
        {
            return await _userService.UpdateUser(id, userDTO);
        }
        [HttpDelete]
        [IdentityFilter(Permission.DeleteUser)]
        public async Task<string> DeleteUser(int id)
        {
            return await _userService.DeleteUser(id);
        }
        [HttpGet("download")]
        [IdentityFilter(Permission.GetUserPDF)]
        public async Task<IActionResult> DownloadFile()
        {
            var filePath = await _userService.GetPdfPath();

            if (!System.IO.File.Exists(filePath))
                return NotFound("File not found");


            var fileBytes = System.IO.File.ReadAllBytes(filePath);


            var contentType = "application/octet-stream";

            var fileExtension = Path.GetExtension(filePath).ToLowerInvariant();

            // Send the file 
            return File(fileBytes, contentType, Path.GetFileName(filePath));
        }
    }
}