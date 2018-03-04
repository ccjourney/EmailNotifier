using System;
using System.Configuration;
using EmailNotifier.Events;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace EmailNotifier.Sender
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var serviceBusConnectionString = ConfigurationManager.AppSettings["serviceBusConnectionString"];
            var topicName = ConfigurationManager.AppSettings["topic"];
            var subscriptionName = ConfigurationManager.AppSettings["subscription"];

            var nsm = NamespaceManager.CreateFromConnectionString(serviceBusConnectionString);
            if (!nsm.SubscriptionExists(topicName, subscriptionName))
                nsm.CreateSubscription(topicName, subscriptionName);

            var client = SubscriptionClient.CreateFromConnectionString(serviceBusConnectionString, topicName, subscriptionName);

            var options = new OnMessageOptions()
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };

            client.OnMessage(msg =>
            {
                var flaggedUser = msg.GetBody<UserFlaggedForEmail>();
                // We can send the email from here
                Console.WriteLine(flaggedUser.Email);
                msg.Complete();
            }, options);

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
