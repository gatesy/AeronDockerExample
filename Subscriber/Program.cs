using System;
using System.Threading;
using System.Threading.Tasks;
using Adaptive.Aeron;
using AeronDockerExample.Protocol.Sbe;
using CommandLine;

namespace Subscriber
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class Options
    {
        [Option('s', "stream-id", Default = 42)]
        public int StreamId { get; set; }
        
        [Option('c', "channel", Default = "aeron:ipc")]
        public string Channel { get; set; }
    }
    
    class Program
    {
        static void OnDataValue(DataValue dataValue)
        {
            var instanceBytes = new byte[DataValue.InstanceLength];
            for (int i = 0; i < DataValue.InstanceLength; ++i)
            {
                instanceBytes[i] = dataValue.GetInstance(i);
            }

            var guid = new Guid(instanceBytes);
            
            Console.Out.WriteLine($"Instance={guid}; Id={dataValue.Id}; Index={dataValue.Index}; Value={dataValue.Value}");
        }

        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(Run);
        }
        
        static void Run(Options options)
        {
            var fragmentCount = 0L;

            try
            {
                using var aeron = Aeron.Connect();
                Console.WriteLine($"Connected to Aeron {aeron.ClientId.ToString()}");
                using var subscription = aeron.AddSubscription(options.Channel, options.StreamId);
                using var cts = new CancellationTokenSource();
                var messageDecoder = new MessageDecoder();
                var messageReceiver = new MessageReceiver(subscription, messageDecoder, OnDataValue);

                var ct = cts.Token;

                var pollingTask = Task.Run(async () =>
                {
                    while (!ct.IsCancellationRequested)
                    {
                        var fragmentsProcessed = messageReceiver.Poll();
                        if (fragmentsProcessed == 0)
                        {
                            await Task.Delay(TimeSpan.FromMilliseconds(10), ct);
                        }
                        else
                        {
                            fragmentCount += fragmentsProcessed;
                        }
                    }
                }, ct);

                Console.In.ReadLine();
                cts.Cancel();
                pollingTask.GetAwaiter().GetResult();
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unhandled Exception: {e}");
            }
            finally
            {
                Console.Out.WriteLine($"Received {fragmentCount} fragments");
                Console.WriteLine("Subscriber finished");
            }
        }
    }
}
