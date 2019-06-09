package network;
import java.net.MalformedURLException;
//import java.sql.Timestamp;
import java.util.HashMap;
import java.util.Iterator;

import java.util.Vector;
import java.util.Map.Entry;

import org.apache.xmlrpc.XmlRpcException;
import org.apache.xmlrpc.client.XmlRpcClient;


public class JavaClient {


//	private Timestamp timestamp;
	String addr,ide;
	public void join(String sendAddress) throws MalformedURLException, XmlRpcException {
		// TODO Auto-generated method stub
		NetworkBean data = new NetworkBean();
		ServerOperations serveroperaation = new ServerOperations();
		XmlRpcClient server = serveroperaation.setXmlRpcConfig(sendAddress);        
        Vector<String> params = new Vector<String>();
        params.addElement(new String(data.getpId())); 
        params.addElement(new String(data.getAddress()));
        Object sendId = server.execute("DCIT.joinNetwork", params);
        //@SuppressWarnings("unchecked")
		//HashMap<String, String>  addressListHashMap=(HashMap<String, String>) addressListMap;
        //data.setpId_Address(addressListHashMap);
        String recievedId=(String)sendId;
        data.addValuesToHashMap(recievedId, sendAddress);
	}
	
	//function added because xmlrpc cannot return dictionary type..
	// function to handle c# returns
	public void addReturnAdressToList(String Address)
	{
	try {NetworkBean data = new NetworkBean();
	    ServerOperations serveroperaation = new ServerOperations();
		XmlRpcClient server = serveroperaation.setXmlRpcConfig(Address);
		Iterator<Entry<String, String>> it = data.getpId_Address().entrySet().iterator();
		while(it.hasNext())
		{
			Entry <String,String> entry = it.next();
			
			Vector<String> params = new Vector<String>();
	        params.addElement(new String(entry.getKey())); 
	        params.addElement(new String(entry.getValue()));
			server.execute("DCIT.updateAddressMap",params);
			
		}
	} catch (MalformedURLException | XmlRpcException e) {
		// TODO Auto-generated catch block
	//	e.printStackTrace();
	}
		
	}
	
	public void propagateNewNodeAddress(HashMap<String, String> returnAddressIdMap, String iD, String address) throws MalformedURLException, XmlRpcException {
		// TODO Auto-generated method stub
		ServerOperations serveroperation = new ServerOperations();
		Iterator<Entry<String, String>> it = returnAddressIdMap.entrySet().iterator();
		Vector<String> params = new Vector<String>();
        params.addElement(new String(iD)); 
        params.addElement(new String(address));
		while(it.hasNext())
		{
			Entry <String,String> entry = it.next();
			String sendAddress = entry.getValue();
			XmlRpcClient server =serveroperation.setXmlRpcConfig(sendAddress);
			server.execute("DCIT.updateAddressMap",params);
			
		}
	}
	
	// set master node
	
