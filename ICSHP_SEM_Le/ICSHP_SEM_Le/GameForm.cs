using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ICSHP_SEM_Le
{
    public partial class GameForm : Form
    {
        private readonly Form mainMenu;
        public Form MainMenu
        {
            get { return mainMenu; }
        }

        public Game GameObject { get; set; }

        #region events
        private void TogglePlayer()
        {
            if (GameObject.GameOver == true)
            {
                SetPlayerLabelText("");
                return;
            }
            GameObject.XsTurn = !GameObject.XsTurn;
            SetPlayerLabelFontColor(GameObject.XsTurn);
            SetPlayerLabelText(GameObject.XsTurnToString());
        }

        private void WinnerChanged(bool? xClicked)
        {
            SetWinnerLabelFontColor(xClicked);
            string message = (xClicked == true) ? "Player X wins" : (xClicked == false ? "Player O wins" : "It's a tie");
            SetWinnerLabelText(message);
        }
        #endregion

        public GameForm(Form mainMenu, Game game)
        {
            this.mainMenu = mainMenu;
            GameObject = game;
            InitializeComponent();
            game.GameBoard.PlayerChange += TogglePlayer;
            game.GameBoard.WinnerChanged += WinnerChanged;
        }

        private void GameForm_Load(object sender, EventArgs e)
        {
            WaitDialog wd = new WaitDialog("Your level is getting ready. Please wait...", "Please wait...");
            wd.Show();
            for (int i = 0; i < GameObject.GameBoard.BoardSize; i++)
            {
                for (int j = 0; j < GameObject.GameBoard.BoardSize; j++)
                {
                    splitContainer1.Panel2.Controls.Add(GameObject.GameBoard.Buttons[i, j]);
                }
            }
            SetPlayerLabelFontColor(GameObject.XsTurn);
            SetPlayerLabelText(GameObject.XsTurnToString());
            wd.Close();
        }

        private void SaveGame()
        {
            SaveFileDialog dialog2 = new SaveFileDialog();
            dialog2.Filter = "TicTacToe savegame file (*.save)|*.save|TicTacToe savegame file (*.txt)|*.txt";
            dialog2.FileName = DateTime.UtcNow.ToString("MM-dd-yyyy");
            if (dialog2.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                if (Path.GetExtension(dialog2.FileName).Equals(".save"))
                    GameObject.SerializeItem(Path.GetFullPath(dialog2.FileName));
                else if (Path.GetExtension(dialog2.FileName).Equals(".txt"))
                {
                    Stream fileStream;
                    if ((fileStream = dialog2.OpenFile()) != null)
                    {
                        StreamWriter writer = new StreamWriter(fileStream, Encoding.UTF8);
                        writer.WriteLine(GameObject.XsTurnToString());
                        writer.WriteLine(GameObject.GameBoard.BoardSize);
                        writer.WriteLine(GameObject.GameBoard.BoardToString());
                        writer.Close();
                        fileStream.Close();
                    }
                }
                else
                    throw new Exception("Invalid file extension");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GameForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (GameObject.GameOver == false)
            {
                if (MessageBox.Show("Do you want to exit from this game?", "Confirmation",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (MessageBox.Show("Do you want to save this game progress?", "Confirmation",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        SaveGame();
                    }
                    e.Cancel = false;
                }
                else
                    e.Cancel = true;
            }
        }

        private void GameForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainMenu.Show();
        }

        #region public methods
        public void SetPlayerLabelText(string text)
        {
            playerLabel.Text = text;
        }

        public void SetPlayerLabelFontColor(bool xPlaying)
        {
            playerLabel.ForeColor = xPlaying ? Color.Red : Color.Blue;
        }

        public void SetWinnerLabelText(string text)
        {
            winnerLabel.Text = text;
        }

        public void SetWinnerLabelFontColor(bool? playerXWins)
        {
            if (playerXWins == null)
                winnerLabel.ForeColor = Color.Black;
            else
                winnerLabel.ForeColor = playerXWins == true ? Color.Red : Color.Blue;
        }

        public SplitterPanel GetSplitterPanel2() {
            return splitContainer1.Panel2;
        }

        #endregion

        private void mainMenuBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
