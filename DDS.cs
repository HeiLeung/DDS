using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using NetMQ;
using Arrow.Library;

namespace Arrow
{
    public class DDS
    {        
        private NetMQ.Sockets.SubscriberSocket _root;           // the NetMQ socket (sub socket) connecting to the root DDS, subscribe data from publisher (PUB socket), dedicated to DDS connection
        private NetMQ.Sockets.SubscriberSocket _sub;            // the NetMQ socket (sub socket) listening for publisher connection, subscribe data from publisher (PUB socket)
        private NetMQ.Sockets.XPublisherSocket _pub;            // the NetMQ socket (X Pub socket) listening for subscriber connection, publish data to subscriber (SUB socket)            
        private NetMQ.Sockets.RouterSocket _router;             // the NetMQ socket (router socket) listening for subscriber connection, reply LVC to client (REQ socket)
        private NetMQ.NetMQPoller _poller;                      // the NetMQ poller to manage NetMQ sockets                        

        public string pubAddr = string.Empty;                   // the publisher ip address and port
        public string subAddr = string.Empty;                   // the subscriber ip address and port
        public string rootAddr = string.Empty;                  // the root ip address and port

        private ThreadedLogger _logger;           // logger
        private Dictionary<string, Dictionary<string, string>> _lvc;        // the LVC collection           
        private Int64 _msgRxCounter = 0;                        // the counter of message received 

        public Int64 MsgRxCounter
        {
            get { return _msgRxCounter; }          
        }        

        /// <summary>
        /// constructor
        /// </summary>
        public DDS()
        {
            // initialise log, and LVC 
            _logger = new Arrow.Library.ThreadedLogger(Properties.Settings.Default.LogFileName, true, true, "C:\\Arrow\\Log\\");
            _lvc = new Dictionary<string, Dictionary<string, string>>();

            _logger.Log("=======================");
            _logger.Log("=== DDS initialised ===");
            _logger.Log("=======================");
        }

        /// <summary>
        /// Start DDS 
        /// </summary>
        /// <param name="sub">sub socket IP and port object</param>
        /// <param name="pub">pub socket IP and port object</param>
        /// <param name="root">root socket IP and port object</param>
        public void Start(DDSIP_Port sub, DDSIP_Port router, DDSIP_Port pub, DDSIP_Port root = null)
        {
            try
            {
                _sub = new NetMQ.Sockets.SubscriberSocket();
                _sub.Options.ReceiveHighWatermark = 5000;
                _sub.ReceiveReady += Sub_ReceiveReady;

                _pub = new NetMQ.Sockets.XPublisherSocket();
                _pub.Options.SendHighWatermark = 5000;
                _pub.Options.XPubVerbose = true;
                _pub.ReceiveReady += Pub_ReceiveReady;

                _root = new NetMQ.Sockets.SubscriberSocket();
                _root.Options.ReceiveHighWatermark = 5000;
                _root.ReceiveReady += Root_ReceiveReady;

                _router = new NetMQ.Sockets.RouterSocket();
                _router.Options.ReceiveHighWatermark = 100;
                _router.ReceiveReady += Router_ReceiveReady;

                _poller = new NetMQPoller();

                if (root != null)
                {
                    _root.Connect("tcp://" + root.IP + ":" + root.Port);
                    _root.SubscribeToAnyTopic();
                    _poller.Add(_root);
                }

                _sub.Bind("tcp://" + sub.IP + ":" + sub.Port);
                _sub.SubscribeToAnyTopic();

                _pub.Bind("tcp://" + pub.IP + ":" + pub.Port);

                _router.Bind("tcp://" + router.IP + ":" + router.Port);

                _poller.Add(_sub);
                _poller.Add(_pub);
                _poller.Add(_router);
                _poller.Run();
            }
            catch (Exception ex)
            {
                _logger.Log("Start ::: Error when try to start");
                _logger.Log("Start ::: " + ex.ToString());
            }
        }