	public void setMasterNode(String address) throws MalformedURLException, XmlRpcException
	   {
		NetworkBean data = new NetworkBean();
		ServerOperations serveroperaation = new ServerOperations();
		XmlRpcClient server = serveroperaation.setXmlRpcConfig(address);
		Vector<String> params = new Vector<String>();
		params.addElement(new String(data.getMasterPid()));
        params.addElement(new String(data.getMasterAddress())); 
        server.execute("DCIT.setMasterAddressId",params);  
	   }
	

	
	// read operation for CME
	
	
	
	
	public void bullyAlgorithm() {
		// TODO Auto-generated method stub
		NetworkBean data = new NetworkBean();
		boolean noMasterFound = true;
		ServerOperations serveroperation = new ServerOperations();
		Iterator<Entry<String, String>> it = data.getpId_Address().entrySet().iterator();
		Vector<String> params = new Vector<String>();
		params.addElement(new String(data.getpId()));
		String response = "";
		while(it.hasNext())
		{
			Entry <String,String> entry = it.next();
			String sendAddress = entry.getValue();
			long pId = Long.parseLong(entry.getKey());
			long currentNodepId = Long.parseLong(data.getpId());
			
			if (pId>currentNodepId)
			{
			XmlRpcClient server;
			try {
				server = serveroperation.setXmlRpcConfig(sendAddress);
				response=(String) server.execute("DCIT.electMasterNode",params);
				System.out.println(data.getpId()+"  Sends the bully election message to :"+sendAddress);
				System.out.println("response is :"+response);
				
			} catch (MalformedURLException | XmlRpcException e) {
				// TODO Auto-generated catch block
				System.out.print("no response");
				//remove node from network
				removeNode(sendAddress);			
				data.getpId_Address().remove(sendAddress);
				//e.printStackTrace();
			}
			
			}
			if (response.length() > 0) {
				noMasterFound = false;
				break;
			}
			
		}
		
		// if no master is found then this node is the new master
		if (noMasterFound)
		{   
			System.out.println("No master found I am the new master");
			data.setMasterPid(data.getpId());
			data.setMasterAddress(data.getAddress());
			propagateNewMasterNode(data.getpId(), data.getAddress());
			
		}
		
	}
	
	public void propagateNewMasterNode(String masterid, String masteraddress) {
		// TODO Auto-generated method stub
		NetworkBean data = new NetworkBean();
		ServerOperations serveroperation = new ServerOperations();
		Iterator<Entry<String, String>> it = data.getpId_Address().entrySet().iterator();
		Vector<String> params = new Vector<String>();
        params.addElement(new String(masterid)); 
        params.addElement(new String(masteraddress));
		while(it.hasNext())
		{
			Entry <String,String> entry = it.next();
			String sendAddress = entry.getValue();
			String iD = entry.getKey();
			XmlRpcClient server;
			try {
				server = serveroperation.setXmlRpcConfig(sendAddress);
				server.execute("DCIT.setMasterAddressId",params);
				System.out.println("sending message I am New Master to :"+sendAddress);
				
			} catch (MalformedURLException | XmlRpcException e) {
				// TODO Auto-generated catch block
				System.out.println("Could not send the message succesfully.. node denied connection");
			//	e.printStackTrace();
				removeNode(iD);
				data.getpId_Address().remove(iD);
			}
			
			
		}
		
	}

