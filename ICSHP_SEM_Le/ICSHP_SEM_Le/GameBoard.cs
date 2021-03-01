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
        #region constants
        private const int BUTTON_SIZE = 30; // can't be changed
        private const int STEPS = 50;
        private const int STEPS_HALF = STEPS/2;
        #endregion

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
                        Size = new Size(BUTTON_SIZE, BUTTON_SIZE),
                        Location = new Point(j * BUTTON_SIZE + 50, i * BUTTON_SIZE + 50),
                        FlatStyle = FlatStyle.Flat,
                        BackColor = SystemColors.Window
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

        private void DrawX(Graphics g, int step)
        {
            if (step >= STEPS_HALF)
            {
                g.DrawLine(new Pen(Color.Red, 3), new Point(5, 5), new Point(BUTTON_SIZE - 5, BUTTON_SIZE - 5));
                g.DrawLine(new Pen(Color.Red, 3), new PointF(25, 5), new PointF(BUTTON_SIZE-5 - (step - STEPS_HALF) / (STEPS_HALF/20f), 5 + (step - STEPS_HALF) / (STEPS_HALF / 20f)));
            }
            else
            {
                g.DrawLine(new Pen(Color.Red, 3), new Point(5, 5), new PointF(step / (STEPS_HALF / 20f) + 5, step / (STEPS_HALF / 20f) + 5));
            }
                
        }

        private void DrawO(Graphics g, int step)
        {
            g.DrawArc(new Pen(Color.Blue, 3), new Rectangle(5, 5, 20, 20), 0, (360f / STEPS) * (step+1));
        }

        private void Draw(int i, int j,bool? player)
        {
            Graphics g = Buttons[i, j].CreateGraphics();
            Timer timer1 = new Timer();
            timer1.Interval = 1; // in miliseconds
            int step = 0;

            switch (player)
            {
                case true:
                    Buttons[i, j].XClicked = true;
                    timer1.Tick += delegate (object sender2, EventArgs e2)
                    {
                        if (step < STEPS)
                            DrawX(g, step++);
                        else
                        {
                            Buttons[i, j].Painted = true;
                            timer1.Stop();
                        }
                    };
                    timer1.Start();
                    break;
                case false:
                    Buttons[i, j].XClicked = false;
                    timer1.Tick += delegate (object sender2, EventArgs e2)
                    {
                        if (step < STEPS)
                            DrawO(g, step++);
                        else
                        {
                            Buttons[i, j].Painted = true;
                            timer1.Stop();
                        }
                    };
                    timer1.Start();
                    break;
                case null:
                    Buttons[i, j].XClicked = null;
                    break;
                default:
                    throw new ArgumentException("Not a valid boolean value");
            }
        }

        private void CrossOutWinner(int rowL, int rowR, int diagLU, int diagRD, int diagLD, int diagRU, int columnU, int columnD, int x, int y)
        {
            GameForm myForm = Buttons[0, 0].FindForm() as GameForm;
            SplitterPanel panel = myForm.GetSplitterPanel2();

            if (rowL + rowR + 1 >= 5){
                int startX = Buttons[x - rowL, y].Location.X;
                int startY = Buttons[x, y].Location.Y;
                Label label = new Label() {
                    Location = new Point(startX, startY),
                    Size = new Size((rowL + rowR + 1) * BUTTON_SIZE, BUTTON_SIZE),
                    Text = "HOUBY"
                };
                myForm.GetSplitterPanel2().Controls.Add(label);
                label.BringToFront();
                Graphics g = label.CreateGraphics();
                g.DrawLine(new Pen(Color.Black, 3), new Point(startX, startY+ BUTTON_SIZE/2), new Point(startX+(rowL + rowR + 1)* BUTTON_SIZE, BUTTON_SIZE/2));
                
            }
            else if(diagLU + diagRD + 1 >= 5){

            }else if (diagLD + diagRU + 1 >= 5){

            }else
            {
                //TODO fix all
            }
        }

        private void CheckWinnerFromLastClick(int i, int j, bool xClicked)
        {
            GameForm myForm = Buttons[0, 0].FindForm() as GameForm;
            
            int rowL, rowR, columnD, columnU, diagLU, diagLD, diagRU, diagRD;
            bool rowLBool, rowRBool, columnDBool, columnUBool, diagLUBool, diagLDBool, diagRUBool, diagRDBool;
            rowLBool = rowRBool = columnDBool = columnUBool = diagLUBool = diagLDBool = diagRUBool = diagRDBool = true;
            rowL = rowR = columnD = columnU = diagLU = diagLD = diagRU = diagRD = 0;
            for (int k = 1; k < 5; k++)
            {
                if (j - k >= 0)
                {
                    //row left side
                    rowLBool = rowLBool ? xClicked == Buttons[i, j - k].XClicked : false;
                    rowL = rowLBool ? ++rowL : rowL;
                    //diagonal up left side
                    if (i - k >= 0)
                    {
                        diagLUBool = diagLUBool ? xClicked == Buttons[i - k, j - k].XClicked : false;
                        diagLU = diagLUBool ? ++diagLU : diagLU;
                    }
                    //diagonal down left side
                    if (i + k < BoardSize)
                    {
                        diagLDBool = diagLDBool ? xClicked == Buttons[i + k, j - k].XClicked : false;
                        diagLD = diagLDBool ? ++diagLD : diagLD;
                    }

                }
                if (j + k < boardSize)
                {
                    rowRBool = rowRBool ? xClicked == Buttons[i, j + k].XClicked : false;
                    rowR = rowRBool ? ++rowR : rowR;
                    //diagonal up right side
                    if (i - k >= 0)
                    {
                        diagRUBool = diagRUBool ? xClicked == Buttons[i - k, j + k].XClicked : false;
                        diagRU = diagRUBool ? ++diagRU : diagRU;
                    }
                    //diagonal down right side
                    if (i + k < BoardSize)
                    {
                        diagRDBool = diagRDBool ? xClicked == Buttons[i + k, j + k].XClicked : false;
                        diagRD = diagRDBool ? ++diagRD : diagRD;
                        
                    }
                }
                if (i - k >= 0)
                {
                    columnUBool = columnUBool ? xClicked == Buttons[i - k, j].XClicked : false;
                    columnU = columnUBool ? ++columnU : columnU;
                }
                if (i + k < boardSize)
                {
                    columnDBool = columnDBool ? xClicked == Buttons[i + k, j].XClicked : false;
                    columnD = columnDBool ? ++columnD : columnD;
                    
                }
            }
            if (rowL + rowR + 1 >= 5 || diagLU + diagRD + 1 >= 5 || diagLD + diagRU + 1 >= 5  || columnU + columnD + 1 >= 5)
            {
                string message = xClicked ? "Player X wins" : "Player O wins";
                foreach (BoardButton button in Buttons){
                    button.Enabled = false;
                }
                myForm.SetWinnerLabelFontColor(xClicked);
                myForm.SetWinnerLabelText(message);
                CrossOutWinner(rowL, rowR, diagLU, diagRD, diagLD, diagRU, columnU, columnD, j, i);
                MessageBox.Show(message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                GameObject.GameOver = true;
                //TODO cross out winner
            }
            else if (NumOfFreeButtons <= 0)
            {
                myForm.SetWinnerLabelFontColor(null);
                myForm.SetWinnerLabelText("It's a tie");
                MessageBox.Show("It's a tie", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                GameObject.GameOver = true;
            }
        }

        private void TogglePlayer()
        {
            GameForm myForm = Buttons[0, 0].FindForm() as GameForm;
            if (GameObject.GameOver == true)
            {
                myForm.SetPlayerLabelText("");
                return;
            }
            GameObject.XsTurn = !GameObject.XsTurn;
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
            public bool Painted { get; set; }
            public BoardButton()
            {

            }

            public BoardButton(bool xClicked)
            {
                XClicked = xClicked;
            }

            protected override void OnPaint(PaintEventArgs pevent)
            {
                base.OnPaint(pevent);

                if (Painted)
                {
                    Graphics g = pevent.Graphics;
                    if (XClicked == true) {
                        g.Clear(Color.Transparent);
                        g.FillRectangle(Brushes.White, new Rectangle(1,1, BUTTON_SIZE-2, BUTTON_SIZE-2));
                        g.DrawLine(new Pen(Color.Red, 3), new Point(5, 5), new Point(BUTTON_SIZE-5, BUTTON_SIZE-5));
                        g.DrawLine(new Pen(Color.Red, 3), new Point(BUTTON_SIZE-5, 5), new Point(5, BUTTON_SIZE-5));
                    }
                    else if(XClicked == false)
                    {
                        g.DrawArc(new Pen(Color.Blue, 3), new Rectangle(5, 5, 20, 20), 0, 360);
                    }
                }
            }
  
        }
        #endregion

    }
}
