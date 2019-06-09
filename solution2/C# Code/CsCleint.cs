using CookComputing.XmlRpc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Node1
{


    class CsClient
    {
        String addr, ide;
        public void join(String sendAddress)
        {

            //

            NetworkBean data = new NetworkBean();
            joinNetw proxy = XmlRpcProxyGen.Create<joinNetw>();
            proxy.Url = sendAddress;
            try
            {
                String recievedId = proxy.joinNetwork(data.getpId(), data.getAddress());
                data.addValuesToDictionary(recievedId, sendAddress);
            }
            catch (Exception e)
            {
                // TODO Auto-generated catch block
                Console.WriteLine(e.StackTrace);
            }


        }


        public void addReturnAdressToList(String Address)
        {

            NetworkBean data = new NetworkBean();
            ServerOperations serveroperaation = new ServerOperations();
            updateAddrMap proxy = XmlRpcProxyGen.Create<updateAddrMap>();
            proxy.Url = Address;
            // XmlRpcClient server = serveroperaation.setXmlRpcConfig(Address);
            foreach (KeyValuePair<string, string> entry in data.getpId_Address())
            {
                try
                {
                    proxy.updateAddressMap(entry.Key, entry.Value);
                }
                catch (Exception e)
                {
                    // TODO Auto-generated catch block
                    Console.WriteLine(e.StackTrace);
                }

            }


        }

        public void propagateNewNodeAddress(Dictionary<String, String> returnAddressIdMap, String iD, String address)
        {
            // TODO Auto-generated method stub

            ServerOperations serveroperation = new ServerOperations();

            updateAddrMap proxy = XmlRpcProxyGen.Create<updateAddrMap>();

            foreach (KeyValuePair<string, string> entry in returnAddressIdMap)
            {
                try
                {
                    proxy.Url = entry.Value;
                    proxy.updateAddressMap(iD, address);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }

        }

        public void setMasterNode(String address)
        {
            try
            {
                NetworkBean data = new NetworkBean();
                ServerOperations serveroperaation = new ServerOperations();
                setMasterAddrId proxy = XmlRpcProxyGen.Create<setMasterAddrId>();
                proxy.Url = address;
                proxy.setMasterAddressId(data.getMasterPid(), data.getMasterAddress());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }






        public void bullyAlgorithm()
        {

            NetworkBean data = new NetworkBean();
            Boolean noMasterFound = true;
            // ServerOperations serveroperation = new ServerOperations();                        
            String response = "";
            foreach (KeyValuePair<string, string> entry in data.getpId_Address())
            {
                String sendAddress = entry.Value;
                long pId = Int64.Parse(entry.Key);
                long currentNodepId = Int64.Parse(data.getpId());

                if (pId > currentNodepId)
                {

                    try
                    {

                        elctMasterNode proxy = XmlRpcProxyGen.Create<elctMasterNode>();
                        proxy.Url = sendAddress;
                        response = proxy.electMasterNode(data.getpId());
                        Console.WriteLine(data.getpId() + "  Sends the bully election message to :" + sendAddress);
                        Console.WriteLine("response is :" + response);

                    }
                    catch (Exception e)
                    {
                        // TODO Auto-generated catch block
                        Console.WriteLine("no response");
                        //

                        removeNode(sendAddress);
                        data.getpId_Address().Remove(sendAddress);


                    }

                }
                if (response.Length > 0)
                {
                    noMasterFound = false;
                    break;
                }

            }

            // if no master is found then this node is the new master
            if (noMasterFound)
            {
                Console.WriteLine("No master found I am the new master");
                data.setMasterPid(data.getpId());
                data.setMasterAddress(data.getAddress());
                propagateNewMasterNode(data.getpId(), data.getAddress());

            }


        }

        public void propagateNewMasterNode(String masterid, String masteraddress)
        {
            // TODO Auto-generated method stub
            NetworkBean data = new NetworkBean();
            //ServerOperations serveroperation = new ServerOperations();           

            foreach (KeyValuePair<string, string> entry in data.getpId_Address())
            {
                String sendAddress = entry.Value;

                try
                {
                    setMasterAddrId proxy = XmlRpcProxyGen.Create<setMasterAddrId>();
                    proxy.Url = sendAddress;
                    proxy.setMasterAddressId(masterid, masteraddress);
                    Console.WriteLine("sending message I am New Master to :" + sendAddress);

                }
                catch (Exception e)
                {
                    // TODO Auto-generated catch block
                    Console.WriteLine("Could not send the message succesfully.. node denied connection");
                    // Console.WriteLine(e.StackTrace);
                }


            }
        }
        public void signOff()
        {
            NetworkBean data = new NetworkBean();

           
            foreach (KeyValuePair<string, string> entry in data.getpId_Address())
            {
                String sendAddress = entry.Value;
                String iD = entry.Key;

                try
                {
                    nodeSignoff proxy = XmlRpcProxyGen.Create<nodeSignoff>();
                    proxy.Url = sendAddress;
                    proxy.nodeSignOff(data.getpId());
                    Console.WriteLine("sending message Signing off to :" + sendAddress);

                }
                catch (Exception e)
                {
                    // TODO Auto-generated catch block
                    Console.WriteLine("Could not send the message succesfully.. node denied connection");
                    removeNode(iD);
                    //  Console.WriteLine(e.StackTrace);
                }


            }


            if (data.getMasterPid().Equals(data.getpId()))
            {

                foreach (KeyValuePair<string, string> entry in data.getpId_Address())
                {
                    addr = entry.Value;
                    ide = entry.Key;
                }

                foreach (KeyValuePair<string, string> entry in data.getpId_Address())
                {
                    try
                    {
                       // NetworkBean data = new NetworkBean();
                        ServerOperations serveroperaation = new ServerOperations();
                        setMasterAddrId proxy = XmlRpcProxyGen.Create<setMasterAddrId>();
                        proxy.Url = entry.Value;
                        proxy.setMasterAddressId(ide, addr);
                    }
                    catch (Exception e)
                    {
                      //  Console.WriteLine(e.StackTrace);
                    }
                }




                String response = "";
                foreach (KeyValuePair<string, string> entry in data.getpId_Address())
                {
                    String sendAddress = entry.Value;

                    try
                    {

                        callBullyAfterSignoff proxy = XmlRpcProxyGen.Create<callBullyAfterSignoff>();
                        proxy.Url = sendAddress;
                        response = proxy.callBullyAfterSignOff(data.getpId());
                        Console.WriteLine(data.getpId() + "  Sends the bully election message to :" + sendAddress);
                        Console.WriteLine("response is :" + response);

                    }
                    catch (Exception e)
                    {
                        // TODO Auto-generated catch block
                        Console.WriteLine("no response");
                        //  Console.WriteLine(e.StackTrace);
                    }


                    if (response.Length > 0)
                    {

                        break;
                    }
                }

            }



            data.getpId_Address().Clear();
            data.setMasterAddress(data.getAddress());
            data.setMasterPid(data.getpId());
        }

        public void removeNode(String iD)
        {
            NetworkBean data = new NetworkBean();
            ServerOperations serveroperation = new ServerOperations();




            rmvNodeFromNetwork proxy = XmlRpcProxyGen.Create<rmvNodeFromNetwork>();
            foreach (KeyValuePair<string, string> entries in data.getpId_Address())
            {
                try
                {
                    String Address = entries.Value;
                    String sendiD = entries.Key;
                    if (!sendiD.Equals(iD))
                    {
                        proxy.Url = entries.Value;
                        proxy.removerNodeFromNetwork(iD);
                    }
                }
                catch (Exception E)
                {
                    Console.WriteLine(E.StackTrace);
                    //

                }
            }

        }


        //
        public void startDistributedOpeartion(String approach)
        {
            // TODO Auto-generated method stub
            // call all the nodes to start operation (CME).
            NetworkBean data = new NetworkBean();
            ServerOperations serveroperation = new ServerOperations();



            foreach (KeyValuePair<string, string> entries in data.getpId_Address())
            {
                String sendAddress = entries.Value;
                String sendiD = entries.Key;
                try
                {
                    strtDistributedOperations proxy = XmlRpcProxyGen.Create<strtDistributedOperations>();
                    proxy.Url = sendAddress;
                    proxy.startDistributedOperations(approach);

                }


                catch (Exception e)
                {
                    // TODO Auto-generated catch block
                    //   removeNode(sendiD);
                    //  data.getpId_Address().Remove(sendiD);
                    //  if (sendiD.Equals(data.getMasterPid()))
                    // { bullyAlgorithm(); }
                    //   Console.WriteLine(e.StackTrace);
                }
            }





            if (approach.Equals("CME"))
            {

                AutomatedDistributedOperation ADO = new AutomatedDistributedOperation();
                Thread thread = new Thread(() => ADO.startDistributedCME());
                thread.Start();
                data.setStartCME(true);
            }
            if (approach.Equals("RA"))
            {

                AutomatedDistributedOperation ADO = new AutomatedDistributedOperation();
                Thread thread = new Thread(() => ADO.startDistributedRicartAndAgarwala());
                thread.Start();
                data.setStartRicartAgarwala(true);
            }
        }


        public void resetDistributedStringVariable()
        {
            NetworkBean data = new NetworkBean();
            ServerOperations serveroperation = new ServerOperations();

            try
            {
                resetDistributedMasterStr proxy = XmlRpcProxyGen.Create<resetDistributedMasterStr>();
                proxy.Url = data.getMasterAddress();
                proxy.resetDistributedMasterString(data.getpId());



            }
            catch (Exception e)
            {
                // TODO Auto-generated catch block
                Console.WriteLine("No response from the master node");
                Console.WriteLine("Starting election to elect new master node ( Bully algorith)");
                //remove node from network			
                removeNode(data.getMasterPid());
                data.getpId_Address().Remove(data.getMasterPid());
                bullyAlgorithm();
                //   Console.WriteLine(e.StackTrace);
            }

        }

    }
}
