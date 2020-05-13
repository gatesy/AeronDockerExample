using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Adaptive.Aeron;
using AeronDockerExample.Protocol.Sbe;
using CommandLine;

namespace Subscriber
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
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
            var guid = new Guid(dataValue.Instance);
            
            Console.Out.WriteLine(
                $"Instance={guid}; Id={dataValue.Id}; Index={dataValue.Index}; Value={dataValue.Value}; " +
                $"Timestamp={DateTime.FromBinary((long) dataValue.Timestamp):O}");
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

                
                Console.CancelKeyPress += (o, args) =>
                {
                    args.Cancel = true;
                    Console.Out.WriteLine("Cancel key event intercepted");
                    cts.Cancel();
                };
                
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
