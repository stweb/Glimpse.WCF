using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Xml;
using Glimpse.Core.Extensibility;

// http://blogs.msdn.com/b/carlosfigueira/archive/2011/04/19/wcf-extensibility-message-inspectors.aspx
// http://www.universalthread.com/ViewPageArticle.aspx?ID=191

namespace Glimpse.Wcf
{
    public class GlimpseWcfClientInspector : IClientMessageInspector
    {
        private static IMessageBroker _messageBroker;
        private static Func<IExecutionTimer> _timerStrategy;

        public static void Initialize(IMessageBroker messageBroker, Func<IExecutionTimer> timerStrategy)
        {
            _messageBroker = messageBroker;
            _timerStrategy = timerStrategy;
        }

        private string ActionStr(string action)
        {
            // http://tempuri.org/ISessionService/BeginSession
            if (action.StartsWith("http://tempuri.org/"))
                action = action.Substring(19);

            return action.Replace('/', '.');
        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            // copy message and re-create
            MessageBuffer buffer = request.CreateBufferedCopy(Int32.MaxValue);
            request = buffer.CreateMessage();

            var entry = new WcfEntry
                {
                    MessageId = request.Headers.MessageId.ToString(),
                    EventName = ActionStr(request.Headers.Action),
                    RequestBody = MessageToString(buffer.CreateMessage()),
                    //EventSubText = "test"
                    //                    ExternalId = externalId
                };

            if (_timerStrategy != null && _messageBroker != null)
            {
                IExecutionTimer timer = _timerStrategy();
                if (timer != null)
                    entry.Timer = timer;
            }

            return entry;
        }

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            var entry = correlationState as WcfEntry;
            if (entry != null)
            {
                entry.CalcDuration();

                // copy message and re-create
                MessageBuffer buffer = reply.CreateBufferedCopy(Int32.MaxValue);
                reply = buffer.CreateMessage();

                entry.ResponseBody = MessageToString(buffer.CreateMessage());
                entry.Cleanup();

                _messageBroker.Publish(entry);
            }
        }

        private string MessageToString(Message message)
        {
            using (var ms = new MemoryStream())
            {
                using (var w = XmlWriter.Create(ms, new XmlWriterSettings
                    {
                        Indent = true,
                        IndentChars = "  ",
                        OmitXmlDeclaration = true,                        
                    }))
                {
                    XmlDictionaryReader bodyReader = message.GetReaderAtBodyContents();
                    while (bodyReader.NodeType != XmlNodeType.EndElement && bodyReader.LocalName != "Body" &&
                           bodyReader.NamespaceURI != "http://schemas.xmlsoap.org/soap/envelope/")
                    {
                        if (bodyReader.NodeType != XmlNodeType.Whitespace)
                        {
                            w.WriteNode(bodyReader, true);
                        }
                        else
                        {
                            bodyReader.Read(); // ignore whitespace; maintain if you want
                        }
                    }
                    w.Flush();

                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
        }
    }
}