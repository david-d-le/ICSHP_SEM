using System;
using System.Drawing;
using System.Windows.Forms;

namespace ICSHP_SEM_Le
{
    public static class InputDialog
    {
        public static decimal? Show(string text, string header, decimal defaultValue, decimal min, decimal max)
        {
            if (defaultValue < min || defaultValue > max)
                throw new ArgumentException("invalid default value");
            Form prompt = new Form()
            {
                Width = 500,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                StartPosition = FormStartPosition.CenterScreen,
                Text = header
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Text = text, MaximumSize = new Size(400, 0), AutoSize = true, Font = new Font("Microsoft Sans Serif", 12) };
            NumericUpDown numericUpDown = new NumericUpDown() { Left = 50, Width = 400, DecimalPlaces = 0, Value = defaultValue, Minimum = min, Maximum = max, Font = new Font("Microsoft Sans Serif", 12) };
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(numericUpDown);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;
            numericUpDown.Top = textLabel.Height + 30;
            confirmation.Top = numericUpDown.Top + 30;
            prompt.Height = confirmation.Top + 75;
            return prompt.ShowDialog() == DialogResult.OK ? (decimal?)numericUpDown.Value : null;
        }
    }
}
