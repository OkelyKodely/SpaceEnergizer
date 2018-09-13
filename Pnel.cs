using System.Windows.Forms;

public class Pnel : Panel
{
    public Pnel()
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
