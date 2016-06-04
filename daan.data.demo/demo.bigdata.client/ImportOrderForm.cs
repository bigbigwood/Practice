using BBW.Utility.File;
using SevenZip;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace demo.bigdata.client
{
    public partial class ImportOrderForm : Form
    {
        private Logger _logger;
        private BackgroundWorker _backgroundWorker;

        public ImportOrderForm()
        {
            InitializeComponent();

            _logger = new Logger(tbxLogger);
            _backgroundWorker = new BackgroundWorker()
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true,
            };
            _backgroundWorker.DoWork += new DoWorkEventHandler(DoWork);
            _backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(UpdateProgress);
            _backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(CompletedWork);
        }


        private void ImportOrderForm_Load(object sender, EventArgs e)
        {
        }

        private void EnableControls(bool enabled)
        {
            this.btnChooseFile.Enabled = enabled;
            this.btnImportOrders.Enabled = enabled;
        }


        private void btnChooseFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "csv文件(*.csv)|*.csv|excel文件(*.xls)|*.xls|所有文件(*.*)|*.*";
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                tbxfilePath.Text = openFileDialog.FileName;
            }
        }

        private void btnImportOrders_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbxfilePath.Text))
            {
                MessageBox.Show("请先选择文件");
            }

            EnableControls(false);

            _backgroundWorker.RunWorkerAsync(this);
        }

        void ShowMessage(string message)
        {
            BeginInvoke(new Action(() => 
            {
                string formattedMessage = string.Format("[{0}]: {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), message);
                tbxLogger.AppendText(formattedMessage + "\r\n"); 
            }));
        }

        void DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bw = sender as BackgroundWorker;
            ImportOrderForm win = e.Argument as ImportOrderForm;

            ShowMessage("处理文件中...");
            FileInfo originalFileInfo = new FileInfo(tbxfilePath.Text.Trim());
            string zippedFileName = originalFileInfo.Name + ".7z";

            string directory = string.Format(@"{0}\{1}", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "demo.bigdata.client");
            if (Directory.Exists(directory) == false)
            {
                Directory.CreateDirectory(directory);
            }

            string fileFullPath = string.Format(@"{0}\{1}", directory, zippedFileName);
            if (File.Exists(fileFullPath))
            {
                File.Delete(fileFullPath);
            }
            ShowMessage("处理文件完成...");

            bw.ReportProgress(10);

            ShowMessage("压缩文件中...");
            var tmp = new SevenZipCompressor();
            tmp.ScanOnlyWritable = true;
            tmp.CompressFiles(fileFullPath, tbxfilePath.Text);

            var zippedFileInfo = new FileInfo(fileFullPath);
            ShowMessage(string.Format("压缩文件完成, 原文件大小: {0}MB，压缩文件大小: {1}MB",
                FileSizeUtility.CalculateFileSizeInMBytes(originalFileInfo.Length), FileSizeUtility.CalculateFileSizeInMBytes(zippedFileInfo.Length)));
            bw.ReportProgress(50);

            ShowMessage("上传文件中...");
            string url = ConfigurationManager.AppSettings.Get("UploadFileUrl");
            var response = Upload(url, fileFullPath);
            if (response.Contains("Success"))
            {
                int numberStartIndex = response.IndexOf("[") + 1;
                int numberLength = response.IndexOf("]") - numberStartIndex;
                string counter = response.Substring(numberStartIndex, numberLength);
                ShowMessage(string.Format("完成上传{0}个订单...", counter));
                bw.ReportProgress(100);
            }
            else
            {
                Invoke(new Action(() => { ShowMessage(string.Format("上传失败: 错误为: {0}", response)); })); // wait until show message
                throw new Exception(string.Format("上传失败: 错误为: {0}", response));
            }
        }

        void UpdateProgress(object sender, ProgressChangedEventArgs e)
        {
            int progress = e.ProgressPercentage;
            progressBar1.Value = progress;
        }

        void CompletedWork(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);  
            }
            else if (e.Cancelled)
            {
                MessageBox.Show("已取消上传!");
            }
            else
            {
                MessageBox.Show("上传完成！");
            }

            EnableControls(true);
        }


        private string Upload(string url, string filePath)
        {
            System.Net.WebClient myWebClient = new System.Net.WebClient();
            var byteArray = myWebClient.UploadFile(url, "POST", filePath);
            return System.Text.Encoding.UTF8.GetString(byteArray);
        }
    }
}
