using Confluent.Kafka;

namespace App_practical.Services
{
    public class KafkaProducerService<K, V>
    {
        private readonly IProducer<K, V> _producer;

        public KafkaProducerService(KafkaProducerHandler handler)
        {
            _producer = (IProducer<K, V>)handler.Handle;
        }

        public async Task ProduceAsync(string topic, Message<K, V> message)
        {
            await _producer.ProduceAsync(topic, message);
        }
    }
}