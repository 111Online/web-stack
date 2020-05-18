using KafkaNet;
using KafkaNet.Protocol;
using NHS111.Utils.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NHS111.Utils.Kafka
{
    public class KafkaClient : IKafkaClient
    {
        private readonly Producer _producer;

        public KafkaClient(Producer producer)
        {
            _producer = producer;
        }


        public async Task Append(string topic, string[] messages)
        {
            try
            {
                var messagesList = messages.Select(message => new Message(message)).ToList();
                var client = _producer;
                await client.SendMessageAsync(topic, messagesList);
            }

            catch (Exception ex)
            {
                Log4Net.Error(string.Format("ERROR on KafkaClient:  {0} - {1} - {2}", ex.Message, ex.StackTrace, ex.Data));
            }

        }
    }

    public interface IKafkaClient
    {
        Task Append(string topic, string[] messages);
    }
}