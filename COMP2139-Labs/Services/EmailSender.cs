using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using SendGrid;
using SendGrid.Helpers.Mail;
using MailKit.Net.Smtp;
using MimeKit;
namespace COMP2139_Labs.Services;

public class EmailSender: IEmailSender
{
    private readonly IConfiguration _configuration;

    public EmailSender(IConfiguration configuration)
    {
        _configuration = configuration
                         ?? throw new ArgumentNullException("SendGrid key is missing");
    }

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        try
        {
            /*var client=new SendGridClient(_sendGridApiKey);
            var from = new EmailAddress("diana.mohammadi@georgebrown.ca", "PM Tool Default Sender");
            var to = new EmailAddress(email);
            var msg = MailHelper
                .CreateSingleEmail(from, to, subject, "Welcome to PM Tool Inc", message);
            var response = await client.SendEmailAsync(msg);
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Body.ReadAsStringAsync();
                Console.WriteLine($"An error occured while sending email: {errorMessage}");
            }*/
            // Retrieve Mailgun SMTP settings from configuration
            Console.WriteLine($"Email body: {message}");
            var smtpHost = _configuration["Mailgun:SmtpHost"] ?? throw new ArgumentNullException("SmtpHost is missing");
            var smtpPort = int.Parse(_configuration["Mailgun:SmtpPort"] ?? throw new ArgumentNullException("SmtpPort is missing"));
            var smtpUsername = _configuration["Mailgun:SmtpUsername"] ?? throw new ArgumentNullException("SmtpUsername is missing");
            var smtpPassword = _configuration["Mailgun:SmtpPassword"] ?? throw new ArgumentNullException("SmtpPassword is missing");
            var fromEmail = _configuration["Mailgun:FromEmail"] ?? throw new ArgumentNullException("FromEmail is missing");
            var fromName = _configuration["Mailgun:FromName"] ?? "PM Tool Default Sender";

            // Create the email message
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress(fromName, fromEmail));
            mimeMessage.To.Add(new MailboxAddress("", email));
            mimeMessage.Subject = subject;

            // Set the email body (HTML or plain text)
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = message // Assuming 'message' is HTML; use TextBody for plain text
            };
            mimeMessage.Body = bodyBuilder.ToMessageBody();
           
            // Send the email using MailKit
            using var client = new SmtpClient();
            await client.ConnectAsync(smtpHost, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(smtpUsername, smtpPassword);
            await client.SendAsync(mimeMessage);
            await client.DisconnectAsync(true);
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured while sending email: {ex.Message}");
            throw;
        }
    }
}