using Domain.Entities.common;
using Domain.Entities.course;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.user
{
    public class ApplicationUser : IdentityUser
    {
        public string? image { get; set; }
        public string? field { get; set; }
        public ICollection<CourseEntity> Courses { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime createdTime { get; set; }
        public DateTime updatedTime { get; set; }

        public static implicit operator ApplicationUser(Task<ApplicationUser> v)
        {
            throw new NotImplementedException();
        }
    }
}