        /// <summary>
        /// Stop DDS
        /// </summary>
        public void Stop()
        {
            _poller.Stop();
        }

        /// <summary>
        /// Event handler of root receiving message, publish (forward) message by using pub socket
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Root_ReceiveReady(object sender, NetMQSocketEventArgs e)
        {
            try
            {
                NetMQMessage netMqMsg = null;

                while (e.Socket.TryReceiveMultipartMessage(ref netMqMsg, 2))
                {
                    _pub.TrySendMultipartMessage(netMqMsg);
                    _msgRxCounter++;

                    Process_LVC(netMqMsg[0].ConvertToString(), netMqMsg[1].ConvertToString());
                }
            }
            catch (Exception ex)
            {
                _logger.Log("Root ::: Error in Root_ReceiveReady");
                _logger.Log("Root ::: " + ex.ToString());

            }
        }

        /// <summary>
        /// Event handler of sub socket receiving message, publish (forward) message by using pub socket
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Sub_ReceiveReady(object sender, NetMQSocketEventArgs e)
        {
            try
            {
                NetMQMessage netMqMsg = null;
                while (e.Socket.TryReceiveMultipartMessage(ref netMqMsg, 2))
                {
                    _pub.TrySendMultipartMessage(netMqMsg);
                    _msgRxCounter++;

                    Process_LVC(netMqMsg[0].ConvertToString(), netMqMsg[1].ConvertToString());
                }
            }
            catch (Exception ex)
            {
                _logger.Log("Sub ::: Error in Sub_ReceiveReady");
                _logger.Log("Sub ::: " + ex.ToString());
            }
        }

        /// <summary>
        /// Event handler of pub socket receiving subcription message, publish LVC
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pub_ReceiveReady(object sender, NetMQSocketEventArgs e)
        {
            try
            {
                Byte[] byteRx;

                while (e.Socket.TryReceiveFrameBytes(out byteRx))
                {
                    byte firstByte = byteRx[0];
                    string topic = ASCIIEncoding.ASCII.GetString(byteRx, 1, byteRx.Count() - 1);

                    // Byte array received is
                    // first byte -> 0=unsub or 1=sub
                    // the following bytes -> topic 
                    if (firstByte == 0x01)
                    {
                        if (topic != string.Empty)
                        {
                            //Console.WriteLine(topic);
                            //e.Socket.TrySendMultipartMessage(new TimeSpan(10000), GetLVC(topic));

                            /*
                            List<NetMQMessage> _nmqMsgList = GetLVC(topic);

                            foreach (NetMQMessage nmqMsg in _nmqMsgList)
                            {
                                e.Socket.TrySendMultipartMessage(new TimeSpan(10000), nmqMsg);
                                //_pub.TrySendMultipartMessage(new TimeSpan(10000), nmqMsg);
                            }*/
                        }
                    }  /*              
                else if (firstByte == 0x00)
                {
                    if (topic.EndsWith(".*"))
                    {
                        List<string> subTopicList = _lvc.Keys.Where(key => key.StartsWith(topic.TrimEnd('*'))).ToList();

                        foreach (string subTopic in subTopicList)
                        {
                            ((NetMQ.Sockets.SubscriberSocket)e.Socket).Unsubscribe(subTopic);                            
                        }
                    }
                }*/
                }
            }
            catch (Exception ex)
            {
                _logger.Log("Pub ::: Error in Pub_ReceiveReady");
                _logger.Log("Pub ::: " + ex.ToString());
            }
        }

