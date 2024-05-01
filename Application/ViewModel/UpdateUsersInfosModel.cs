using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModel
{
    public class UpdateUsersInfosModel
    {
        public string Id { get; set; }
        [MinLength (3)]
        public string? Username { get; set; }
        public string? email { get; set; }
        public string? phoneNumber { get; set; }
        public string? field { get; set; }
        public string?  image { get; set; }
        public bool? IsDeleted { get; set; }


    }
}
