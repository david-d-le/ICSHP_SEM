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
        public int NumOfFreeButtons { get; set; }

        private readonly int boardSize;
        public int BoardSize
        {
            get { return boardSize; }
        }

        private readonly Game gameObject;

        public Game GameObject
        {
            get { return gameObject; }
        }

        public BoardButton[,] Buttons { get; set; }
        #endregion
        
        #region GameBoard methods associated with ctors
        private void GenerateButtons(int boardSize, bool? [,] boardBool)
        {
            Buttons = new BoardButton[boardSize, boardSize];
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    Buttons[i, j] = new BoardButton() {
                        Size = new Size(30,30),
                        Location = new Point(j * 30 + 50, i * 30 + 50),
                        FlatStyle = FlatStyle.Flat,
                    };
                    //https://codingvision.net/c-eventhandler-with-arguments
                    //local variables, so it is copied to delegate
                    int x = i;
                    int y = j;
                    Buttons[i, j].Click += delegate (object sender2, EventArgs e2)
                    {
                        button_Click(sender2, e2, x, y);
                    };

                    Draw(i,j, boardBool[i,j]);
                }
            }
        }

        private void Draw(int i, int j,bool? player)
        {
            //TODO draw shapes - new method - same method for animation when click
            switch (player)
            {
                case true:
                    Buttons[i, j].Text = "x";
                    Buttons[i, j].XClicked = true;
                    break;
                case false:
                    Buttons[i, j].Text = "o";
                    Buttons[i, j].XClicked = false;
                    break;
                case null:
                    Buttons[i, j].Text = "";
                    Buttons[i, j].XClicked = null;
                    break;
                default:
                    throw new ArgumentException("Not a valid boolean value");
            }
        }

        private void CheckWinnerFromLastClick(int i, int j, bool xClicked)
        {
            GameForm myForm = Buttons[0, 0].FindForm() as GameForm;
            //TODO check for winner, if someone wins disable all buttons
            if (NumOfFreeButtons <= 0)
            {
                MessageBox.Show("It's a tie", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                myForm.SetWinnerLabelFontColor(null);
                myForm.SetWinnerLabelText("It's a tie");
            }
            int rowL, rowR, columnD, columnU, diagLU, diagLD, diagRU, diagRD;
            bool rowLBool, rowRBool, columnDBool, columnUBool, diagLUBool, diagLDBool, diagRUBool, diagRDBool;
            rowLBool = rowRBool = columnDBool = columnUBool = diagLUBool = diagLDBool = diagRUBool = diagRDBool = true;
            rowL = rowR = columnD = columnU = diagLU = diagLD = diagRU = diagRD = 0;
            for (int k = 1; k < 5; k++)
            {
                if (j - k >= 0)
                {
                    //row left side
                    rowLBool = xClicked == Buttons[i, j - k].XClicked;
                    rowL = (rowLBool && (xClicked == Buttons[i,j - k].XClicked)) ? ++rowL : rowL;
                    //diagonal up left side
                    if (i - k >= 0)
                    {
                        diagLUBool = xClicked == Buttons[i - k, j - k].XClicked;
                        diagLU = (diagLUBool && (xClicked == Buttons[i - k, j - k].XClicked)) ? ++diagLU : diagLU;
                    }
                    //diagonal down left side
                    if (i + k < BoardSize)
                    {
                        diagLDBool = xClicked == Buttons[i + k, j - k].XClicked;
                        diagLD = (diagLDBool && (xClicked == Buttons[i + k, j - k].XClicked)) ? ++diagLD : diagLD;
                    }

                }
                if (j + k < boardSize)
                {
                    rowRBool = xClicked == Buttons[i, j + k].XClicked;
                    rowR = (rowRBool && (xClicked == Buttons[i, j + k].XClicked)) ? ++rowR : rowR;
                    //diagonal up right side
                    if (i - k >= 0)
                    {
                        diagRUBool = xClicked == Buttons[i - k, j + k].XClicked;
                        diagRU = (diagRUBool && (xClicked == Buttons[i - k, j + k].XClicked)) ? ++diagRU : diagRU;
                    }
                    //diagonal down right side
                    if (i + k < BoardSize)
                    {
                        diagRDBool = xClicked == Buttons[i + k, j + k].XClicked;
                        diagRD = (diagRDBool && (xClicked == Buttons[i + k, j + k].XClicked)) ? ++diagRD : diagRD;
                        
                    }
                }
                if (i - k >= 0)
                {
                    columnUBool = xClicked == Buttons[i - k, j].XClicked;
                    columnU = (columnUBool && (xClicked == Buttons[i - k, j].XClicked)) ? ++columnU : columnU;
                }
                if (i + k < boardSize)
                {
                    columnDBool = xClicked == Buttons[i + k, j].XClicked;
                    columnD = (columnDBool && (xClicked == Buttons[i + k, j].XClicked)) ? ++columnD : columnD;
                    
                }
            }
            if (rowL + rowR + 1 == 5 || diagLU + diagRD + 1 == 5 || diagLD + diagRU + 1 == 5 || columnU + columnD + 1 == 5)
                throw new Exception("there is a winner");

            //TODO fix with space
        }

        private void TogglePlayer()
        {
            GameObject.XsTurn = !GameObject.XsTurn;
            GameForm myForm = Buttons[0, 0].FindForm() as GameForm;
            myForm.SetPlayerLabelFontColor(GameObject.XsTurn);
            myForm.SetPlayerLabelText(GameObject.XsTurnToString());
        }

        void button_Click(object sender, EventArgs e, int i, int j)
        {
            BoardButton button = sender as BoardButton;
            if(button.XClicked != null)
                return;
            --NumOfFreeButtons;
            button.XClicked = GameObject.XsTurn;
            Draw(i, j, GameObject.XsTurn);
            CheckWinnerFromLastClick(i, j, (bool)button.XClicked);
            TogglePlayer();
        }

        private bool CheckRowsAndColumns(int boardSize, bool?[,] board)
        {
            int rowX, rowO, columnX, columnO;
            rowX = rowO = columnX = columnO = 0;
            bool? previousRow = null;
            bool? previousColumn = null;
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    //row
                    if (board[i, j] == null)
                    {
                        rowX = rowO = 0;
                        previousRow = null;
                    }
                    else if (board[i, j] == true)
                    {
                        rowX = (previousRow == null) ? 1 : ++rowX;
                        rowO = 0;
                        previousRow = true;
                        --NumOfFreeButtons;
                    }
                    else
                    {
                        rowO = (previousRow == null) ? 1 : ++rowO;
                        rowX = 0;
                        previousRow = false;
                        --NumOfFreeButtons;
                    }

                    //column
                    if (board[j, i] == null)
                    {
                        columnX = columnO = 0;
                        previousColumn = null;
                    }
                    else if (board[j, i] == true)
                    {
                        columnX = (previousColumn == null) ? 1 : ++columnX;
                        columnO = 0;
                        previousColumn = true;
                    }
                    else
                    {
                        columnO = (previousColumn == null) ? 1 : ++columnO;
                        columnX = 0;
                        previousColumn = false;
                    }
                    //check if there is a winner can be changed
                    if (rowX == 5 || rowO == 5 || columnX == 5 || columnO == 5)
                        return false;
                }
            }
            return true;
        }
        private void CheckWholeBoard(int boardSize, bool?[,] board)
        {
            //true = valid board - game can continue
            if (CheckRowsAndColumns(boardSize, board) == false || CheckDialonals(boardSize, board) == false)
                throw new ArgumentException("Not a valid saveGame file - there is already a winner");
            if (NumOfFreeButtons == 0)
                throw new ArgumentException("Not a valid saveGame file - no more moves");
        }

        private bool CheckDialonals(int boardSize, bool?[,] board)
        {
            for (int i = 0; i <= boardSize-5; i++)
            {
                for (int j = 0; j <= boardSize-5; j++)
                {
                    if ((board[i, j] == true && board[i + 1, j + 1] == true && board[i + 2, j + 2] == true
                            && board[i + 3, j + 3] == true && board[i + 4, j + 4] == true)
                        || (board[i, j] == false && board[i + 1, j + 1] == false && board[i + 2, j + 2] == false 
                            && board[i + 3, j + 3] == false && board[i + 4, j + 4] == false)
                        || (board[i, boardSize-j-1] == true && board[i + 1, boardSize - (j + 2)] == true && board[i + 2, boardSize - (j + 3)] == true
                            && board[i + 3, boardSize - (j + 4)] == true && board[i + 4, boardSize - (j + 5)] == true)
                        || (board[i, boardSize - j - 1] == false && board[i + 1, boardSize - (j + 2)] == false && board[i + 2, boardSize - (j + 3)] == false
                            && board[i + 3, boardSize - (j + 4)] == false && board[i + 4, boardSize - (j + 5)] == false))
                        return false;
                }
            }
            return true;
        }
        #endregion

        #region Gameboard constturctors
        public GameBoard(Game gameObject, int boardSize)
        {
            this.boardSize = boardSize;
            this.gameObject = gameObject;
            NumOfFreeButtons = boardSize * boardSize;
            GenerateButtons(boardSize, new bool?[boardSize, boardSize]);
        }

        public GameBoard(Game gameObject, int boardSize, bool?[,] board)
        {
            this.boardSize = boardSize;
            this.gameObject = gameObject;
            NumOfFreeButtons = boardSize * boardSize;
            CheckWholeBoard(boardSize, board);
            GenerateButtons(boardSize, board);
        }
        #endregion

        public string BoardToString()
        {
            StringBuilder ss = new StringBuilder();
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    switch (Buttons[i, j].XClicked)
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

        #region custom Button class
        public class BoardButton : Button
        {
            public bool? XClicked { get; set; }

            public BoardButton()
            {

            }

            public BoardButton(bool xClicked)
            {
                XClicked = xClicked;
            }
        }
        #endregion

    }
}