        private void Router_ReceiveReady(object sender, NetMQSocketEventArgs e)
        {
            try
            {
                // expected request frame format :
                // Frame[0] sock id
                // Frame[1] 
                // Frame[2] ticker
                // Frame[3] topic (optional)

                NetMQMessage nmqMsgRx = _router.ReceiveMultipartMessage();                        

                /*
                for (int i = 1; i < nmqMsgRx.FrameCount; i++)
                {
                    Console.WriteLine("Frame F[" + i + "] " + nmqMsgRx[i].ConvertToString());
                }
                */
                if (nmqMsgRx.FrameCount == 3)       // only ticker given, return full image
                {
                    string ticker = nmqMsgRx[2].ConvertToString();                    

                    List<NetMQMessage> _nmqMsgList = GetLVC(ticker);
                    NetMQMessage nmqMsgTx = nmqMsgRx;
                    
                    foreach (NetMQMessage nmqMsg in _nmqMsgList)
                    {
                        //nmqMsgTx.Append("image");
                        nmqMsgTx.Append(nmqMsg[1].ConvertToString());                        
                    }

                    _router.SendMultipartMessage(nmqMsgTx);                    
                }
                else if (nmqMsgRx.FrameCount == 4)     // both ticker and topic given, return the topic value only
                {
                    string ticker = nmqMsgRx[2].ConvertToString();
                    string topic = nmqMsgRx[3].ConvertToString();

                    List<string> tickerLVCList = GetLVC_topicValuePair(ticker, topic);
                    NetMQMessage nmqMsgTx = nmqMsgRx;
                    nmqMsgTx.RemoveFrame(nmqMsgTx[nmqMsgTx.FrameCount - 1]);
                    foreach (string LVC in tickerLVCList)
                    {
                        nmqMsgTx.Append(LVC);
                    }
                    _router.SendMultipartMessage(nmqMsgTx);
                }
                else                    // incorrect format received
                {
                    NetMQMessage nmqMsgTx = nmqMsgRx;

                    nmqMsgTx.Append("Unknown request!");
                    _router.SendMultipartMessage(nmqMsgTx);
                }
            }
            catch (Exception ex)
            {
                _logger.Log("ROUTER ::: Error occur while receiving message from client");
                _logger.Log("ROUTER ::: " + ex.ToString());
            }
        }

        /// <summary>
        /// Process LVC, deserialise message into key value pair, update if record found, add if not exists.
        /// </summary>
        /// <param name="topic">the topic of the message</param>
        /// <param name="message">message payload</param>
        private void Process_LVC(string topic, string message)
        {
            try
            {
                string theTopic = topic;
                string theMsg = message;

                Dictionary<string, string> LVCDict;
                string tempValue;

                Dictionary<string, string> DDSMsgDict = Utilites.ConvertToDictionary(theMsg);

                // updating the LVC store
                if (_lvc.TryGetValue(theTopic, out LVCDict))
                {
                    foreach (KeyValuePair<string, string> kvp in DDSMsgDict)
                    {
                        if (LVCDict.TryGetValue(kvp.Key, out tempValue))
                        {
                            LVCDict[kvp.Key] = kvp.Value;
                        }
                        else
                        {
                            LVCDict.Add(kvp.Key, kvp.Value);
                        }
                    }
                }
                else
                {
                    _lvc.Add(theTopic, DDSMsgDict);
                }
            }
            catch (Exception ex)
            {
                _logger.Log("LVC ::: Error in Process_LVC");
                _logger.Log("LVC ::: " + ex.ToString());
            }
        }                               
          
