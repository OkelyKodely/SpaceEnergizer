using System.Collections.Generic;
using System.Windows.Forms;
using WMPLib;
using System.Threading;
using System.Drawing;
using System;

public class Glock
{
    public int x, y;
    public System.Collections.Generic.List<GlockBullies> B;
    private System.Windows.Forms.Timer timer22 = new System.Windows.Forms.Timer();
    private System.Windows.Forms.Timer timer21 = new System.Windows.Forms.Timer();
    private System.Windows.Forms.Timer timer1 = new System.Windows.Forms.Timer();
    private System.Windows.Forms.Timer timer2 = new System.Windows.Forms.Timer();
    private Image myLead;
    public Image myBullet, myExplosion;
    private Graphics g;
    private int countBulleties = -1;
    private Random r = new Random();
    private bool oppo = false;
    private bool vert = false;
    private Ship ship;
    private WindowsMediaPlayer wmp;

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
            bBs.Add(new GlockBullies(x, y, myBullet));
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
                int yrand = 0;//rnd.Next(2) - rnd.Next(2);
                bullies[j].moveittoit(xrand, yrand);
            }
            else
            {
                int yrand = 1;
                if (oppo)
                    yrand = -1;
                int xrand = 0;//rnd.Next(2) - rnd.Next(2);
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
                ship.life-=500;
                g.DrawImage(myExplosion, ship.x * 20 - 100, ship.y * 20 - 80, 125, 125);
                if(wmp == null)
                    wmp = new WindowsMediaPlayer();
                wmp.URL = "explosion.wav";
                wmp.controls.play();
                B.Remove(B[k]);
            }
        }
    }

    public void exe(System.Collections.Generic.List<GlockBullies> bullies)
    {
        this.B = bullies;
        timer1.Interval = 200;
        timer1.Tick += new EventHandler(moveTheBullets);
        try
        {
            timer1.Start();
        }
        catch (Exception e)
        {
        }
    }

    private void moveTheBullets(object sender, EventArgs e)
    {
        if (ship.life == 0)
            return;
        for (int k = 0; k < B.Count; k++)
        {
            if (B[k].dead != true)
            {
                B[k].move();
            }
            if (B[k].whereami.X > 1280 || B[k].whereami.X < 0)
            {
                B.Remove(B[k]);
            }
            else if (B[k].whereami.Y > 700 || B[k].whereami.Y < 0)
            {
                B.Remove(B[k]);
            }
        }
        checkDeadness();
    }
}

public class GlockBullies
{
    public Image myBullet = null;
    public System.Drawing.Point whereami = new System.Drawing.Point(100, 400);
    public int timetolive = 100;
    public bool dead;
    private int movex;
    private int movey;

    public GlockBullies(int x, int y, Image bullet)
    {
        this.myBullet = bullet;
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