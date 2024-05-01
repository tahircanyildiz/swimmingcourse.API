using Domain.Entities.user;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModel
{
    public class GetAllCoursesWithTeacher
    {
        public Guid Id { get; set; }
        public ApplicationUser student { get; set; }
        public ApplicationUser teacher { get; set; }
        public string name { get; set; }
        public string day { get; set; }
        public string hour { get; set; }

    }
}
