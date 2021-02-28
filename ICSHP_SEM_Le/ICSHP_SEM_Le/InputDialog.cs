﻿using System;
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
                Height = 175,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterScreen,
                Text = header
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Text = text, Width=400 };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 100, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;
            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }
    }
}