	public void signOff() {
		// TODO Auto-generated method stub
		ServerOperations serveroperation = new ServerOperations();
		NetworkBean data = new NetworkBean();
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
				server.execute("DCIT.nodeSignOff",params);
				System.out.println("Sending message signing off to :"+sendAddress);
			} catch (MalformedURLException | XmlRpcException e) {
				// TODO Auto-generated catch block
			//	e.printStackTrace();
			}
			
			
		}
		if(data.getMasterPid().equals(data.getpId()))
		{  
			//sign off master, change master
			
			
			 Iterator<Entry<String, String>> it1 = data.getpId_Address().entrySet().iterator();
			    while (it1.hasNext()) {
			    	Entry <String,String> entry = it1.next();
			    	 addr= entry.getValue();
			    	ide = entry.getKey();
			    }
			    
		
		Iterator<Entry<String, String>> it2 = data.getpId_Address().entrySet().iterator();
	    while (it2.hasNext()) {
	    	Entry <String,String> entry = it2.next();
		  
				ServerOperations serveroperaation = new ServerOperations();
				//String first = data.getpId_Address().firstEntry().getValue();
				XmlRpcClient server1;
				try {
					server1 = serveroperaation.setXmlRpcConfig(entry.getValue());
				
				Vector<String> params2 = new Vector<String>();
				params2.addElement(new String(ide));
		        params2.addElement(new String(addr)); 
		        server1.execute("DCIT.setMasterAddressId",params); 
				} catch (MalformedURLException | XmlRpcException e) {
					// TODO Auto-generated catch block
				//	e.printStackTrace();
				}
	    }
			
	    
	    
	    
    		
    		String response="";
			XmlRpcClient server;
		it = data.getpId_Address().entrySet().iterator();
		
		
		
		//delete master
		
		
		
		while(it.hasNext())
		{Entry <String,String> entry = it.next();
			try {
				server = serveroperation.setXmlRpcConfig(entry.getValue());
				 response = (String) server.execute("DCIT.callBullyAfterSignOff",params);
				 System.out.println(data.getpId()+"  Sends the bully election message to :"+entry.getValue());
				 System.out.println("response is :"+response);
			} catch (MalformedURLException | XmlRpcException e) {
				// TODO Auto-generated catch block
				//e.printStackTrace();
			}
			if (response.length() > 0) {
				
				break;
			}
		}
		}	
		data.getpId_Address().clear();
		data.setMasterAddress(data.getAddress());
		data.setMasterPid(data.getpId());
		
	}

		
	//ricart agarwala read operation
	
		


	public void startDistributedOpeartion(String approach) {
		// TODO Auto-generated method stub
		// call all the nodes to start operation (CME).
		NetworkBean data = new NetworkBean();
		ServerOperations serveroperation = new ServerOperations();
		Iterator<Entry<String, String>> it = data.getpId_Address().entrySet().iterator();
		 Vector<String> params = new Vector<String>();
        params.addElement(new String(approach));     
        
		while(it.hasNext())
		{  // int i=0;
		
			Entry <String,String> entry = it.next();
			String sendAddress = entry.getValue();
			 XmlRpcClient server;
			
			try { server = serveroperation.setXmlRpcConfig(sendAddress);
			
			server.execute("DCIT.startDistributedOperations",params);
				   
				
			} catch (MalformedURLException | XmlRpcException e) {
				// TODO Auto-generated catch block
				
				//e.printStackTrace();
			}
			
			

		}
		
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
		{
			Thread thread = new Thread(new Runnable()
			{
				public void run()
				{
					AutomatedDistributedOperation ADO = new AutomatedDistributedOperation();

					ADO.startDistributedRicartAndAgarwala();
				}
			});
			thread.start();
			data.setStartRicartAgarwala(true);
		}
	}

	



	public void removeNode(String iD)
	{
		NetworkBean data = new NetworkBean();
		ServerOperations serveroperation = new ServerOperations();
		Iterator<Entry<String, String>> its = data.getpId_Address().entrySet().iterator();
		Vector<String> params1 = new Vector<String>();
        params1.addElement(new String(iD)); 
       
		while(its.hasNext())
		{
			Entry <String,String> entries = its.next();
			String sendAddress = entries.getValue();
			String sendiD= entries.getKey();
			XmlRpcClient servers;
			try {
				if(!sendiD.equals(iD))
				{
				servers = serveroperation.setXmlRpcConfig(sendAddress);
				servers.execute("DCIT.removerNodeFromNetwork",params1);
				}
			} catch (MalformedURLException | XmlRpcException e1) {
				// TODO Auto-generated catch block
			//	e1.printStackTrace();
			}
			
			
		}
	}
	
	public void resetDistributedStringVariable()
	{
		NetworkBean data = new NetworkBean();
		ServerOperations serveroperation = new ServerOperations();
		Vector<String> params = new Vector<String>();
        params.addElement(new String(data.getpId()));
        XmlRpcClient server;
        try {
			server = serveroperation.setXmlRpcConfig(data.getMasterAddress());
			server.execute("DCIT.resetDistributedMasterString",params);
		} catch (MalformedURLException | XmlRpcException e) {
			// TODO Auto-generated catch block
			System.out.println("No response from the master node");
			System.out.println("Starting election to elect new master node ( Bully algorith)");
			//remove node from network			
			removeNode(data.getMasterPid());
			data.getpId_Address().remove(data.getMasterPid());
			bullyAlgorithm();
			//e.printStackTrace();
		}
		
	}
	
}
