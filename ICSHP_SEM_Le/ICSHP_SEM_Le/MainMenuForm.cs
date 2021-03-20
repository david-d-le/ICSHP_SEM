using System;
using System.Windows.Forms;
using System.IO;

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
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            Stream fileStream = dialog.OpenFile();
            try
            {
                Game game = new Game(fileStream);
                GameForm form = new GameForm(this, game);
                form.Show();
                Hide();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            fileStream.Close();
        }

        private void NewGameBtn_Click(object sender, EventArgs e)
        {

            string input = InputDialog.Show($"Insert board size from {GameBoard.MIN_BOARD_SIZE} to {GameBoard.MAX_BOARD_SIZE}. Board will have n*n dimmension", "Insert size");
            if (input == "")
                return;
            else if (int.TryParse(input, out int size) && size >= GameBoard.MIN_BOARD_SIZE && size <= GameBoard.MAX_BOARD_SIZE)
            {
                Game game = new Game(size);
                GameForm form = new GameForm(this, game);
                form.Show();
                Hide();
            }
            else
            {
                MessageBox.Show("Invalid board size. Please insert whole numbers only from 5 to 30", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
    }
}
