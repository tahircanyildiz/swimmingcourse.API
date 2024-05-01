using Domain.Entities.common;
using Domain.Entities.user;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.course
{
    public class CourseEntity : BaseEntity
    {
        public string teacherId { get; set; }
        public string day { get; set; }
        public string hour { get; set; }
        public ApplicationUser? Student { get; set; }


    }
}
