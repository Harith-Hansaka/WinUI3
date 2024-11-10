using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace UNDAI.MODELS.MASTER
{
    public class SendDataMaster
    {
        string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MasterSend.txt");

        public SendDataMaster(TcpClient client, string text, bool isLongPressed) 
        {
            if (client != null)
            {
                bool longPress = isLongPressed;
                if (longPress) { HandleLongPress(client, text); }
                else { HandleShortPress(client, text); }
                using (StreamWriter writer = new StreamWriter(_filePath, true, Encoding.UTF8))
                {
                    writer.WriteLine(text); // This writes the response on a new line
                }
            }            
        }

        public async void HandleShortPress(TcpClient client, string text)
        {
            var messageBytes = Encoding.UTF8.GetBytes(text);
            await client.GetStream().WriteAsync(messageBytes, 0, messageBytes.Length);

        }

        public async void HandleLongPress(TcpClient client, string text)
        {
            var messageBytes = Encoding.UTF8.GetBytes(text);
            await client.GetStream().WriteAsync(messageBytes, 0, messageBytes.Length);
        }
    }
}