using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Net.Mail;
using System.Net;
using NotificationService;

namespace CheckoutNotificationService;

public class CheckoutNotificationEmail
{
    private readonly ILogger _logger;

    public CheckoutNotificationEmail(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<CheckoutNotificationEmail>();
    }

    [Function("CheckoutNotificationEmail")]
    public async Task Run(
        [KafkaTrigger("kafka:9092", "checkout-topic", ConsumerGroup = "function-consumer-group")]
        KafkaMessage kafkaEvent)
    {
        _logger.LogInformation($"Message received from checkout-topic: {kafkaEvent.Value}");

        var email = "test@receiver.pl";

        await SendEmailAsync("Order:", kafkaEvent.Value, email);
    }

    static async Task SendEmailAsync(string subject, string message, string toEmail)
    {
        try
        {
            string smtpHost = Environment.GetEnvironmentVariable("smtpHost");
            int smtpPort = int.Parse(Environment.GetEnvironmentVariable("smtpPort"));
            using var client = new SmtpClient(smtpHost, smtpPort)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(
                    Environment.GetEnvironmentVariable("smtpUsername"),
                    Environment.GetEnvironmentVariable("smtpPassword"))
            };

            var mailMessage = new MailMessage("test@gmail.pl", toEmail, subject, message)
            {
                IsBodyHtml = false
            };

            await client.SendMailAsync(mailMessage);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in sending email: {ex.Message}");
        }
    }
}
