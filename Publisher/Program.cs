using System;
using System.Threading;
using System.Threading.Tasks;
using Adaptive.Aeron;
using CommandLine;

namespace Publisher
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class Options
    {
        [Option('p', "period")] 
        public TimeSpan SenderPeriod { get; set; } = TimeSpan.FromSeconds(1);
        
        [Option('s', "stream-id", Default = 42)]
        public int StreamId { get; set; }
        
        [Option('c', "channel", Default = "aeron:ipc")]
        public string Channel { get; set; }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(Run);
        }
        
        static void Run(Options options)
        {
            var senderId = 1;
            Console.Out.WriteLine($"Sending messages periodically every {options.SenderPeriod}");
            
            var counter = 0;

            try
            {
                Console.WriteLine("Publisher started");

                using var aeron = Aeron.Connect();
                Console.WriteLine($"Connected to Aeron {aeron.ClientId.ToString()}");

                using var publication = aeron.AddPublication(options.Channel, options.StreamId);
                var encoder = new MessageEncoder(senderId);
                var sender = new MessageSender(publication, encoder);
                
                using var cts = new CancellationTokenSource();
                var ct = cts.Token;

                var offerTask = Task.Run(async () =>
                {
                    while (!ct.IsCancellationRequested)
                    {
                        var sendResult = sender.Send();

                        if (sendResult >= 0)
                        {
                            ++counter;
                        }
                        else
                        {
                            switch (sendResult)
                            {
                                case Publication.NOT_CONNECTED:
                                    Console.Out.WriteLine("Send failed: no one connected");
                                    break;
                                case Publication.BACK_PRESSURED:
                                    Console.Out.WriteLine("Send failed: back pressured");
                                    break;
                                default:
                                    Console.Out.WriteLine($"Send failed: error code -> {sendResult}");
                                    break;
                            }
                        }
                        // Wait for the sender period before trying again.
                        await Task.Delay(options.SenderPeriod, ct);
                    }
                }, ct);

                Console.Out.WriteLine("Press enter key to quit");
                Console.In.ReadLine();

                cts.Cancel();
                offerTask.GetAwaiter().GetResult();
            }
            catch (OperationCanceledException)
            {
                // we were terminated - ignore this.
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unhandled Exception: {e}");
            }
            finally
            {
                Console.Out.WriteLine($"Sent {counter} messages");
                Console.WriteLine("Publisher finished");
            }
        }
    }
}
