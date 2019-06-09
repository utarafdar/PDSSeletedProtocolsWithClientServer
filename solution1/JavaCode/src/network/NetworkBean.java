package network;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.Iterator;
import java.util.Map.Entry;

public class NetworkBean {
private static String pId;
private static int port;
private static String address;
private static String masterAddress;
private static String masterPid;
private static String distributedStringVariable;
private static boolean startCME;
private static boolean startRicartAgarwala;
private static String message;
private static ArrayList<String> addedWords = new ArrayList<String>();
private static HashMap<String,String> pId_Address = new HashMap<String,String>();
private static int noOfNodesFinishedOperation;

public  int getNoOfNodesFinishedOperation() {
	return noOfNodesFinishedOperation;
}
public  void setNoOfNodesFinishedOperation(int noOfNodesFinishedOperation) {
	NetworkBean.noOfNodesFinishedOperation = noOfNodesFinishedOperation;
}
public  ArrayList<String> getAddedWords() {
	return addedWords;
}
public  void setAddedWords(ArrayList<String> addedWords) {
	NetworkBean.addedWords = addedWords;
}
public  String getMessage() {
	return message;
}
public  void setMessage(String message) {
	NetworkBean.message = message;
}
public  boolean isStartCME() {
	return startCME;
}
public  void setStartCME(boolean startCME) {
	NetworkBean.startCME = startCME;
}
public  boolean isStartRicartAgarwala() {
	return startRicartAgarwala;
}
public  void setStartRicartAgarwala(boolean startRicartAgarwala) {
	NetworkBean.startRicartAgarwala = startRicartAgarwala;
}
public  String getDistributedStringVariable() {
	return distributedStringVariable;
}
public  void setDistributedStringVariable(String distributedStringVariable) {
	NetworkBean.distributedStringVariable = distributedStringVariable;
}

public  String getMasterAddress() {
	return masterAddress;
}
public  void setMasterAddress(String masterAddress) {
	NetworkBean.masterAddress = masterAddress;
}
public  String getMasterPid() {
	return masterPid;
}
public  void setMasterPid(String masterPid) {
	NetworkBean.masterPid = masterPid;
}

public  int getPort() {
	return port;
}
public void setPort(int port) {
	NetworkBean.port = port;
}
public String getpId() {
	return pId;
}
public void setpId(String pId) {
	NetworkBean.pId = pId;
}
public String getAddress() {
	return address;
}
public void setAddress(String address) {
	NetworkBean.address = address;
}
public HashMap<String, String> getpId_Address() {
	return pId_Address;
}
public void setpId_Address(HashMap<String, String> pId_Address) {
	NetworkBean.pId_Address.clear();
	NetworkBean.pId_Address.putAll(pId_Address);
}
//public NetworkBean(String pId, String address, HashMap<String, String> pId_Address) {
//	super();
//	NetworkBean.pId = pId;
//	NetworkBean.address = address;
//	NetworkBean.pId_Address = pId_Address;
//}
//public NetworkBean()
//{
//	
//}

public void diaplayIdAddress()
{
	Iterator<Entry<String, String>> it = NetworkBean.pId_Address.entrySet().iterator();
	while(it.hasNext())
	{
		Entry <String,String> entry = it.next();
		System.out.println(entry.getKey()+":\t"+entry.getValue());
	}
	
}

public void addValuesToHashMap(String iD, String address)
{
	NetworkBean.pId_Address.put(iD,address );
}

}
