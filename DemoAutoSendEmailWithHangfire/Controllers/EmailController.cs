using FluentEmail.Core;
using FluentEmail.Core.Models;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace DemoAutoSendEmailWithHangfire.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController(EmailService emailService) : ControllerBase
    {

        [HttpPost("send")]
        public IActionResult SendEmail(SendMail mail)
        {
            RecurringJob.AddOrUpdate("send news letter",
                () => emailService.SendNewsLetter(mail),
                Cron.Minutely);
            return Ok("Sent!");
        }
    }


    public record SendMail(string ToEmail, string Subject, string Message);
    public class EmailService(IFluentEmail fluentEmail)
    {
        public async Task SendNewsLetter(SendMail sendMail)
        {
            await fluentEmail
                 .To(sendMail.ToEmail)
                 .Subject(sendMail.Subject)
                 .Body(sendMail.Message, isHtml: true)
                 .SendAsync();
        }
    }
}
