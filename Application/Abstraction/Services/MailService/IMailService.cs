using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstraction.Services.MailService
{
    public interface IMailService
    {
        Task sendMessageAsync(string to, string subject, string body, bool isBodyHtml = true);
        Task sendMessageAsync(string[] tos, string subject, string body, bool isBodyHtml = true);
    }
}
