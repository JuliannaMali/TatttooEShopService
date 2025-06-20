﻿using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using ShoppingCartInfrastructure.Kafka;

namespace ShoppingCartInfrastructure.Kafka;
public class KafkaProducer : IKafkaProducer
{
    private readonly IProducer<Null, string> _producer;
    private readonly ILogger<KafkaProducer> _logger;

    public KafkaProducer(ILogger<KafkaProducer> logger)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = "kafka:9092"
        };

        _producer = new ProducerBuilder<Null, string>(config).Build();
        _logger = logger;
    }

    public async Task SendMessageAsync(string topic, string message)
    {
        var result = await _producer.ProduceAsync(topic, new Message<Null, string> { Value = message });
        _logger.LogInformation($"Wysłano wiadomość: {message} na temat {topic}");
    }
}

