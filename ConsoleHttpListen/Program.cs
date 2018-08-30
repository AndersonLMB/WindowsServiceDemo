using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Net.Sockets;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ConsoleHttpListen
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            new Thread(() =>
            {
                StartTestListener(771);
            }).Start();

            new Thread(() =>
            {
                StartTileListener(772);
            }).Start();

        }

        public static void StartTileListener(int port)
        {
            HttpListener listener777 = new HttpListener();
            listener777.Prefixes.Add(String.Format("http://localhost:{0}/", port));
            while (true)
            {
                listener777.Start();
                HttpListenerContext context = listener777.GetContext();
                HttpListenerRequest request = context.Request;
                var special = false;
                byte[] buff = { 0 };
                var rawUrl = request.RawUrl;
                if (rawUrl.Split('?')[0] != "/tile")
                {
                    if (rawUrl.Split('?')[0] == "favicon.ico")
                    {
                        special = true;
                        var url = String.Format("http://t0.tianditu.com/DataServer?T=vec_w&x={0}&y={1}&l={2}", 1, 1, 1);
                        WebClient client = new WebClient();
                        var data = client.DownloadData(new Uri(url));
                        special = true;
                        buff = data;
                        ;
                    }
                }
                else
                {
                    if (rawUrl.Split('?').Length > 0)
                    {

                        var x = request.QueryString["x"];
                        var y = request.QueryString["y"];
                        var z = request.QueryString["z"];
                        var nul = request.QueryString["null"];
                        var url = String.Format("http://t0.tianditu.com/DataServer?T=vec_w&x={0}&y={1}&l={2}", x, y, z);
                        WebClient client = new WebClient();
                        var data = client.DownloadData(new Uri(url));
                        special = true;
                        buff = data;
                        ;
                    }
                }
                HttpListenerResponse response = context.Response;
                response.ContentLength64 = buff.Length;
                var outstream = response.OutputStream;
                for (int i = 0; i < buff.Length; i++)
                {
                    outstream.WriteByte(buff[i]);
                }
                response.Close();

                //http://t0.tianditu.com/DataServer?T=vec_w&x=0&y=0&l=1
                listener777.Stop();
            }


        }
        public static void StartTestListener(int port)
        {


            var listener = new HttpListener();
            listener.Prefixes.Add(String.Format("http://localhost:{0}/", port));


            while (1 == 1)
            {
                listener.Start();

                listener.BeginGetContext((a) =>
                {

                    HttpListener hl = (HttpListener)a.AsyncState;
                    var ctx = hl.EndGetContext(a);
                    var req = ctx.Request;
                    var res = ctx.Response;
                    string responseString = "<HTML><BODY> Hello world!</BODY></HTML>";
                    var buff = System.Text.Encoding.UTF8.GetBytes(responseString);
                    res.ContentLength64 = buff.Length;

                    var output = res.OutputStream;
                    output.Write(buff, 0, buff.Length);
                    output.Close();

                }, listener);
                var context = listener.GetContext();
                var request = context.Request;
                var response = context.Response;
                var outStream = response.OutputStream;
            }

        }
    }
}
