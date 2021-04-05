using System;
using System.Configuration;
using System.Net.Http;
using Autofac;
using Microsoft.Owin.Hosting;
using Serilog;

namespace DeviceMessagesConsumer
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
            
            // Start OWIN host 
            string baseAddress = ConfigurationManager.AppSettings["baseAddress"];
            WebApp.Start<Startup>(url: baseAddress);
            Log.Information($"Started on {baseAddress}");
            
            Console.ReadLine();
        }
    }
}