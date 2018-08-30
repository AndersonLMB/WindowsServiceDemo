using System.Web;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System;

namespace TcpListen
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpListener listener777 = new HttpListener();
            listener777.Prefixes.Add("http://localhost:777/");
            while (true)
            {
                listener777.Start();
                HttpListenerContext context = listener777.GetContext();
                HttpListenerRequest request = context.Request;
                // "/tile?x=0&y=0&z=1"

                var rawUrl = request.RawUrl;
                var split = rawUrl.Split('?')[1].Split('&');



                HttpListenerResponse response = context.Response;

                //http://t0.tianditu.com/DataServer?T=vec_w&x=0&y=0&l=1
                string responseString = String.Format("<HTML><BODY> Hello world! </BODY></HTML>");
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                response.ContentLength64 = buffer.Length;
                System.IO.Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();
                listener777.Stop();
            }

            ListenOn(777);

            //Continue(778);
        }

        public static void ListenOn(int port)
        {

        }

        static void Continue(int port)
        {
            HttpListener httpListener = new HttpListener();
            httpListener.Prefixes.Add(String.Format("http://localhost:{0}/", port));
            httpListener.Start();
            HttpListenerContext context = httpListener.GetContext();
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;
            string responseString = String.Format("<HTML><BODY> Hello world!  {0} <a href='http://localhost:{1}/'>http://localhost:{1}/</a>   </BODY></HTML>", port, port + 1);
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();
            httpListener.Stop();

            Continue(port + 1);
        }
    }
}
