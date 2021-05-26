using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using Serilog;

namespace Arrow
{
    public class LastValueCache 
    {
        private ConcurrentDictionary<string, Dictionary<int, byte[]>> _lvc;       // the LVC collection, value in byte

        /// <summary>
        /// 
        /// </summary>
        public LastValueCache()
        {
            _lvc = new ConcurrentDictionary<string, Dictionary<int, byte[]>>();
        }

        /// <summary>
        /// Update last value cache
        /// </summary>
        /// <param name="topic">message topic</param>
        /// <param name="message">message in byte[]</param>
        public void Update(string topic, byte[] message)
        {
            try
            {
                Dictionary<int, byte[]> DDSMsgDict = Utilites.ConvertToDictionary(message);
                //Dictionary<int, byte[]> DDSMsgDict = Utilites.DeseriailseAsciiOnly(message);

                // check if the topic is already existed
                if (_lvc.TryGetValue(topic, out Dictionary<int, byte[]> LVCDict))
                {
                    // update the tag value pair with the message
                    foreach (KeyValuePair<int, byte[]> kvp in DDSMsgDict)
                    {
                        LVCDict[kvp.Key] = kvp.Value;
                    }
                }
                else
                {
                    _lvc.TryAdd(topic, DDSMsgDict);
                }
            }
            catch (Exception ex)
            {                
                Log.Error("LastValueCache ::: Error in Update");
                Log.Error("LastValueCache ::: " + ex.ToString());
                Log.Error("LastValueCache ::: topic : {0}   message : {1}", topic, Encoding.ASCII.GetString(message));
            }
        }

        /// <summary>
        /// Get last value cache of given topic
        /// </summary>
        /// <param name="topic"></param>
        /// <returns>value in terms of serialise byte[]</returns>
        public KeyValuePair<string, byte[]>[] TryGet(string topic)
        {            
            if (topic.EndsWith("."))
            {                
                string[] topicList = _lvc.Keys.Where(key => key.StartsWith(topic.TrimEnd('.'))).ToArray();

                if (topicList.Length >= 1)
                {
                    KeyValuePair<string, byte[]>[] kvp = new KeyValuePair<string, byte[]>[topicList.Length];

                    for (int i = 0; i < topicList.Length; i++)
                    {
                        kvp[i] = new KeyValuePair<string, byte[]>(topicList[i], Utilites.Serialise(_lvc[topicList[i]]));
                    }

                    return kvp;
                }                
            }
            else
            {
                if (_lvc.ContainsKey(topic))
                {
                    KeyValuePair<string, byte[]>[] messages = new KeyValuePair<string, byte[]>[1];

                    messages[0] = new KeyValuePair<string, byte[]>(topic, Utilites.Serialise(_lvc[topic]));

                    return messages;
                }               
            }

            return null;
        }

        /// <summary>
        /// Get last value cache of given topic and tag
        /// </summary>
        /// <param name="topic">topic</param>
        /// <param name="tag">tag</param>
        /// <returns>byte[] (value)</returns>
        public byte[] TryGet(string topic, int tag)
        {
            if (_lvc.TryGetValue(topic, out Dictionary<int, byte[]> value))
            {
                if (value.ContainsKey(tag))
                {
                    return Utilites.Serialise(tag, value[tag]);
                }

                return null;
            }

            return null;
        }        
    }
}
