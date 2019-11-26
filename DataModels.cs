using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace DDS
{
    public class DDS_MessageQueue
    {
        private Queue<string> _Queue;
        private string _incomplete;
        private string[] _splittedStringPieces;     // reusable temp storage for Equeue

        /// <summary>
        /// The constructor, initialized variables
        /// </summary>
        public DDS_MessageQueue()
        {
            _incomplete = string.Empty;
            _Queue = new Queue<string>();
            _splittedStringPieces = null;
        }

        /// <summary>
        /// Enqueue a response from a async DDS socket
        /// </summary>
        /// <param name="Resource">The response from DDS</param>
        /// <remarks>
        /// What defines the end of a command is a CRLF aka carige return + line feed</remarks>
        public void Enqueue(string Resource)
        {
            //Combine the remainder with the new resource and then split it
            //the many lines ensure the desired results of all operations
            _incomplete = _incomplete + Resource;
            //string[] pieces = this._incomplete.Split(char.Parse("\n"));
            //string[] pieces = _incomplete.Split('\n');
            //_pieces = null;
            _splittedStringPieces = _incomplete.Split('\n');

            //Clear out our incomplete buffer
            _incomplete = string.Empty;

            //Grab all the valid commands and throw them in queue
            for (int i = 0; i < _splittedStringPieces.Length - 1; i++)
                _Queue.Enqueue(_splittedStringPieces[i]);

            //If there is a partial command in the buffer, save it for next packet
            if (_splittedStringPieces[_splittedStringPieces.Length - 1] != string.Empty)
                _incomplete = _splittedStringPieces[_splittedStringPieces.Length - 1];
        }

        /// <summary>
        /// Get the next availiable complete message that DDS has sent
        /// </summary>
        /// <returns>A string value representing the complete message</returns>
        /// <remarks>
        /// Very simple hook to the internal queue without making it public</remarks>
        public string Dequeue()
        {
            return _Queue.Dequeue();
        }

        /// <summary>
        /// Clear everything in the queue and clear everything in the waiting phase
        /// </summary>
        /// <remarks>
        /// Very rarely, if not ever, needed</remarks>
        public void Clear()
        {
            _incomplete = string.Empty;
            _Queue.Clear();
        }

        /// <summary>
        /// A quick function to determine if commands are still availabe to be obtained
        /// </summary>
        /// <returns>True if it is empty, otherwise it returns false</returns>
        /// <remarks>
        /// Very useful in conjunction with Dequeue and a while loop
        /// <code>
        /// while(!commandQueue.isEmpty())
        /// {
        ///		string newCommand  = commandQueue.Dequeue();
        ///		...
        /// }
        /// </code></remarks>
        public bool isEmpty()
        {
            if (_Queue.Count == 0) return true;
            else return false;
        }
    }

    public class Token
    {
        public DDS_MessageQueue DDSMQ;
        public string CompleteMsg;
    }
}