        /// <summary>
        /// Return LVC message string in terms of NetMqMessage format. Serialise key value pair to a single string with delimiter '|'
        /// </summary>
        /// <param name="ticker"></param>
        /// <returns></returns>
        private List<NetMQMessage> GetLVC(string ticker)
        {
            List<NetMQMessage> nmqMsgList = new List<NetMQMessage>();
            Dictionary<string, string> tempDict;
            
            if (ticker.EndsWith("."))           
            {
                List<string> subTopicList = _lvc.Keys.Where(key => key.StartsWith(ticker.TrimEnd('.'))).ToList();

                if (subTopicList.Count >= 1)
                {
                    foreach (string subTopic in subTopicList)
                    {
                        NetMQMessage nmqMsg = new NetMQMessage();

                        nmqMsg.Append(subTopic);
                        nmqMsg.Append(DictToString(_lvc[subTopic], subTopic));

                        nmqMsgList.Add(nmqMsg);
                    }
                }
                else
                {
                    NetMQMessage nmqMsg = new NetMQMessage();

                    nmqMsg.Append(ticker);
                    nmqMsg.Append("image|" + ticker + "|10039|ticker not found|" + '\n');

                    nmqMsgList.Add(nmqMsg);
                }
            }
            else
            {
                if (_lvc.TryGetValue(ticker, out tempDict))
                {
                    NetMQMessage nmqMsg = new NetMQMessage();

                    nmqMsg.Append(ticker);
                    nmqMsg.Append(DictToString(tempDict, ticker));

                    nmqMsgList.Add(nmqMsg);
                }
                else
                {
                    NetMQMessage nmqMsg = new NetMQMessage();

                    nmqMsg.Append(ticker);
                    nmqMsg.Append("image|" + ticker + "|10039|ticker not found|" + '\n');

                    nmqMsgList.Add(nmqMsg);
                }
            }            

            return nmqMsgList;            
        }

        /// <summary>
        /// Return LVC message string in terms of NetMqMessage format. Serialise key value pair to a single string with delimiter '|'
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        private List<string> GetLVC_topicValuePair(string ticker, string topic)
        {
            List<string> result = new List<string>();
            List<NetMQMessage> nmqMsgList = new List<NetMQMessage>();
            Dictionary<string, string> tempDict;
            string topicValue = "Null";

            if (ticker.EndsWith("."))
            {                
                List<string> tickerList = _lvc.Keys.Where(key => key.StartsWith(ticker.TrimEnd('.'))).ToList();

                if (tickerList.Count > 0)
                {
                    foreach (string theTicker in tickerList)
                    {
                        _lvc[theTicker].TryGetValue(topic, out topicValue);

                        //result.Add(theTicker + "|" + topicValue);    
                        result.Add("image|" + theTicker + "|" + topic + "|" + topicValue + "|");
                    }
                }
                else
                {
                    result.Add("image|" + ticker + "|10039|ticker not found|" + '\n');
                }                                               
            }
            else
            {
                if (_lvc.TryGetValue(ticker, out tempDict))
                {
                    _lvc[ticker].TryGetValue(topic, out topicValue);

                    //result.Add(ticker + "|" + topicValue);
                    result.Add("image|" + ticker + "|" + topic + "|" + topicValue + "|");
                    
                }
                else
                {

                    result.Add("image|" + ticker + "|10039|ticker not found|" + '\n');                                        
                }
            }

            return result;
        }

        /// <summary>
        /// Return LVC message string
        /// </summary>
        /// <param name="Dict"></param>
        /// <param name="topic"></param>
        /// <returns></returns>
        private string DictToString(Dictionary<string, string> Dict, string topic)
        {
            string format = string.Empty;
            format = String.IsNullOrEmpty(format) ? "{0}|{1}|" : format;
            StringBuilder itemString = new StringBuilder();            

            if (Dict.ContainsKey("update"))
            {
                Dict.Remove("update");
            }
            if (Dict.ContainsKey("image"))
            {
                Dict.Remove("image");
            }

            foreach (var item in Dict)
            {
                itemString.AppendFormat(format, item.Key, item.Value);
            }

            return "image|" + topic + "|" + itemString + '\n';                                    
        }                
    }

    public class DDSIP_Port
    {
        public string IP;
        public int Port;
        public int HighWaterMark;

        public DDSIP_Port(string IP_addr, int port_num, int highWaterMark = 1000)
        {
            IP = IP_addr;
            Port = port_num;
            HighWaterMark = highWaterMark;
        }
    }
}
