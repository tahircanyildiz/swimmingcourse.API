using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModel
{
    public class UpdateUserPasswordModel
    {
        public string? Id { get; set; }
        public string currentPassword { get; set; }
        public string newPassword { get; set; }
    }
}
