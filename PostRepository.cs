using JobBoard.Application.Abstractions;
using JobBoard.Domain.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobBoard.Infrastructure.Persistance;

namespace JobBoard.Infrastructure.BaseRepositories
{
    public class PostRepository : BaseRepository<Post>, IPostRepository
    {
        public PostRepository(JobBoardDbContext context) : base(context) 
        { 
        
        }
    }
}