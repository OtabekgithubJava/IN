using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Entities.Enums
{
    public enum Permission
    {
        CreateUser=1,
        GetAll=2,
        GetByUserId=3,
        GetByRole=4,
        GetByUserFullName=5,
        UpdateUser=6,
        DeleteUser=7,
        AddPost=8,
        GetByPostId=9,
        GetAllPost=10,
        UpdatePost=11,
        DeletePost=12,
        GetUserPDF=13,
        GetJobsBySalaryRange=14,
        AddAttachment=15,
        GetJobsPDF=16
    }
}