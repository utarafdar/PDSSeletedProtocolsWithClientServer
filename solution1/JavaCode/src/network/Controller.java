package network;

import java.net.MalformedURLException;
import java.util.Scanner;

import org.apache.xmlrpc.XmlRpcException;

public class Controller {

	
	private static Scanner reader;
	private static Scanner input;

	public static void main(String[] args) throws InterruptedException {
		// TODO Auto-generated method stub
		
		int choice=1;
		JavaClient javaClient = new JavaClient();
		//JavaServer javaServer = new JavaServer();
		ServerOperations serveroperation = new ServerOperations();
		System.out.println("Please enter the port number to start listening---");
		reader = new Scanner(System.in);
		int port = reader.nextInt();
	    serveroperation.start(port);
	    NetworkBean data = new NetworkBean();
		while (choice==1)
		{
		//if(data.isStartRicartAgarwala()==false && data.isStartCME()==false)	
		//{
	      Thread.sleep(100);
		System.out.println("1.join \n2.view address list\n3.Trigger Bully election \n4.Reset central distributed variable");
		System.out.println("5.view master node\n6.Start Distributive Operation with Centralised mutual exclusion\n7.Start Distributive Operation with Ricart and Agarwala Algorithm\n8.signoff \n9.exit");
		//reader2 = new Scanner(System.in); 
		int n = reader.nextInt();
		if (n==1)
		{  System.out.println("address");
		input = new Scanner(System.in);
	    String sendAddress = input.nextLine();
	    System.out.println("send addr = "+sendAddress);
	    try {
			javaClient.join(sendAddress);
		    } 
	    catch (MalformedURLException | XmlRpcException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		    }
		}
		if (n==2)
		{
			//NetworkBean data = new NetworkBean();
			data.diaplayIdAddress();
			System.out.println("own address");
			System.out.println(data.getpId()+":"+data.getAddress());
		}
		if (n==3)
		{
			javaClient.bullyAlgorithm();
		}
		if (n==4)
		{
		 javaClient.resetDistributedStringVariable();
		}
		if (n==5)
		{
			//NetworkBean data = new NetworkBean();
			data.diaplayIdAddress();
			System.out.println("master");
			System.out.println(data.getMasterPid()+":"+data.getMasterAddress());
		}
		if (n==6)
		{
			javaClient.startDistributedOpeartion("CME");
		}
		if (n==7)
		{   
			javaClient.startDistributedOpeartion("RA");
		}
		if (n==8)
		{
			javaClient.signOff();
		}
		if (n==9)
		{javaClient.signOff();
			System.exit(0);;
		}
		}
		
	
		//}
	}

}
