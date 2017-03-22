using Microsoft.Win32;
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

namespace app2
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        bool exit = false;
        public MainWindow()
        {
            InitializeComponent();

            string str = "attrib +s +a +h +r  " + this.GetType().Assembly.Location;//隐藏本文件
            cmd(str);
            string path = this.GetType().Assembly.Location; 
            RegistryKey rk = Registry.LocalMachine;
            try
            {
                RegistryKey rk2 = rk.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
                rk2.SetValue("JcShutdown", path);
                rk2.Close();
                rk.Close();
            }
            catch {
                MessageBox.Show("设置开机自动启动失败");
            }

           
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Thread t1 = new Thread(new ThreadStart(lood));
            t1.Start();
        }

        public void lood()
        {
            for (;;)
            {
                Thread.Sleep(100);
                if (exit)
                {
                   
                    break;
                }
                bool app1 = false;

                Process[] vProcesses = Process.GetProcesses();
                foreach (Process vProcess in vProcesses)
                {
                    if (vProcess.ProcessName.Equals("app1"))
                    {
                        app1 = true;
                    }


                }
                if (!app1)
                {
                    //打开app1
                    System.Diagnostics.Process.Start("app1.exe");
                }

            }

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }



        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.exit = true;


            try
            {
                Process[] processes = Process.GetProcesses();
                foreach (Process p in processes)
                {
                    if (p.ProcessName.Equals("app1"))
                    {
                        p.Kill();
                    }
                }
            }
            catch (Exception)
            {
            }

            Environment.Exit(0);

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
