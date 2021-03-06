using System;
using System.Drawing;
using System.Windows.Forms;

namespace ICSHP_SEM_Le
{
    public static class InputDialog
    {
        public static string Show(string text, string header)
        {
            Form prompt = new Form()
            {
                Width = 500,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterScreen,
                Text = header
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Text = text, MaximumSize = new Size(400, 0), AutoSize = true, Font = new Font("Microsoft Sans Serif", 12) };
            TextBox textBox = new TextBox() { Left = 50, Width = 400, Font = new Font("Microsoft Sans Serif", 12) };
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;
            textBox.Top = textLabel.Height + 30;
            confirmation.Top = textBox.Top + 30;
            prompt.Height = confirmation.Top + 75;
            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }
    }
}
