using Application.Abstraction.Services.MailService;
using Application.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MailKit.Net.Pop3;
using MailKit;
using MimeKit;
using MailKit.Net.Imap;
using MailKit.Search;

namespace UserManagement.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        readonly Application.Abstraction.Services.MailService.IMailService _mailservice;
        private readonly string mailServer = "imap.gmail.com";
        private readonly int port = 993;
        private readonly bool useSsl = true;
        private readonly string username = "kenmiles4145@gmail.com";
        private readonly string password = "smmujwnuaxlayoep";
        private readonly string specificSender = "\"Swimming Course\" <kenmiles4145@gmail.com>";
        public MailController(Application.Abstraction.Services.MailService.IMailService mailservice)
        {
            _mailservice = mailservice;
        }
        [HttpPost]
        public async Task<IActionResult> Get(CreateMailModel model)
        {
            await _mailservice.sendMessageAsync(model.to, model.subject, model.body);
            return Ok("Başarılı");
        }
        [HttpGet("all")]
        public IActionResult GetPageOfMails(int page = 1, int pageSize = 10)
        {
            try
            {
                var emails = new List<object>();
                using (var client = new ImapClient())
                {
                    client.Connect(mailServer, port, useSsl); client.Authenticate(username, password);
                    var inbox = client.Inbox;  inbox.Open(FolderAccess.ReadOnly);
                    var totalMessageCount = inbox.Count; var start = Math.Max(totalMessageCount - (page * pageSize), 0);
                    var end = Math.Max(totalMessageCount - ((page - 1) * pageSize), 1);
                    var results = inbox.Fetch(start, end, MessageSummaryItems.Full | MessageSummaryItems.UniqueId);
                    foreach (var summary in results)
                    {
                        var message = inbox.GetMessage(summary.UniqueId);
                        var emailInfo = new
                        { From = message.From.ToString(),Subject = message.Subject, Body = message.HtmlBody};
                        if (emailInfo.From == specificSender)                      
                           emails.Add(emailInfo);                       
                    }
                    client.Disconnect(true);
                }
                return Ok(new { emails = emails });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Sunucuda bir hata oluştu: {ex.Message}");
            }
        }

    }
}
