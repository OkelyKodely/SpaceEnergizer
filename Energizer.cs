using System;
using System.Windows.Forms;
using System.Drawing;
using WMPLib;

public class Energizer
{

    private WindowsMediaPlayer wmp = new WindowsMediaPlayer();

    private GifImage gi = new GifImage(Environment.CurrentDirectory + "\\space.gif");

    private System.Collections.Generic.List<Energy> caps = new System.Collections.Generic.List<Energy>();

    private int level = 0;

    private Random rd = new Random();

    private Graphics g;

    private Image bg;

    private System.Collections.Generic.List<Goal> goals = new System.Collections.Generic.List<Goal>();

    private System.Collections.Generic.List<Shape> shapes = new System.Collections.Generic.List<Shape>();

    public class Energy
    {
        public int x;

        public int y;
    }

    private enum lastKey
    {
        Up,
        Down,
        Left,
        Right
    }

    private lastKey lk = lastKey.Right;

    private Random r = new Random(); 

    public Frm form = new Frm();

    private Pnel panel = new Pnel();

    private GifImage shipleft, shipright, shipup, shipdown;

    private Image explosion, bulletOne, bulletTwo, bulletThree, bulletFour, energy;

    private Image submarineLeftUp, submarineLeftDown, submarineTopLeft, submarineTopRight, submarineRightUp, submarineRightDown, submarineBottomLeft, submarineBottomRight;

    private int onex, oney, twox, twoy, threex, threey, fourx, foury, fivex, fivey, sixx, sixy, sevenx, seveny, eightx, eighty;

    private bool oneup, twoleft, threeup, fourleft;

    private Ship ship;

    private bool fal = false;

    private bool start = true;

    public Energizer()
    {
        form.SetBounds(0, 0, 1280, 750);
        panel.SetBounds(0, 0, 1280, 750);
        form.Controls.Add(panel);

        g = panel.CreateGraphics();

        bg = gi.GetNextFrame();

        energy = Image.FromFile(Environment.CurrentDirectory + "\\energy.png");

        shipleft = new GifImage(Environment.CurrentDirectory + "\\shipleft.gif");
        shipright = new GifImage(Environment.CurrentDirectory + "\\shipright.gif");
        shipup = new GifImage(Environment.CurrentDirectory + "\\shipup.gif");
        shipdown = new GifImage(Environment.CurrentDirectory + "\\shipdown.gif");

        explosion = Image.FromFile(Environment.CurrentDirectory + "\\explosion.png");

        bulletOne = Image.FromFile(Environment.CurrentDirectory + "\\bulletOne.png");
        bulletTwo = Image.FromFile(Environment.CurrentDirectory + "\\bulletTwo.png");
        bulletThree = Image.FromFile(Environment.CurrentDirectory + "\\bulletThree.png");
        bulletFour = Image.FromFile(Environment.CurrentDirectory + "\\bulletFour.png");

        submarineLeftUp = Image.FromFile(Environment.CurrentDirectory + "\\submarineLeftUp.png");
        submarineLeftDown = Image.FromFile(Environment.CurrentDirectory + "\\submarineLeftDown.png");
        submarineTopLeft = Image.FromFile(Environment.CurrentDirectory + "\\submarineTopLeft.png");
        submarineTopRight = Image.FromFile(Environment.CurrentDirectory + "\\submarineTopRight.png");
        submarineRightUp = Image.FromFile(Environment.CurrentDirectory + "\\submarineRightUp.png");
        submarineRightDown = Image.FromFile(Environment.CurrentDirectory + "\\submarineRightDown.png");
        submarineBottomLeft = Image.FromFile(Environment.CurrentDirectory + "\\submarineBottomLeft.png");
        submarineBottomRight = Image.FromFile(Environment.CurrentDirectory + "\\submarineBottomRight.png");

        ship = new Ship();
        ship.x = 5;
        ship.y = 4;
        ship.direction = "right";
    }

