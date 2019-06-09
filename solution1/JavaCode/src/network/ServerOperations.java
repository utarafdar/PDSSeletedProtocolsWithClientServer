package network;

import java.io.IOException;
import java.net.InetAddress;
import java.net.MalformedURLException;
import java.net.URL;
import java.text.SimpleDateFormat;
import java.util.Date;
import org.apache.xmlrpc.XmlRpcException;
import org.apache.xmlrpc.client.XmlRpcClient;
import org.apache.xmlrpc.client.XmlRpcClientConfigImpl;
import org.apache.xmlrpc.server.PropertyHandlerMapping;
import org.apache.xmlrpc.server.XmlRpcServer;
import org.apache.xmlrpc.webserver.WebServer;

public class ServerOperations {
	public void start(int port) {
		// TODO Auto-generated method stub
		try {
			startListening(port);
	         // set ID and address and port
			CentralisedMutualExclusion cme = new CentralisedMutualExclusion();
			Ricart_Agarwala RA = new Ricart_Agarwala();
	         String address = "http://"+InetAddress.getLocalHost().getHostAddress()+":"+port;
	         System.out.println(address);
	         Date dNow = new Date();	         
	         SimpleDateFormat ft = new SimpleDateFormat("yyyyMMddHHmmssSSS");	         
	         String iD = ft.format(dNow);		         
	         NetworkBean data = new NetworkBean();
	         LamportClock clock = new LamportClock();
	         data.setAddress(address);
	         //data.setpId(Long.parseLong(iD));
	         data.setpId(iD);
	         data.setPort(port);
	         data.setMasterAddress(address);
	         data.setMasterPid(iD);
	         cme.setCrInUse(false);
	         RA.setRequestCR(false);
	         RA.setUsingCR(false);
	         RA.setNumberOfNodesResponded(0);
	         RA.setMyTimeStamp("0");
	         data.setDistributedStringVariable("");
	         data.setStartCME(false);
	         data.setStartRicartAgarwala(false);
	         data.setMessage("");
	         clock.setC(1);
	         data.setNoOfNodesFinishedOperation(0);	         
	      } catch (Exception exception){
	         System.err.println("JavaServer: " + exception);
	      }
	}

	
	public XmlRpcClient setXmlRpcConfig(String address) throws MalformedURLException
	{
		XmlRpcClientConfigImpl config = new XmlRpcClientConfigImpl();
		config.setServerURL(new URL(address));
	    XmlRpcClient server = new XmlRpcClient(); 
	    server.setConfig(config);
	    return server;
	}
	
	public void stopListening()
	{   NetworkBean data = new NetworkBean();
		WebServer server = new WebServer(data.getPort());
		server.shutdown();
	}
	
/*	public void startListening() throws XmlRpcException, IOException
	{NetworkBean data = new NetworkBean();
		 System.out.println("Attempting to start XML-RPC Server...");
         WebServer server = new WebServer(data.getPort());
         XmlRpcServer xmlRpcServer = server.getXmlRpcServer();
        //server.addHandler("sample", new JavaServer());
         PropertyHandlerMapping phm = new PropertyHandlerMapping();
         phm.addHandler("DCIT", JavaServer.class);
         xmlRpcServer.setHandlerMapping(phm);
         server.start();
         System.out.println("Started successfully.");
         System.out.println("Accepting requests. (Halt program to stop.)");
	}*/
	
	public void startListening(int port) throws XmlRpcException, IOException
	{   // NetworkBean data = new NetworkBean();
		 System.out.println("Attempting to start XML-RPC Server...");
         WebServer server = new WebServer(port);
         XmlRpcServer xmlRpcServer = server.getXmlRpcServer();
        //server.addHandler("sample", new JavaServer());
         PropertyHandlerMapping phm = new PropertyHandlerMapping();
         phm.addHandler("DCIT", JavaServer.class);
         xmlRpcServer.setHandlerMapping(phm);
         server.start();
         System.out.println("Started successfully.");
        // System.out.println("Accepting requests. (Halt program to stop.)");
	}

	
}
