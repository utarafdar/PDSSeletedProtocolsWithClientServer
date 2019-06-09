using CookComputing.XmlRpc;
using Node1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Node1
{
  public  class CsServer :  XmlRpcListenerService
    {


        [XmlRpcMethod("DCIT.joinNetwork",
         Description = "Sends the id and address of joining node to all the nodes in network and sends the adressess of all the nodes in the network to the joining node ")]
        public String joinNetwork(String iD, String address)
        {
            NetworkBean data = new NetworkBean();
            CsClient csclient = new CsClient();
            Dictionary<String, String> returnAddressIdMap = new Dictionary<String, String>();
           
            foreach (KeyValuePair<string, string> entry in data.getpId_Address())
            {
                returnAddressIdMap.Add(entry.Key, entry.Value);
            }
            // call function to send the new address to all nodes
            try
            {
                csclient.addReturnAdressToList(address);
                csclient.propagateNewNodeAddress(returnAddressIdMap, iD, address);
            }
            catch (Exception e) {
                // TODO Auto-generated catch block
                Console.WriteLine(e.StackTrace);
            }
            returnAddressIdMap.Add(data.getpId(), data.getAddress());
            data.addValuesToDictionary(iD, address);
            try
            {
                csclient.setMasterNode(address);
            }
            catch (Exception e)
            {
                // TODO Auto-generated catch block
                Console.WriteLine(e.StackTrace);
            }
            return data.getpId();

            }

        [XmlRpcMethod("DCIT.updateAddressMap",
         Description = "Adds the address and Id to the Dictionary")]
          public Boolean updateAddressMap(String iD, String address)
        {
            NetworkBean data = new NetworkBean();
            data.addValuesToDictionary(iD, address);

            return true;
        }

        [XmlRpcMethod("DCIT.setMasterAddressId",
        Description = "Updates the master node")]
        public Boolean setMasterAddressId(String iD, String address)
        {
            NetworkBean data = new NetworkBean();
            data.setMasterAddress(address);
            data.setMasterPid(iD);
            return true;
        }

        [XmlRpcMethod("DCIT.distributedRead",
       Description = "Does the distributed read operation")]
        public String distributedRead(String iD)
        { // perform distributed read operations
            NetworkBean data = new NetworkBean();

            return data.getDistributedStringVariable();
        }

        [XmlRpcMethod("DCIT.distributedWrite",
      Description = "Does the distributed write operation")]
        public Boolean distributedWrite(String iD, String message)
        { // perform distributed read operations
            NetworkBean data = new NetworkBean();
            // input.nextLine();

            data.setDistributedStringVariable(data.getDistributedStringVariable() + message);
            return true;
        }

        [XmlRpcMethod("DCIT.requestCMEDistributiveOperation",
     Description = "Request for a distributed operation with Centralised Mutual Exclusion algorithm")]
        public String requestCMEDistributiveOperation(String iD)
        { //request for resource
            CentralisedMutualExclusion cme = new CentralisedMutualExclusion();
            //System.out.println("5");
            return cme.request(iD);
        }

        [XmlRpcMethod("DCIT.CMEreleaseResource",
    Description = "Function to release  the resource from queue after distributed operation is done")]
        public Boolean CMEreleaseResource(String iD)
        { // perform distributed read operations
            NetworkBean data = new NetworkBean();
            CentralisedMutualExclusion cme = new CentralisedMutualExclusion();            
            cme.release(iD);
            return true;
        }

        [XmlRpcMethod("DCIT.electMasterNode",
      Description = "Starts the master node election process")]
        public String electMasterNode(String iD)
        {
            CsClient csclient = new CsClient();
            csclient.bullyAlgorithm();
            return "I am AlIVE! I will take over from here";
        }
        [XmlRpcMethod("DCIT.nodeSignOff",
       Description = "Removes the node from network when signed off")]
        public Boolean nodeSignOff(String iD)
        {
            NetworkBean data = new NetworkBean();
            data.getpId_Address().Remove(iD);
            return true;
        }
        
        [XmlRpcMethod("DCIT.callBullyAfterSignOff",
       Description = "calls bully if the master node has signed off")]
        public String callBullyAfterSignOff(String iD)
        {
            //NetworkBean data = new NetworkBean();
            //data.getpId_Address().remove(iD);
            CsClient client = new CsClient();
            client.bullyAlgorithm();

            return "OK";
            
        }

        [XmlRpcMethod("DCIT.RA_sendRequest",
       Description = "Send Request to access critical section in Ricart and Agarwala")]
        public String RA_sendRequest(String iD, String timeStamp)
        {
            Ricart_Agarwala RA = new Ricart_Agarwala();
            return RA.RA_RecieveRequest(iD, timeStamp);
        }



        [XmlRpcMethod("DCIT.removerNodeFromNetwork",
       Description = "removes the node from the network")]
        public Boolean removerNodeFromNetwork(String iD)
        {
            NetworkBean data = new NetworkBean();
            data.getpId_Address().Remove(iD);
            return true;
        }

        //recieveOkFromRicartAgarwala
        [XmlRpcMethod("DCIT.recieveOkFromRicartAgarwala",
        Description = "Recieves Ok messages when a node starts dequeuing after finishing distributed operation in Ricart and Agarwala")]
        public Boolean recieveOkFromRicartAgarwala(String oK, String timestamp)
        {
            LamportClock clock = new LamportClock();
            clock.receiveAction(timestamp);
            NetworkBean data = new NetworkBean();
            Ricart_Agarwala RA = new Ricart_Agarwala();
            RA.setNumberOfNodesResponded(RA.getNumberOfNodesResponded() + 1);
            Console.WriteLine("recieved OK message from " + oK + " at Time " + DateTime.Now.ToString("HH:mm:ss:fff") + " \nnumber of ok messages = " + RA.getNumberOfNodesResponded());
            Console.WriteLine(" number of nodes = " + (data.getpId_Address().Count()));

            return true;
        }

        [XmlRpcMethod("DCIT.distributedProcessDone",
      Description = "Nodes send notification after completing distributed operation")]
        public Boolean distributedProcessDone(String iD)
        {
            NetworkBean data = new NetworkBean();
            data.setNoOfNodesFinishedOperation(data.getNoOfNodesFinishedOperation() + 1);
            return true;
        }

        [XmlRpcMethod("DCIT.resetDistributedMasterString",
     Description = "Resets the Variable in the master node ,which is accessed during distributed operations")]
        public Boolean resetDistributedMasterString(String iD)
        {
            NetworkBean data = new NetworkBean();
            data.setDistributedStringVariable("");
            return true;
        }

        [XmlRpcMethod("DCIT.startDistributedOperations",
    Description = "Starts the distributed operation process after recieving start from one of the nodes")]
        public Boolean startDistributedOperations(String approach)
        {
            NetworkBean data = new NetworkBean();
            if (approach.Equals("CME"))
            {
                AutomatedDistributedOperation ADO = new AutomatedDistributedOperation();
                Thread thread = new Thread(() => ADO.startDistributedCME());
                thread.Start();               
			    data.setStartCME(true);
	        }
	   if(approach.Equals("RA"))
            {
                AutomatedDistributedOperation ADO = new AutomatedDistributedOperation();
                Thread thread = new Thread(() => ADO.startDistributedRicartAndAgarwala());
                thread.Start();            
			
				data.setStartRicartAgarwala(true);
			//data.setStartRicartAgarwala(true);
		}
	   return true;
  }



    }
}
