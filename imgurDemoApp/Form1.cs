using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace imgurDemoApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public class resp
        {
            public List<img> data { get; set; }
        }

        public class img
        {
            public string link { get; set; }
        }

        private resp GetImages(string albumHash, string clientId)
        {
            resp result = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"https://api.imgur.com/3/album/{albumHash}/images");

                //ADD Header
                WebHeaderCollection myWebHeaderCollection = request.Headers;
                myWebHeaderCollection.Add("Authorization", $"Client-ID {clientId}");


                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                string json = readStream.ReadToEnd();

                result = JsonConvert.DeserializeObject<resp>(json);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.ToString());
                throw;
            }
            return result;
        }

        private Image GetImageFromUrl(string url)
        {
            Image result;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream receiveStream = response.GetResponseStream();
                result = System.Drawing.Image.FromStream(receiveStream);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.ToString());
                throw;
            }
            return result;
        }

        private void Btn_go_Click(object sender, EventArgs e)
        {
            //抓API list 出有多少東西
            var imgurData = GetImages("tzZDWAD", "4f37f9a94479861");

            if (imgurData == null)
            {
                return;
            }

            //download 一張照片下來
            Image image = GetImageFromUrl(imgurData.data[0].link);

            if (image == null)
            {
                return;
            }

            pb.Image = image;

            //json --> object
            Console.WriteLine(imgurData.data.Count);

            


        }
    }
}
