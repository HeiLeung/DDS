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
        /// <returns>value in terms of serilaise byte[]</returns>
        public KeyValuePair<string, byte[]>[] Get(string topic)
        {            
            if (topic.EndsWith("."))
            {                
                string[] topicList = _lvc.Keys.Where(key => key.StartsWith(topic.TrimEnd('.'))).ToArray();

                if (topicList.Length >= 1)
                {
                    KeyValuePair<string, byte[]>[] messages = new KeyValuePair<string, byte[]>[topicList.Length];

                    for (int i = 0; i < topicList.Length; i++)
                    {
                        messages[i] = new KeyValuePair<string, byte[]>(topicList[i], Serialise(_lvc[topicList[i]]));
                    }

                    return messages;
                }                
            }
            else
            {
                if (_lvc.ContainsKey(topic))
                {
                    KeyValuePair<string, byte[]>[] messages = new KeyValuePair<string, byte[]>[1];

                    messages[0] = new KeyValuePair<string, byte[]>(topic, Serialise(_lvc[topic]));

                    return messages;
                }               
            }

            return null;
        }
        
        /// <summary>
        /// Return serialise byte[]
        /// </summary>
        /// <param name="Dict">DDS message Dictionary<int, byte[]></param>
        /// <param name="topic">Topic</param>
        /// <returns>serialise byte[]</returns>
        private byte[] Serialise(Dictionary<int, byte[]> dict)
        {
            List<byte> byteList = new List<byte>(8192);
            
            foreach (var kvp in dict)
            {
                byteList.AddRange(System.Text.Encoding.ASCII.GetBytes(kvp.Key.ToString() + '|'));
                byteList.AddRange(kvp.Value);
                byteList.Add((byte)'|');
            }
            byteList.TrimExcess();
            return byteList.ToArray();
        }

        /// <summary>
        /// Return serialise byte[]
        /// </summary>
        /// <param name="Dict">DDS message Dictionary<int, byte[]></param>
        /// <param name="topic">Topic</param>
        /// <returns>serialise byte[]</returns>
        public byte[] Serialise(string topic, KeyValuePair<int, byte[]>[] keyValuePairs)
        {
            List<byte> byteList = new List<byte>(128);
            
            for (int i =0; i < keyValuePairs.Length; i++)
            {
                byteList.AddRange(System.Text.Encoding.ASCII.GetBytes(keyValuePairs[i].Key.ToString() + '|'));
                byteList.AddRange(keyValuePairs[i].Value);
                byteList.Add((byte)'|');
            }
            byteList.TrimExcess();
            return byteList.ToArray();
        }
    }
}
