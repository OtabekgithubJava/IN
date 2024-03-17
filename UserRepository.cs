using JobBoard.Application.Abstractions;
using JobBoard.Domain.Entities.Models;
using JobBoard.Infrastructure.Persistance;


namespace JobBoard.Infrastructure.BaseRepositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(JobBoardDbContext context) : base(context) 
        {
        }
    }
}