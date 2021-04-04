﻿using Data;
using GreenPipes;
using MassTransit;
using MessageContracts;
using System;
using System.Threading.Tasks;

namespace FirstConsumer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(new Uri(Constants.RabbitMqConnectionString));

                cfg.ReceiveEndpoint("first-queue", e =>
                {
                    e.Consumer<FirstEventConsumer>();
                    e.UseMessageRetry(x => x.Exponential(retryLimit: 15,
                        minInterval: TimeSpan.FromSeconds(5),
                        maxInterval: TimeSpan.FromSeconds(10),
                        intervalDelta: TimeSpan.FromSeconds(15)));
                });
            });

            await busControl.StartAsync();
            try
            {
                await Task.Run(() => Console.ReadLine());
            }
            finally
            {
                await busControl.StopAsync();
            }
        }
    }

    public class FirstEventConsumer : IConsumer<PaymentConfirmedEvent>
    {
        public async Task Consume(ConsumeContext<PaymentConfirmedEvent> context)
        {
            var collection = new Connection().GetCollection();
            collection.InsertOne(new($"First Consumer Received message with Id {context.Message.MessageId}"));
            return;
        }
    }
}
