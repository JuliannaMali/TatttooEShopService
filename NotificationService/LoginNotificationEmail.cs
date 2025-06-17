using System;
using System.Net.Mail;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Azure.Storage.Queues.Models;
using System.Threading.Tasks;

namespace NotificationService;
public class LoginNotificationEmail
{
    private readonly ILogger<LoginNotificationEmail> _logger;

    public LoginNotificationEmail(ILogger<LoginNotificationEmail> logger)
    {
        _logger = logger;
    }

    [Function("KafkaTriggerFunction")]
    public async Task Run(
        [KafkaTrigger(
                "kafka:9092",
                "after-login-email-topic",
                ConsumerGroup = "function-consumer-group")]
            KafkaMessage message)
    {
        _logger.LogInformation($"Kafka's message received: {message.ToString()}");
        await SendEmailAsync("Login registered: ", message.Value);
    }

    static async Task SendEmailAsync(string message, string toEmail)
    {
        try
        {
            string smtpHost = Environment.GetEnvironmentVariable("smtpHost");
            int smtpPort = Int32.Parse(Environment.GetEnvironmentVariable("smtpPort"));
            using (var client = new SmtpClient(smtpHost, smtpPort))
            {
                client.EnableSsl = true;
                string smtpUsername = Environment.GetEnvironmentVariable("smtpUsername");
                string smtpPassword = Environment.GetEnvironmentVariable("smtpPassword");
                client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("tattoeshop@net-course.pl"),
                    Subject = "Login Alert",
                    Body = message,
                    IsBodyHtml = false
                };
                mailMessage.To.Add(toEmail);

                await client.SendMailAsync(mailMessage);
                Console.WriteLine($"E-mail sent to {toEmail} with message: {message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Caught error: {ex.Message}");
        }
    }
}