using Data;
using Data.Entities;
using MassTransit;
using MessageContracts;
using System;
using System.Threading.Tasks;

namespace Publisher
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(new Uri(Constants.RabbitMqConnectionString));
            });

            await busControl.StartAsync();

            try
            {
                var collection = new Connection().GetCollection();
                int i = 0;
                while (i != 1000)
                {
                    Guid messageId = Guid.NewGuid();
                    await busControl.Publish<PaymentConfirmedEvent>(new
                    {
                        MessageId = messageId
                    });

                    Log doc = new($"Published message with Id {messageId}");
                    collection.InsertOne(doc);
                    i++;
                }
            }
            finally
            {
                await busControl.StopAsync();
            }
        }
    }
}
