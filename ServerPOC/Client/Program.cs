using System.Net.Sockets;
using System.Text;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            using (TcpClient client = new TcpClient("127.0.0.1", 4040))
            {
                SendMessage(client, "Hello\r\nThis is line two\r\nAnd line three\r\n");
                string Line4 = "Line Four\r\n";
                SendMessage(client, "Line 5\r\nLine 6");
                SendMessage(client, "\r\n");

                foreach(var character in Line4)
                {
                    SendMessage(client, character.ToString());
                }
            }
        }

        static void SendMessage(TcpClient client, string messageToSend)
        {
            var buffer = Encoding.ASCII.GetBytes(messageToSend);
            client.GetStream().Write(buffer, 0, buffer.Length);
        }
    }
}
