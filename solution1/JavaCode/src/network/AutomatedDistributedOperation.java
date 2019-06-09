package network;

import java.net.MalformedURLException;
import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Iterator;

import java.util.Vector;
import java.util.Map.Entry;

import org.apache.xmlrpc.XmlRpcException;
import org.apache.xmlrpc.client.XmlRpcClient;

public class AutomatedDistributedOperation {
	
	
public void startDistributedRicartAndAgarwala()
{ 
	try
	{
	Thread.sleep(1);
    } 
	catch (InterruptedException e1) 
	{
	// TODO Auto-generated catch block
	e1.printStackTrace();
    }	
	JavaClient client = new JavaClient();
	DateFormat dateFormat = new SimpleDateFormat("HH:mm:ss:SSS");
	//Date date = new Date();
	NetworkBean data = new NetworkBean();
	//run the loop for 20 seconds
	System.out.println("Distributed operation has started with Ricart and Agarwala algorithm");
	System.out.println("*********************ID = "+data.getpId()+"*********************");
	if(data.getpId().equals(data.getMasterPid()))
	System.out.println("****************************I am the Master Node*****************");
	Ricart_Agarwala RA = new Ricart_Agarwala();
	String response;
	String message = "";

	ServerOperations serveroperaation = new ServerOperations();		
	LamportClock clock = new LamportClock();
	long t= System.currentTimeMillis();
	long end = t+20000;
	while(System.currentTimeMillis() < end) {
		//a) Wait a random amount of time (1-4 seconds)
		try 
		{
			
			Thread.sleep((long)(Math.random() * 8000+1000));
			//System.out.println((long)(Math.random() * 4000+1000));
		} 
		catch (InterruptedException e) 
		{
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
       //b) Read the string variable from the master node
		//c) Append some random english word to this string
		//d) Write the updated string to the master node 
		// request for critical section to perform these steps
		 		
		
		//long time =timestamp.getTime();
		//RA.setMyTimeStamp(String.valueOf(TimeStamp.getTime()));
		RA.setMyTimeStamp(String.valueOf(clock.getValue()+1));
		Vector<String> params = new Vector<String>();
		params.addElement(new String(data.getpId()));
		//params.addElement(new String(RA.getMyTimeStamp()));
		Iterator<Entry<String, String>> it = data.getpId_Address().entrySet().iterator();
		clock.tick();
		 clock.setRequestTimeStamp(clock.getValue()+1);
		while(it.hasNext())
		{
			Entry <String,String> entry = it.next();
		try {
			XmlRpcClient server = serveroperaation.setXmlRpcConfig(entry.getValue());			
			//System.out.println("waiting for the response from server");
			 RA.setRequestCR(true);
			 clock.sendAction();		
			
			 params.addElement(new String(String.valueOf(clock.getValue())));	
			 System.out.println("sending request message to "+entry.getKey()+"at local time :"+dateFormat.format(new Date()));
			response =(String) server.execute("DCIT.RA_sendRequest",params);
			//clock.receiveAction(sentValue);
			//System.out.println("response :"+response);
			params.remove(1);
			if (response.contains("OK"))
			{   String[] parts = response.split("_");
				clock.receiveAction(parts[1]);
				System.out.println("Recieved OK message from "+entry.getKey()+"at local time :"+dateFormat.format(new Date()));
				RA.setNumberOfNodesResponded(RA.getNumberOfNodesResponded()+1);
				
			}
			else
			{
				 String[] parts = response.split("_");
					clock.receiveAction(parts[1]);
			}
			
		} catch (MalformedURLException | XmlRpcException e) {
			// TODO Auto-generated catch block
			//call for election to elect a new master node (bully algorithm)
			System.out.println("Exception from the  node/nremove node from network");
			client.removeNode(entry.getValue());
			data.getpId_Address().remove(entry.getValue());
			if(entry.getValue().equals(data.getMasterPid()))
			client.bullyAlgorithm();
			RA.setNumberOfNodesResponded(RA.getNumberOfNodesResponded()+1);
			//e.printStackTrace();
		}
		}
		System.out.println("Number of OK messages recieved ="+RA.getNumberOfNodesResponded());
		System.out.println("Number of nodes in network ="+data.getpId_Address().size());
		if(RA.getNumberOfNodesResponded()!=data.getpId_Address().size())
		System.out.println("Waiting for other nodes to send OK message...");
		// System.out.println(data.getpId_Address().size()+""+RA.getNumberOfNodesResponded());
		 int numberOfOkMsgs = RA.getNumberOfNodesResponded();
		 while(data.getpId_Address().size()!=numberOfOkMsgs)
		{
			 numberOfOkMsgs = RA.getNumberOfNodesResponded();
			 //System.out.println(numberOfOkMsgs);
			 try {
				Thread.sleep(100);
			} catch (InterruptedException e) {
				// TODO Auto-generated catch block
				//e.printStackTrace();
			}
		//keep waiting 	
		//break from the loop only when all the nodes have sent OK message
		}
		// System.out.println(data.getpId_Address().size()+RA.getNumberOfNodesResponded());
	//	TimeStamp.setStartTime();
		 System.out.println("Recieved OK message from all the nodes");
		 System.out.println("Critical region in my control");
		 System.out.println("Distributed process started at "+dateFormat.format(new Date()));
		 RA.setRequestCR(false);
	     RA.setUsingCR(true);
		 //waiting for some time( for demonstration purpose )
		 
		
		 
		XmlRpcClient server;
		try {
			server = serveroperaation.setXmlRpcConfig(data.getMasterAddress());
			System.out.println("Reading the variable String from master node");		  		 
	         message=(String) server.execute("DCIT.distributedRead",params);
			// long timeElapsed=TimeStamp.getEndTime()-TimeStamp.getStartTime();
			// System.out.println(message);
			 System.out.println("The string is "+message);
			// System.out.println("time in critical section"+timeElapsed);
			
			 String randomWord = WordList.getWords()[(int) (Math.random() * 12)];
			 System.out.println("Appending '"+randomWord+"' to the string "+message );
			 //waiting for some time( for demonstration purpose )
			 Vector<String> writeparams = new Vector<String>();
			 writeparams.addElement(new String(data.getMasterPid()));
			 writeparams.addElement(new String(randomWord));
			
			  server.execute("DCIT.distributedWrite",writeparams);
			  
			  data.getAddedWords().add(randomWord);
//			  try 
//				{
//					
//					Thread.sleep((long)(Math.random() * 3000+1000));
//					//System.out.println((long)(Math.random() * 4000+1000));
//				} 
//				catch (InterruptedException e) 
//				{
//					// TODO Auto-generated catch block
//					e.printStackTrace();
//				}
//			 
		} catch (MalformedURLException | XmlRpcException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			System.out.println("No response from the master node");
			System.out.println("Starting election to elect new master node ( Bully algorith)");
	        client.removeNode(data.getMasterAddress());
			data.getpId_Address().remove(data.getMasterPid());
			client.bullyAlgorithm();
			//e.printStackTrace();
		}			
		 System.out.println("Distributed operation done at time "+dateFormat.format(new Date()));
		 clock.tick();
		//release all the requests from queue		
		 System.out.println("Exiting critical Section");
		 System.out.println("Releasing all deffered requests");
		 RA.releaseDeferedRequests(); 
		
	} 

	//c) After the process has ended all the nodes read the final string from the master node and
	//write it to the screen. Moreover they check if all the words they added to the string are
	//present in the final string. The result of this check is also written to the screen. 
	
	
	// propagate that it is done with the process.
	System.out.println("done with distributed process");
	
//	data.setNoOfNodesFinishedOperation(data.getNoOfNodesFinishedOperation()+1);
	ServerOperations serveroperation = new ServerOperations();
	Iterator<Entry<String, String>> it = data.getpId_Address().entrySet().iterator();
	Vector<String> params = new Vector<String>();
    params.addElement(new String(data.getpId())); 
   
	while(it.hasNext())
	{
		Entry <String,String> entry = it.next();
		String sendAddress = entry.getValue();
		XmlRpcClient server;
		try {
			server = serveroperation.setXmlRpcConfig(sendAddress);
			server.execute("DCIT.distributedProcessDone",params);
		} catch (MalformedURLException | XmlRpcException e) {
			// TODO Auto-generated catch block
			//e.printStackTrace();
		}
		
		
	}
	RA.setMyTimeStamp("0");
	int number =data.getNoOfNodesFinishedOperation();
	while(data.getpId_Address().size()!=number)
	{ number =data.getNoOfNodesFinishedOperation();
	
		try {
			Thread.sleep(100);
		} catch (InterruptedException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}
	 System.out.println("out loop");


		 checkFinalString();
}
	//
	public void checkFinalString()	
	 {
		
		long t= System.currentTimeMillis();
		long end = t+0;
		t= System.currentTimeMillis();
		 end = t+4000;
		 while(System.currentTimeMillis() < end) {
			
		 }
		String message = "";
		NetworkBean data = new NetworkBean();
		JavaClient client = new JavaClient();
		ServerOperations serveroperaation = new ServerOperations();	
	System.out.println("**************Reading The final String*******************");
	
	Vector<String> params = new Vector<String>();
	params.addElement(new String(data.getpId()));
	XmlRpcClient server;
	try {
		server = serveroperaation.setXmlRpcConfig(data.getMasterAddress());
		//System.out.println("Reading the variable String from master node");		  		 
         message=(String) server.execute("DCIT.distributedRead",params);
		// long timeElapsed=TimeStamp.getEndTime()-TimeStamp.getStartTime();
		 System.out.println("the final string is \n");
		 System.out.println(message+"\n");
		 
	} catch (MalformedURLException | XmlRpcException e) {
		// TODO Auto-generated catch block
		//e.printStackTrace();
		System.out.println("No response from the master node");
		System.out.println("Starting election to elect new master node ( Bully algorith)");
		client.removeNode(data.getMasterPid());
		data.getpId_Address().remove(data.getMasterPid());
		client.bullyAlgorithm();
		//e.printStackTrace();
	}			
	//Moreover they check if all the words they added to the string are
	//present in the final string. The result of this check is also written to the screen. 
	boolean wordFlag=true;
	for(String word : data.getAddedWords())
	{    if(message.contains(word))
		 System.out.println(word+" is present in the final string");
	     else
	     {	 System.out.println(word+" is not present in the final string");
	     wordFlag=false;
	     }
	}
	if (wordFlag)
		System.out.println("all the words are added to the string in master node");
	//end//
	data.getAddedWords().clear();
	data.setNoOfNodesFinishedOperation(0);
	data.setStartRicartAgarwala(false);
	data.setStartCME(false);

	
	 }
	


public void startDistributedCME()
{
	try
	{
	Thread.sleep(1);
    } 
	catch (InterruptedException e1) 
	{
	// TODO Auto-generated catch block
	e1.printStackTrace();
    }
	
	
	long t= System.currentTimeMillis();
	long end = t+20000;
	JavaClient client = new JavaClient();
	DateFormat dateFormat = new SimpleDateFormat("HH:mm:ss:SSS");
	
	NetworkBean data = new NetworkBean();
	//run the loop for 20 seconds
	System.out.println("Distributed operation has started with Centralized Mutual Exclusion");
	System.out.println("*********************ID = "+data.getpId()+"*********************");
	if(data.getpId().equals(data.getMasterPid()))
	System.out.println("************************I am the Master Node********************");


	String response;
	String message = "";
	
	ServerOperations serveroperaation = new ServerOperations();		

	while(System.currentTimeMillis() < end) 
	{
		//a) Wait a random amount of time (1-4 seconds)
		try 
		{
			
			Thread.sleep((long)(Math.random() * 8000+1000));
			//System.out.println((long)(Math.random() * 4000+1000));
		} 
		catch (InterruptedException e) 
		{
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
       //b) Read the string variable from the master node
		//c) Append some random english word to this string
		//d) Write the updated string to the master node 
		// request for critical section to perform these steps
		
		
		Vector<String> params = new Vector<String>();
		params.addElement(new String(data.getpId()));
		try {
			  
			XmlRpcClient server = serveroperaation.setXmlRpcConfig(data.getMasterAddress());
		  	//request
			//get response
			// if OK do operation
			//release message
			
			System.out.println("Request Sent to Master for Distributed Operation at "+dateFormat.format(new Date()));
			System.out.println("waiting for the response from Master");
			response =(String) server.execute("DCIT.requestCMEDistributiveOperation",params);
			
			if (response.equals("OK"))
			{
				System.out.println("Recived OK message from Master at "+dateFormat.format(new Date()));
				server = serveroperaation.setXmlRpcConfig(data.getMasterAddress());
				System.out.println("Reading the variable String from master node");		  		 
		         message=(String) server.execute("DCIT.distributedRead",params);
				// long timeElapsed=TimeStamp.getEndTime()-TimeStamp.getStartTime();
				  System.out.println("The string is "+message);
				// System.out.println("time in critical section"+timeElapsed);
				
				 String randomWord = WordList.getWords()[(int) (Math.random() * 12)];
				 System.out.println("Appending '"+randomWord+"' to the string "+message );
				 //waiting for some time( for demonstration purpose )
				 Vector<String> writeparams = new Vector<String>();
				 writeparams.addElement(new String(data.getMasterPid()));
				 writeparams.addElement(new String(randomWord));
				
				  server.execute("DCIT.distributedWrite",writeparams);
//				  try 
//					{
//						
//						Thread.sleep((long)(Math.random() * 3000+1000));
//						//System.out.println((long)(Math.random() * 4000+1000));
//					} 
//					catch (InterruptedException e) 
//					{
//						// TODO Auto-generated catch block
//						e.printStackTrace();
//					}
				  data.getAddedWords().add(randomWord);
				  
				System.out.println(randomWord+" appended to string");
				System.out.println("distributed Operatiion completed at "+dateFormat.format(new Date()));
				System.out.println("Releasing the CR");
			    server.execute("DCIT.CMEreleaseResource",params);
			}
			
		} catch (MalformedURLException | XmlRpcException e) {
			// TODO Auto-generated catch block
			//call for election to elect a new master node (bully algorithm)
			System.out.println("No response from the master node");
			System.out.println("Starting election to elect new master node ( Bully algorith)");
			//remove node from network			
			client.removeNode(data.getMasterPid());
			data.getpId_Address().remove(data.getMasterPid());
			client.bullyAlgorithm();
			//e.printStackTrace();
		}
	
		
		 		
	}
	//c) After the process has ended all the nodes read the final string from the master node and
		//write it to the screen. Moreover they check if all the words they added to the string are
		//present in the final string. The result of this check is also written to the screen. 
		
		
		// propagate that it is done with the process.
		System.out.println("done with distributed process");
		
//		data.setNoOfNodesFinishedOperation(data.getNoOfNodesFinishedOperation()+1);
		ServerOperations serveroperation = new ServerOperations();
		Iterator<Entry<String, String>> it = data.getpId_Address().entrySet().iterator();
		Vector<String> params = new Vector<String>();
	    params.addElement(new String(data.getpId())); 
	   
		while(it.hasNext())
		{
			Entry <String,String> entry = it.next();
			String sendAddress = entry.getValue();
			XmlRpcClient server;
			try {
				server = serveroperation.setXmlRpcConfig(sendAddress);
				server.execute("DCIT.distributedProcessDone",params);
			} catch (MalformedURLException | XmlRpcException e) {
				// TODO Auto-generated catch block
				//e.printStackTrace();
			}
			
			
		}
		int number =data.getNoOfNodesFinishedOperation();
		while(data.getpId_Address().size()!=number)
		{ number =data.getNoOfNodesFinishedOperation();
		
			try {
				Thread.sleep(100);
			} catch (InterruptedException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		}
		 System.out.println("out loop");


			 checkFinalString();

}

}
