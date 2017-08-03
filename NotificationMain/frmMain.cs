using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace notifaction
{
    public partial class FrmMain : Form
    {
        private int intClickCount = 0;

        delegate void SetNotificationFlg(bool hasNotice);

        public FrmMain()
        {
            InitializeComponent();

            //OpenMessageFrom();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnSendMessage_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMessage.Text)) {
                MessageBox.Show("メッセージを入力してください");
                return;
            }

            SetNotification();

            FrmShowDialog sub = new FrmShowDialog();
            sub.ShowDialog();

        }

        private void SetNotification()
        {
            //intClickCount += 1;

            //if (intClickCount > 2)
            //{
            //    return;
            //}

            //指定されたマニフェストリソースを読み込む
            Bitmap img = (Bitmap)Properties.Resources.notification;


            ////PictureBox1に表示
            //Bitmap img = notifyIcon.Icon.ToBitmap();

            Graphics g = Graphics.FromImage(img);
            Font font_front = new Font("MS UI Gothic", 120, FontStyle.Bold);
            //Font font_backend = new Font("MS UI Gothic", 128,FontStyle.Bold);
            Brush whiteBrush = new SolidBrush(Color.White);
            Brush redBrush = new SolidBrush(Color.Red);

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;


            //Console.WriteLine(intClickCount.ToString());

            int circleWidth = 200;
            g.FillEllipse(redBrush, 98, 100, circleWidth, circleWidth);

            //string displayContent = intClickCount.ToString();
            //if (intClickCount > 0)
            //{
            //    displayContent = "！";
            //}

            string displayContent = "!";
            //g.DrawString(intClickCount.ToString(), font_backend, grayBrush, 64, 180, sf);
            g.DrawString(displayContent, font_front, whiteBrush, 198, 210, sf);

            Icon newIcon = System.Drawing.Icon.FromHandle(img.GetHicon());
            notifyIcon.Icon = newIcon;

            //FileStream ms = new FileStream(@"c:/temp/count.png",FileMode.Create);
            ////保存为Jpg类型
            //img.Save(ms, ImageFormat.Png);

            //ms.Dispose();

            g.Dispose();
            img.Dispose();
            newIcon.Dispose();
        }


        private void ClearNotification()
        {
            
            //指定されたマニフェストリソースを読み込む
            Bitmap img = (Bitmap)Properties.Resources.notification;

            Icon newIcon = System.Drawing.Icon.FromHandle(img.GetHicon());
            notifyIcon.Icon = newIcon;

            img.Dispose();
            newIcon.Dispose();
        }

        private void OpenMessageFrom()
        {
            System.Threading.Thread t = new System.Threading.Thread(OpenMessageThread);
            t.Start();
        }


        private void SetNotificationFlgHandler(bool hasNotice) {
            if (hasNotice)
            {
                SetNotification();
            }
            else
            {
                ClearNotification();
            }

        }

        private void OpenMessageThread() {
            Process hProcess = new Process();

            hProcess.StartInfo.FileName = @"NotificationListener.exe";
            hProcess.StartInfo.Arguments = txtMessage.Text;
            hProcess.EnableRaisingEvents = true;
            hProcess.Start();

            // 終了するまで待機する
            hProcess.WaitForExit();

            // 終了コードを取得する
            int iExitCode = hProcess.ExitCode;

            if (iExitCode == 0)
            {
                SetNotificationFlg notice = new SetNotificationFlg(SetNotificationFlgHandler);
                this.Invoke(notice, false);
            }
            //// 取得した終了コードを表示する
            //MessageBox.Show(iExitCode.ToString());

            // 不要になった時点で破棄する (正しくは オブジェクトの破棄を保証する を参照)
            hProcess.Close();
            hProcess.Dispose();
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Process[] proc = Process.GetProcessesByName("NotificationListener");//创建一个进程数组，把与此进程相关的资源关联。
            for (int i = 0; i < proc.Length; i++)
            {
                proc[i].Kill();  //逐个结束进程.
            }
        }

        private void NotifyIcon_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMessage.Text))
            {
                return;
            }

            Process[] proc = Process.GetProcessesByName("NotificationListener");//创建一个进程数组，把与此进程相关的资源关联。
            if (proc.Length > 0)
            {
                return;
            }
            else {
                OpenMessageFrom();
            }
        }
    }
}
