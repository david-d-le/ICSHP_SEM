using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;

namespace ICSHP_SEM_Le
{
    [Serializable]
    public class Game : ISerializable
    {
        #region Game properties and attributes
        public GameBoard GameBoard { get; set; }
        public bool XsTurn { get; set; }
        public bool GameOver { get; set; }
        #endregion

        #region Game constructors and associated methods
        private bool IsPlayerChar(char playerChar)
        {
            return (playerChar == 'x' || playerChar == 'o');
        }
        private void CheckSaveGameFileFormat(Stream fileStream, out int boardSize, out bool?[,] board)
        {
            StreamReader savedGameFile = new StreamReader(fileStream);
            if (char.TryParse(savedGameFile.ReadLine(), out char turnChar) && IsPlayerChar(turnChar)
                && int.TryParse(savedGameFile.ReadLine(), out boardSize)
                && boardSize >= GameBoard.MIN_BOARD_SIZE && boardSize <= GameBoard.MAX_BOARD_SIZE)
            {
                XsTurn = (turnChar == 'x');
                board = new bool?[boardSize, boardSize];
                Regex regex = new Regex("[x,o,#]{" + boardSize + "}");
                for (int i = 0; i < boardSize; i++)
                {
                    string line = savedGameFile.ReadLine();
                    if (line == null || !regex.IsMatch(line))
                        throw new ArgumentException("Not a valid saveGame file");
                    for (int j = 0; j < boardSize; j++)
                    {
                        board[i, j] = (line[j]) switch
                        {
                            'x' => true,
                            'o' => false,
                            '#' => null,
                            _ => throw new ArgumentException("Not a valid saveGame file"),
                        };
                    }
                }
                //if there are more lines than expected
                string rest = savedGameFile.ReadToEnd().TrimEnd('\r', '\n');
                if (rest.Length != 0)
                    throw new ArgumentException("Not a valid saveGame file");
                return;
            }
            throw new ArgumentException("Not a valid saveGame file");
        }
        private void LoadGame(Stream fileStream)
        {
            CheckSaveGameFileFormat(fileStream, out int boardSize, out bool?[,] board);
            GameBoard = new GameBoard(this, boardSize, board);
        }

        public Game() { }

        public Game(Stream fileStream)
        {
            GameOver = false;
            LoadGame(fileStream);
        }

        public Game(int baordSize)
        {
            GameOver = false;
            XsTurn = true;
            GameBoard = new GameBoard(this, baordSize);
        }
        #endregion

        public string XsTurnToString()
        {
            return XsTurn ? "x" : "o";
        }


        #region Game Serialization
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("XsTurn", XsTurn, typeof(bool));
            info.AddValue("GameOver", GameOver, typeof(bool));
            info.AddValue("GameBoard", GameBoard, typeof(GameBoard));
        }
        public Game(SerializationInfo info, StreamingContext context)
        {
            try
            {
                XsTurn = (bool)info.GetValue("XsTurn", typeof(bool));
                GameOver = (bool)info.GetValue("GameOver", typeof(bool));
                GameBoard = (GameBoard)info.GetValue("GameBoard", typeof(GameBoard));
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
                Game game = (Game)formatter.Deserialize(s);
                return game;
            }
            catch (Exception)
            {
            }
            throw new SerializationException("Invalid savegame file");
        }
        #endregion
    }
}