    private void SetGoals(int level)
    {
        for (int x = 0; x < level/2; x++)
        {
            Goal triangleg = new Goal();
            triangleg.type = "triangle";
            triangleg.x = rd.Next(40) + 10;
            triangleg.y = rd.Next(30) + 10;
            goals.Add(triangleg);
            for (int i = 0; i < level/2; i++)
            {
                Shape triangle = new Shape();
                triangle.type = "triangle";
                triangle.x = rd.Next(40) + 10;
                triangle.y = rd.Next(30) + 10;
                shapes.Add(triangle);
            }

            Goal squareg = new Goal();
            squareg.type = "square";
            squareg.x = rd.Next(40) + 10;
            squareg.y = rd.Next(30) + 10;
            goals.Add(squareg);
            for (int i = 0; i < level / 2; i++)
            {
                Shape square = new Shape();
                square.type = "square";
                square.x = rd.Next(40) + 10;
                square.y = rd.Next(30) + 10;
                shapes.Add(square);
            }

            Goal rectangleg = new Goal();
            rectangleg.type = "rectangle";
            rectangleg.x = rd.Next(40) + 10;
            rectangleg.y = rd.Next(30) + 10;
            goals.Add(rectangleg);
            for (int i = 0; i < level / 2; i++)
            {
                Shape rectangle = new Shape();
                rectangle.type = "rectangle";
                rectangle.x = rd.Next(40) + 10;
                rectangle.y = rd.Next(30) + 10;
                shapes.Add(rectangle);
            }

            Goal circleg = new Goal();
            circleg.type = "circle";
            circleg.x = rd.Next(40) + 10;
            circleg.y = rd.Next(30) + 10;
            goals.Add(circleg);
            for (int i = 0; i < level / 2; i++)
            {
                Shape circle = new Shape();
                circle.type = "circle";
                circle.x = rd.Next(40) + 10;
                circle.y = rd.Next(30) + 10;
                shapes.Add(circle);
            }
        }
    }

    public void Play()
    {
        if(start)
        {
            start = false;
            form.StartPosition = FormStartPosition.Manual;
            form.Left = 0;
            form.Top = 0;
        }

        SetSubmarinePositions();

        Timer timer = new Timer();
        timer.Interval = 40;
        timer.Tick += new EventHandler(Move);
        timer.Start();

        Timer timer2 = new Timer();
        timer2.Interval = 4;
        timer2.Tick += new EventHandler(Mv);
        timer2.Start();

        form.KeyDown += new KeyEventHandler(MoveShip);

        PlaySong(null, null);

        Timer timer3 = new Timer();
        timer3.Interval = 3 * 60 * 1000;
        timer3.Tick += new EventHandler(PlaySong);
        timer3.Start();

        Timer timer4 = new Timer();
        timer4.Interval = 600;
        timer4.Tick += new EventHandler(AnimBg);
        //timer4.Start();
    }

    private void PlaySong(object sender, EventArgs e)
    {
        wmp.URL = "raiden2lvl1.mp3";
        wmp.controls.play();
    }

    private void MoveShip(object sender, KeyEventArgs e)
    {
        if(e.KeyCode == Keys.Up)
        {
            if(lk != lastKey.Down)
            {
                ship.direction = "up";
                lk = lastKey.Up;
            }
        }
        else if(e.KeyCode == Keys.Down)
        {
            if(lk != lastKey.Up)
            {
                ship.direction = "down";
                lk = lastKey.Down;
            }
        }
        else if(e.KeyCode == Keys.Left)
        {
            if(lk != lastKey.Right)
            {
                ship.direction = "left";
                lk = lastKey.Left;
            }
        }
        else if(e.KeyCode == Keys.Right)
        {
            if(lk != lastKey.Left)
            {
                ship.direction = "right";
                lk = lastKey.Right;
            }
        }
    }

    private void SetSubmarinePositions()
    {
        onex = 0;
        oney = 600;
        oneup = true;

        threex = 1190;
        threey = 300;
        threeup = true;

        twox = 600;
        twoy = 0;
        twoleft = true;

        fourx = 1000;
        foury = 630;
        fourleft = true;
    }

