using CookComputing.XmlRpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Node1;



namespace server_starter
{
    class ServerStarter
    {
        static void Main(string[] args)
        {
            Console.WriteLine("jhsgdfjds");
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:80/");
            try { listener.Start(); }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            //   while (true)
            //   {
            Console.WriteLine("jhsgdfjds");
            HttpListenerContext context = listener.GetContext();
            Console.WriteLine("jhsgdfjds");
            XmlRpcListenerService svc = new CsServer();
            // XmlRpcListenerService svc = new XmlRpcStateNameService();
            Console.WriteLine("jhsgdfjds");
           // svc.ProcessRequest(context);
            //   }

            Console.WriteLine("server started");
        }
    }
}
