using System;
using System.Net.Http;
using Microsoft.Owin.Hosting;

namespace DeviceMessagesConsumer
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            string baseAddress = "http://localhost:9000/"; 

            // Start OWIN host 
            WebApp.Start<Startup>(url: baseAddress);
            Console.ReadLine();
        }
    }
}