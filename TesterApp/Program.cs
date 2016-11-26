using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TesterApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //for (int i = 0; i < 10; i++)
            //{
            //    Helpers.Analytics.analyseMessage(new Helpers.Message { Influenser = "carlosazaustre", Type = Helpers.Message.MessageType.Tweet, Content = "I love iot" });
            //}

            //for (int i = 0; i < 3; i++)
            //{
            //    Helpers.Analytics.analyseMessage(new Helpers.Message { Influenser = "carlosazaustre", Type = Helpers.Message.MessageType.Tweet, Content = "I love devops" });
            //}

            //for (int i = 0; i < 1; i++)
            //{
            //    Helpers.Analytics.analyseMessage(new Helpers.Message { Influenser = "carlosazaustre", Type = Helpers.Message.MessageType.Tweet, Content = "I love cloud computing" });
            //}

            //string person = Helpers.Analytics.getInfluencer("I need a person experiensed in cloud computing");

            string exp = Helpers.Analytics.getInfluencerExperience("carlosazaustre");

            //Helpers.Message mes = new Helpers.Message("David", Helpers.Message.MessageType.Tweet, "Hello from David tweet");
            //Helpers.StorageOperations.AddMessageToQueue(mes);
            //Thread.Sleep(1000);

            //Helpers.Message mes1 = Helpers.StorageOperations.GetMessageFromQueue(Helpers.Message.MessageType.Tweet);
            //Thread.Sleep(1000);

        }
    }
}
