using Application.Abstraction.AbsCourseEntity;
using Application.ViewModel;
using Domain.Entities.course;
using Domain.Entities.user;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace UserManagement.Presentation.Controllers 
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICourseReadRepo _readrepository;
        private readonly ICourseWriteRepo _writerepository;

        public CourseController(UserManager<ApplicationUser> userManager, ICourseReadRepo readrepository, ICourseWriteRepo writerepository)
        {
            _userManager = userManager;
            _readrepository = readrepository;
            _writerepository = writerepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var res = _readrepository.GetAll(false).Where(p => p.status == true).Select(
                p => new
                {
                    p.Id,
                    p.Student,
                    p.teacherId,
                    p.name,
                    p.day,
                    p.hour,
                }
                );

            var transformedAllCourses = res.Select(course => new GetAllCoursesWithTeacher
            {
                Id = course.Id,
                name = course.name,
                student = course.Student,
                teacher = _userManager.Users.SingleOrDefault(u => u.Id == course.teacherId),
                day = course.day,
                hour = course.hour,
            }).ToList();

            foreach (var courseViewModel in transformedAllCourses)
            {
                if (courseViewModel.teacher == null)
                {
                    // Öğretmen bulunamadı
                }
            }

            return Ok(transformedAllCourses);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> GetCourseByTime(GetCourseEntityByTime model)
        {
            var specificCourses = _readrepository.GetWhere(p => p.day == model.day);

            var transformedCourses = specificCourses.Select(p => new
            {
                p.Id,
                p.Student,
                p.name,
                p.day,
                p.hour,
                p.teacherId,
                p.createdTime,
                p.updatedTime,
            });

            if (transformedCourses.FirstOrDefault()?.teacherId != null)
            {
                var teacher = await _userManager.FindByIdAsync(transformedCourses.FirstOrDefault().teacherId);
                if (teacher != null)
                {
                    var finalFormofCourse = transformedCourses.Select(p => new
                    {
                        p.Id,
                        p.Student,
                        p.name,
                        p.day,
                        p.hour,
                        teacher,
                        p.createdTime,
                        p.updatedTime,
                    });

                    return Ok(finalFormofCourse);
                }
            }

            

            return Ok(transformedCourses);
        }


        [HttpPost]
        public async Task<IActionResult> Post(CreateCourseEntity model)
        {
            var student = await _userManager.FindByIdAsync(model.studentId);


            if (!string.IsNullOrEmpty(student.Id))
            {
                await _writerepository.AddAsync(new CourseEntity()
                {
                    teacherId = model.teacherId,
                    Student = student,
                    name = model.name,
                    status = model.status,
                    day = model.day,
                    hour = model.hour,
                });
            }

            

            await _writerepository.SaveAsync();
            return Ok("Başarılı");
        }

        [HttpPut]
        public async Task<IActionResult> Put(CreateCourseEntity model)
        {
            CourseEntity p = await _readrepository.GetByIdAsync(model.Id.ToString());
            var student = _userManager.FindByIdAsync(model.studentId);

            p.teacherId = model.teacherId;
            p.Student = student;
            p.name = model.name;
            p.status = model.status;
            p.day = model.day;
            p.hour = model.hour;

            await _writerepository.SaveAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _writerepository.RemoveAsync(id.ToString());
            await _writerepository.SaveAsync();
            return Ok("Başarılı");
        }
    }
}
