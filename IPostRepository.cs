using JobBoard.Domain.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Application.Abstractions
{
    public interface IPostRepository : IBaseRepository<Post>
    {
    }
}