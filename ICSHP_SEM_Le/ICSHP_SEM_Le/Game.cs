using System;
using System.IO;
using System.Text.RegularExpressions;

namespace ICSHP_SEM_Le
{
    public class Game
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
    }
}
