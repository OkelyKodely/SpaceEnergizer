using System;
using System.Windows.Forms;
using System.Drawing;
using WMPLib;

public class Energizer
{
    private bool gameOver = false;

    private Timer timer = new Timer();

    private Timer timer2 = new Timer();

    private Timer timer3 = new Timer();

    private Timer timer4 = new Timer();

    private WindowsMediaPlayer wmp = new WindowsMediaPlayer();

    private GifImage gi = new GifImage(Environment.CurrentDirectory + "\\space.gif");

    private System.Collections.Generic.List<Energy> caps = new System.Collections.Generic.List<Energy>();

    private int level = 0;

    private Random rd = new Random();

    private Graphics g;

    private Image bg;

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

        timer.Interval = 40;
        timer.Tick += new EventHandler(Move);
        timer.Start();

        timer2.Interval = 200;
        timer2.Tick += new EventHandler(Mv);
        timer2.Start();

        form.KeyDown += new KeyEventHandler(MoveShip);

        PlaySong(null, null);

        timer3.Interval = 3 * 60 * 1000;
        timer3.Tick += new EventHandler(PlaySong);
        timer3.Start();

        timer4.Interval = 200;
        timer4.Tick += new EventHandler(AnimBg);
        timer4.Start();
    }

    private void PlaySong(object sender, EventArgs e)
    {
        wmp.URL = "raiden2lvl1.mp3";
        wmp.controls.play();
    }

    private void StopSong()
    {
        wmp.controls.stop();
    }

    private void MoveShip(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            if(gameOver)
            {
                ship.x = 5;
                ship.y = 4;
                ship.direction = "right";
                ship.life = 100;

                level = 0;

                caps = new System.Collections.Generic.List<Energy>();

                timer.Start();
                timer2.Start();

                PlaySong(null, null);

                timer3.Start();
            }
        }
        if (e.KeyCode == Keys.Up)
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
        if (ship.x < 0 || ship.x > 62 || ship.y < 0 || ship.y > 36)
        {
            doGameOver();
            return;
        }

        if (bg != null)
        {
            g.DrawImage(bg, 0, 0, panel.Width, panel.Height);
            fal = false;
        }

        for (int i = 0; i < caps.Count; i++)
        {
            g.DrawImage(energy, caps[i].x * 20, caps[i].y * 20, 20, 20);
            if (ship.x >= caps[i].x - 1 && ship.x <= caps[i].x + 1 && ship.y >= caps[i].y - 1 && ship.y <= caps[i].y + 1)
            {
                ship.life += 8;
                caps.Remove(caps[i]);
            }
        }

        if (ship.direction.Equals("up"))
        {
            Image gg = shipup.GetNextFrame();
            g.DrawImage(gg, ship.x * 20, ship.y * 20, 40, 40);
            ship.y -= 1;
        }
        if (ship.direction.Equals("down"))
        {
            Image gg = shipdown.GetNextFrame();
            g.DrawImage(gg, ship.x * 20, ship.y * 20, 40, 40);
            ship.y += 1;
        }
        if (ship.direction.Equals("left"))
        {
            Image gg = shipleft.GetNextFrame();
            g.DrawImage(gg, ship.x * 20, ship.y * 20, 40, 40);
            ship.x -= 1;
        }
        if (ship.direction.Equals("right"))
        {
            Image gg = shipright.GetNextFrame();
            g.DrawImage(gg, ship.x * 20, ship.y * 20, 40, 40);
            ship.x += 1;
        }
    }

    private void Level1()
    {
        if (level == 0)
        {
            level++;
            Lvl1.ToCaps(caps);
        }
    }

    private void Level2()
    {
        if (level == 1)
        {
            if (caps.Count == 0)
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
            if (caps.Count == 0)
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
            if (caps.Count == 0)
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
            if (caps.Count == 0)
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
            if (caps.Count == 0)
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
            if (caps.Count == 0)
            {
                level++;
                Lvl7.ToCaps(caps);
            }
        }
    }

    private void Level8()
    {
        if (level == 7)
        {
            if (caps.Count == 0)
            {
                level++;
                Lvl8.ToCaps(caps);
            }
        }
    }

    private void Level9()
    {
        if (level == 8)
        {
            if (caps.Count == 0)
            {
                level++;
                Lvl9.ToCaps(caps);
            }
        }
    }
    private void Level10()
    {
        if (level == 9)
        {
            if (caps.Count == 0)
            {
                level++;
                Lvl10.ToCaps(caps);
            }
        }
    }
    private void Level11()
    {
        if (level == 10)
        {
            if (caps.Count == 0)
            {
                level++;
                Lvl11.ToCaps(caps);
            }
        }
    }
    private void Level12()
    {
        if (level == 11)
        {
            if (caps.Count == 0)
            {
                level++;
                Lvl12.ToCaps(caps);
            }
        }
    }
    private void Level13()
    {
        if (level == 12)
        {
            if (caps.Count == 0)
            {
                level++;
                Lvl13.ToCaps(caps);
            }
        }
    }
    private void Level14()
    {
        if (level == 13)
        {
            if (caps.Count == 0)
            {
                level++;
                Lvl14.ToCaps(caps);
            }
        }
    }
    private void Level15()
    {
        if (level == 14)
        {
            if (caps.Count == 0)
            {
                level++;
                Lvl15.ToCaps(caps);
            }
        }
    }
    private void Level16()
    {
        if (level == 15)
        {
            if (caps.Count == 0)
            {
                level++;
                Lvl16.ToCaps(caps);
            }
        }
    }
    private void Level17()
    {
        if (level == 16)
        {
            if (caps.Count == 0)
            {
                level++;
                Lvl17.ToCaps(caps);
            }
        }
    }
    private void Level18()
    {
        if (level == 17)
        {
            if (caps.Count == 0)
            {
                level++;
                Lvl18.ToCaps(caps);
            }
        }
    }
    private void Level19()
    {
        if (level == 18)
        {
            if (caps.Count == 0)
            {
                level++;
                Lvl19.ToCaps(caps);
            }
        }
    }
    private void Level20()
    {
        if (level == 19)
        {
            if (caps.Count == 0)
            {
                level++;
                Lvl20.ToCaps(caps);
            }
        }
    }
    private void Level21()
    {
        if (level == 20)
        {
            if (caps.Count == 0)
            {
                level++;
                Lvl21.ToCaps(caps);
            }
        }
    }
    private void Level22()
    {
        if (level == 21)
        {
            if (caps.Count == 0)
            {
                level++;
                Lvl22.ToCaps(caps);
            }
        }
    }
    private void Level23()
    {
        if (level == 22)
        {
            if (caps.Count == 0)
            {
                level++;
                Lvl23.ToCaps(caps);
            }
        }
    }
    private void Level24()
    {
        if (level == 23)
        {
            if (caps.Count == 0)
            {
                level++;
                Lvl24.ToCaps(caps);
            }
        }
    }
    private void Level25()
    {
        if (level == 24)
        {
            if (caps.Count == 0)
            {
                level++;
                Lvl25.ToCaps(caps);
            }
        }
    }
    private void Level26()
    {
        if (level == 25)
        {
            if (caps.Count == 0)
            {
                level++;
                Lvl26.ToCaps(caps);
            }
        }
    }
    private void Level27()
    {
        if (level == 26)
        {
            if (caps.Count == 0)
            {
                level++;
                Lvl27.ToCaps(caps);
            }
        }
    }
    private void Level28()
    {
        if (level == 27)
        {
            if (caps.Count == 0)
            {
                level++;
                Lvl28.ToCaps(caps);
            }
        }
    }

    private void Level29()
    {
        if (level == 28)
        {
            if (caps.Count == 0)
            {
                level++;
                Lvl29.ToCaps(caps);
            }
        }
    }
    private void Level30()
    {
        if (level == 29)
        {
            if (caps.Count == 0)
            {
                level++;
                Lvl30.ToCaps(caps);
            }
        }
    }

    private void AnimBg(object sender, EventArgs e)
    {
        bg = gi.GetNextFrame();
        fal = true;
    }

    private void doGameOver()
    {
        timer.Stop();
        timer2.Stop();
        timer3.Stop();

        StopSong();

        form.Text = "Game Over, Energy: 0 (Press ENTER to replay)";

        gameOver = true;
    }

    private void Move(object sender, EventArgs e)
    {
        ship.life--;

        if (ship.life <= 0)
        {
            doGameOver();
            return;
        }

        form.Text = "Energy: " + ship.life;

        Level1();
        Level2();
        Level3();
        Level4();
        Level5();
        Level6();
        Level7();
        Level8();
        Level9();
        Level10();
        Level11();
        Level12();
        Level13();
        Level14();
        Level15();
        Level16();
        Level17();
        Level18();
        Level19();
        Level20();
        Level21();
        Level22();
        Level23();
        Level24();
        Level25();
        Level26();
        Level27();
        Level28();
        Level29();
        Level30();

        if(level == 30)
        {
            if(caps.Count == 0)
            {
                doGameOver();
                form.Text = "You won the galaxy!";
                return;
            }
        }

        if (oneup)
        {
            oney-=10;
            g.DrawImage(submarineLeftUp, onex, oney, 80, 300);
            int v = r.Next((35 - level)*15 - level/2);
            if (v == 1 || v == 2 || v == 3 || v == 4)
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
            oney+=10;
            g.DrawImage(submarineLeftDown, onex, oney, 80, 300);
            int v = r.Next((35 - level) * 15 - level/2);
            if (v == 1 || v == 2 || v == 3 || v == 4)
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
            threey -= 10;
            g.DrawImage(submarineRightUp, threex, threey, 80, 300);
            int v = r.Next((35 - level) * 15 - level/2);
            if (v == 1 || v == 2 || v == 3 || v == 4)
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
            threey += 10;
            g.DrawImage(submarineRightDown, threex, threey, 80, 300);
            int v = r.Next((35 - level) * 15 - level/2);
            if (v == 1 || v == 2 || v == 3 || v == 4)
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
            twox -= 10;
            g.DrawImage(submarineTopLeft, twox, twoy, 300, 80);
            int v = r.Next((35 - level) * 15 - level/2);
            if (v == 1 || v == 2 || v == 3 || v == 4)
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
            twox += 10;
            g.DrawImage(submarineTopRight, twox, twoy, 300, 80);
            int v = r.Next((35 - level) * 15 - level/2);
            if (v == 1 || v == 2 || v == 3 || v == 4)
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
            fourx -= 10;
            if (fourx < 0)
                fourx += 10;
            g.DrawImage(submarineBottomLeft, fourx, foury, 300, 80);
            int v = r.Next((35 - level) * 15 - level/2);
            if (v == 1 || v == 2 || v == 3 || v == 4)
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
            fourx += 10;
            if (fourx > 1280)
            {
                fourleft = true;
            }
            if (fourx > 1280)
                fourx -= 10;
            g.DrawImage(submarineBottomRight, fourx, foury, 300, 80);
            int v = r.Next((35 - level) * 15 - level/2);
            if (v == 1 || v == 2 || v == 3 || v == 4)
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