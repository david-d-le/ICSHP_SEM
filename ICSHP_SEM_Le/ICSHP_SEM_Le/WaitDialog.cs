using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ICSHP_SEM_Le
{
    public class WaitDialog
    {
        public string Text { get; set; }
        public string Heading { get; set; }
        public Form Prompt { get; set; }

        public WaitDialog(string text, string heading)
        {
            Text = text ?? throw new ArgumentNullException(nameof(text));
            Heading = heading ?? throw new ArgumentNullException(nameof(heading));

            Text = text;
            Heading = heading;
        }

        public void Show()
        {
            Prompt = new Form()
            {
                Width = 350,
                FormBorderStyle = FormBorderStyle.FixedSingle,
                StartPosition = FormStartPosition.CenterScreen,
                Text = Heading,
                ControlBox = false,
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Text = this.Text, MaximumSize = new Size(200, 0), AutoSize = true, Font = new Font("Microsoft Sans Serif", 12) };
            Prompt.Controls.Add(textLabel);
            Prompt.Height = textLabel.Height + 75;
            Prompt.Refresh();
            Prompt.Show();
            Prompt.Refresh();
        }
        
        public void Close()
        {
            Prompt.Close();
        }
    }
}
