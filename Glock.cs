using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using System;

public class Glock
{
    public int x, y;
    private System.Collections.Generic.List<GlockBullies> B;
    private System.Windows.Forms.Timer timer22 = new System.Windows.Forms.Timer();
    private System.Windows.Forms.Timer timer21 = new System.Windows.Forms.Timer();
    private System.Windows.Forms.Timer timer1 = new System.Windows.Forms.Timer();
    private System.Windows.Forms.Timer timer2 = new System.Windows.Forms.Timer();
    private Image myLead;
    private Image myBullet, myExplosion;
    private Graphics g;
    private int countBulleties = -1;
    private Random r = new Random();
    private bool oppo = false;
    private bool vert = false;
    private Ship ship;

    public Glock(Image explosion, Graphics g, int x, int y, Image image, Ship ship)
    {
        this.ship = ship;
        this.myExplosion = explosion;
        this.myBullet = image;
        this.g = g;
        this.x = x;
        this.y = y;
    }

    public void opp()
    {
        oppo = true;
    }

    public void ver()
    {
        vert = true;
    }

    public System.Collections.Generic.List<GlockBullies> Sheut()
    {
        countBulleties = 1;
        System.Collections.Generic.List<GlockBullies> bBs = new System.Collections.Generic.List<GlockBullies>();
        for (int i = 0; i < countBulleties; i++)
        {
            bBs.Add(new GlockBullies(x, y));
        }
        return bBs;
    }

    public System.Collections.Generic.List<GlockBullies> randomizeShiets(System.Collections.Generic.List<GlockBullies> bullies)
    {
        Random rnd = new Random();
        for (int j = 0; j < bullies.Count; j++)
        {
            if (!vert)
            {
                int xrand = 1;
                if (oppo)
                    xrand = -1;
                int yrand = rnd.Next(2) - rnd.Next(2);
                bullies[j].moveittoit(xrand, yrand);
            }
            else
            {
                int yrand = 1;
                if (oppo)
                    yrand = -1;
                int xrand = rnd.Next(2) - rnd.Next(2);
                bullies[j].moveittoit(xrand, yrand);
            }
        }
        return bullies;
    }

    public void checkDeadness()
    {
        for (int k = 0; k < B.Count; k++)
        {
            if (B[k].whereami.X == ship.x && B[k].whereami.Y == ship.y)
            {
                ship.life-=18;
            }
        }
    }

    public void exe(System.Collections.Generic.List<GlockBullies> bullies)
    {
        this.B = bullies;
        timer1.Interval = 110;
        timer1.Tick += new EventHandler(moveTheBullets);
        try
        {
            timer1.Start();
        }
        catch (Exception e)
        {
        }
        timer2.Interval = 15 * 1000;
        timer2.Tick += new EventHandler(StopMovingBullets);
        try
        {
            timer2.Start();
        }
        catch (Exception e)
        {
        }
    }

    private void StopMovingBullets(object sender, EventArgs e)
    {
        for (int i = 0; i < B.Count; i++)
        {
            B.Remove(B[i]);
        }
        timer1.Stop();
        timer1.Dispose();
        timer2.Stop();
        timer2.Dispose();
    }

    private void moveTheBullets(object sender, EventArgs e)
    {
        for (int k = 0; k < B.Count; k++)
        {
            if (B[k].dead != true)
            {
                B[k].move();
                try
                {
                    g.DrawImage(myBullet, B[k].whereami.X * 20, B[k].whereami.Y * 20, myBullet.Width, myBullet.Height);
                }
                catch (Exception ex)
                {
                }
            }
            if (B[k].whereami.X > 1280 || B[k].whereami.X < 0)
            {
                B.Remove(B[k]);
            }
            else if (B[k].whereami.Y > 1024 || B[k].whereami.Y < 0)
            {
                B.Remove(B[k]);
            }
        }
        checkDeadness();
    }
}

public class GlockBullies
{
    public System.Drawing.Point whereami = new System.Drawing.Point(100, 400);
    public int timetolive = 100;
    public bool dead;
    private int movex;
    private int movey;

    public GlockBullies(int x, int y)
    {
        whereami.X = x;
        whereami.Y = y;
    }
    public void moveittoit(int movex, int movey)
    {
        this.movex = movex;
        this.movey = movey;
    }
    public void move()
    {
        timetolive--;
        whereami.X += this.movex;
        whereami.Y += this.movey;
    }
}