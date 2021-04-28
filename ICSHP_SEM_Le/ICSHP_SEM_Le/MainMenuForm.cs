using System;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

namespace ICSHP_SEM_Le
{
    public partial class MainMenuForm : Form
    {
        public MainMenuForm()
        {
            InitializeComponent();
        }

        private void EndBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void LoadGameBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "TicTacToe savegame file (*.save)|*.save|TicTacToe savegame file (*.txt)|*.txt"
            };
            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            using Stream fileStream = dialog.OpenFile();
            try
            {
                Game game;
                if (Path.GetExtension(dialog.FileName).Equals(".save"))
                    game = Game.DeserializeItem(fileStream as FileStream);
                else
                    throw new Exception("Invalid file extension");

                if (game.GameOver)
                {
                    MessageBox.Show("This savegame is completed. It can be used only for replay", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                GameForm form = new GameForm(this, game, false);
                form.Show();
                Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void NewGameBtn_Click(object sender, EventArgs e)
        {
            decimal? input = InputDialog.Show($"Insert board size from {GameBoard.MIN_BOARD_SIZE} to {GameBoard.MAX_BOARD_SIZE}. Board will have n*n dimmension", "Insert size", GameBoard.MIN_BOARD_SIZE, GameBoard.MIN_BOARD_SIZE, GameBoard.MAX_BOARD_SIZE);
            if (input == null)
                return;
            else if (input >= GameBoard.MIN_BOARD_SIZE && (input % 1 == 0) && input <= GameBoard.MAX_BOARD_SIZE && input >= GameBoard.MIN_BOARD_SIZE)
            {
                Game game = new Game((int)input);
                GameForm form = new GameForm(this, game, false);
                form.Show();
                Hide();
            }
            else
            {
                MessageBox.Show($"Invalid board size. Please insert whole numbers only from {GameBoard.MIN_BOARD_SIZE} to {GameBoard.MAX_BOARD_SIZE}", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void MainMenuForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to leave?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                e.Cancel = false;
            else
                e.Cancel = true;
        }

        private void ReplayBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "TicTacToe savegame file (*.save)|*.save"
            };
            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            Stream fileStream = dialog.OpenFile();
            try
            {
                Game game;
                if (Path.GetExtension(dialog.FileName).Equals(".save"))
                    game = Game.DeserializeItem(fileStream as FileStream);
                else
                    throw new Exception("Invalid file extension");

                game.GameBoard = new GameBoard(game,game.GameBoard.BoardSize);
                game.GameOver = false;
                game.XsTurn = true;
                foreach (GameBoard.BoardButton item in game.GameBoard.Buttons)
                {
                    item.Enabled = false;
                }
                GameForm form = new GameForm(this, game, true);

                form.Show();
                Hide();
                game.PlayReplay();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            fileStream.Close();
        }

        private void MainMenuForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics;
            graphics = CreateGraphics();
            int xPadding = 100;
            int yPadding = 150;

            graphics.DrawLine(new Pen(Color.Black, 3), new Point(30 + xPadding, yPadding), new Point(30 + xPadding, 90 + yPadding));
            graphics.DrawLine(new Pen(Color.Black, 3), new Point(60 + xPadding, yPadding), new Point(60 + xPadding, 90 + yPadding));
            graphics.DrawLine(new Pen(Color.Black, 3), new Point(xPadding, 30 + yPadding), new Point(90 + xPadding, 30 + yPadding));
            graphics.DrawLine(new Pen(Color.Black, 3), new Point(xPadding, 60 + yPadding), new Point(90 + xPadding, 60 + yPadding));

            graphics.DrawLine(new Pen(Color.Red, 3), new Point(5 + xPadding, 5 + yPadding), new Point(25 + xPadding, 25 + yPadding));
            graphics.DrawLine(new Pen(Color.Red, 3), new PointF(25 + xPadding, 5 + yPadding), new PointF(5 + xPadding, 25 + yPadding));
            graphics.DrawLine(new Pen(Color.Red, 3), new Point(35 + xPadding, 35 + yPadding), new Point(55 + xPadding, 55 + yPadding));
            graphics.DrawLine(new Pen(Color.Red, 3), new PointF(55 + xPadding, 35 + yPadding), new PointF(35 + xPadding, 55 + yPadding));
            graphics.DrawArc(new Pen(Color.Blue, 3), new Rectangle(65 + xPadding, 65 + yPadding, 20, 20), 0, 360);

        }
    }
}
