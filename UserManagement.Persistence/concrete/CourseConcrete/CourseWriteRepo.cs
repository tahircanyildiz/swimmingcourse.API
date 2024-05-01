using Application.Abstraction;
using Domain.Entities.course;
using UserManagement.Persistence.context;
using Application.Abstraction.AbsCourseEntity;


namespace UserManagement.Persistence.concrete.CourseConcrete
{
    public class CourseWriteRepo : WriteRepository<CourseEntity>, ICourseWriteRepo
    {
        public CourseWriteRepo(ApplicationDbContext context) : base(context)
        {
        }
    }
}