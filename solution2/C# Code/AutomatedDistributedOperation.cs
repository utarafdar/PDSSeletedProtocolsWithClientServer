using CookComputing.XmlRpc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Node1
{
    class AutomatedDistributedOperation
    {
        public void startDistributedRicartAndAgarwala()
        {
            try
            {
                Thread.Sleep(1);
            }
            catch (Exception e1)
            {
                // TODO Auto-generated catch block
                Console.WriteLine(e1.StackTrace);
            }
            CsClient client = new CsClient();
            NetworkBean data = new NetworkBean();
            Console.WriteLine("Distributed operation has started with Ricart and Agarwala algorithm");
            Console.WriteLine("*********************ID = " + data.getpId() + "*********************");
            if (data.getpId().Equals(data.getMasterPid()))
                Console.WriteLine("****************************I am the Master Node*****************");
            Ricart_Agarwala RA = new Ricart_Agarwala();
            String response;
            String message = "";

            ServerOperations serveroperaation = new ServerOperations();
            LamportClock clock = new LamportClock();
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            while (stopwatch.Elapsed < TimeSpan.FromSeconds(20))
            {
                //a) Wait a random amount of time (1-4 seconds)
                Random rnd = new Random();
                int waitTime = data.getRnd();
                
                Thread.Sleep(waitTime);
               
                Console.WriteLine(waitTime);
                //b) Read the string variable from the master node
                //c) Append some random english word to this string
                //d) Write the updated string to the master node 
                // request for critical section to perform these steps

               // RA.setMyTimeStamp((clock.getValue() + 1).ToString());
                clock.tick();
                clock.setRequestTimeStamp(clock.getValue()+1);
                foreach (KeyValuePair<string, string> entry in data.getpId_Address())
                {
                    String sendAddress = entry.Value;
                    String iD = entry.Key;

                    try
                    {
                        RA_sendReq proxy = XmlRpcProxyGen.Create<RA_sendReq>();
                        proxy.Url = sendAddress;
                        RA.setRequestCR(true);
                        clock.sendAction();
                     
                        //("sending request message to "+entry.getKey()+"at local time :"+dateFormat.format(new Date()));
                        Console.WriteLine("sending request message to " + iD + "at local time :" + DateTime.Now.ToString("HH:mm:ss:fff"));

                        response = proxy.RA_sendRequest(data.getpId(), (clock.getValue()).ToString());
                        //Console.WriteLine("sending message I am New Master to :" + sendAddress);

                        if (response.Contains("OK"))
                        {
                            String[] parts = response.Split('_');
                            clock.receiveAction(parts[1]);
                            Console.WriteLine("Recieved OK message from " + iD + "at local time :" + DateTime.Now.ToString("HH:mm:ss:fff"));
                            RA.setNumberOfNodesResponded(RA.getNumberOfNodesResponded() + 1);

                        }
                        else
                        {
                            String[] parts = response.Split('_');
                            clock.receiveAction(parts[1]);
                        }

                    }
                    catch (Exception e)
                    {
                        // TODO Auto-generated catch block
                        Console.WriteLine("Exception from the  node/nremove node from network");

                        client.removeNode(iD);
                        data.getpId_Address().Remove(sendAddress);
                        if (iD.Equals(data.getMasterPid()))
                            client.bullyAlgorithm();
                        RA.setNumberOfNodesResponded(RA.getNumberOfNodesResponded() + 1);

                      //  Console.WriteLine(e.StackTrace);
                    }


                }//for each
                Console.WriteLine("Number of OK messages recieved =" + RA.getNumberOfNodesResponded());
                Console.WriteLine("Number of nodes in network =" + data.getpId_Address().Count());
                if (RA.getNumberOfNodesResponded() != data.getpId_Address().Count())
                    Console.WriteLine("Waiting for other nodes to send OK message...");
                // System.out.println(data.getpId_Address().size()+""+RA.getNumberOfNodesResponded());
                int numberOfOkMsgs = RA.getNumberOfNodesResponded();
                while (data.getpId_Address().Count() != RA.getNumberOfNodesResponded())
                {
                    numberOfOkMsgs = RA.getNumberOfNodesResponded();
                    //System.out.println(numberOfOkMsgs);
                    try
                    {
                        Thread.Sleep(100);
                    }
                    catch (Exception e)
                    {
                        // TODO Auto-generated catch block
                        Console.WriteLine(e.StackTrace);
                    }
                    //keep waiting 	
                    //break from the loop only when all the nodes have sent OK message
                }
                Console.WriteLine("Recieved OK message from all the nodes");
                Console.WriteLine("Critical region in my control");
                Console.WriteLine("Distributed process started at " + DateTime.Now.ToString("HH:mm:ss:fff"));
                RA.setRequestCR(false);
                RA.setUsingCR(true);
                try
                {

                    distribRead proxy = XmlRpcProxyGen.Create<distribRead>();
                    proxy.Url = data.getMasterAddress();
                    Console.WriteLine("Reading the variable String from master node");
                    message = proxy.distributedRead(data.getpId());
                    Console.WriteLine("The string is " + message);
                    String randomWord = WordList.words[rnd.Next(0, WordList.words.Length)];
                    Console.WriteLine("Appending '" + randomWord + "' to the string " + message);
                    distribWrite proxyWrite = XmlRpcProxyGen.Create<distribWrite>();
                    proxyWrite.Url = data.getMasterAddress();
                    proxyWrite.distributedWrite(data.getpId(), randomWord);
                    data.getAddedWords().Add(randomWord);
                  /*  try
                    {

                        Thread.Sleep(rnd.Next(1000, 3000));
                        //System.out.println((long)(Math.random() * 4000+1000));
                    }
                    catch (Exception e)
                    {
                        // TODO Auto-generated catch block
                        Console.WriteLine(e.StackTrace);
                    }*/
                }
                catch (Exception e)
                {
                   // Console.WriteLine(e.StackTrace);
                    Console.WriteLine("No response from the master node");
                    Console.WriteLine("Starting election to elect new master node ( Bully algorith)");
                    client.removeNode(data.getMasterAddress());
                    data.getpId_Address().Remove(data.getMasterPid());
                    client.bullyAlgorithm();

                }
                Console.WriteLine("Distributed operation done at time " + DateTime.Now.ToString("HH:mm:ss:fff"));
                //release all the requests from queue		
                Console.WriteLine("Exiting critical Section");
                Console.WriteLine("Releasing all deffered requests");
                RA.releaseDeferedRequests();
            }
            //c) After the process has ended all the nodes read the final string from the master node and
            //write it to the screen. Moreover they check if all the words they added to the string are
            //present in the final string. The result of this check is also written to the screen. 


            // propagate that it is done with the process.
            Console.WriteLine("done with distributed process");

            clock.tick();
            //loop
            foreach (KeyValuePair<string, string> entry in data.getpId_Address())
            {
                String sendAddress = entry.Value;
                String iD = entry.Key;

                try
                {
                    distdProcessDone proxy = XmlRpcProxyGen.Create<distdProcessDone>();
                    proxy.Url = sendAddress;
                    proxy.distributedProcessDone(iD);
                    // Console.WriteLine("sending message I am New Master to :" + sendAddress);

                }

               
                catch (Exception e)
                {
                    // TODO Auto-generated catch block
                    Console.WriteLine("Could not send the message succesfully.. node denied connection");
                  //  Console.WriteLine(e.StackTrace);
                }

                RA.setMyTimeStamp("0");
            }
            //int number = data.getNoOfNodesFinishedOperation();
            while (data.getpId_Address().Count() > data.getNoOfNodesFinishedOperation())
            {
              //  number = data.getNoOfNodesFinishedOperation();

                try
                {
                    Thread.Sleep(100);
                }
                catch (Exception e)
                {
                    // TODO Auto-generated catch block
                    Console.WriteLine(e.StackTrace);
                }
            }

            Console.WriteLine("out loop");
            checkFinalString();
        }

        public void checkFinalString()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            while (stopwatch.Elapsed < TimeSpan.FromSeconds(20))
            {

            }
            String message = "";
            NetworkBean data = new NetworkBean();
            CsClient client = new CsClient();
            ServerOperations serveroperaation = new ServerOperations();
            Console.WriteLine("**************Reading The final String*******************");


            try
            {
                distribRead proxy = XmlRpcProxyGen.Create<distribRead>();
                proxy.Url = data.getMasterAddress();
                message = proxy.distributedRead(data.getpId());
                Console.WriteLine("the final string is \n");
                Console.WriteLine(message + "\n");


            }
            catch (Exception e)
            {
                // TODO Auto-generated catch block
                Console.WriteLine("Could not send the message succesfully.. node denied connection");
                Console.WriteLine("No response from the master node");
                Console.WriteLine("Starting election to elect new master node ( Bully algorith)");
                client.removeNode(data.getMasterPid());
                data.getpId_Address().Remove(data.getMasterPid());
                client.bullyAlgorithm();
                Console.WriteLine(e.StackTrace);
            }
            Boolean wordFlag = true;
            foreach (String word in data.getAddedWords())
            {
                if (message.Contains(word))
                    Console.WriteLine(word + " is present in the final string");
                else
                {
                    Console.WriteLine(word + " is not present in the final string");
                    wordFlag = false;
                }
            }
            if (wordFlag)
                Console.WriteLine("all the words are added to the string in master node");
            //end//
            data.getAddedWords().Clear();
            data.setNoOfNodesFinishedOperation(0);
            data.setStartRicartAgarwala(false);
            data.setStartCME(false);


        }

        public void startDistributedCME()
        {
            try
            {
                Thread.Sleep(100);
            }
            catch (Exception e1)
            {
                // TODO Auto-generated catch block
                Console.WriteLine(e1.StackTrace);
            }

            CsClient client = new CsClient();
            NetworkBean data = new NetworkBean();
            //run the loop for 20 seconds
            Console.WriteLine("Distributed operation has started with Centralized Mutual Exclusion");
            Console.WriteLine("*********************ID = " + data.getpId() + "*********************");
            if (data.getpId().Equals(data.getMasterPid()))
                Console.WriteLine("************************I am the Master Node********************");


            String response;
            String message = "";

            ServerOperations serveroperaation = new ServerOperations();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            while (stopwatch.Elapsed < TimeSpan.FromSeconds(20))
            {
                //a) Wait a random amount of time (1-4 seconds)
                Random rnd = new Random();
                int waitTime = data.getRnd();

                Thread.Sleep(waitTime);

                //b) Read the string variable from the master node
                //c) Append some random english word to this string
                //d) Write the updated string to the master node 
                // request for critical section to perform these steps

                try
                {//request
                 //get response
                 // if OK do operation
                 //release message
                    reqCMEDistributiveOperation proxy = XmlRpcProxyGen.Create<reqCMEDistributiveOperation>();
                    proxy.Url = data.getMasterAddress();
                    Console.WriteLine("Request Sent to Master for Distributed Operation at " + DateTime.Now.ToString("HH:mm:ss:fff"));
                    Console.WriteLine("waiting for the response from Master");
                    response = proxy.requestCMEDistributiveOperation(data.getpId());
                    if (response.Equals("OK"))
                    {

                        Console.WriteLine("Recived OK message from Master at " + DateTime.Now.ToString("HH:mm:ss:fff"));
                        distribRead proxyread = XmlRpcProxyGen.Create<distribRead>();
                        proxyread.Url = data.getMasterAddress();
                        Console.WriteLine("Reading the variable String from master node");
                        message = proxyread.distributedRead(data.getpId());
                        Console.WriteLine("The string is " + message);
                    /*    try
                        {

                            Thread.Sleep(rnd.Next(1000, 3000));
                            //System.out.println((long)(Math.random() * 4000+1000));
                        }
                        catch (Exception e)
                        {
                            // TODO Auto-generated catch block
                            Console.WriteLine(e.StackTrace);
                        }*/
                        String randomWord = WordList.words[rnd.Next(0, 12)];
                        Console.WriteLine("Appending '" + randomWord + "' to the string " + message);
                        distribWrite proxyWrite = XmlRpcProxyGen.Create<distribWrite>();
                        proxyWrite.Url = data.getMasterAddress();
                        proxyWrite.distributedWrite(data.getpId(), randomWord);
                        data.getAddedWords().Add(randomWord);
                        
                        Console.WriteLine(randomWord + " appended to string");
                        Console.WriteLine("distributed Operatiion completed at " + DateTime.Now.ToString("HH:mm:ss:fff"));
                        Console.WriteLine("Releasing the CR");
                        CMErelResource proxyRelease = XmlRpcProxyGen.Create<CMErelResource>();
                        proxyRelease.Url = data.getMasterAddress();
                        proxyRelease.CMEreleaseResource(data.getpId());
                    }


                }
                catch (Exception e)
                {
                    // TODO Auto-generated catch block
                   // Console.WriteLine(e.StackTrace);
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Could not send the message succesfully.. node denied connection");
                    Console.WriteLine("No response from the master node");
                    Console.WriteLine("Starting election to elect new master node ( Bully algorith)");
                    client.removeNode(data.getMasterPid());
                    data.getpId_Address().Remove(data.getMasterPid());
                    client.bullyAlgorithm();
                //    Console.WriteLine(e.StackTrace);
                }

            }

            //c) After the process has ended all the nodes read the final string from the master node and
            //write it to the screen. Moreover they check if all the words they added to the string are
            //present in the final string. The result of this check is also written to the screen. 


            // propagate that it is done with the process.
            Console.WriteLine("done with distributed process");
            //loop
            foreach (KeyValuePair<string, string> entry in data.getpId_Address())
            {
                String sendAddress = entry.Value;
                String iD = entry.Key;

                try
                {
                    distdProcessDone proxy = XmlRpcProxyGen.Create<distdProcessDone>();
                    proxy.Url = sendAddress;
                    proxy.distributedProcessDone(iD);
                    // Console.WriteLine("sending message I am New Master to :" + sendAddress);

                }
                catch (Exception e)
                {
                    // TODO Auto-generated catch block
                    Console.WriteLine("Could not send the message succesfully.. node denied connection");
                    // Console.WriteLine(e.StackTrace);
                    Console.WriteLine("remove node from network");
                    client.removeNode(iD);
                    data.getpId_Address().Remove(sendAddress);
                    if(data.getMasterPid().Equals(iD))
                    {
                        client.bullyAlgorithm();
                    }
                }


            }
          //  int number = data.getNoOfNodesFinishedOperation();
            while (data.getpId_Address().Count() > data.getNoOfNodesFinishedOperation())
            {
             //   number = data.getNoOfNodesFinishedOperation();

                try
                {
                    Thread.Sleep(100);
                }
                catch (Exception e)
                {
                    // TODO Auto-generated catch block
                    Console.WriteLine(e.StackTrace);
                }
            }

            Console.WriteLine("out loop");


            checkFinalString();

        }

    }

}

