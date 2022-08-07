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
    public partial class KakaoMapPage : Form
    {
        List<Tuple<string, double, double>> tuples = new List<Tuple<string, double, double>>();
        public KakaoMapPage()
        {
            InitializeComponent();
        }

        private void KakaoMapPage_Load(object sender, EventArgs e)
        {
            // WebBrowser 컨트롤에 "kakaoMap.html" 을 표시한다. 
            Version ver = webBrowser1.Version;
            string name = webBrowser1.ProductName;
            string str = webBrowser1.ProductVersion;
            string html = "kakaoMap.html";
            string dir = System.IO.Directory.GetCurrentDirectory();
            string path = System.IO.Path.Combine(dir, html);
            webBrowser1.Navigate(path);
        }
        public void Search(string area)//지역 검색
        {
            // 요청을 보낼 url 
            string site = "https://dapi.kakao.com/v2/local/search/address.json";
            string query = string.Format("{0}?query={1}", site, area);

            WebRequest request = WebRequest.Create(query); // 요청 생성. 
            string api_key = "9342ab3ff7365f224c463cd56577fa6f"; // API 인증키 입력. (각자 발급한 API 인증키를 입력하자)
            string header = "KakaoAK " + api_key;

            request.Headers.Add("Authorization", header); // HTTP 헤더 "Authorization" 에 header 값 설정. 
            WebResponse response = request.GetResponse(); // 요청을 보내고 응답 객체를 받는다. 
            Stream stream = response.GetResponseStream(); // 응답객체의 결과물
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            String json = reader.ReadToEnd(); // JOSN 포멧 문자열

            JavaScriptSerializer js = new JavaScriptSerializer(); // (Reference 에 System.Web.Extensions.dll 을 추가해야한다)
            var dob = js.Deserialize<dynamic>(json);
            var docs = dob["documents"];
            object[] buf = docs;
            int length = buf.Length;

            for (int i = 0; i < length; i++) // 지역명, 위도, 경도 읽어오기. 
            {
                string address_name = docs[i]["address_name"];
                double x = double.Parse(docs[i]["x"]); // 위도
                double y = double.Parse(docs[i]["y"]); // 경도
                tuples.Add(new Tuple<string, double, double>(address_name, x, y));
            }
        }
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void input(string str)
        {
            listBox1.Items.Clear();
            tuples.Clear();

            Search(textBox1.Text);
            for (int i = 0; i < tuples.Count; i++)
            {
                listBox1.Items.Add(tuples[i].Item1);
            }
        }
        private void ShowMap() // 위도, 경도에 해당하는 지역을 지도에 표시
        {
            var sel = tuples[listBox1.SelectedIndex];
            object[] arr = new object[] { sel.Item3, sel.Item2 }; // 위도, 경도
            object res = webBrowser1.Document.InvokeScript("panTo", arr); // html 의 panTo 자바스크립트 함수 호출. 
        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
                ShowMap();
        }
        private void textBox1_KeyDown(object sender,KeyEventArgs e)
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
                object[] arr = new object[] { textBox1.Text };
                webBrowser1.Document.InvokeScript("geo", arr); // html 의 geo 자바스크립트 함수 호출. 
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            webBrowser1.Document.InvokeScript("zoomIn"); // 줌인
        }

        private void button3_Click(object sender, EventArgs e)
        {
            webBrowser1.Document.InvokeScript("zoomOut"); // 줌아웃

        }


    }
}
