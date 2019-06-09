using CookComputing.XmlRpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Remoting;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Node1
{
    class ServerOperations : XmlRpcService
    {
      
        public void start(int port)
        {
            // for CookComputing.XmlRpcV2
            // RemotingConfiguration.Configure("SumAndDiff.exe.config", false);
            // for CookComputing.XmlRpc
            /*  try { RemotingConfiguration.Configure("C:\\Users\\UMAIR\\Documents\\Visual Studio 2015\\Projects\\Node1\\Node1\\App.config"); }
              catch(Exception e)
              {
                  Console.WriteLine(e.Message);
              }

              RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(CsServer),
                "CsServer.rem",
                WellKnownObjectMode.Singleton);
              Console.WriteLine("Press to shutdown");
              Console.ReadLine();*/
            // system.("csc /r:system.web.dll /r:CookComputing.XmlRpcV2.dll /target:library CsServer.cs ");
            HttpListener listener = new HttpListener();
            string ip = Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString();
           
            String url = "http://"+ip+":" + port+"/" ;
            Console.WriteLine(url);
            String iD = DateTime.Now.ToString("yyyyMMddHHmmssfff");            
            NetworkBean data = new NetworkBean();
            CentralisedMutualExclusion cme = new CentralisedMutualExclusion();
            Ricart_Agarwala RA = new Ricart_Agarwala();
            LamportClock clock = new LamportClock();
            data.setAddress(url);
            //data.setpId(Long.parseLong(iD));
            data.setpId(iD);
            data.setPort(port);
            data.setMasterAddress(url);
            data.setMasterPid(iD);
            cme.setCRInUse(false);
            RA.setRequestCR(false);
            RA.setUsingCR(false);
            RA.setNumberOfNodesResponded(0);
            RA.setMyTimeStamp("0");
            data.setDistributedStringVariable("");
            data.setStartCME(false);
            data.setStartRicartAgarwala(false);
            data.setMessage("");
            clock.setC(1);
            data.setNoOfNodesFinishedOperation(0);
            data.setRnd();
            //Console.WriteLine(url+iD);
            listener.Prefixes.Add(url);
            try { listener.Start(); }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            while (true)
            {

            HttpListenerContext context = listener.GetContext();


                XmlRpcListenerService svc = new CsServer();
               
                svc.ProcessRequest(context);
                Thread.Sleep(1);
            }

            Console.WriteLine("server started");
        }

    }
}
