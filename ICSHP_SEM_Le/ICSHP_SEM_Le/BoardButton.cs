using System.Drawing;
using System.Windows.Forms;

namespace ICSHP_SEM_Le
{
    public partial class GameBoard
    {
        #region custom Button class
        public class BoardButton : Button
        {
            #region BoardButton properties
            public bool? XClicked { get; set; }
            public bool Painted { get; set; }
            public CrossOutType CrossOutType { get; set; }
            public Graphics Graphics { get; set; }
            #endregion

            #region BoardButton constructors
            public BoardButton(){
                CrossOutType = CrossOutType.None;
            }

            public BoardButton(bool xClicked) : base()
            {
                XClicked = xClicked;
            }
            #endregion

            protected override void OnPaint(PaintEventArgs pevent)
            {
                base.OnPaint(pevent);
                if (Painted)
                {
                    Graphics g = pevent.Graphics;
                    if (XClicked == true)
                    {
                        g.Clear(Color.Transparent);
                        g.FillRectangle(Brushes.White, new Rectangle(1, 1, BUTTON_SIZE - 2, BUTTON_SIZE - 2));
                        g.DrawLine(new Pen(Color.Red, 3), new Point(5, 5), new Point(BUTTON_SIZE - 5, BUTTON_SIZE - 5));
                        g.DrawLine(new Pen(Color.Red, 3), new Point(BUTTON_SIZE - 5, 5), new Point(5, BUTTON_SIZE - 5));
                    }
                    else if (XClicked == false)
                    {
                        g.DrawArc(new Pen(Color.Blue, 3), new Rectangle(5, 5, 20, 20), 0, 360);
                    }
                    switch (CrossOutType)
                    {
                        case CrossOutType.Row:
                            g.DrawLine(new Pen(Color.Black, 3), new Point(0, BUTTON_SIZE / 2), new Point(BUTTON_SIZE, BUTTON_SIZE / 2));
                            break;
                        case CrossOutType.Column:
                            g.DrawLine(new Pen(Color.Black, 3), new Point(BUTTON_SIZE / 2, 0), new Point(BUTTON_SIZE / 2, BUTTON_SIZE));
                            break;
                        case CrossOutType.LeftDiagonal:
                            g.DrawLine(new Pen(Color.Black, 3), new Point(0, 0), new Point(BUTTON_SIZE, BUTTON_SIZE));
                            break;
                        case CrossOutType.RightDiagonal:
                            g.DrawLine(new Pen(Color.Black, 3), new Point(BUTTON_SIZE, 0), new Point(0, BUTTON_SIZE));
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        #endregion
    }
}
