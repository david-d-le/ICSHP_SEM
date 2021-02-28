using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ICSHP_SEM_Le
{
    public class GameBoard
    {
        #region GameBoard properties and attributes
        private readonly int boardSize;
        public int BoardSize
        {
            get { return boardSize; }
        }
        public bool?[,] BoardBool { get; set; }
        public Button[,] Buttons { get; set; }
        #endregion

        #region GameBoard constructors and associated methods
        private void GenerateButtons(int boardSize)
        {
            Buttons = new Button[boardSize, boardSize];
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    Buttons[i, j] = new Button() {
                        Size = new Size(30,30),
                        Location = new Point(j * 30 + 50, i * 30 + 50),
                        FlatStyle = FlatStyle.Flat,
                    };
                    //TODO draw shapes - new method
                    //https://codingvision.net/c-eventhandler-with-arguments
                    //local variables, so it is copied to delegate
                    int x = i;
                    int y = j;
                    Buttons[i, j].Click += delegate (object sender2, EventArgs e2)
                    {
                        button_Click(sender2, e2, x, y);
                    };
                }
            }
        }

        void button_Click(object sender, EventArgs e, int i, int j)
        {
            Button button = sender as Button;
            if (BoardBool[i, j] != null)
                return;
            BoardBool[i, j] = Game.XsTurn;
            button.Text = Game.XsTurn ? "x" : "o"; //TODO remove
            // TODO Mark the block with current players icon - new method - animation
            TogglePlayer();
        }

        private void TogglePlayer()
        {
            Game.XsTurn = !Game.XsTurn;
        }

        public string BoardBoolToString()
        {
            StringBuilder ss = new StringBuilder();
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    switch (BoardBool[i,j])
                    {
                        case true:
                            ss.Append("x");
                            break;
                        case false:
                            ss.Append("o");
                            break;
                        case null:
                            ss.Append("#");
                            break;
                        default:
                            throw new ArgumentException("Not a valid saveGame file");
                    }
                }
                ss.Append("\n");
            }
            return ss.ToString();
        }

        public GameBoard(int boardSize)
        {
            this.boardSize = boardSize;
            BoardBool = new bool?[boardSize, boardSize];
            GenerateButtons(boardSize);
        }

        public GameBoard(int boardSize, bool?[,] board)
        {
            this.boardSize = boardSize;
            BoardBool = board;
            GenerateButtons(boardSize);
            
        }
        #endregion
    }
}
