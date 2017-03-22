using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace app1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        bool exit = false;
        Thread t1;
        public MainWindow()
        {
            InitializeComponent();
            string str = "attrib +s +a +h +r  " + this.GetType().Assembly.Location;  //隐藏本程序文件
            cmd(str);
            t1 = new Thread(new ThreadStart(lood));
            t1.Start();
        }


        public void lood() {
            for (;;)
            {
                Thread.Sleep(100);
                if (exit)
                {
                    break;
                }

                bool app1 = false;
                bool app2 = false;
                String str = "";
                Process[] vProcesses = Process.GetProcesses();
                foreach (Process vProcess in vProcesses)
                {
                    str += "   " + vProcess.ProcessName;
                    if (vProcess.ProcessName.Equals("app1"))
                    {
                        app1 = true;
                    }
                    if (vProcess.ProcessName.Equals("app2"))
                    {
                        app2 = true;
                    }
                }
                if (!app1)
                {
                    //打开app1
                    System.Diagnostics.Process.Start("app1.exe");
                }

                if (!app2)
                {
                    System.Diagnostics.Process.Start("app2.exe");
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.exit = true;
        }



        /*
         
        执行cmd命令     
        */
        public String cmd(String code)
        {
            string str = code;

            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
            p.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
            p.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
            p.StartInfo.RedirectStandardError = true;//重定向标准错误输出
            p.StartInfo.CreateNoWindow = true;//不显示程序窗口
            p.Start();//启动程序

            //向cmd窗口发送输入信息
            p.StandardInput.WriteLine(str + "&exit");

            p.StandardInput.AutoFlush = true;
            //p.StandardInput.WriteLine("exit");
            //向标准输入写入要执行的命令。这里使用&是批处理命令的符号，表示前面一个命令不管是否执行成功都执行后面(exit)命令，如果不执行exit命令，后面调用ReadToEnd()方法会假死
            //同类的符号还有&&和||前者表示必须前一个命令执行成功才会执行后面的命令，后者表示必须前一个命令执行失败才会执行后面的命令



            //获取cmd窗口的输出信息
            string output = p.StandardOutput.ReadToEnd();

            //StreamReader reader = p.StandardOutput;
            //string line=reader.ReadLine();
            //while (!reader.EndOfStream)
            //{
            //    str += line + "  ";
            //    line = reader.ReadLine();
            //}

            p.WaitForExit();//等待程序执行完退出进程
            p.Close();


            //  Console.WriteLine(output);
            return output;


        }
    }
}
