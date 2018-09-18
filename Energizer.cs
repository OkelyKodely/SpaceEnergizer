using System;
using System.Windows.Forms;
using System.Drawing;
using WMPLib;

public class Energizer
{
    public Frm form = new Frm();

    private Pnel panel = new Pnel();

    private bool gameOver = false;

    private Timer timer = new Timer();

    private Timer timer2 = new Timer();

    private Timer timer3 = new Timer();

    //private Timer timer4 = new Timer();

    private System.Collections.Generic.List<GlockBullies> B = new System.Collections.Generic.List<GlockBullies>();

    private WindowsMediaPlayer wmp = new WindowsMediaPlayer();

    private WindowsMediaPlayer wmpDing = null;

    private System.Collections.Generic.List<Energy> caps = new System.Collections.Generic.List<Energy>();

    private System.Collections.Generic.List<Monster> monsters = new System.Collections.Generic.List<Monster>();

    private int level = 0;

    private Random rd = new Random();

    private Graphics g;

    private Image bg;

    private lastKey lk = lastKey.Right;

    private Random r = new Random(); 

    private GifImage shipleft, shipright, shipup, shipdown;

    private Image explosion, bulletOne, bulletTwo, bulletThree, bulletFour, energy, monster, monsterFollow;

    private Image submarineLeftUp, submarineLeftDown, submarineTopLeft, submarineTopRight, submarineRightUp, submarineRightDown, submarineBottomLeft, submarineBottomRight;

    private int onex, oney, twox, twoy, threex, threey, fourx, foury, fivex, fivey, sixx, sixy, sevenx, seveny, eightx, eighty;

    private bool oneup, twoleft, threeup, fourleft;

    private Ship ship;

    private bool fal = false;

    private bool start = true;

    private ToolStripMenuItem newGame = new ToolStripMenuItem();

    private ToolStripMenuItem quit = new ToolStripMenuItem();

    private void startNew(object sender, EventArgs e)
    {
        B.Clear();

        ship.x = 5;
        ship.y = 4;
        ship.direction = "right";
        ship.life = 100;
        lk = lastKey.Right;

        level = 0;

        caps = new System.Collections.Generic.List<Energy>();

        timer.Start();
        timer2.Start();

        PlaySong(null, null);

        timer3.Start();
    }

    private void quitGame(object sender, EventArgs e)
    {
        Application.Exit();
    }

    public Energizer()
    {
        form.SetBounds(0, 0, 1280, 750);

        var menuStrip = new MenuStrip();
        menuStrip.BackColor = Color.Gold;
        menuStrip.Name = "File";
        menuStrip.Text = "File";
        menuStrip.Items.Add(newGame);
        var menu1 = new ToolStripMenuItem();
        menuStrip.Items.Add(menu1);
        menu1.Name = "menu1";
        menu1.Text = "File";
        menu1.ForeColor = Color.Black;
        menu1.BackColor = Color.Yellow;
        menu1.DropDownItems.Add(newGame);
        newGame.Name = "menu1";
        newGame.Text = "New Game";
        newGame.Click += startNew;
        menu1.DropDownItems.Add(quit);
        quit.Name = "submenu";
        quit.Text = "Quit";
        quit.Click += quitGame;

        form.Controls.Add(menuStrip);

        panel.SetBounds(0, 0, 1280, 750);
        panel.BackColor = Color.Red;

        form.Controls.Add(panel);

        g = panel.CreateGraphics();

        energy = Image.FromFile(Environment.CurrentDirectory + "\\energy.png");

        monster = Image.FromFile(Environment.CurrentDirectory + "\\monster.png");
        monsterFollow = Image.FromFile(Environment.CurrentDirectory + "\\monsterFollow.png");

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

        form.KeyDown += new KeyEventHandler(MoveShip);

        timer.Interval = 20;
        timer.Tick += new EventHandler(Move);
        timer.Start();

        timer2.Interval = 100;
        timer2.Tick += new EventHandler(Mv);
        timer2.Start();

        PlaySong(null, null);

        timer3.Interval = 3 * 60 * 1000;
        timer3.Tick += new EventHandler(PlaySong);
        timer3.Start();

        //timer4.Interval = 500;
        //timer4.Tick += new EventHandler(AnimBg);
        //timer4.Start();
    }

