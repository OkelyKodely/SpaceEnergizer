using System.Windows.Forms;

public class Frm : Form
{
    public Frm()
    {
        this.DoubleBuffered = true;
        this.SetStyle(ControlStyles.UserPaint |
                      ControlStyles.AllPaintingInWmPaint |
                      ControlStyles.ResizeRedraw |
                      ControlStyles.ContainerControl |
                      ControlStyles.OptimizedDoubleBuffer |
                      ControlStyles.SupportsTransparentBackColor
                      , true);
    }
}
