using Confluent.Kafka;

namespace App_practical.Services
{
    public class KafkaProducerHandler : IDisposable
    {
        public IProducer<Null, string> Handle { get; }

        public KafkaProducerHandler()
        {
           
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = "kafka:9092"
            };

            Handle = new ProducerBuilder<Null, string>(producerConfig).Build();
        }

        public void Dispose()
        {
            Handle.Flush(TimeSpan.FromSeconds(10));
            Handle.Dispose();
        }
    }
}
