using System;
using System.Threading.Tasks;
using Serilog;

namespace ClientSimulator
{
    internal class Program
    {
        private const string ResourceUrl = "http://localhost:9000/api/v1/device-measurements";

        public static async Task Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: <deviceId> <requestsIntervalMs>");
                return;
            }

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
            try
            {
                var deviceId = Convert.ToInt16(args[0]);
                var interval = Convert.ToInt16(args[1]);

                Log.Information("Will send request to {ResourceUrl} every {interval} ms", ResourceUrl, interval);

                var client = new DeviceMeasurementsClient(ResourceUrl);
                await client.StartSendingRequestsAsync(deviceId, interval);
            }
            catch (Exception e)
            {
                Log.Error(e.GetBaseException().Message);
            }
        }
    }
}