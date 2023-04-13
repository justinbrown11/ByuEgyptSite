using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace ByuEgyptSite
{
    /// <summary>
    /// The email sender class. This handles the sending out of authentication emails, password resets, etc.
    /// Integrated with AspNetCore.Identity and SendGrid
    /// </summary>
    public class EmailSender : IEmailSender // inherits from send grid
    {
        private readonly ILogger _logger;

        // Constructor
        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor,
                           ILogger<EmailSender> logger)
        {
            var sendGridKey = Environment.GetEnvironmentVariable("SENDGRID_KEY"); // grab send grid api key from .env

            // Instatiate message options with sendgrid key
            Options = new AuthMessageSenderOptions
            {
                SendGridKey = sendGridKey,
            };

            _logger = logger;
        }

        // email options
        public AuthMessageSenderOptions Options { get; }

        /// <summary>
        /// Sends SendGrid email based on parameters
        /// </summary>
        /// <param name="toEmail"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            // Throw error if no sendgrid key
            if (string.IsNullOrEmpty(Options.SendGridKey))
            {
                throw new Exception("Null SendGridKey");
            }

            // Send email
            await Execute(Options.SendGridKey, subject, message, toEmail);
        }

        /// <summary>
        /// Executes sending the email
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <param name="toEmail"></param>
        /// <returns></returns>
        public async Task Execute(string apiKey, string subject, string message, string toEmail)
        {
            // Instantiate SendGrid client with api key
            var client = new SendGridClient(apiKey);

            // Instatiate a SendGrid message object
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("intex2.team24@gmail.com", "Password Recovery"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };

            // Add email address
            msg.AddTo(new EmailAddress(toEmail));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);

            // Send the email
            var response = await client.SendEmailAsync(msg);

            // Log results
            _logger.LogInformation(response.IsSuccessStatusCode
                                   ? $"Email to {toEmail} queued successfully!"
                                   : $"Failure Email to {toEmail}");
        }
    }
}
