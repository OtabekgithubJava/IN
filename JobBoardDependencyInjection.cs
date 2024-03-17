using JobBoard.Application.Abstractions.IService;
using JobBoard.Application.Services.AuthService;
using JobBoard.Application.Services.BookService;
using JobBoard.Application.Services.UserService;
using Microsoft.Extensions.DependencyInjection;


namespace JobBoard.Application
{
    public static class JobBoardDependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IAuthSevice, AuthService>();
            services.AddScoped<IUserService, UserService>();
            return services;
        }
    }
}