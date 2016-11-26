using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Helpers
{
    public class Message
    {
        public string Influenser { get; set; }
        public MessageType Type { get; set; }
        public string Content { get; set; }

        public Message() { }

        public Message(string influenser, MessageType type, string content)
        {
            Influenser = influenser;
            Type = type;
            Content = content;
        }

        public Message(string serializedMessage)
        {
            Message MS = deserialize(serializedMessage);
            this.Content = MS.Content;
            this.Influenser = MS.Influenser;
            this.Type = MS.Type;
        }

        public string serialize()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Message));
                string result = string.Empty;
                using (StringWriter SW = new StringWriter())
                {
                    serializer.Serialize(SW, this);
                    result = SW.ToString();
                }

                return result;
            }
            catch
            {
                return null;
            }
        }

        public Message deserialize(string message)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Message));
                Message result = (Message)serializer.Deserialize(new StringReader(message));
                return result;
            }
            catch
            {
                return null;
            }
        }

        public enum MessageType
        {
            Tweet, Retweet
        }
    }

    //public static class StorageOperations
    //{
    //    static string connectionString = "DefaultEndpointsProtocol=https;AccountName=mlhol;AccountKey=jqWjZrsZWWgbbumEmpktZPlhCxyq13YItTgyxYxvzMz8LlSLtFWU7FB1guAMvafO1DfHwwkJO+BwXRqy8WRRZg==";
    //    static CloudStorageAccount storageAccount;
    //    static CloudQueueClient queueClient;

    //    static CloudQueue tweet;
    //    static CloudQueue retweet;

    //    static bool IsInitialized = false;

    //    public static void AddMessageToQueue(Message message)
    //    {
    //        Initialize();
    //        switch (message.Type)
    //        {
    //            case Message.MessageType.Tweet:
    //                tweet.AddMessage(new CloudQueueMessage(message.serialize()));
    //                break;
    //            case Message.MessageType.Retweet:
    //                retweet.AddMessage(new CloudQueueMessage(message.serialize()));
    //                break;
    //            default:
    //                break;
    //        }
    //    }

    //    public static Message GetMessageFromQueue(Message.MessageType type)
    //    {
    //        Initialize();

    //        return type == Message.MessageType.Tweet ? 
    //            new Message(tweet.GetMessage().AsString) : 
    //            new Message(retweet.GetMessage().AsString);
    //    }

    //    static void Initialize()
    //    {
    //        if(!IsInitialized)
    //        {
    //            storageAccount = CloudStorageAccount.Parse(connectionString);
    //            queueClient = storageAccount.CreateCloudQueueClient();
    //            tweet = queueClient.GetQueueReference("tweet");
    //            tweet.CreateIfNotExists();
    //            retweet = queueClient.GetQueueReference("retweet");
    //            retweet.CreateIfNotExists();
    //            IsInitialized = true;
    //        }
    //    }
    //}

    public static class Analytics
    {
        static SqlConnection dbConnection = new SqlConnection("Server=tcp:servicefabric-db-server.database.windows.net,1433;Data Source=servicefabric-db-server.database.windows.net;Initial Catalog=servicefabric-db;Persist Security Info=False;User ID=prague;Password=DXreadiness16;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        public static void analyseMessage(Message message)
        {
            //Check an influenser
            bool IsInfluenserValid = false;
            for (int x = 0; x < Data.Influencers.Length; x++)
            {
                if(message.Influenser.ToLower().Trim().Contains(Data.Influencers[x].ToLower().Trim()))
                {
                    IsInfluenserValid = true;
                }
            }

            if(IsInfluenserValid)
            {
                //Check a key words in a mesage
                for (int i = 0; i < Data.Keywords.Length; i++)
                {
                    if (message.Content.ToLower().Contains(Data.Keywords[i].ToLower()))
                    {
                        //Trying to find a keyword in DB
                        if (dbConnection.State != ConnectionState.Open)
                        {
                            dbConnection.Open();
                        }

                        //Update DB with retries
                        for (int j = 0; j < 2; j++)
                        {
                            try { UpdateDB(message, i); j = 1000; }
                            catch
                            {
                                try
                                {
                                    //reopen connction
                                    dbConnection.Close();
                                    dbConnection.Open();
                                }
                                catch { }
                            }
                        }
                    }
                }
            }
        }

        public static string getInfluencer(string message)
        {
            for (int i = 0; i < Data.Keywords.Length; i++)
            {
                if (message.ToLower().Contains(Data.Keywords[i].ToLower()))
                {
                    //Trying to find a keyword in DB
                    if (dbConnection.State != ConnectionState.Open)
                    {
                        dbConnection.Open();
                    }

                    string sqlCommand = string.Format("select top 1 [influenser] from counters where [key_word] = '{0}' order by [kw_counter] desc", Data.Keywords[i]);

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand, dbConnection);

                    DataSet ds_counter = new DataSet();
                    adapter.Fill(ds_counter, "counter");

                    DataTable DT = ds_counter.Tables[0];

                    if (DT.Rows.Count > 0)
                    {
                        return DT.Rows[0][0].ToString();
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            return null;
        }

        public static string getInfluencerExperience(string person)
        {
            //Trying to find a keyword in DB
            if (dbConnection.State != ConnectionState.Open)
            {
                dbConnection.Open();
            }

            string sqlCommand = string.Format("select top 1 [key_word] from counters where [influenser] = '{0}' order by [kw_counter] desc", person);

            SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand, dbConnection);

            DataSet ds_counter = new DataSet();
            adapter.Fill(ds_counter, "counter");

            DataTable DT = ds_counter.Tables[0];

            if (DT.Rows.Count > 0)
            {
                return DT.Rows[0][0].ToString();
            }
            else
            {
                return null;
            }
        }

        static void UpdateDB(Message message, int i)
        {
            string sqlCommand = string.Format("select [kw_counter] from counters where [influenser] = '{0}' and [tweet_type] = '{1}' and [key_word] = '{2}'", message.Influenser, (message.Type == Message.MessageType.Tweet ? "T" : "R"), Data.Keywords[i]);
            SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand, dbConnection);

            DataSet ds_counter = new DataSet();
            adapter.Fill(ds_counter, "counter");

            DataTable DT = ds_counter.Tables[0];

            if (DT.Rows.Count == 0)
            {
                string insertSqlCommand = string.Format("insert into [counters] ([influenser], [tweet_type], [key_word], [kw_counter]) values ('{0}', '{1}', '{2}', 1)", message.Influenser, (message.Type == Message.MessageType.Tweet ? "T" : "R"), Data.Keywords[i]);

                SqlCommand insertCommand = new SqlCommand(insertSqlCommand, dbConnection);
                insertCommand.ExecuteNonQuery();
            }
            else
            {
                int newCounter = int.Parse(DT.Rows[0][0].ToString()) + 1;
                string updateSqlCommand = string.Format("update [counters] set [kw_counter] = {3} where [influenser] = '{0}' and [tweet_type] = '{1}' and [key_word] = '{2}'", message.Influenser, (message.Type == Message.MessageType.Tweet ? "T" : "R"), Data.Keywords[i], newCounter);
                SqlCommand updateCommand = new SqlCommand(updateSqlCommand, dbConnection);
                updateCommand.ExecuteNonQuery();
            }
        }

        public static List<Counter> counters { get; set; }
        public class Counter
        {
            public string influenser { get; set; }
            public Message.MessageType type { get; set; }
            public string keyWord { get; set; }
            public int counter { get; set; }
        }
    }
}
