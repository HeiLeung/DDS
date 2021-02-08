using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetMQ;
using Serilog;

namespace Arrow
{
    public class DDS
    {        
        private NetMQ.Sockets.SubscriberSocket _root;           // the NetMQ socket (sub socket) connecting to the root DDS, subscribe data from publisher (PUB socket), dedicated to DDS connection
        private NetMQ.Sockets.SubscriberSocket _sub;            // the NetMQ socket (sub socket) listening for publisher connection, subscribe data from publisher (PUB socket)
        private NetMQ.Sockets.PublisherSocket _pub;             // the NetMQ socket (X Pub socket) listening for subscriber connection, publish data to subscriber (SUB socket)            
        private NetMQ.Sockets.RouterSocket _router;             // the NetMQ socket (router socket) listening for subscriber connection, reply LVC to client (REQ socket)
        private NetMQ.NetMQPoller _poller;                      // the NetMQ poller to manage NetMQ sockets                        
  
        public string pubAddr = string.Empty;                   // the publisher ip address and port
        public string subAddr = string.Empty;                   // the subscriber ip address and port
        public string rootAddr = string.Empty;                  // the root ip address and port

        private LastValueCache _lvc;
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
            Log.Information("=======================");
            Log.Information("=== DDS initialised ===");
            Log.Information("=======================");
        }

        /// <summary>
        /// Start DDS 
        /// </summary>
        /// <param name="sub">sub socket IP and port object</param>
        /// <param name="pub">pub socket IP and port object</param>
        /// <param name="root">root socket IP and port object</param>
        public void Start(DdsIpPortConnObj sub, DdsIpPortConnObj router, DdsIpPortConnObj pub, DdsIpPortConnObj root = null)
        {
            try
            {
                _lvc = new LastValueCache();

                _sub = new NetMQ.Sockets.SubscriberSocket();
                _sub.Options.ReceiveHighWatermark = 5000;
                _sub.ReceiveReady += Sub_ReceiveReady;

                _pub = new NetMQ.Sockets.PublisherSocket();
                _pub.Options.SendHighWatermark = 5000;
                _pub.Options.XPubVerbose = true;                

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
                Log.Information("Start ::: Error when try to start");
                Log.Information("Start ::: " + ex.ToString());
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

                    // update LVC
                    _lvc.Update(netMqMsg[0].ConvertToString(), netMqMsg[1].Buffer);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Root ::: Error in Root_ReceiveReady");
                Log.Error("Root ::: " + ex.ToString());

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

                    // update LVC
                    _lvc.Update(netMqMsg[0].ConvertToString(), netMqMsg[1].Buffer);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Sub ::: Error in Sub_ReceiveReady");
                Log.Error("Sub ::: " + ex.ToString());
            }
        }     

        /// <summary>
        /// Event handler of router socket receving message, reply with message according to the topic request.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Router_ReceiveReady(object sender, NetMQSocketEventArgs e)
        {
            try
            {
                // expected request frame format :
                // Frame[0] sock id                
                // Frame[1] topic
                NetMQMessage nmqMsgRx = _router.ReceiveMultipartMessage();

                NetMQMessage nmqMsgTx = new NetMQMessage();
                string topic = nmqMsgRx[1].ConvertToString();

                KeyValuePair<string, byte[]>[] messages = _lvc.Get(topic);
                if (messages != null)
                {
                    for (int i = 0; i < messages.Length; i++)
                    {
                        nmqMsgTx.Append(nmqMsgRx[0]);
                        nmqMsgTx.Append(messages[i].Key);
                        nmqMsgTx.Append(messages[i].Value);

                        _router.TrySendMultipartMessage(nmqMsgTx);

                        nmqMsgTx.Clear();
                    }
                }
                else
                {
                    nmqMsgTx.Append(nmqMsgRx[0]);
                    nmqMsgTx.Append(topic);
                    nmqMsgTx.Append("image|" + topic + "|10039|ticker not found|" + '\n');

                    _router.TrySendMultipartMessage(nmqMsgTx);

                    nmqMsgTx.Clear();
                }                 
            }
            catch (Exception ex)
            {
                Log.Error("ROUTER ::: Error occur while receiving message from client");
                Log.Error("ROUTER ::: " + ex.ToString());
            }
        }
    }
}
