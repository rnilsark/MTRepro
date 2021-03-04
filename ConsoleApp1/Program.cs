using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Azure.ServiceBus.Core;
using MassTransit.Azure.ServiceBus.Core.Testing;
using MassTransit.Azure.ServiceBus.Core.Topology;
using MassTransit.Testing;

namespace ConsoleApp1
{
    class TestConsumer : IConsumer<MyMessage>
    {
        public Task Consume(ConsumeContext<MyMessage> context) => Task.CompletedTask;
    }
    class TestFaultConsumer : IConsumer<Fault<MyMessage>>
    {
        public Task Consume(ConsumeContext<Fault<MyMessage>> context) => Task.CompletedTask;
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(InterfaceHelper.CountInterfaces(typeof(MyMessage)));
            ThisShouldAlsoCrash().GetAwaiter().GetResult();
        }

        private static async Task ThisShouldAlsoCrash()
        {
            using var harness = new AzureServiceBusTestHarness(
                new Uri(""),
                "",
                "");
            harness.Consumer<TestConsumer>();
            harness.Consumer<TestFaultConsumer>();

            harness.OnConfigureServiceBusBus += Harness_OnConfigureServiceBusBus<MyMessage>;
            await harness.Start();

            await harness.Bus.Publish(new MyMessage());
            while (!(await harness.Consumed.Any<MyMessage>()))
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
            await harness.Stop();
        }

        private static void Harness_OnConfigureServiceBusBus<T>(IServiceBusBusFactoryConfigurator configurator) where T : ITheEnd
        {
            foreach (var t in GetEventAbstractionsFromAssembly<T>()) InvokeConfigurePublish(configurator, t);
        }
        
        public static IEnumerable<Type> GetEventAbstractionsFromAssembly<T>() where T: ITheEnd
        {
            var type = typeof(ITheEnd);
            var eventsInAssembly = typeof(T).Assembly.GetTypes().Where(t => type.IsAssignableFrom(t));
            var excludeEventTypes = eventsInAssembly.Where(t => t.IsInterface || t.IsAbstract).ToArray();
            return excludeEventTypes;
        }

        private static void InvokeConfigurePublish(IBusFactoryConfigurator configurator, params Type[] types)
        {
            void ConfigureTopology(IServiceBusMessagePublishTopologyConfigurator x) => x.Exclude = true;

            var method = typeof(IServiceBusBusFactoryConfigurator).GetMethod(nameof(IServiceBusBusFactoryConfigurator.Publish));

            foreach (var type in types)
            {
                var generic = method.MakeGenericMethod(type);
                generic.Invoke(configurator, new object[]
                {
                    (Action<IServiceBusMessagePublishTopologyConfigurator>) ConfigureTopology
                });

                var genericFault = method.MakeGenericMethod(typeof(Fault<>).MakeGenericType(type));
                genericFault.Invoke(configurator, new object[]
                {
                    (Action<IServiceBusMessagePublishTopologyConfigurator>) ConfigureTopology
                });
            }
        }
    }
}
