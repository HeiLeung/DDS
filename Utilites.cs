using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace Arrow
{
    public static class Utilites
    {
        public static Dictionary<string, string> ConvertToDictionary_Special(string messageReceived)
        {
            string message = messageReceived;
            int counter = 1;
            string tag = null;
            string value = null;
            bool blankTagFound = false;
            string[] splittedMsg = message.Split('|');

            // for handling the stupid message which consits of additional delimiter '|'

            //Dictionary<string, string> image = new Dictionary<string, string>();
            Dictionary<string, string> image = new Dictionary<string, string>();

            List<string> list = new List<string>(splittedMsg);

            foreach (string item in list)
            {
                // odd element == tag
                if (counter % 2 != 0)
                {
                    try
                    {
                        if (item != null && item != "" && IsAlphanumericPresent(item))
                        {
                            tag = item;
                        }
                        else
                        {
                            blankTagFound = true;
                        }

                    }
                    catch (Exception)
                    {
                        //BaliLib.DebugLog.Write("msgToDict exception - tag phase " + e.ToString());
                    }
                }
                // even element == value
                else
                {
                    {
                        value = item;
                    }
                    try
                    {
                        if (value != null || value != "")
                        {
                            try
                            {
                                if (tag == "36")
                                {
                                    if (value.Contains("505"))
                                    {
                                        image.Add(tag, value);
                                        tag = "505";
                                        counter--;
                                    }
                                }
                                else if (tag == "505")
                                {
                                    if (value.Contains("31"))
                                    {
                                        image.Add(tag, value);
                                        tag = "31";
                                        counter--;
                                    }
                                }
                                else
                                {
                                    // stupid msg handling
                                    if (image.ContainsKey(tag))
                                    {
                                        image[tag] = value;
                                    }
                                    else
                                    {
                                        image.Add(tag, value);
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                //BaliLib.DebugLog.Write("msg2Dict error, message -> " + message);
                                //BaliLib.DebugLog.Write("tag value error" + tag + "," + value);
                                //BaliLib.DebugLog.Write("exception -> " + e.ToString());
                                break;
                            }
                        }
                    }
                    catch (Exception )
                    {
                        //BaliLib.DebugLog.Write("message error in message2dictionary ---- message recived to be parse -> " + message + " EXCEPTION as follow" + e.ToString());
                    }
                }

                if (!blankTagFound)
                {
                    counter++;
                }
                else
                {
                    // do nothing, ignore the blank tag elements (counter not imcrement)
                    // action by checking blank tag done, (the above if-case), reset the boolean flag
                    blankTagFound = false;
                }

            }

            return image;
        }

        public static Dictionary<string, string> ConvertToDictionary(string messageReceived)
        {
            string message = messageReceived;
            int counter = 1;
            string tag = null;
            string value = null;
            bool blankTagFound = false;
            string[] splittedMsg = message.Split('|');

            // for handling the stupid message which consits of additional delimiter '|'

            //Dictionary<string, string> image = new Dictionary<string, string>();
            Dictionary<string, string> dict = new Dictionary<string, string>();

            List<string> list = new List<string>(splittedMsg);

            foreach (string item in list)
            {
                // odd element == tag
                if (counter % 2 != 0)
                {
                    try
                    {
                        if (item != null && item != "")
                        {
                            tag = item;
                        }
                        else
                        {
                            blankTagFound = true;
                        }
                    }
                    catch (Exception )
                    {
                        //BaliLib.DebugLog.Write("msgToDict exception - tag phase " + e.ToString());
                    }
                }
                // even element == value
                else
                {                    
                    try
                    {
                        value = item;                        
                    }
                    catch (Exception )
                    {
                        //BaliLib.DebugLog.Write("msg2Dict error, message -> " + message);
                        //BaliLib.DebugLog.Write("tag value error" + tag + "," + value);
                        //BaliLib.DebugLog.Write("exception -> " + e.ToString());
                        break;
                    }

                    if (dict.ContainsKey(tag))
                    {
                        dict[tag] = value;
                    }
                    else
                    {
                        dict.Add(tag, value);
                    }
                }

                if (!blankTagFound)
                {
                    counter++;
                }
                else
                {
                    // do nothing, ignore the blank tag elements (counter not imcrement)
                    // action by checking blank tag done, (the above if-case), reset the boolean flag
                    blankTagFound = false;
                }
            }

            return dict;
        }

        public static ConcurrentDictionary<string, string> ConvertToConcurrentDictionary(string messageReceived)
        {
            string message = messageReceived;
            int counter = 1;
            string tag = null;
            string value = null;
            bool blankTagFound = false;
            string[] splittedMsg = message.Split('|');

            // for handling the stupid message which consits of additional delimiter '|'

            //Dictionary<string, string> image = new Dictionary<string, string>();
            ConcurrentDictionary<string, string> image = new ConcurrentDictionary<string, string>();

            List<string> list = new List<string>(splittedMsg);

            foreach (string item in list)
            {
                // odd element == tag
                if (counter % 2 != 0)
                {
                    try
                    {
                        if (item != null && item != "" && IsAlphanumericPresent(item))
                        {
                            tag = item;
                        }
                        else
                        {
                            blankTagFound = true;
                        }

                    }
                    catch (Exception )
                    {
                        //BaliLib.DebugLog.Write("msgToDict exception - tag phase " + e.ToString());
                    }
                }
                // even element == value
                else
                {
                    {
                        value = item;
                    }
                    try
                    {
                        if (value != null || value != "")
                        {
                            try
                            {
                                if (tag == "36")
                                {
                                    if (value.Contains("505"))
                                    {
                                        image.TryAdd(tag, value);
                                        tag = "505";
                                        counter--;
                                    }
                                }
                                else if (tag == "505")
                                {
                                    if (value.Contains("31"))
                                    {
                                        image.TryAdd(tag, value);
                                        tag = "31";
                                        counter--;
                                    }
                                }
                                else
                                {
                                    // stupid msg handling
                                    if (image.ContainsKey(tag))
                                    {
                                        image[tag] = value;
                                    }
                                    else
                                    {
                                        image.TryAdd(tag, value);
                                    }
                                }
                            }
                            catch (Exception )
                            {
                                //BaliLib.DebugLog.Write("msg2Dict error, message -> " + message);
                                //BaliLib.DebugLog.Write("tag value error" + tag + "," + value);
                                //BaliLib.DebugLog.Write("exception -> " + e.ToString());
                                break;
                            }
                        }
                    }
                    catch (Exception )
                    {
                        //BaliLib.DebugLog.Write("message error in message2dictionary ---- message recived to be parse -> " + message + " EXCEPTION as follow" + e.ToString());
                    }
                }

                if (!blankTagFound)
                {
                    counter++;
                }
                else
                {
                    // do nothing, ignore the blank tag elements (counter not imcrement)
                    // action by checking blank tag done, (the above if-case), reset the boolean flag
                    blankTagFound = false;
                }

            }

            return image;
        }

        /// <summary>
        /// Convert EBS DDS message (byte[]) to Dictioanry<int, byte[]>
        /// </summary>
        /// <param name="messageReceived"></param>
        /// <returns>Dictioanry<int, byte[]></returns>
        public static Dictionary<int, byte[]> ConvertToDictionary(byte[] messageReceived)
        {
            ReadOnlySpan<byte> msgRx = messageReceived;
            bool isTagFound = false;
            int itemStartIdx = 0;
            int tag = 0;
            byte[] value;
            Dictionary<int, byte[]> dict = new Dictionary<int, byte[]>();

            for (int i = 0; i < msgRx.Length; i++)
            {
                //if (msgRx[i] == byteCode_VB)
                if (msgRx[i] == '|')
                {
                    if (!isTagFound)
                    {
                        //tag = GetIntFromAsciiEncoded(msgRx.Slice(itemStartIdx, i - itemStartIdx).ToArray());
                        tag = GetIntFromAsciiEncodedString(msgRx[itemStartIdx..i].ToArray());

                        isTagFound = true;
                    }
                    else
                    {
                        //value = msgRx.Slice(itemStartIdx, i - itemStartIdx).ToArray();
                        value = msgRx[itemStartIdx..i].ToArray();

                        // add to dictionary with the previous tag found
                        dict.Add(tag, value);

                        isTagFound = false;
                    }
                    itemStartIdx = i + 1;
                }
            }

            return dict;
        }

        /// <summary>
        /// This is a special deserialisation. Remove double-byte character set (DBCS) from the message.
        /// DBSC is used in EBS for the traditional and simplified chiness characters.
        /// The return dictionary will remove the tag 36 and 505, that is the tag for trad. chinese and simp. chinese stock name.
        /// </summary>
        /// <param name="messageReceived">byte[] message</param>
        /// <returns>Dictionary of integer as key, ASCII only value</returns>
        public static Dictionary<int, byte[]> DeseriailseAsciiOnly(byte[] messageReceived)
        {
            ReadOnlySpan<byte> msgRx = messageReceived;
            bool isTagFound = false;
            int itemStartIdx = 0;
            int tag = 0;
            byte[] value;
            Dictionary<int, byte[]> dict = new Dictionary<int, byte[]>();

            for (int i = 0; i < msgRx.Length; i++)
            {
                //if (msgRx[i] == byteCode_VB)
                if (msgRx[i] == '|')
                {
                    if (!isTagFound)
                    {
                        //tag = GetIntFromAsciiEncoded(msgRx.Slice(itemStartIdx, i - itemStartIdx).ToArray());
                        tag = GetIntFromAsciiEncodedString(msgRx[itemStartIdx..i].ToArray());

                        isTagFound = true;
                    }
                    else if (msgRx[i-1] < 127)  //  it is NOT a double-byte character set (DBCS)
                    {

                        //value = msgRx.Slice(itemStartIdx, i - itemStartIdx).ToArray();
                        value = msgRx[itemStartIdx..i].ToArray();

                        // add to dictionary with the previous tag found
                        dict.Add(tag, value);

                        isTagFound = false;
                    }
                    itemStartIdx = i + 1;
                }
            }

            dict.Remove(36);
            dict.Remove(505);

            return dict;
        }

        /// <summary>
        /// Static method for checking if aphpanumeric is present in the message
        /// </summary>
        /// <param name="message">the string to be check</param>
        /// <returns>True if only alphanumeric and underscore is present, otherwise, False</returns>
        public static bool IsAlphanumericPresent(string message)
        {
            string msg = message;

            return Regex.IsMatch(msg, @"^[a-zA-Z0-9_\r]+$", RegexOptions.Compiled);
        }

        /// <summary>
        /// Get int from an ascii encoded byte[]
        /// </summary>
        /// <param name="asciiEncoded"></param>
        /// <returns>int</returns>
        public static int GetIntFromAsciiEncodedString(byte[] asciiEncoded)
        {
            int result = 0;

            for (int i = 0; i < asciiEncoded.Length; i++)
            {
                result = (10 * result) + (asciiEncoded[i] - '0');           // char '0' = (int)48 
            }

            return result;
        }
    }
}
