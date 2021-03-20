using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ICSHP_SEM_Le
{
    public partial class GameBoard
    {
        #region constants
        private const int BUTTON_SIZE = 30; // can't be changed
        private const int STEPS = 50;
        private const int STEPS_HALF = STEPS / 2;
        private const int CROSS_OUT_STEPS = 25;
        public const int MIN_BOARD_SIZE = 5;
        public const int MAX_BOARD_SIZE = 30;
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
        private void GenerateButtons(int boardSize, bool?[,] boardBool)
        {
            Buttons = new BoardButton[boardSize, boardSize];
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    Buttons[i, j] = new BoardButton()
                    {
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
                        Button_Click(sender2, e2, x, y);
                    };

                    Draw(i, j, boardBool[i, j]);
                }
            }
        }

        private void DrawX(Graphics g, int step)
        {
            if (step >= STEPS_HALF)
            {
                g.DrawLine(new Pen(Color.Red, 3), new Point(5, 5), new Point(BUTTON_SIZE - 5, BUTTON_SIZE - 5));
                g.DrawLine(new Pen(Color.Red, 3), new PointF(25, 5), new PointF(BUTTON_SIZE - 5 - (step - STEPS_HALF) / (STEPS_HALF / 20f), 5 + (step - STEPS_HALF) / (STEPS_HALF / 20f)));
            }
            else
                g.DrawLine(new Pen(Color.Red, 3), new Point(5, 5), new PointF(step / (STEPS_HALF / 20f) + 5, step / (STEPS_HALF / 20f) + 5));

        }

        private void DrawO(Graphics g, int step)
        {
            g.DrawArc(new Pen(Color.Blue, 3), new Rectangle(5, 5, 20, 20), 0, (360f / STEPS) * (step + 1));
        }

        private void Draw(int i, int j, bool? player)
        {
            Buttons[i, j].Graphics = Buttons[i, j].CreateGraphics();
            Graphics g = Buttons[i, j].Graphics;
            Timer timer1 = new Timer { Interval = 1 };
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
        private void CrossOutButton(int x, int y, int i, int step, CrossOutType type)
        {
            switch (type)
            {
                case CrossOutType.Row:
                    Buttons[x, y - i].Graphics.DrawLine(new Pen(Color.Black, 3), new PointF(0, BUTTON_SIZE / 2), new PointF(BUTTON_SIZE * (step / (float)CROSS_OUT_STEPS), BUTTON_SIZE / 2));
                    break;
                case CrossOutType.Column:
                    Buttons[x - i, y].Graphics.DrawLine(new Pen(Color.Black, 3), new PointF(BUTTON_SIZE / 2, 0), new PointF(BUTTON_SIZE / 2, BUTTON_SIZE * (step / (float)CROSS_OUT_STEPS)));
                    break;
                case CrossOutType.LeftDiagonal:
                    Buttons[x - i, y - i].Graphics.DrawLine(new Pen(Color.Black, 3), new PointF(0, 0), new PointF(BUTTON_SIZE * (step / (float)CROSS_OUT_STEPS), BUTTON_SIZE * (step / (float)CROSS_OUT_STEPS)));
                    break;
                case CrossOutType.RightDiagonal:
                    float calcX = BUTTON_SIZE - (float)(BUTTON_SIZE * (step / (float)CROSS_OUT_STEPS));
                    float calcY = BUTTON_SIZE * (step / (float)CROSS_OUT_STEPS);
                    Buttons[x - i, y + i].Graphics.DrawLine(new Pen(Color.Black, 3), new PointF(BUTTON_SIZE, 0), new PointF(calcX, calcY));
                    break;
                default:
                    break;
            }
        }

        private void CrossOutWinner(int rowL, int rowR, int diagLU, int diagRD, int diagLD, int diagRU, int columnU, int columnD, int x, int y)
        {
            if (rowL + rowR + 1 >= 5)
                CrossOut(rowL, rowR, x, y, CrossOutType.Row);
            else if (columnU + columnD + 1 >= 5)
                CrossOut(columnU, columnD, x, y, CrossOutType.Column);
            else if (diagLU + diagRD + 1 >= 5)
                CrossOut(diagLU, diagRD, x, y, CrossOutType.LeftDiagonal);
            else if (diagLD + diagRU + 1 >= 5)
                CrossOut(diagRU, diagLD, x, y, CrossOutType.RightDiagonal);
        }

        private void ChangeCrossedBool(int x, int y, int i, CrossOutType type)
        {
            switch (type)
            {
                case CrossOutType.Row:
                    Buttons[x, y - i].CrossOutType = type;
                    break;
                case CrossOutType.Column:
                    Buttons[x - i, y].CrossOutType = type;
                    break;
                case CrossOutType.LeftDiagonal:
                    Buttons[x - i, y - i].CrossOutType = type;
                    break;
                case CrossOutType.RightDiagonal:
                    Buttons[x - i, y + i].CrossOutType = type;
                    break;
                default:
                    break;
            }
        }

        private void CrossOut(int startSideCount, int endSideCount, int x, int y, CrossOutType type)
        {
            Queue<Timer> timerStack = new Queue<Timer>();
            for (int i = startSideCount; i >= 1; i--)
            {
                Timer timer1 = new Timer { Interval = 1 };
                int step = 0;
                int actualI = i;
                timer1.Tick += delegate (object sender2, EventArgs e2)
                {
                    if (step < CROSS_OUT_STEPS)
                        CrossOutButton(x, y, actualI, step++, type);
                    else
                    {
                        ChangeCrossedBool(x, y, actualI, type);
                        timer1.Stop();
                        if (timerStack.Count > 0)
                            timerStack.Dequeue().Start();
                    }
                };
                timerStack.Enqueue(timer1);
            }
            // clicked button
            Timer timer2 = new Timer { Interval = 1 };
            int step2 = 0;
            timer2.Tick += delegate (object sender2, EventArgs e2)
            {
                if (step2 < 25)
                    CrossOutButton(x, y, 0, step2++, type);
                else
                {
                    ChangeCrossedBool(x, y, 0, type);
                    timer2.Stop();
                    if (timerStack.Count > 0)
                        timerStack.Dequeue().Start();
                }
            };

            timerStack.Enqueue(timer2);
            for (int i = 1; i <= endSideCount; i++)
            {
                Timer timer1 = new Timer { Interval = 1 };
                int step = 0;
                int actualI = -1 * i;
                timer1.Tick += delegate (object sender2, EventArgs e2)
                {
                    if (step < 25)
                        CrossOutButton(x, y, actualI, step++, type);
                    else
                    {
                        ChangeCrossedBool(x, y, actualI, type);
                        timer1.Stop();
                        if (timerStack.Count > 0)
                            timerStack.Dequeue().Start();
                    }
                };
                timerStack.Enqueue(timer1);
            }

            timerStack.Dequeue().Start();
        }

        public delegate void WriteWinnerEventHandler(bool? xClicked);

        public event WriteWinnerEventHandler WinnerChanged;
        private void CheckWinnerFromLastClick(int i, int j, bool xClicked)
        {
            int rowL, rowR, columnD, columnU, diagLU, diagLD, diagRU, diagRD;
            bool rowLBool, rowRBool, columnDBool, columnUBool, diagLUBool, diagLDBool, diagRUBool, diagRDBool;
            rowLBool = rowRBool = columnDBool = columnUBool = diagLUBool = diagLDBool = diagRUBool = diagRDBool = true;
            rowL = rowR = columnD = columnU = diagLU = diagLD = diagRU = diagRD = 0;
            for (int k = 1; k < 5; k++)
            {
                if (j - k >= 0)
                {
                    //row left side
                    rowLBool = rowLBool && xClicked == Buttons[i, j - k].XClicked;
                    rowL = rowLBool ? ++rowL : rowL;
                    //diagonal up left side
                    if (i - k >= 0)
                    {
                        diagLUBool = diagLUBool && xClicked == Buttons[i - k, j - k].XClicked;
                        diagLU = diagLUBool ? ++diagLU : diagLU;
                    }
                    //diagonal down left side
                    if (i + k < BoardSize)
                    {
                        diagLDBool = diagLDBool && xClicked == Buttons[i + k, j - k].XClicked;
                        diagLD = diagLDBool ? ++diagLD : diagLD;
                    }

                }
                if (j + k < boardSize)
                {
                    //row right side
                    rowRBool = rowRBool && xClicked == Buttons[i, j + k].XClicked;
                    rowR = rowRBool ? ++rowR : rowR;
                    //diagonal up right side
                    if (i - k >= 0)
                    {
                        diagRUBool = diagRUBool && xClicked == Buttons[i - k, j + k].XClicked;
                        diagRU = diagRUBool ? ++diagRU : diagRU;
                    }
                    //diagonal down right side
                    if (i + k < BoardSize)
                    {
                        diagRDBool = diagRDBool && xClicked == Buttons[i + k, j + k].XClicked;
                        diagRD = diagRDBool ? ++diagRD : diagRD;

                    }
                }
                if (i - k >= 0)
                {
                    columnUBool = columnUBool && xClicked == Buttons[i - k, j].XClicked;
                    columnU = columnUBool ? ++columnU : columnU;
                }
                if (i + k < boardSize)
                {
                    columnDBool = columnDBool && xClicked == Buttons[i + k, j].XClicked;
                    columnD = columnDBool ? ++columnD : columnD;

                }
            }
            if (rowL + rowR + 1 >= 5 || diagLU + diagRD + 1 >= 5 || diagLD + diagRU + 1 >= 5 || columnU + columnD + 1 >= 5)
            {
                WinnerChanged?.Invoke(xClicked);
                CrossOutWinner(rowL, rowR, diagLU, diagRD, diagLD, diagRU, columnU, columnD, i, j);
                GameObject.GameOver = true;
            }
            else if (NumOfFreeButtons <= 0)
            {
                WinnerChanged?.Invoke(null);
                GameObject.GameOver = true;
            }
        }

        public delegate void TogglePlayerEventHandler();

        public event TogglePlayerEventHandler PlayerChange;

        void Button_Click(object sender, EventArgs e, int i, int j)
        {
            BoardButton button = sender as BoardButton;
            if (button.XClicked != null || gameObject.GameOver)
                return;
            --NumOfFreeButtons;
            button.XClicked = GameObject.XsTurn;
            Draw(i, j, GameObject.XsTurn);
            CheckWinnerFromLastClick(i, j, (bool)button.XClicked);
            PlayerChange?.Invoke();
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
            for (int i = 0; i <= boardSize - 5; i++)
            {
                for (int j = 0; j <= boardSize - 5; j++)
                {
                    if ((board[i, j] == true && board[i + 1, j + 1] == true && board[i + 2, j + 2] == true
                            && board[i + 3, j + 3] == true && board[i + 4, j + 4] == true)
                        || (board[i, j] == false && board[i + 1, j + 1] == false && board[i + 2, j + 2] == false
                            && board[i + 3, j + 3] == false && board[i + 4, j + 4] == false)
                        || (board[i, boardSize - j - 1] == true && board[i + 1, boardSize - (j + 2)] == true && board[i + 2, boardSize - (j + 3)] == true
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
    }
}
