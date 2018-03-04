using System.Threading.Tasks;
using EmailNotifier.Events;
using Microsoft.ServiceBus.Messaging;

namespace EmailNotifier.Publisher
{
    public class EmailPublisher : IEmailPublisher
    {
        private readonly string _serviceBusConnectionString;
        private readonly string _topicPath;

        public EmailPublisher(string serviceBusConnectionString, string topic)
        {
            _topicPath = topic;
            _serviceBusConnectionString = serviceBusConnectionString;
        }

        public async Task SendEmail(string email)
        {
            var topicClient = TopicClient.CreateFromConnectionString(_serviceBusConnectionString, _topicPath);
            var msg = new BrokeredMessage(new UserFlaggedForEmail { Email = email });
            await topicClient.SendAsync(msg);
        }
    }
}