    private void PlaySong(object sender, EventArgs e)
    {
        try
        {
            wmp.URL = "raiden2lvl1.mp3";
            wmp.controls.play();
        } catch(Exception ex)
        {
        }
    }

    private void StopSong()
    {
        try
        {
            wmp.controls.stop();
        } catch(Exception ex)
        {
        }
    }

    private void createMonsters()
    {
        monsters.Clear();

        for(int i=0; i<6; i++)
        {
            int x = r.Next(64);
            int y = r.Next(37);
            monsters.Add(new Monster(x, y));
        }
    }

    private void drawRandomSpace()
    {
        SolidBrush brush = new SolidBrush(Color.Black);
        Rectangle rect = new Rectangle(0, 0, 1280, 750);
        g.FillRectangle(brush, rect);

        SolidBrush whiteBrush = new SolidBrush(Color.White);
        Pen pen = new Pen(whiteBrush);
        for(int i=0; i<70; i++)
        {
            int x = r.Next(1280);
            int y = r.Next(750);

            g.DrawEllipse(pen, x, y, 1, 1);
        }

        if (oneup)
        {
            oney -= 10;
            g.DrawImage(submarineLeftUp, onex, oney, 80, 300);
            int v = r.Next((35 - level) * 15 - level / 2);
            if (v == 1 || v == 2 || v == 3 || v == 4 || v == 5 || v == 6 || v == 7 || v == 8)
            {
                Glock gl = new Glock(explosion, g, onex / 20, oney / 20, bulletOne, ship);
                gl.exe(gl.randomizeShiets(gl.Sheut()));
                B.AddRange(gl.B);
            }
            if (oney < 1)
            {
                oneup = false;
            }
        }
        else
        {
            oney += 10;
            g.DrawImage(submarineLeftDown, onex, oney, 80, 300);
            int v = r.Next((35 - level) * 15 - level / 2);
            if (v == 1 || v == 2 || v == 3 || v == 4 || v == 5 || v == 6 || v == 7 || v == 8)
            {
                Glock gl = new Glock(explosion, g, onex / 20, oney / 20, bulletOne, ship);
                gl.exe(gl.randomizeShiets(gl.Sheut()));
                B.AddRange(gl.B);
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
            int v = r.Next((35 - level) * 15 - level / 2);
            if (v == 1 || v == 2 || v == 3 || v == 4 || v == 5 || v == 6 || v == 7 || v == 8)
            {
                Glock gl = new Glock(explosion, g, threex / 20, threey / 20, bulletThree, ship);
                gl.opp();
                gl.exe(gl.randomizeShiets(gl.Sheut()));
                B.AddRange(gl.B);
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
            int v = r.Next((35 - level) * 15 - level / 2);
            if (v == 1 || v == 2 || v == 3 || v == 4 || v == 5 || v == 6 || v == 7 || v == 8)
            {
                Glock gl = new Glock(explosion, g, threex / 20, threey / 20, bulletThree, ship);
                gl.opp();
                gl.exe(gl.randomizeShiets(gl.Sheut()));
                B.AddRange(gl.B);
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
            int v = r.Next((35 - level) * 15 - level / 2);
            if (v == 1 || v == 2 || v == 3 || v == 4 || v == 5 || v == 6 || v == 7 || v == 8)
            {
                Glock gl = new Glock(explosion, g, twox / 20, twoy / 20, bulletTwo, ship);
                gl.ver();
                gl.exe(gl.randomizeShiets(gl.Sheut()));
                B.AddRange(gl.B);
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
            int v = r.Next((35 - level) * 15 - level / 2);
            if (v == 1 || v == 2 || v == 3 || v == 4 || v == 5 || v == 6 || v == 7 || v == 8)
            {
                Glock gl = new Glock(explosion, g, twox / 20, twoy / 20, bulletTwo, ship);
                gl.ver();
                gl.exe(gl.randomizeShiets(gl.Sheut()));
                B.AddRange(gl.B);
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
            int v = r.Next((35 - level) * 15 - level / 2);
            if (v == 1 || v == 2 || v == 3 || v == 4 || v == 5 || v == 6 || v == 7 || v == 8)
            {
                Glock gl = new Glock(explosion, g, fourx / 20, foury / 20, bulletFour, ship);
                gl.ver();
                gl.opp();
                gl.exe(gl.randomizeShiets(gl.Sheut()));
                B.AddRange(gl.B);
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
            int v = r.Next((35 - level) * 15 - level / 2);
            if (v == 1 || v == 2 || v == 3 || v == 4 || v == 5 || v == 6 || v == 7 || v == 8)
            {
                Glock gl = new Glock(explosion, g, fourx / 20, foury / 20, bulletFour, ship);
                gl.ver();
                gl.opp();
                gl.exe(gl.randomizeShiets(gl.Sheut()));
                B.AddRange(gl.B);
            }
        }

        for (int k = 0; k < B.Count; k++)
        {
            if (B[k].dead != true)
            {
                try
                {
                    g.DrawImage(B[k].myBullet, B[k].whereami.X * 20, B[k].whereami.Y * 20, B[k].myBullet.Width / 2, B[k].myBullet.Height / 2);
                }
                catch (Exception ex)
                {
                }
            } else
            {
                B.Remove(B[k]);
            }
        }

        if (ship.direction.Equals("up"))
        {
            ship.y -= 1;
            Image gg = shipup.GetNextFrame();
            g.DrawImage(gg, ship.x * 20, ship.y * 20, 40, 40);
        }
        if (ship.direction.Equals("down"))
        {
            ship.y += 1;
            Image gg = shipdown.GetNextFrame();
            g.DrawImage(gg, ship.x * 20, ship.y * 20, 40, 40);
        }
        if (ship.direction.Equals("left"))
        {
            ship.x -= 1;
            Image gg = shipleft.GetNextFrame();
            g.DrawImage(gg, ship.x * 20, ship.y * 20, 40, 40);
        }
        if (ship.direction.Equals("right"))
        {
            ship.x += 1;
            Image gg = shipright.GetNextFrame();
            g.DrawImage(gg, ship.x * 20, ship.y * 20, 40, 40);
        }

        for (int i = 0; i < monsters.Count; i++)
        {
            if (i < monsters.Count)
            {
                int x = r.Next(2) - r.Next(2);
                int y = r.Next(2) - r.Next(2);
                int v = r.Next(20);
                int v2 = r.Next(7);
                if (level % 2 == 0)
                {
                    if (v == 4)
                    {
                        monsters[i].follow = !monsters[i].follow;
                    }
                    else if (v2 == 4)
                    {
                        monsters[i].follow = !monsters[i].follow;
                    }

                    if (monsters[i].follow)
                    {
                        monsters[i].move = !monsters[i].move;
                        if (ship.x < monsters[i].x && monsters[i].move)
                            monsters[i].x--;
                        if (ship.y < monsters[i].y && monsters[i].move)
                            monsters[i].y--;

                        if (ship.x > monsters[i].x && monsters[i].move)
                            monsters[i].x++;
                        if (ship.y > monsters[i].y && monsters[i].move)
                            monsters[i].y++;
                    }
                }
                if (!monsters[i].follow)
                {
                    monsters[i].move = !monsters[i].move;
                    if (monsters[i].move)
                    {
                        monsters[i].x += x;
                        monsters[i].y += y;
                    }
                }
                if (ship.x == monsters[i].x && ship.y == monsters[i].y)
                {
                    ship.life -= 100;
                    explode();
                }
                try
                {
                    Image m = null;
                    if (monsters[i].follow)
                    {
                        m = monsterFollow;
                    }
                    else
                    {
                        m = monster;
                    }
                    g.DrawImage(m, monsters[i].x * 20, monsters[i].y * 20, 35, 35);
                }
                catch (Exception ex)
                {
                }
            }
        }

        for (int i = 0; i < caps.Count; i++)
        {
            if (i < caps.Count)
            {
                try
                {
                    g.DrawImage(energy, caps[i].x * 20, caps[i].y * 20, 40, 40);
                    if (ship.x == caps[i].x && ship.y == caps[i].y)
                    {
                        ship.life += 20;
                        g.DrawImage(explosion, ship.x * 20, ship.y * 20, 68, 68);
                        //threadPlayDing.Start();
                        explode();
                        eliminateCapsule(caps[i]);
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }
    }

    private void explode()
    {
        if (wmpDing == null)
        {
            wmpDing = new WindowsMediaPlayer();
            wmpDing.URL = "explosion.wav";
        }
        try
        {
            wmpDing.URL = "explosion.wav";
            wmpDing.controls.play();
        }
        catch (Exception ex)
        {
        }
    }

    private void MoveShip(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            if(gameOver)
            {
                startNew(null, null);
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
        onex = 0+50;
        oney = 600;
        oneup = true;

        threex = 1150;
        threey = 300;
        threeup = true;

        twox = 600;
        twoy = 50;
        twoleft = true;

        fourx = 1000;
        foury = 620;
        fourleft = true;
    }

    private void playDing()
    {
        try
        {
            //wmpDing.URL = "ding.wav";
            //wmpDing.controls.play();
        } catch(Exception ex)
        {
        }
    }

    private void Mv(object sender, EventArgs e)
    {
        if (ship.x < 3 || ship.x > 59 || ship.y < 3 || ship.y > 33)
        {
            doGameOver();
            return;
        }

        drawRandomSpace();
        fal = false;

        //System.Threading.Thread threadPlayDing = new System.Threading.Thread(() => playDing());
    }

    private void Level1()
    {
        if (level == 0)
        {
            level++;
            Lvl1.ToCaps(caps);

            createMonsters();
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

                createMonsters();
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

                createMonsters();
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

                createMonsters();
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

                createMonsters();
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

                createMonsters();
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

                createMonsters();
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

                createMonsters();
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

                createMonsters();
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

                createMonsters();
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

                createMonsters();
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

                createMonsters();
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

                createMonsters();
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

                createMonsters();
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

                createMonsters();
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

                createMonsters();
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

                createMonsters();
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

                createMonsters();
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

                createMonsters();
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

                createMonsters();
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

                createMonsters();
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

                createMonsters();
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

                createMonsters();
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

                createMonsters();
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

                createMonsters();
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

                createMonsters();
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

                createMonsters();
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

                createMonsters();
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

                createMonsters();
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

                createMonsters();
            }
        }
    }

    //private void AnimBg(object sender, EventArgs e)
    //{
        //bg = gi.GetNextFrame();
        //fal = true;
    //}

    private void doGameOver()
    {
        timer.Stop();
        timer2.Stop();
        timer3.Stop();

        try
        {
            StopSong();
        } catch(Exception ex)
        {
        }

        form.Text = "Game Over, Vitals: 0 (Press ENTER to replay)";

        gameOver = true;
    }

    private void eliminateCapsule(Energy cap)
    {
        Random r = new Random();

        int x = r.Next(10) - r.Next(10);
        int y = r.Next(10) - r.Next(10);

        if (x == 0)
            x++;

        if (y == 0)
            y++;

        System.Threading.Thread thread = new System.Threading.Thread(() => moveCapsuleOut(cap, x, y));
        thread.Start();
    }

    private void moveCapsuleOut(Energy cap, int x, int y)
    {
        int count = 0;
        while(true)
        {
            cap.x += x;
            cap.y += y;
            System.Threading.Thread.Sleep(400);
            if (count > 5)
            {
                break;
            }
            count++;
        }

        try
        {
            caps.Remove(cap);
        } catch(Exception ex)
        {
        }
    }

    private void Move(object sender, EventArgs e)
    {
        ship.life--;

        if (ship.life <= 0)
        {
            doGameOver();
            return;
        }

        form.Text = "Space Energizer (by Daniel Cho) | Vitals: " + ship.life;

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

        if (level == 30)
        {
            if (caps.Count == 4)
            {
                doGameOver();
                form.Text = "You won the galaxy!";
                return;
            }
        }
    }

    public class Energy
    {
        public int x;

        public int y;
    }

    public class Monster
    {
        public int x;

        public int y;

        public Boolean follow, move;

        public Monster(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    private enum lastKey
    {
        Up,
        Down,
        Left,
        Right
    }

    public static void Main(string[] args)
    {
        Energizer energizer = new Energizer();
        energizer.Play();
        Application.Run(energizer.form);
    }
}