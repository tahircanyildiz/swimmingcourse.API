using Domain.Entities.user;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModel
{
    public class CreateCourseEntity
    {
        public Guid Id { get; set; }
        public string studentId { get; set; }
        public string teacherId { get; set; }
        public string name { get; set; }
        public bool status { get; set; }
        public string day { get; set; }
        public string hour { get; set; }
    }
}