    private void Mv(object sender, EventArgs e)
    {
        for (int x = 0; x < goals.Count; x++)
        {
            Image goal = null;
            if (goals[x].type.Equals("triangle"))
            {
                goal = Image.FromFile(Environment.CurrentDirectory + "\\shape1.png");
            }
            else if (goals[x].type.Equals("square"))
            {
                goal = Image.FromFile(Environment.CurrentDirectory + "\\shape2.png");
            }
            else if (goals[x].type.Equals("rectangle"))
            {
                goal = Image.FromFile(Environment.CurrentDirectory + "\\shape3.png");
            }
            else if (goals[x].type.Equals("circle"))
            {
                goal = Image.FromFile(Environment.CurrentDirectory + "\\shape4.png");
            }
            g.DrawImage(goal, goals[x].x * 20, goals[x].y * 20, 70, 70);
        }

        for (int x = 0; x < shapes.Count; x++)
        {
            Image shape = null;
            if (shapes[x].type.Equals("triangle"))
            {
                shape = Image.FromFile(Environment.CurrentDirectory + "\\shape1.png");
            }
            else if (shapes[x].type.Equals("square"))
            {
                shape = Image.FromFile(Environment.CurrentDirectory + "\\shape2.png");
            }
            else if (shapes[x].type.Equals("rectangle"))
            {
                shape = Image.FromFile(Environment.CurrentDirectory + "\\shape3.png");
            }
            else if (shapes[x].type.Equals("circle"))
            {
                shape = Image.FromFile(Environment.CurrentDirectory + "\\shape4.png");
            }
            g.DrawImage(shape, shapes[x].x * 20, shapes[x].y * 20, 45, 45);
        }

        if (ship.x < 0 || ship.x > 58 || ship.y < 0 || ship.y > 40)
        {
            Application.Exit();
        }

        if (bg != null)
        {
            g.DrawImage(bg, 0, 0, panel.Width, panel.Height);
            fal = false;
        }

        for (int i = 0; i < caps.Count; i++)
        {
            g.DrawImage(energy, caps[i].x * 20, caps[i].y * 20, 50, 50);
            if (ship.x >= caps[i].x - 1 && ship.x <= caps[i].x + 1 && ship.y >= caps[i].y - 1 && ship.y <= caps[i].y + 1)
            {
                ship.life += 10;
                caps.Remove(caps[i]);
            }
        }

        if (ship.direction.Equals("up"))
        {
            Image gg = shipup.GetNextFrame();
            g.DrawImage(gg, ship.x * 20, ship.y * 20, 100, 100);
            ship.y -= 1;
        }
        if (ship.direction.Equals("down"))
        {
            Image gg = shipdown.GetNextFrame();
            g.DrawImage(gg, ship.x * 20, ship.y * 20, 100, 100);
            ship.y += 1;
        }
        if (ship.direction.Equals("left"))
        {
            Image gg = shipleft.GetNextFrame();
            g.DrawImage(gg, ship.x * 20, ship.y * 20, 100, 100);
            ship.x -= 1;
        }
        if (ship.direction.Equals("right"))
        {
            Image gg = shipright.GetNextFrame();
            g.DrawImage(gg, ship.x * 20, ship.y * 20, 100, 100);
            ship.x += 1;
        }

        if (oneup)
        {
            g.DrawImage(submarineLeftUp, onex, oney, 80, 300);
        }
        else
        {
            g.DrawImage(submarineLeftDown, onex, oney, 80, 300);
        }

        if (threeup)
        {
            g.DrawImage(submarineRightUp, threex, threey, 80, 300);
        }
        else
        {
            g.DrawImage(submarineRightDown, threex, threey, 80, 300);
        }




        if (twoleft)
        {
            g.DrawImage(submarineTopLeft, twox, twoy, 300, 80);
        }
        else
        {
            g.DrawImage(submarineTopRight, twox, twoy, 300, 80);
        }

        if (fourleft)
        {
            g.DrawImage(submarineBottomLeft, fourx, foury, 300, 80);
        }
        else
        {
            g.DrawImage(submarineBottomRight, fourx, foury, 300, 80);
        }

        if (ship.direction.Equals("right"))
        {
            for (int x = 0; x < shapes.Count; x++)
            {
                if (ship.x == shapes[x].x - 2 && ship.y >= shapes[x].y - 2 && ship.y <= shapes[x].y + 2)
                {
                    shapes[x].x++;
                }
            }
        }
        else if (ship.direction.Equals("left"))
        {
            for (int x = 0; x < shapes.Count; x++)
            {
                if (ship.x == shapes[x].x + 2 && ship.y >= shapes[x].y - 2 && ship.y <= shapes[x].y + 2)
                {
                    shapes[x].x--;
                }
            }
        }
        else if (ship.direction.Equals("up"))
        {
            for (int x = 0; x < shapes.Count; x++)
            {
                if (ship.y == shapes[x].y + 2 && ship.x >= shapes[x].x - 2 && ship.x <= shapes[x].x + 2)
                {
                    shapes[x].y--;
                }
            }
        }
        else if (ship.direction.Equals("down"))
        {
            for (int x = 0; x < shapes.Count; x++)
            {
                if (ship.y == shapes[x].y - 2 && ship.x >= shapes[x].x - 2 && ship.x <= shapes[x].x + 2)
                {
                    shapes[x].y++;
                }
            }
        }

        try
        {
            for (int i = 0; i < shapes.Count; i++)
            {
                for (int j = 0; j < goals.Count; j++)
                {
                    if (shapes[i].x >= goals[j].x - 2 && shapes[i].x <= goals[j].x + 2 &&
                        shapes[i].y >= goals[j].y - 2 && shapes[i].y <= goals[j].y + 2)
                    {
                        if (shapes[i].type.Equals(goals[j].type))
                        {
                            wmp.URL = "explosion.wav";
                            wmp.controls.play();
                            shapes.Remove(shapes[i]);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    private void Level1()
    {
        if (level == 0)
        {
            if (shapes.Count == 0)
            {
                level++;
                Lvl1.ToCaps(caps);
            }
        }
    }

    private void Level2()
    {
        if (level == 1)
        {
            if (shapes.Count == 0 && caps.Count == 0)
            {
                level++;
                Lvl2.ToCaps(caps);
            }
        }
    }

    private void Level3()
    {
        if (level == 2)
        {
            if (shapes.Count == 0 && caps.Count == 0)
            {
                level++;
                Lvl3.ToCaps(caps);
            }
        }
    }

    private void Level4()
    {
        if (level == 3)
        {
            if (shapes.Count == 0 && caps.Count == 0)
            {
                level++;
                Lvl4.ToCaps(caps);
            }
        }
    }

    private void Level5()
    {
        if (level == 4)
        {
            if (shapes.Count == 0 && caps.Count == 0)
            {
                level++;
                Lvl5.ToCaps(caps);
            }
        }
    }

    private void Level6()
    {
        if (level == 5)
        {
            if (shapes.Count == 0 && caps.Count == 0)
            {
                level++;
                Lvl6.ToCaps(caps);
            }
        }
    }

    private void Level7()
    {
        if (level == 6)
        {
            if (shapes.Count == 0 && caps.Count == 0)
            {
                level++;
                Lvl7.ToCaps(caps);
            }
        }
    }

    private void AnimBg(object sender, EventArgs e)
    {
        bg = gi.GetNextFrame();
        fal = true;
    }

    private void Move(object sender, EventArgs e)
    {
        ship.life--;

        form.Text = "Energy: " + ship.life;
        if (ship.life <= 0)
        {
            Application.Exit();
        }

        Level1();
        Level2();
        Level3();
        Level4();
        Level5();
        Level6();
        Level7();

        if (oneup)
        {
            oney-=100;
            g.DrawImage(submarineLeftUp, onex, oney, 20, 200);
            int v = r.Next(30);
            if (v == 1)
            {
                Glock gl = new Glock(explosion, g, onex / 20, oney / 20, bulletOne, ship);
                gl.exe(gl.randomizeShiets(gl.Sheut()));
            }
            if (oney < 1)
            {
                oneup = false;
            }
        }
        else
        {
            oney+=100;
            g.DrawImage(submarineLeftDown, onex, oney, 20, 200);
            int v = r.Next(30);
            if (v == 1)
            {
                Glock gl = new Glock(explosion, g, onex / 20, oney / 20, bulletOne, ship);
                gl.exe(gl.randomizeShiets(gl.Sheut()));
            }
            if (oney > 803)
            {
                oneup = true;
            }
        }

        if (threeup)
        {
            threey -= 100;
            g.DrawImage(submarineRightUp, threex, threey, 20, 200);
            int v = r.Next(30);
            if (v == 1)
            {
                Glock gl = new Glock(explosion, g, threex / 20, threey / 20, bulletThree, ship);
                gl.opp();
                gl.exe(gl.randomizeShiets(gl.Sheut()));
            }
            if (threey < 1)
            {
                threeup = false;
            }
        }
        else
        {
            threey += 100;
            g.DrawImage(submarineRightDown, threex, threey, 20, 200);
            int v = r.Next(30);
            if (v == 1)
            {
                Glock gl = new Glock(explosion, g, threex / 20, threey / 20, bulletThree, ship);
                gl.opp();
                gl.exe(gl.randomizeShiets(gl.Sheut()));
            }
            if (threey > 803)
            {
                threeup = true;
            }
        }




        if (twoleft)
        {
            twox -= 100;
            g.DrawImage(submarineTopLeft, twox, twoy, 300, 80);
            int v = r.Next(30);
            if (v == 1)
            {
                Glock gl = new Glock(explosion, g, twox / 20, twoy / 20, bulletTwo, ship);
                gl.ver();
                gl.exe(gl.randomizeShiets(gl.Sheut()));
            }
            if (twox < 1)
            {
                twoleft = false;
            }
        }
        else
        {
            twox += 100;
            g.DrawImage(submarineTopRight, twox, twoy, 300, 80);
            int v = r.Next(30);
            if (v == 1)
            {
                Glock gl = new Glock(explosion, g, twox / 20, twoy / 20, bulletTwo, ship);
                gl.ver();
                gl.exe(gl.randomizeShiets(gl.Sheut()));
            }
            if (twox > 1280)
            {
                twoleft = true;
            }
        }

        if (fourleft)
        {
            fourx -= 100;
            if (fourx < 0)
                fourx += 100;
            g.DrawImage(submarineBottomLeft, fourx, foury, 300, 80);
            int v = r.Next(30);
            if (v == 1)
            {
                Glock gl = new Glock(explosion, g, fourx / 20, foury / 20, bulletFour, ship);
                gl.ver();
                gl.opp();
                gl.exe(gl.randomizeShiets(gl.Sheut()));
            }
            if (fourx < 1)
            {
                fourleft = false;
            }
        }
        else
        {
            fourx += 100;
            if (fourx > 1280)
            {
                fourleft = true;
            }
            if (fourx > 1280)
                fourx -= 100;
            g.DrawImage(submarineBottomRight, fourx, foury, 300, 80);
            int v = r.Next(30);
            if (v == 1)
            {
                Glock gl = new Glock(explosion, g, fourx / 20, foury / 20, bulletFour, ship);
                gl.ver();
                gl.opp();
                gl.exe(gl.randomizeShiets(gl.Sheut()));
            }
        }
    
    }

    public static void Main(string[] args)
    {
        Energizer energizer = new Energizer();
        energizer.Play();
        Application.Run(energizer.form);
    }
}

public class Ship
{
    public int x;

    public int y;

    public string direction = "up";

    public int life = 100;
}