using Confluent.Kafka;
using App_practical.Models; 
using System.Text.Json;

namespace App_practical.Services
{
    public class KafkaConsumerService : BackgroundService
    {
        private readonly string _topic;
        private readonly IConsumer<Null, string> _kafkaConsumer;
        private readonly IHttpClientFactory _clientFactory;

        public KafkaConsumerService(IConfiguration config, IHttpClientFactory clientFactory)
        {
            var consumerConfig = new ConsumerConfig();
            config.GetSection("Kafka:ConsumerSettings").Bind(consumerConfig);
            _topic = config.GetValue<string>("Kafka:TopicName");
            _kafkaConsumer = new ConsumerBuilder<Null, string>(consumerConfig).Build();
            _clientFactory = clientFactory;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() => StartConsumerLoop(stoppingToken), stoppingToken);
        }

        private async Task StartConsumerLoop(CancellationToken cancellationToken)
        {
            Console.WriteLine("KafkaConsumerService: Iniciando suscripción al tópico...");
            _kafkaConsumer.Subscribe(_topic);

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
           
                    var cr = _kafkaConsumer.Consume(cancellationToken);

                    if (cr.Message != null)
                    {
                        var inputData = JsonSerializer.Deserialize<Variant>(cr.Message.Value);

                        if (inputData != null)
                        {
                            Console.WriteLine($"KafkaConsumerService: Mensaje recibido. Calculando: {inputData.Value1} {inputData.Operation} {inputData.Value2}");


                            switch (inputData.Operation)
                            {
                                case "Сложить (+)":
                                    inputData.Result = inputData.Value1 + inputData.Value2;
                                    break;
                                case "Вычесть (-)":
                                    inputData.Result = inputData.Value1 - inputData.Value2;
                                    break;
                                case "Умножить (*)":
                                    inputData.Result = inputData.Value1 * inputData.Value2;
                                    break;
                                case "Разделить (/)":
                                    inputData.Result = inputData.Value2 != 0 ? inputData.Value1 / inputData.Value2 : 0;
                                    break;
                                case "Возвести в степень (^)":
                                    inputData.Result = Math.Pow(inputData.Value1, inputData.Value2);
                                    break;
                                default:
                                    inputData.Result = 0;
                                    break;
                            }

                            Console.WriteLine($"KafkaConsumerService: Resultado calculado exitosamente -> {inputData.Result}");

                       
                            var httpClient = _clientFactory.CreateClient();

                          
                            await httpClient.PostAsJsonAsync("http://localhost:5009/Home/Callback", inputData);

                            Console.WriteLine("KafkaConsumerService: Resultado enviado de vuelta al Callback HTTP.");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"KafkaConsumerService ERROR: {e.Message}");
                    break;
                }
            }
        }

        public override void Dispose()
        {
            _kafkaConsumer.Close();
            _kafkaConsumer.Dispose();
            base.Dispose();
        }
    }
}