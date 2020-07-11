using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;

namespace SlideShareDownloader
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnDir_Click(object sender, EventArgs e)
        {
            if(folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtDir.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            ///còn cái kiểm tra thư mục có tồn tại hay không thì nên kiểm tra ngay trước khi lưu file nhỉ
            //thêm cái nữa là tạo 1 cái progressbar để hiện tiến trình tải cái, đơn vị tính bằng số lượng ảnh lấy được
            //1. kiểm tra có phải link của slideshare.net hay không; 
            if(!CheckLink())
            {
                txtLink.SelectAll();
                return;
            }
            //2. kiểm tra thư mục lưu
            if(txtDir.Text == string.Empty)
            {
                if(MessageBox.Show("Xuất ra thư mục hiện tại?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    txtDir.Focus();
                    return;
                }
            }
            //3. Tải mã nguồn trang này về, Đọc mã nguồn và tách lấy địa chỉ của các ảnh
            if(!GetLinks(GetSrc()))
            {
                Err("Không tải được dữ liệu");
                return;
            }
            //4. Lấy link từng ảnh và tải xuống thư mục kia
            ProgressValue = 0;
            ProgressMax = Links.Count;
            Status = $"{ProgressValue}/{ProgressMax}";
            backgroundWorker.RunWorkerAsync();
        }

        MatchCollection Links;
        Regex tmpRex;

        private bool CheckLink()
        {
            if(txtLink.Text == "")
            {
                Err("Link tài liệu không được bỏ trống");
                return false;
            }
            tmpRex = new Regex("(https://){0,1}([a-zA-Z]*.){0,1}slideshare.net/[a-zA-Z0-9-./]+");
            if(!tmpRex.IsMatch(txtLink.Text))
            {
                Err("Link tài liệu không đúng");
                return false;
            }
            tmpRex = new Regex("slideshare.net/[a-zA-Z0-9-./]+");
            txtLink.Text = "https://www." + tmpRex.Match(txtLink.Text).Value;
            return true;
        }

        WebClient webClient = new WebClient();

        private string GetSrc()
        {
            try
            {
                byte[] data = webClient.DownloadData(txtLink.Text);
                if(data is null)
                {
                    Err("Không thể tải dữ liệu");
                    return "";
                }
                return Encoding.UTF8.GetString(data);
            }
            catch
            {
                Err("Không thể tải dữ liệu");
                return "";
            }
        }

        private bool GetLinks(string data)
        {
            if(string.IsNullOrEmpty(data))
                return false;
            tmpRex = new Regex("data-full=\"https://[a-zA-Z0-9./-]*.jpg");
            Links = tmpRex.Matches(data);
            if(Links?.Count > 0)
            {
                return true;
            }
            return false;

        }

        Regex rexLnk = new Regex("https://[a-zA-Z0-9./-]*.jpg");

        private string GetLink(Match data)
        {
            return rexLnk.Match(data.Value).Value;
        }

        

        Regex rexName = new Regex("[1-9][0-9]*-[1-9][0-9]{2,3}.jpg$");

        private string GetName(string link)
        {
            return rexName.Match(link).Value;
        }

        private bool CheckDir()
        {
            if(!txtDir.Text.EndsWith("\\"))
            {
                if(InvokeRequired)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        txtDir.AppendText("\\");
                    });
                }
                else
                {
                    txtDir.AppendText("\\");
                }
                
            }
            if(Directory.Exists(txtDir.Text))
            {
                return true;
            }
            try
            {
                Directory.CreateDirectory(txtDir.Text);
                return true;
            }
            catch
            {
                Err("Không tạo được thư mục lưu ảnh");
                txtDir.SelectAll();
                return false;
            }
        }

        string Status
        {
            get
            {
                return lblStatus.Text;
            }
            set
            {
                if(InvokeRequired)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        lblStatus.Text = value;
                    });
                }
                else
                {
                    lblStatus.Text = value;
                }
            }
        }

        int ProgressMax
        {
            get
            {
                return progressBar.Maximum;
            }
            set
            {
                if(InvokeRequired)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        progressBar.Maximum = value;
                    });
                }
                else
                {
                    progressBar.Maximum = value;
                }
            }
        }

        int ProgressValue
        {
            get
            {
                return progressBar.Value;
            }
            set
            {
                if(InvokeRequired)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        progressBar.Value = value;
                    });
                }
                else
                {
                    progressBar.Value = value;
                }
            }
        }

        private DialogResult Err(string msg)
        {
            return MessageBox.Show(msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        #region Tải ảnh       
        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string link = "";
            if(!CheckDir())
            {
                //e.Cancel = true;
                return;
            }    
            foreach(Match match in Links)
            {
                try
                {
                    webClient.DownloadFile(new Uri(link = GetLink(match)), txtDir.Text + GetName(link));
                    ProgressValue++;
                    Status = $"{ProgressValue}/{ProgressMax}";
                }
                catch
                {
                    Err("Tải file không thành công");

                    Status = "Tải không thành công";
                    return;
                    //e.Cancel = true;
                }
                //if(backgroundWorker.CancellationPending)
                //{
                //    return;
                //}
            }    
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if(ProgressValue == ProgressMax)
            {
                MessageBox.Show(Status = "Tải xong", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"Đã tải {ProgressValue}/{ProgressMax}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion Tải ảnh;
    }
}
