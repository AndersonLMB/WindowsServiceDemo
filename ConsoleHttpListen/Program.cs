using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleHttpListen
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            StartTileListener(772);

            //HttpListener listener777 = new HttpListener();
            //listener777.Prefixes.Add("http://localhost:777/");
            //while (true)
            //{
            //    listener777.Start();
            //    HttpListenerContext context = listener777.GetContext();
            //    HttpListenerRequest request = context.Request;
            //    // "/tile?x=0&y=0&z=1"


            //    var special = false;
            //    byte[] buff = { };

            //    var rawUrl = request.RawUrl;
            //    if (rawUrl.Split('?')[0] != "/tile")
            //    {
            //        //listener777.Stop();
            //    }
            //    else
            //    {
            //        if (rawUrl.Split('?').Length > 0)
            //        {
            //            var split = rawUrl.Split('?')[1].Split('&');
            //            Dictionary<string, string> kvs = new Dictionary<string, string>();
            //            foreach (var item in split)
            //            {
            //                var itemSplit = item.Split('=');
            //                var k = itemSplit[0];
            //                var v = itemSplit[1];
            //                kvs.Add(k, v);
            //            }
            //            var url = String.Format("http://t0.tianditu.com/DataServer?T=vec_w&x={0}&y={1}&l={2}", kvs["x"], kvs["y"], kvs["z"]);
            //            WebClient client = new WebClient();
            //            var data = client.DownloadData(new Uri(url));
            //            special = true;
            //            buff = data;
            //            ;
            //        }
            //    }





            //    HttpListenerResponse response = context.Response;

            //    //http://t0.tianditu.com/DataServer?T=vec_w&x=0&y=0&l=1
            //    string responseString = String.Format("<HTML><BODY> Hello world! </BODY></HTML>");
            //    byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            //    response.ContentLength64 = buffer.Length;
            //    var finalLength = buffer.Length;

            //    if (special)
            //    {
            //        buffer = buff;
            //        response.ContentLength64 = buff.Length;
            //        finalLength = buff.Length;

            //    }


            //    System.IO.Stream output = response.OutputStream;



            //    output.Write(buffer, 0, finalLength);
            //    output.Close();
            //    listener777.Stop();
            //}
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
                        var split = rawUrl.Split('?')[1].Split('&');
                        Dictionary<string, string> kvs = new Dictionary<string, string>();
                        foreach (var item in split)
                        {
                            var itemSplit = item.Split('=');
                            var k = itemSplit[0];
                            var v = itemSplit[1];
                            kvs.Add(k, v);
                        }
                        var url = String.Format("http://t0.tianditu.com/DataServer?T=vec_w&x={0}&y={1}&l={2}", kvs["x"], kvs["y"], kvs["z"]);
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
    }
}
