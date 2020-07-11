using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace SlideShareDownloader
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMain());
            //test1();
        }

        static void test()
        {
            System.IO.Directory.CreateDirectory(@"C:\users\ngn\desktop\ngndien\kkkk");
         }

        static void test2()
        {
            System.Net.WebClient c = new System.Net.WebClient();
            byte[] datas = c.DownloadData("https://www.slideshare.net/nhathieu91/tuyn-tp-ton-thi-vo-lp-10");
            MessageBox.Show(datas.Length.ToString());
            //MessageBox.Show(System.Text.Encoding.UTF8.GetString(datas));
        }
        static void test1()
        {
            string tstr = "< img class=\"slide_image\"" +
                  "src=\"https://image.slidesharecdn.com/detoanhaiduong20140728-150122220957-conversion-gate01/95/tuyn-tp-ton-thi-vo-lp-10-qua-cc-nm-tnh-hi-dng-nht-hiu-1-638.jpg?cb=1593216818\"" +
                  "data-small=\"https://image.slidesharecdn.com/detoanhaiduong20140728-150122220957-conversion-gate01/85/tuyn-tp-ton-thi-vo-lp-10-qua-cc-nm-tnh-hi-dng-nht-hiu-1-320.jpg?cb=1593216818\"" +
                  "data-normal=\"https://image.slidesharecdn.com/detoanhaiduong20140728-150122220957-conversion-gate01/95/tuyn-tp-ton-thi-vo-lp-10-qua-cc-nm-tnh-hi-dng-nht-hiu-1-638.jpg?cb=1593216818\"" +
                  "data-full=\"https://image.slidesharecdn.com/detoanhaiduong20140728-150122220957-conversion-gate01/95/tuyn-tp-ton-thi-vo-lp-10-qua-cc-nm-tnh-hi-dng-nht-hiu-1-1024.jpg?cb=1593216818\"" +
                  "alt=\"Tuyển tập đề Toán thi vào lớp 10 qua các năm tỉnh Hải Dương [Nhật Hiếu]\"/>" +
            "</section>" +
                  "src=\"https://image.slidesharecdn.com/detoanhaiduong20140728-150122220957-conversion-gate01/95/tuyn-tp-ton-thi-vo-lp-10-qua-cc-nm-tnh-hi-dng-nht-hiu-1-638.jpg?cb=1593216818\"" +
                  "data-small=\"https://image.slidesharecdn.com/detoanhaiduong20140728-150122220957-conversion-gate01/85/tuyn-tp-ton-thi-vo-lp-10-qua-cc-nm-tnh-hi-dng-nht-hiu-1-320.jpg?cb=1593216818\"" +
                  "data-normal=\"https://image.slidesharecdn.com/detoanhaiduong20140728-150122220957-conversion-gate01/95/tuyn-tp-ton-thi-vo-lp-10-qua-cc-nm-tnh-hi-dng-nht-hiu-1-638.jpg?cb=1593216818\"" +
                  "data-full=\"https://image.slidesharecdn.com/detoanhaiduong20140728-150122220957-conversion-gate01/95/tuyn-tp-ton-thi-vo-lp-10-qua-cc-nm-tnh-hi-dng-nht-hiu-2-1024.jpg?cb=1593216818\"" +
                  "alt=\"Tuyển tập đề Toán thi vào lớp 10 qua các năm tỉnh Hải Dương [Nhật Hiếu]\"/>" +
            "</section>";
            Regex trex = new Regex("data-full=\"https://[a-zA-Z0-9./-]*.jpg");
            Regex rexLnk = new Regex("https://[a-zA-Z0-9./-]*.jpg");
            Regex rexName = new Regex("[1-9][0-9]*-[1-9][0-9]{2,3}.jpg$");
            foreach(Match match in trex.Matches(tstr))
            {
                MessageBox.Show(rexName.Match(rexLnk.Match(match.Value).Value).Value);
            }
            
        }
    }
}
