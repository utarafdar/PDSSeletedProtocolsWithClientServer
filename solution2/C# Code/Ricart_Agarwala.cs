using CookComputing.XmlRpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node1
{
    class Ricart_Agarwala
    {
        private static String myTimeStamp;
        private static Queue<String> ricartAgarwalaQueue = new Queue<String>();
        private static Boolean requestCR, usingCR;
        private static int numberOfNodesResponded;



        public int getNumberOfNodesResponded()
        {
            return numberOfNodesResponded;
        }
        public void setNumberOfNodesResponded(int numberOfNodesResponded)
        {
            Ricart_Agarwala.numberOfNodesResponded = numberOfNodesResponded;
        }

        public String getMyTimeStamp()
        {
            return myTimeStamp;
        }
        public void setMyTimeStamp(String myTimeStamp)
        {
            Ricart_Agarwala.myTimeStamp = myTimeStamp;
        }
        public Queue<String> getRicartAgarwalaQueue()
        {
            return ricartAgarwalaQueue;
        }
        public void setRicartAgarwalaQueue(Queue<String> ricartAgarwalaQueue)
        {
            Ricart_Agarwala.ricartAgarwalaQueue = ricartAgarwalaQueue;
        }
        public Boolean isRequestCR()
        {
            return requestCR;
        }
        public void setRequestCR(Boolean requestCR)
        {
            Ricart_Agarwala.requestCR = requestCR;
        }
        public Boolean isUsingCR()
        {
            return usingCR;
        }
        public void setUsingCR(Boolean usingCR)
        {
            Ricart_Agarwala.usingCR = usingCR;
        }
        //determine the critical section entry
        public String RA_RecieveRequest(String iD, String timeStamp)
        {
            LamportClock clock = new LamportClock();
            clock.receiveAction(timeStamp);
           
            NetworkBean data = new NetworkBean();
            
            Console.WriteLine("recieved RA operation request from " + iD + "at time " + DateTime.Now.ToString("HH:mm:ss:fff"));
            if (usingCR)
            {
                Console.WriteLine("Already using CR.. Defer the request " + iD);
                deferRequest(iD);
                return "Deferred_"+clock.sendAction();

            }
            //myTimeStamp
            if (requestCR)
            {
               // clock.receiveAction(timeStamp);
                //timeStamp = clock.getValue().ToString();
                if ((clock.getRequestTimeStamp().ToString()).CompareTo(timeStamp) > 0)
                {
                    Console.WriteLine("already requested CR..my request Time stamp=" + clock.getRequestTimeStamp() + "  " + iD + " request timestamp =" + timeStamp);
                    Console.WriteLine("Send OK Message");
                    return "OK_" + clock.sendAction();
                    
                }
                if ((clock.getRequestTimeStamp().ToString()).CompareTo(timeStamp) < 0)
                {
                    Console.WriteLine("already requested CR...my request Time stamp= " + clock.getRequestTimeStamp() + " the node " + iD + " request timestamp is =" + timeStamp);
                    Console.WriteLine("Deferr the request");
                    deferRequest(iD);
                    return "Deferred_" + clock.sendAction();
                }
                if ((clock.getRequestTimeStamp().ToString()).CompareTo(timeStamp) == 0)
                {
                    if (iD.CompareTo(data.getpId()) > 0)
                    {
                        Console.WriteLine("already requested CR...my request Time stamp= " + clock.getRequestTimeStamp() + " the node " + iD + " request timestamp is =" + timeStamp);
                        Console.WriteLine("Both clocktimes are same...my process id is" +data.getpId() + " the node is is " + iD + " My id is lesser" );
                        Console.WriteLine("Deferr the request");
                        deferRequest(iD);
                        return "Deferred_" + clock.sendAction();
                    }

                    if (iD.CompareTo(data.getpId()) < 0)
                    {
                        Console.WriteLine("already requested CR...my request Time stamp= " + clock.getRequestTimeStamp() + " the node " + iD + " request timestamp is =" + timeStamp);
                        Console.WriteLine("Both clocktimes are same...my process id is" + data.getpId() + " the node is is " + iD + " My id is higher");
                        Console.WriteLine("send ok message");
                        return "OK_" + clock.sendAction();
                    }
                }
            }
            Console.WriteLine("Not Using or requesting CR... Send OK Message");
            return "OK_" + clock.sendAction();
        }
        private void deferRequest(String iD)
        {
            // TODO Auto-generated method stub
            ricartAgarwalaQueue.Enqueue(iD);
            //wait until its released

        }
        public void releaseDeferedRequests()
        {
            //remove the Ids from queue
            //send OK message to the Ids
            //queue.Count != 0
            while (ricartAgarwalaQueue.Count != 0)
            {
                String iD = ricartAgarwalaQueue.Dequeue();


                NetworkBean data = new NetworkBean();
                String address = data.getpId_Address()[iD];
                ServerOperations serveroperaation = new ServerOperations();
                LamportClock clock = new LamportClock();
                try
                {
                    rcvOkFromRA proxy = XmlRpcProxyGen.Create<rcvOkFromRA>();
                    proxy.Url = address;
                    proxy.recieveOkFromRicartAgarwala(data.getpId(),clock.sendAction().ToString() );
                    Console.WriteLine("Releasing the deferred request from " + iD + " from queue ");
                    Console.WriteLine("OK message sent at " + DateTime.Now.ToString("HH:mm:ss:fff"));

                }
                catch (Exception e)
                {
                    // TODO Auto-generated catch block
                    Console.WriteLine("Could not send the message succesfully.. node denied connection");
                    Console.WriteLine(e.StackTrace);
                }



            }
            numberOfNodesResponded = 0;
            usingCR = false;
        }

    }
}
