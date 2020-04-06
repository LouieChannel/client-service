using Ascalon.Kafka.Dtos;

namespace Ascalon.ClientService.Kafka
{
    /// <summary>
    /// Информация о конфигурации Consumer в Kafka.
    /// </summary>
    public class DumperStateConsumerServiceOptions
    {
        public KafkaConsumerConfig Config { get; set; }
        public string TopicForProducePostDumper { get; set; }
    }
}
