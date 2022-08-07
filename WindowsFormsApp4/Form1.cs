using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web.Script.Serialization;
using System.Net;
using System.IO;

namespace WindowsFormsApp4
{
    public partial class Form1 : Form
    {

        public Tuple<double, double> tuple_test;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // WebBrowser 컨트롤에 "kakaoMap.html" 을 표시한다. 
            Version ver = webBrowser1.Version;
            string name = webBrowser1.ProductName;
            string str = webBrowser1.ProductVersion;
            string html = "C:\\Users\\dydrk\\source\\repos\\WindowsFormsApp4\\WindowsFormsApp4\\resources\\js\\kakaoMap.html";
            string dir = System.IO.Directory.GetCurrentDirectory();
            string path = System.IO.Path.Combine(dir, html);
            webBrowser1.Navigate(path);
           
        }
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    input(textBox1.Text);
                    break;
            }
        }
        
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //object[] arr = new object[] { textBox1.Text };
                string content = textBox1.Text;
                string w = content.Split(',')[0];
                string g = content.Split(',')[1];
                tuple_test=new Tuple<double, double>(Double.Parse(w), Double.Parse(g));
                ShowMap();
                //webBrowser1.Document.InvokeScript("geo", arr); // html 의 geo 자바스크립트 함수 호출. 
            }
            catch (Exception ex)
            {
                ex.ToString();
                
            }
        }
        private void input(string str)
        {
            //listBox1.Items.Clear();

            //Search(textBox1.Text);
           // for (int i = 0; i < tuples.Count; i++)
            //{
            //    listBox1.Items.Add(tuples[i].Item1);
           // }
        }
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
                ShowMap();
        }
        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            
        }
        private void button4_Click(object sender, EventArgs e)
        {
            webBrowser1.Document.InvokeScript("zoomIn"); // 줌인
        }

        private void button3_Click(object sender, EventArgs e)
        {
            webBrowser1.Document.InvokeScript("zoomOut"); // 줌아웃
        }
        private void ShowMap() // 위도, 경도에 해당하는 지역을 지도에 표시
        {
            //var sel = tuples[listBox1.SelectedIndex];
            var sel = tuple_test;
            object[] arr = new object[] { sel.Item1, sel.Item2 }; // 위도, 경도
            object res = webBrowser1.Document.InvokeScript("panTo", arr); // html 의 panTo 자바스크립트 함수 호출. 

        }

        private void button5_Click(object sender, EventArgs e)
        {
            webBrowser1.Document.InvokeScript("marker");
        }
    }
}
