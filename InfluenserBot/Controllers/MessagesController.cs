using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Utilities;
using Newtonsoft.Json;

namespace InfluenserBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<Message> Post([FromBody]Message message)
        {
            if (message.Type == "Message")
            {
                if (message.Text.ToLower().Contains("hello"))
                {
                    return message.CreateReplyMessage("Hello Grand Master :)");
                }
                else if (message.Text.ToLower().Contains("hi"))
                {
                    return message.CreateReplyMessage("Hi man! :)");
                }
                else if (message.Text.ToLower().Contains("hey"))
                {
                    return message.CreateReplyMessage("Yaa!");
                }
                else if 
                    (
                        message.Text.ToLower().Contains("influencer") ||
                        message.Text.ToLower().Contains("person") ||
                        message.Text.ToLower().Contains("people") ||
                        message.Text.ToLower().Contains("human") ||
                        message.Text.ToLower().Contains("man") ||
                        message.Text.ToLower().Contains("developer") ||
                        message.Text.ToLower().Contains("tester") ||
                        message.Text.ToLower().Contains("hero") ||
                        message.Text.ToLower().Contains("administrator") ||
                        message.Text.ToLower().Contains("mvp")
                    )
                {
                    //Trying to find a key word
                    string person = Helpers.Analytics.getInfluencer(message.Text);
                    if(!string.IsNullOrEmpty(person))
                    {
                        return message.CreateReplyMessage(string.Format("It can be a {0}", person));
                    }
                    else
                    {
                        return message.CreateReplyMessage("No influencer found :(");
                    }
                }
                else
                {
                    for (int i = 0; i < Helpers.Data.Influencers.Length; i++)
                    {
                        if(message.Text.ToLower().Trim().Contains(Helpers.Data.Influencers[i].ToLower()))
                        {
                            string exp = Helpers.Analytics.getInfluencerExperience(Helpers.Data.Influencers[i]);
                            if(!string.IsNullOrEmpty(exp))
                            {
                                return message.CreateReplyMessage(string.Format("{0} in mostly experienced in {1}", Helpers.Data.Influencers[i], exp));
                            }
                            else
                            {
                                return message.CreateReplyMessage(string.Format("{0} in not experienced in any strategic technologies. May be, he is not an inbluenser? :)", Helpers.Data.Influencers[i]));
                            }
                        }
                    }

                    Message m = HandleSystemMessage(message);

                    if (m == null)
                    {
                        return message.CreateReplyMessage("Sorry, I'm a stupid bot. Can not undestand your request");
                    }
                    else
                    {
                        return m;
                    }
                    
                }
            }
            else
            {
                return HandleSystemMessage(message);
            }
        }

        private Message HandleSystemMessage(Message message)
        {
            if (message.Type == "Ping")
            {
                Message reply = message.CreateReplyMessage();
                reply.Type = "Ping";
                return reply;
            }
            else if (message.Type == "DeleteUserData")
            {
                Message reply = message.CreateReplyMessage();
                reply.Type = "Ping";
                return reply;
            }
            else if (message.Type == "BotAddedToConversation")
            {
                Message reply = message.CreateReplyMessage();
                reply.Type = "Ping";
                return reply;
            }
            else if (message.Type == "BotRemovedFromConversation")
            {
                Message reply = message.CreateReplyMessage();
                reply.Type = "Ping";
                return reply;
            }
            else if (message.Type == "UserAddedToConversation")
            {
                Message reply = message.CreateReplyMessage();
                reply.Type = "Ping";
                return reply;
            }
            else if (message.Type == "UserRemovedFromConversation")
            {
                Message reply = message.CreateReplyMessage();
                reply.Type = "Ping";
                return reply;
            }
            else if (message.Type == "EndOfConversation")
            {
                Message reply = message.CreateReplyMessage();
                reply.Type = "Ping";
                return reply;
            }

            return null;
        }
    }
}