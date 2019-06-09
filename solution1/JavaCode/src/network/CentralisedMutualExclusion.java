/**
 * 
 */
package network;

import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.LinkedList;
import java.util.Queue;


public class CentralisedMutualExclusion {
	private static boolean isCrInUse;
	private static Queue<String> mutualExclusionQueue = new LinkedList<String>();
	DateFormat dateFormat = new SimpleDateFormat("HH:mm:ss:SSS");
	
	
	public static boolean isCrInUse() {
		return isCrInUse;
	}
	public  void setCrInUse(boolean isCrInUse) {
		CentralisedMutualExclusion.isCrInUse = isCrInUse;
	}
	public  Queue<String> getMutualExclusionQueue() {
		return mutualExclusionQueue;
	}
	public  void setMutualExclusionQueue(Queue<String> mutualExclusionQueue) {
		CentralisedMutualExclusion.mutualExclusionQueue = mutualExclusionQueue;
	}
	public String request(String iD) {
		// TODO Auto-generated method stub
		//System.out.println("1");
		System.out.println("request recieved from "+iD+"at "+dateFormat.format(new Date()));
		if(CentralisedMutualExclusion.isCrInUse)
		{
			System.out.println("CR is in Use, adding the process "+iD+" to wait queue");
			CentralisedMutualExclusion.mutualExclusionQueue.add(iD);
			//System.out.println("request recieved from ");
		//	Boolean CRuse=isCrInUse;
			while (isCrInUse==true || !(CentralisedMutualExclusion.mutualExclusionQueue.element().equals(iD)))
			{ // System.out.println("3");
			//System.out.println(isCrInUse);
				//System.out.println(isCrInUse);
				
				try {
					Thread.sleep(100);
				} catch (InterruptedException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
				//System.out.println("here");
			}
			System.out.println("CR is now free...");
			System.out.println("sending OK message to "+mutualExclusionQueue.element()+"at "+dateFormat.format(new Date()));
			CentralisedMutualExclusion.isCrInUse=true;
			return "OK";
			
		}
		else 
		{//System.out.println("4");
			System.out.println("CR is Not in Use...");
			System.out.println("sending OK message to "+iD+"at "+dateFormat.format(new Date()));
		CentralisedMutualExclusion.isCrInUse=true;	
		return "OK";
		}
	}
	public boolean release(String iD) {
		// TODO Auto-generated method stub
		if(CentralisedMutualExclusion.mutualExclusionQueue.contains(iD))
		{CentralisedMutualExclusion.mutualExclusionQueue.remove(iD);
		//System.out.println("CR is free ... sending OK message to "+iD+"at "+dateFormat.format(date));
		}
		CentralisedMutualExclusion.isCrInUse=false;
		return true;
	}
	 
	}
