using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NotificationListener
{
    public partial class FrmMessage : Form
    {
        private int screenWidth;//屏幕宽度  

        private int screenHeight;//屏幕高度  

        private bool finished = false;//是否完全显示提示窗口 

        public string message;


        public FrmMessage()
        {
            InitializeComponent();
        }

        private void FrmMessage_Load(object sender, EventArgs e)
        {
            label1.Text = message;

            screenHeight = Screen.PrimaryScreen.WorkingArea.Height;

            screenWidth = Screen.PrimaryScreen.WorkingArea.Width;

            //设置提示窗口坐标在屏幕可显示区域之外  

            Location = new Point(screenWidth - Width, screenHeight);

            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!finished)//如果提示窗口没有完全显示  

            {

                //如果提示窗口的纵坐标与提示窗口的高度之和大于屏幕高度  

                if (Location.Y + Height >= screenHeight)

                {

                    Location = new Point(Location.X, Location.Y - 5);

                }

            }

            else//如果提示窗口已经完成了显示，并且点击了确定按钮  

            {

                //如果提示窗口没有完全从屏幕上消失  

                if (Location.Y < screenHeight)

                {

                    Location = new Point(Location.X, Location.Y + 5);

                }

            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

            //设置完成了显示，以便让提示控件移出屏幕可显示区域  

            this.Close();
        }
    }
}
