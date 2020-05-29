using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SPLaba_8
{
    public partial class Form1 : Form
    {
        private static object objLock = new object();
        Random rnd = new Random();
        public Form1()
        {
            InitializeComponent();
                   

        }
        public void InitialigeCar(Control con, int moveDirection)
        {
            if (con.InvokeRequired)
            {
                con.Invoke(new Action<Control, int>(InitialigeCar), con, moveDirection);
            }
            else
            {
                switch (moveDirection)
                {
                    case 1:                       
                        con.Left = -1 * con.Width + 10;
                        break;
                    case -1:                      
                        con.Left = this.ClientSize.Width - 10;
                        break;
                }
            }
        }
                
        

        

        public void CarGo(Control car, Control bridge, object objLock, int moveDirection)
        {   
            InitialigeCar(car, moveDirection);
          
            while (car.Location.X > 0 - car.Width && car.Location.X<ClientSize.Width)
            {
               
                if (car.Location.X + car.Width > bridge.Location.X && car.Location.X < bridge.Location.X + bridge.Width)
                {
                    
                        try
                        {
                        Monitor.Enter(objLock);
                        do
                        {
                            InkrementCar(car, moveDirection);
                            Thread.Sleep(5 * RandomGenerator.Next(1, 5));

                        } while (car.Location.X + car.Width > bridge.Location.X && car.Location.X < bridge.Location.X + bridge.Width);
                                                    
                                           
                       
                        }
                        finally 
                        {
                        Monitor.Exit(objLock);

                        }
                   
                }
                else
                {
                    InkrementCar(car, moveDirection);
                    Thread.Sleep(5 * RandomGenerator.Next(1, 5));
                }
               
            }
                  

        }

        public void InkrementCar(Control con, int moveDirection)
        {
            if (con.InvokeRequired)
            {
                con.Invoke(new Action<Control, int>(InkrementCar), con, moveDirection);
            }
            else
            {
                con.Left = con.Location.X + moveDirection;
            }
        }
        public class RandomGenerator
        {
            static Random  rnd = new Random();
            private static object locObg = new object();
            public static int Next(int min, int max)
            {
                int n = 0;
                lock (locObg)
                {
                    n = rnd.Next(min, max);
                }
                return n;
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Image image1 = Image.FromFile("Car3.png");
            Size size1 = new Size(image1.Size.Width / 4, image1.Height / 4);
            Bitmap bitmap1 = new Bitmap(image1, size1);
            pictureBox1.Image = bitmap1;
            Bitmap b = new Bitmap(pictureBox1.ClientSize.Width, pictureBox2.ClientSize.Height);
            Graphics g = Graphics.FromImage(b);
            g.DrawLine(Pens.Red, new Point(1, 0), new Point(1, pictureBox2.Height));
            Font f = new Font("Verdana", 30, FontStyle.Italic);
            g.DrawString("Bridge", f, Brushes.Red, 10, 10);
            g.DrawLine(Pens.Red, new Point(pictureBox2.Width - 1, 0), new Point(pictureBox2.Width - 1, pictureBox2.Height));
            pictureBox2.Image = b;
            Image image2 = Image.FromFile("Car2.png");
            Size size2 = new Size(image2.Size.Width / 4, image2.Height / 4);
            Bitmap bitmap2 = new Bitmap(image2, size2);
            pictureBox3.Image = bitmap2;
            InitialigeCar(pictureBox1, 1);
            InitialigeCar(pictureBox3, -1);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Thread thread1 = new Thread(new ThreadStart(() => CarGo(pictureBox1, pictureBox2, objLock, 1)));
            Thread thread2 = new Thread(new ThreadStart(() => CarGo(pictureBox3, pictureBox2, objLock, -1)));
            thread1.IsBackground = true;
            thread2.IsBackground = true;
            thread1.Start();
            thread2.Start();
        }
    }
}
