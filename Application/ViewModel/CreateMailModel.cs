using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModel
{
    public class CreateMailModel
    {
        public string[] to { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
    }
}
