using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace ICSHP_SEM_Le
{
    [Serializable]
    public class Game : ISerializable
    {
        #region Game properties and attributes
        public GameBoard GameBoard { get; set; }
        public bool XsTurn { get; set; }
        public bool GameOver { get; set; }
        public Replay Replay { get; set; }
        public Timer ReplayTimer { get; set; }
        #endregion

        #region Game constructors and associated methods
        public Game()
        {
            GameOver = false;
            ReplayTimer = new Timer { Interval = 1000 };
        }

        public Game(int boardSize) : this()
        {
            XsTurn = true;
            GameBoard = new GameBoard(this, boardSize);
            Replay = new Replay();
        }
        #endregion

        public string XsTurnToString()
        {
            return XsTurn ? "x" : "o";
        }


        public void PlayReplay()
        {
            ReplayTimer.Tick += delegate (object sender2, EventArgs e2)
            {
                if (Replay.Moves.Count <= 0)
                {
                    ReplayTimer.Stop();
                    return;
                }
                Move move = Replay.Moves.First.Value;
                Replay.Moves.RemoveFirst();
                GameBoard.Button_Click(GameBoard.Buttons[move.PositionX, move.PositionY], e2, move.PositionX, move.PositionY);
            };
            ReplayTimer.Start();
        }


        #region Game Serialization
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("XsTurn", XsTurn, typeof(bool));
            info.AddValue("GameOver", GameOver, typeof(bool));
            info.AddValue("GameBoard", GameBoard, typeof(GameBoard));
            info.AddValue("Replay", Replay, typeof(Replay));
        }
        public Game(SerializationInfo info, StreamingContext context)
        {
            try
            {
                XsTurn = (bool)info.GetValue("XsTurn", typeof(bool));
                GameOver = (bool)info.GetValue("GameOver", typeof(bool));
                GameBoard = (GameBoard)info.GetValue("GameBoard", typeof(GameBoard));
                Replay = (Replay)info.GetValue("Replay", typeof(Replay));
                ReplayTimer = new Timer { Interval = 1000 };
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

        public static Game DeserializeItem(FileStream s)
        {
            IFormatter formatter = new BinaryFormatter();
            if (s == null)
                throw new SerializationException("Could not open file");
            try
            {
                return (Game)formatter.Deserialize(s);
            }
            catch (Exception)
            {
            }
            throw new SerializationException("Invalid savegame file");
        }
        #endregion
    }
}
