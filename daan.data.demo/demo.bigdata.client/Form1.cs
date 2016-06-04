using demo.bigdata.zipper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace demo.bigdata.client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnChooseFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.InitialDirectory = "c:\\";//注意这里写路径时要用c:\\而不是c:\
            openFileDialog.Filter = "csv文件(*.csv)|*.csv|excel文件(*.xls)|*.xls|所有文件(*.*)|*.*";
            openFileDialog.RestoreDirectory = true;
            //openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                tbxfilePath.Text = openFileDialog.FileName;
            }
        }

        private void btn_UploadFile_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty( tbxfilePath.Text))
            {
                MessageBox.Show("请先选择文件");
            }

            string fileName = tbxfilePath.Text.Substring(tbxfilePath.Text.LastIndexOf(@"\") + 1);
            string zippedFileName = fileName + ".7z";

            string directory = string.Format(@"{0}\{1}", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "demo.bigdata.client");
            if (Directory.Exists(directory) == false)
            {
                Directory.CreateDirectory(directory);
            }

            string fileFullPath = string.Format(@"{0}\{1}", directory, zippedFileName);
            if (File.Exists(fileFullPath))
            {
                // clean file if exist.
                File.Delete(fileFullPath);
            }

            SevenZipper.ExecuteSevenZip(fileFullPath, tbxfilePath.Text);

            string url = @"http://localhost:22206/UploadFileHandler.ashx";
            Upload(url, fileFullPath);

            MessageBox.Show("上传完成！");
        } 

        private void Upload(string url, string filePath)
        {
            System.Net.WebClient myWebClient = new System.Net.WebClient();
            myWebClient.UploadFile(url, "POST", filePath);  
        }



        // <summary>
        /// 将本地文件上传到指定的服务器(HttpWebRequest方法)
        /// 
        /// -----------------------8c64f47716481f0  //时间戳

        //Content-Disposition: form-data; name="file"; filename="a.txt"  //文件名

        //Content-Type: application/octet-stream

        //文件的内容

        //-----------------------8c64f47716481f0
        /// 
        /// 
        /// 
        /// 
        /// </summary>
        /// <param name="address">文件上传到的服务器</param>
        /// <param name="fileNamePath">要上传的本地文件（全路径）</param>
        /// <param name="saveName">文件上传后的名称</param>
        /// <param name="progressBar">上传进度条</param>
        /// <returns>成功返回1，失败返回0</returns>
        private int Upload_Request(string url, string fileNamePath, string saveName, ProgressBar progressBar)
        {
            int returnValue = 0;
            // 要上传的文件
            FileStream fs = new FileStream(fileNamePath, FileMode.Open, FileAccess.Read);
            BinaryReader r = new BinaryReader(fs);
            //时间戳
            string strBoundary = "----------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundaryBytes = Encoding.ASCII.GetBytes(Environment.NewLine + strBoundary + Environment.NewLine);

            //请求头部信息
            string header = @"{0} 
Content-Disposition: form-data; name=""file""; filename=""{1}""
Content-Type: application/octet-stream
";


            string strPostHeader = string.Format(header, strBoundary, saveName);
            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(strPostHeader);

            // 根据uri创建HttpWebRequest对象
            HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(new Uri(url));
            httpReq.Method = "POST";
            //对发送的数据不使用缓存
            httpReq.AllowWriteStreamBuffering = false;
            //设置获得响应的超时时间（300秒）
            httpReq.Timeout = 300000;
            httpReq.ContentType = "multipart/form-data; boundary=" + strBoundary;
            long length = fs.Length + postHeaderBytes.Length + boundaryBytes.Length;
            long fileLength = fs.Length;
            httpReq.ContentLength = length;
            try
            {
                //progressBar.Maximum = int.MaxValue;
                //progressBar.Minimum = 0;
                //progressBar.Value = 0;
                //每次上传4k
                int bufferLength = 4096;
                byte[] buffer = new byte[bufferLength];
                //已上传的字节数
                long offset = 0;
                //开始上传时间
                DateTime startTime = DateTime.Now;
                int size = r.Read(buffer, 0, bufferLength);
                Stream postStream = httpReq.GetRequestStream();
                //发送请求头部消息
                postStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
                while (size > 0)
                {
                    postStream.Write(buffer, 0, size);
                    offset += size;
                    //progressBar.Value = (int)(offset * (int.MaxValue / length));
                    //TimeSpan span = DateTime.Now - startTime;
                    //double second = span.TotalSeconds;
                    //lblTime.Text = "已用时：" + second.ToString("F2") + "秒";
                    //if (second > 0.001)
                    //{
                    //    lblSpeed.Text = " 平均速度：" + (offset / 1024 / second).ToString("0.00") + "KB/秒";
                    //}
                    //else
                    //{
                    //    lblSpeed.Text = " 正在连接…";
                    //}
                    //lblState.Text = "已上传：" + (offset * 100.0 / length).ToString("F2") + "%";
                    //lblSize.Text = (offset / 1048576.0).ToString("F2") + "M/" + (fileLength / 1048576.0).ToString("F2") + "M";
                    //Application.DoEvents();
                    size = r.Read(buffer, 0, bufferLength);
                }
                //添加尾部的时间戳
                postStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                postStream.Close();
                //获取服务器端的响应
                WebResponse webRespon = httpReq.GetResponse();
                Stream s = webRespon.GetResponseStream();
                StreamReader sr = new StreamReader(s);
                //读取服务器端返回的消息
                String sReturnString = sr.ReadLine();
                s.Close();
                sr.Close();
                if (sReturnString == "Success")
                {
                    returnValue = 1;
                }
                else if (sReturnString == "Error")
                {
                    returnValue = 0;
                }
            }
            catch
            {
                returnValue = 0;
            }
            finally
            {
                fs.Close();
                r.Close();
            }
            return returnValue;
        }
    }
}
