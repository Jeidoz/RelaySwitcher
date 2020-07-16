using System;
using System.Net;
using System.Net.Sockets;

namespace Switcher.FakeRelayListener
{
    internal class Program
    {
        private const int listenPort = 60001;

        private static void StartListener()
        {
            Random random = new Random();
            UdpClient listener = new UdpClient(listenPort);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, listenPort);

            try
            {
                while (true)
                {
                    Console.WriteLine("Waiting for broadcast");
                    byte[] bytes = listener.Receive(ref groupEP);

                    Console.WriteLine($"Received broadcast from {groupEP} at {DateTime.Now.ToLongTimeString()}:");
                    foreach (var b in bytes)
                    {
                        Console.Write($"{b:X2} ");
                    }
                    Console.WriteLine();

                    if (bytes[3] == 0)
                    {
                        listener.Connect(groupEP.Address.ToString(), groupEP.Port);
                        byte[] response = { 0xFF, 0xAA, 00, 00, (byte)random.Next(1, 16) };
                        listener.Send(response, response.Length);
                    }
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                listener.Close();
            }
        }

        public static void Main()
        {
            StartListener();
        }
    }
}