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

        public GameForm(Form mainMenu, Game game)
        {
            this.mainMenu = mainMenu;
            GameObject = game;
            InitializeComponent();
        }

        private void GameForm_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < GameObject.GameBoard.BoardSize; i++)
            {
                for (int j = 0; j < GameObject.GameBoard.BoardSize; j++)
                {
                    splitContainer1.Panel2.Controls.Add(GameObject.GameBoard.Buttons[i, j]);
                    //TODO load game (new or saved), draw things etc
                }
            }
            
        }

        private void SaveGame()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Text files (*.txt)|*.txt";
            dialog.FileName = DateTime.UtcNow.ToString("MM-dd-yyyy");
            if (dialog.ShowDialog() != DialogResult.OK)
                return;
            Stream fileStream;
            if ((fileStream = dialog.OpenFile()) != null)
            {
                StreamWriter writer = new StreamWriter(fileStream, Encoding.UTF8);
                writer.WriteLine(Game.XsTurnToString());
                writer.WriteLine(GameObject.GameBoard.BoardSize);
                writer.WriteLine(GameObject.GameBoard.BoardBoolToString());
                writer.Close();
                fileStream.Close();
            }
            
        }

        private void GameForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //TODO end game exit (when someone wins)
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
            {
                e.Cancel = true;
            }

        }

        private void GameForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainMenu.Show();
        }
    }
}
