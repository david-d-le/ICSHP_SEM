using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace ICSHP_SEM_Le
{
    [Serializable]
    public class Move : ISerializable
    {
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public bool XsMove { get; set; }

        public Move(int posX, int posY, bool xsMove)
        {
            PositionX = posX;
            PositionY = posY;
            XsMove = xsMove;
        }

        public override string ToString()
        {
            return (XsMove ? "X: " : "O: ") + PositionX + ", " + PositionY;
        }


        #region Move Serialization
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("PositionX", PositionX, typeof(int));
            info.AddValue("PositionY", PositionY, typeof(int));
            info.AddValue("XsMove", XsMove, typeof(bool));
        }
        public Move(SerializationInfo info, StreamingContext context)
        {
            try
            {
                PositionX = (int)info.GetValue("PositionX", typeof(int));
                PositionY = (int)info.GetValue("PositionY", typeof(int));
                XsMove = (bool)info.GetValue("XsMove", typeof(bool));
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

        public static Move DeserializeItem(FileStream s)
        {
            IFormatter formatter = new BinaryFormatter();
            if (s == null)
                throw new SerializationException("Could not open file");
            try
            {
                Move move = (Move)formatter.Deserialize(s);
                return move;
            }
            catch (Exception)
            {
            }
            throw new SerializationException("Invalid replay file");
        }
        #endregion
    }
}
