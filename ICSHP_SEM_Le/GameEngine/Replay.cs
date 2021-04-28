using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace ICSHP_SEM_Le
{
    [Serializable]
    public class Replay :ISerializable
    {
        public LinkedList<Move> Moves { get; set; }
        public Replay()
        {
            Moves = new LinkedList<Move>();
        }

        #region Replay Serialization
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Moves", Moves, typeof(LinkedList<Move>));
        }
        public Replay(SerializationInfo info, StreamingContext context)
        {
            try
            {
                Moves = (LinkedList<Move>)info.GetValue("Moves", typeof(LinkedList<Move>));
            }
            catch (Exception)
            {
                throw new SerializationException("Invalid saveGame file");
            }
        }

        public void SerializeItem(string fileName)
        {
            IFormatter formatter = new BinaryFormatter();
            FileStream s = new FileStream(fileName, FileMode.Create);
            formatter.Serialize(s, this);
            s.Close();
        }

        public static Replay DeserializeItem(FileStream s)
        {
            IFormatter formatter = new BinaryFormatter();
            if (s == null)
                throw new SerializationException("Could not open file");
            try
            {
                Replay replay = (Replay)formatter.Deserialize(s);
                return replay;
            }
            catch (Exception)
            {
            }
            throw new SerializationException("Invalid replay file");
        }
        #endregion
    }
}
