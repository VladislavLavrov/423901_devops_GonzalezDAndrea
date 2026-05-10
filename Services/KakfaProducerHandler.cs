using Confluent.Kafka;

namespace App_practical.Services
{
    public class KafkaProducerHandler : IDisposable
    {
        public IProducer<Null, string> Handle { get; }

        public KafkaProducerHandler(IConfiguration config)
        {
            var producerConfig = new ProducerConfig();
            config.GetSection("Kafka:ProducerSettings").Bind(producerConfig);
            Handle = new ProducerBuilder<Null, string>(producerConfig).Build();
        }

        public void Dispose()
        {
            Handle.Flush(TimeSpan.FromSeconds(10));
            Handle.Dispose();
        }
    }
}