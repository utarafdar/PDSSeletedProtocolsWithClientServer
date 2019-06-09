/**
 * 
 */
package network;

public class LamportClock {
	public static int c;
	public static int requestTimeStamp;
	
	
	    public  int getRequestTimeStamp() 
	    {
		return requestTimeStamp;
	    }
	public  void setRequestTimeStamp(int requestTimeStamp) 
	{
		LamportClock.requestTimeStamp = requestTimeStamp;
	}
	public static int getC() {
		return c;
	}
		public void setC(int value) {
	        c = value;
	    }
	    public int getValue() {
	        return c;
	    }
	    public void tick() { // on internal actions
	        c = c + 1;
	    }
	    public int sendAction() {
	       // include c in message
	        c = c + 1;
	        return c;
	    }
	    public void receiveAction(String sentValue) {
	        c = Math.max(c, Integer.parseInt(sentValue) + 1);
	    }
}
