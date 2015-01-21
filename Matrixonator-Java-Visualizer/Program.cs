using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net.Sockets;
using System.Threading;

namespace Matrixonator_Java_Visualizer
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket sk = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            while (sk.Connected == false)
            {
                Console.Write("Connecting...");
                try
                {
                    sk.Connect("127.0.0.1", 9876);
                }
                catch { Console.WriteLine("NOT FOUND"); }

                Thread.Sleep(100);
            }

            Console.WriteLine("Connected!");
            Console.WriteLine("----------\n");

            byte[] bytes = new byte[4];
            sbyte PacketOrder = 0;

            while (sk.Connected == true)
            {
                try
                {
                    int i = sk.Receive(bytes);

                    if (bytes[0] == PacketOrder) { Console.WriteLine("Packet Recieved"); }
                    else { Console.WriteLine("Invalid Packet recieved! Expected: {0}, Actual: {1}", PacketOrder, bytes[0]); }

                    string Data = Encoding.UTF8.GetString(bytes);
                    if ((Data == "") || (Data == null)) { Data = "NULL"; }
                    Console.WriteLine(Data);

                    if (PacketOrder + 1 > 3) { PacketOrder = 0; }
                    else { PacketOrder++; }
                }
                catch (SocketException e)
                {
                    //Console.WriteLine("{0} Error code: {1}.", e.Message, e.ErrorCode);
                }
            }

            Console.WriteLine("Connection was closed.");
            Console.ReadKey();
        }
    }
}
