using CookComputing.XmlRpc;
using Node1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Server_starter
{
    class ServerStarter
    {
        static void Main(string[] args)
        {
            
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:89/");
            try { listener.Start(); }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
              while (true)
               {
            
            HttpListenerContext context = listener.GetContext();
               
           
            XmlRpcListenerService svc = new CsServer();
               // svc.InitializeLifetimeService();
                svc.ProcessRequest(context);
               }

            Console.WriteLine("server started");
        }
    }
}
