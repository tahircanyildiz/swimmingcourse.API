using Application.Abstraction.AbsCourseEntity;
using Domain.Entities.course;
using Domain.Entities.user;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Persistence.context;

namespace UserManagement.Persistence.concrete.CourseConcrete
{
    public class CourseReadRepo : ReadRepository<CourseEntity>, ICourseReadRepo
    {
        public CourseReadRepo(ApplicationDbContext context) : base(context)
        {
        }
    }
}
