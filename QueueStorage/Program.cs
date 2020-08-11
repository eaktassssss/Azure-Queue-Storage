using System;
using System.Text;
using System.Threading.Tasks;

namespace QueueStorage
{
    class Program
    {
        static async Task Main(string[] args)
        {
            /*
             * Oluşturulan Queue nesnesine erişiyoruz.
             */
            var queueService = new QueueStorageService("examplequeuemessage");
            /*
             * Gönderilen mesajlarda tr karakter olabileceği için Base64 formatına çeviyoruz
             */
            string message = Convert.ToBase64String(Encoding.UTF8.GetBytes("Bu mesajı kuyruk sistemine yaz"));
            /*
             * Mesaj gönderim metodu çağılır ve oluşturulan mesaj Queue gönderilir
             */
            await queueService.SendMessageAsync(message);


            /*
             * Queue'e yazılmış olan mesajlar okunur.
             */

            var getMessage = await queueService.GetMessageAsync();
            /*
             * FromBase64String mesajı string olarak alır  ve  bize byte bir dizi  döner.
             * GetString metodu dönen byte dizini alır  ve  string olarak döner
             */
            var messageContent = Encoding.UTF8.GetString(Convert.FromBase64String(getMessage.MessageText));
            /*
             * Queueden mesajlar MessageId ve PopReceipt değerine göre silinir
             */
            await queueService.DeleteMessage(getMessage.MessageId, getMessage.PopReceipt);
            Console.ReadLine();
        }
    }
}
