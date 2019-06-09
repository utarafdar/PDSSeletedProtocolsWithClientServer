package network;
import java.net.MalformedURLException;
import java.util.Date;
import java.util.HashMap;


import org.apache.xmlrpc.XmlRpcException;

public class JavaServer {
	
	//start server function

	public String joinNetwork(String iD, String address)
	{	
		NetworkBean data = new NetworkBean();
		JavaClient javaclient = new JavaClient();
	    HashMap<String,String> returnAddressIdMap = new HashMap<String,String>();
	    returnAddressIdMap.putAll(data.getpId_Address());
	    // call function to send the new address to all nodes
	    try {
	    javaclient.addReturnAdressToList(address);	
		javaclient.propagateNewNodeAddress(returnAddressIdMap,iD,address);
	}   catch (MalformedURLException | XmlRpcException e) {
		// TODO Auto-generated catch block
		//e.printStackTrace();
	}
	    returnAddressIdMap.put(data.getpId(), data.getAddress());	
		data.addValuesToHashMap(iD, address);
		try {
			javaclient.setMasterNode(address);
		} catch (MalformedURLException | XmlRpcException e) {
			
			// TODO Auto-generated catch block
			//e.printStackTrace();
		}
		return data.getpId();
		
	}
   public boolean updateAddressMap(String iD, String address)
   {   NetworkBean data = new NetworkBean();
       data.addValuesToHashMap(iD, address);
	      
	   return true;
   }

	 // function to set master node
   
   public boolean setMasterAddressId(String iD, String address)
   {
	   NetworkBean data = new NetworkBean();
       data.setMasterAddress(address);
	   data.setMasterPid(iD);
	   return true;
   }
	
   public String distributedRead (String iD)
   { // perform distributed read operations
	   NetworkBean data = new NetworkBean();
	   
	   return data.getDistributedStringVariable();
   }
   
   public boolean distributedWrite (String iD, String message)
   { // perform distributed read operations
	   NetworkBean data = new NetworkBean();
	   	   // input.nextLine();
	   
	   data.setDistributedStringVariable(data.getDistributedStringVariable()+message);
	   return true;
   }
   //requestDistributiveOperation
   public String requestCMEDistributiveOperation (String iD)
   { //request for resource
	   CentralisedMutualExclusion cme = new CentralisedMutualExclusion();
	   //System.out.println("5");
	   return cme.request(iD);
   }
   //releaseResource
   public boolean CMEreleaseResource (String iD)
   { // perform distributed read operations
	   CentralisedMutualExclusion cme = new CentralisedMutualExclusion();
	   cme.release(iD);
	   return true;
   }
   
   
   public String electMasterNode(String iD)
   {   JavaClient javaclient = new JavaClient();
       javaclient.bullyAlgorithm();
	   return "I am AlIVE! I will take over from here";
   }
   //nodeSignOff
   public Boolean nodeSignOff(String iD)
   {   
	   NetworkBean data = new NetworkBean();
	   data.getpId_Address().remove(iD);
	   
	   return true;
   }
   //callBullyAfterSignOff
   public String callBullyAfterSignOff(String iD)
   {   
	   //NetworkBean data = new NetworkBean();
	   //data.getpId_Address().remove(iD);
	   JavaClient client = new JavaClient();
	   client.bullyAlgorithm();
	   
	  	   return "OK";
	  
   }
   
  public boolean recieveOkFromRicartAgarwala(String oK, String timestamp)
   {  LamportClock clock = new LamportClock();
      clock.receiveAction(timestamp);
	  NetworkBean data = new NetworkBean();
   Ricart_Agarwala RA = new Ricart_Agarwala();
   RA.setNumberOfNodesResponded(RA.getNumberOfNodesResponded()+1);
   System.out.println("recieved OK message from "+oK+" at Time "+ RA.dateFormat.format(new Date())+" \nnumber of ok messages = "+RA.getNumberOfNodesResponded());
   System.out.println(" number of nodes = "+(data.getpId_Address().size()));
	    
   return true;
   }
  //RA_sendRequest
  public String RA_sendRequest(String iD,String timeStamp)
  {Ricart_Agarwala RA = new Ricart_Agarwala();
    return RA.RA_RecieveRequest(iD, timeStamp);
  }
  
  //startDistributedOperations
  public boolean startDistributedOperations(String approach)
  {   
	   NetworkBean data = new NetworkBean();
	   if(approach.equals("CME"))
	   {Thread thread = new Thread(new Runnable()
		{
	public void run()
	{
		AutomatedDistributedOperation ADO = new AutomatedDistributedOperation();
		
		ADO.startDistributedCME();
		
	}
		});
thread.start();
			data.setStartCME(true);
	   }
	   if(approach.equals("RA"))
		{  Thread thread = new Thread(new Runnable()
				{
			public void run()
			{
				AutomatedDistributedOperation ADO = new AutomatedDistributedOperation();
				
				ADO.startDistributedRicartAndAgarwala();
				
			}
				});
		thread.start();
		
		
				data.setStartRicartAgarwala(true);
			//data.setStartRicartAgarwala(true);
		}
	   return true;
  }

  
 public boolean removerNodeFromNetwork(String iD)
 {
	 NetworkBean data = new NetworkBean();
	 data.getpId_Address().remove(iD);
	 return true;
 }
 //data.setNoOfNodesFinishedOperation(data.getNoOfNodesFinishedOperation()+1);
 public boolean distributedProcessDone(String iD)
 {
	 NetworkBean data = new NetworkBean();
	 data.setNoOfNodesFinishedOperation(data.getNoOfNodesFinishedOperation()+1);
	 return true;
 }
  //resetDistributedMasterString
 public boolean resetDistributedMasterString(String iD)
 {
	 NetworkBean data = new NetworkBean();
	data.setDistributedStringVariable("");
	 return true;
 }
}
