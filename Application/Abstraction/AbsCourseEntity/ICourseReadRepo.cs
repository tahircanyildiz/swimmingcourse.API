using Domain.Entities.course;
using Domain.Entities.user;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstraction.AbsCourseEntity
{
    public interface ICourseReadRepo: IReadRepository<CourseEntity>
    {

    }
}
