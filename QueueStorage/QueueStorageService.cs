using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using QueueStorage.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueStorage
{
    public class QueueStorageService
    {
        /*
         * Kuyruk üzerinde işlem yapabilmek için client oluşturulur.
         */
        private readonly QueueClient _queueClient;
        public QueueStorageService(string queueName)
        {
            _queueClient = new QueueClient(QueueConnection.ConnectionString, queueName);
            /*
             * Kuyruk yoksa oluşturulur.
             */
            _queueClient.CreateIfNotExistsAsync();
        }
        public async Task SendMessageAsync(string message)
        {
            if (String.IsNullOrEmpty(message))
                throw new ArgumentNullException("Argument Null Exception");
            else
            {
                /*
                 * SendMessageAsync metodu 3 adet parametre alabilir.
                 * message=gönderilen mesaj içeriği
                 * visiilitiyTimeOut=Mesaj okunduktan sonra kuyrukta ne kadar süre görünmez olacağını belirler.
                 * timeToLive=mesajın kuyrukta ki ömrünü belirler
                 * Ömür boyu kalmasını istersen TimeSpan.FromSeconds(-1) vermemiz yeterlidir
                 */
                await _queueClient.SendMessageAsync(message);
            }
        }



        public async Task<QueueMessage> GetMessageAsync()
        {
            /*
             * Kuyruktaki  mesajların count'u alınır
             */
            QueueProperties properties = await _queueClient.GetPropertiesAsync();
            if (properties.ApproximateMessagesCount > 0)
            {
                /*
                 * ReceiveMessagesAsync ile  mesajlar okunur. İlk parametre max kaç adet mesaj okunacağı ikinci parametresi ise mesajın kuyrukta ne kadar süre görünmez olacağı
                 * _queueClient.PeekMessagesAsync(); metodu ile  görünmez olmasın diyebiliriz
                 */
                QueueMessage[] queueMessages = await _queueClient.ReceiveMessagesAsync(1, TimeSpan.FromMinutes(1));

                if (queueMessages.Any())
                {
                    return queueMessages[0];
                }
            }

            return null;
        }

        public async Task DeleteMessage(string messageId, string popReceipt)
        {
            await _queueClient.DeleteMessageAsync(messageId, popReceipt);
        }

    }
}
