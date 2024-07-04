
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desserts.Domain.Models
{
    public class MessagesModel
    {
        public DateTime DateSent { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public string ReceiverEmail { get; set; }
        public string SenderEmail { get; set; }
        public MessageType Type { get; set; }
    }
    public enum MessageType
    {
        Incoming,
        Outgoing
    }

    public class IncomingMessage : MessagesModel
    {
        public IncomingMessage(string text, DateTime dateSent)
        {
            Text = text;
            Type = MessageType.Incoming;
            DateSent = dateSent;
        }
    }

    public class OutgoingMessage : MessagesModel
    {
        public OutgoingMessage(string text, DateTime dateSent)
        {
            Text = text;
            Type = MessageType.Outgoing;
            DateSent = dateSent;
        }
    }
}
