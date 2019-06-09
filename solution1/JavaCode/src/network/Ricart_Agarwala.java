/**
 * 
 */
package network;

import java.net.MalformedURLException;
import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.LinkedList;
import java.util.Queue;
import java.util.Vector;

import org.apache.xmlrpc.XmlRpcException;
import org.apache.xmlrpc.client.XmlRpcClient;


public class Ricart_Agarwala {
	private static String myTimeStamp;
	private static Queue<String> ricartAgarwalaQueue = new LinkedList<String>();
	private static boolean requestCR, usingCR;
	private static int numberOfNodesResponded;
	DateFormat dateFormat = new SimpleDateFormat("HH:mm:ss:SSS");
	
	
	public  int getNumberOfNodesResponded() {
		return numberOfNodesResponded;
	}
	public  void setNumberOfNodesResponded(int numberOfNodesResponded) {
		Ricart_Agarwala.numberOfNodesResponded = numberOfNodesResponded;
	}
	
	public  String getMyTimeStamp() {
		return myTimeStamp;
	}
	public  void setMyTimeStamp(String myTimeStamp) {
		Ricart_Agarwala.myTimeStamp = myTimeStamp;
	}
	public  Queue<String> getRicartAgarwalaQueue() {
		return ricartAgarwalaQueue;
	}
	public  void setRicartAgarwalaQueue(Queue<String> ricartAgarwalaQueue) {
		Ricart_Agarwala.ricartAgarwalaQueue = ricartAgarwalaQueue;
	}
	public  boolean isRequestCR() {
		return requestCR;
	}
	public  void setRequestCR(boolean requestCR) {
		Ricart_Agarwala.requestCR = requestCR;
	}
	public  boolean isUsingCR() {
		return usingCR;
	}
	public  void setUsingCR(boolean usingCR) {
		Ricart_Agarwala.usingCR = usingCR;
	}
	//determine the critical section entry
	public String RA_RecieveRequest(String iD, String timeStamp)
	{   LamportClock clock = new LamportClock();
		clock.receiveAction(timeStamp);
		NetworkBean data = new NetworkBean();
	
	    System.out.println("recieved RA operation request from "+ iD +"at time "+dateFormat.format(new Date()));
		if(usingCR)
		{
			System.out.println("Already using CR.. Defer the request " +iD);
			deferRequest(iD);			
			return "Deferred_"+clock.sendAction();
			
		}
		//myTimeStamp
		if(requestCR)
		{ 
		    //timeStamp=String.valueOf(clock.getValue());
			if(String.valueOf(clock.getRequestTimeStamp()).compareTo(timeStamp)>0)
			{System.out.println("already requested CR.. my request Time stamp="+clock.getRequestTimeStamp()+"  "+ iD+" request timestamp =" +timeStamp);
			System.out.println("Send OK Message");	
			return"OK_"+clock.sendAction();
			}
			if(String.valueOf(clock.getRequestTimeStamp()).compareTo(timeStamp)<0)
			{
				System.out.println("already requested CR.. my request time stamp = "+clock.getRequestTimeStamp()+ " the node "+iD+" request timestamp is =" +timeStamp);
			    System.out.println("Deferr the request");
				deferRequest(iD);
				return "Deferred_"+clock.sendAction();
			}
			if(String.valueOf(clock.getRequestTimeStamp()).compareTo(timeStamp)==0)
			{
				if(iD.compareTo(data.getpId())>0)
				{
					deferRequest(iD);
					return "Deferred_"+clock.sendAction();
				}
				
				if(iD.compareTo(data.getpId())<0)
				{return"OK_"+clock.sendAction();
				}
			}
		}
		System.out.println("Not Using or requesting CR... Send OK Message");
		return "OK_"+clock.sendAction();
	}
	private void deferRequest(String iD) {
		// TODO Auto-generated method stub
		ricartAgarwalaQueue.add(iD);
		//wait until its released
		
	}
	public void releaseDeferedRequests()
	{   	    
		//remove the Ids from queue
		//send OK message to the Ids
	while (!ricartAgarwalaQueue.isEmpty())
	{String iD=ricartAgarwalaQueue.remove();
	
	
		NetworkBean data = new NetworkBean();
		LamportClock clock = new LamportClock();
		String address = data.getpId_Address().get(iD);		 
		ServerOperations serveroperaation = new ServerOperations();
		try {
			XmlRpcClient server = serveroperaation.setXmlRpcConfig(address);
			 Vector<String> params = new Vector<String>();
		     params.addElement(new String(data.getpId()));
		     params.addElement(new String(String.valueOf(clock.sendAction())));
		     server.execute("DCIT.recieveOkFromRicartAgarwala", params);
		     System.out.println("Releasing the deferred request from "+iD+" from queue ");
		     System.out.println("OK message sent at " +dateFormat.format(new Date()));
		} catch (MalformedURLException | XmlRpcException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}        
        
		
		}
		numberOfNodesResponded=0;
		usingCR=false;
	}
	
}
