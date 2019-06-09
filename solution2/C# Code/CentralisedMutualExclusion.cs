using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Node1
{
    class CentralisedMutualExclusion
    {
         static volatile bool isCrInUse;
       
        
        private static Queue<String> mutualExclusionQueue = new Queue<string>();
       
    public void setCRInUse(Boolean CrInUse)
        {
            isCrInUse = CrInUse;
        }

       public Boolean getisCrInUse()
        {
            return isCrInUse;
        }


        public Queue<String> getMutualExclusionQueue()
        {
            return mutualExclusionQueue;
        }
        public void setMutualExclusionQueue(Queue<String> mutualExclusionQueue)
        {
            CentralisedMutualExclusion.mutualExclusionQueue = mutualExclusionQueue;
        }
        public String request(String iD)
        {
            // TODO Auto-generated method stub
            //System.out.println("1");
            
            Console.WriteLine("request recieved from " + iD + "at " + DateTime.Now.ToString("HH:mm:ss:fff"));
            if (CentralisedMutualExclusion.isCrInUse)
            {
                Console.WriteLine("CR is in Use, adding the process " + iD + " to wait queue");
                CentralisedMutualExclusion.mutualExclusionQueue.Enqueue(iD);
                //System.out.println("request recieved from ");
                Boolean CRuse = isCrInUse;
                do
                { // System.out.println("3");
                  //System.out.println(isCrInUse);
                  //System.out.println(isCrInUse);
                
                    // Console.WriteLine(isCrInUse + CentralisedMutualExclusion.mutualExclusionQueue.Peek() + iD);
                    try
                    {
                         Thread.Sleep(100);
                       // SpinWait.SpinUntil(() => isCrInUse);


                    }
                    catch (Exception e)
                    {
                        // TODO Auto-generated catch block
                        Console.WriteLine(e.StackTrace);
                    }
                    //System.out.println("here");
                    

                } while (!getisCrInUse() || !(CentralisedMutualExclusion.mutualExclusionQueue.Peek().Equals(iD)));

              
                Console.WriteLine("CR is now free...");
                Console.WriteLine("sending OK message to " + mutualExclusionQueue.Peek() + "at " + DateTime.Now.ToString("HH:mm:ss:fff"));
                CentralisedMutualExclusion.isCrInUse = true;
                return "OK";

            }
            else
            {//System.out.println("4");
                Console.WriteLine("CR is Not in Use...");
                Console.WriteLine("sending OK message to " + iD + "at " + DateTime.Now.ToString("HH:mm:ss:fff"));
                CentralisedMutualExclusion.isCrInUse = true;
                return "OK";
            }
        }
        public Boolean release(String iD)
        {
            // TODO Auto-generated method stub
            if (CentralisedMutualExclusion.mutualExclusionQueue.Contains(iD))
            {
                CentralisedMutualExclusion.mutualExclusionQueue.Dequeue();
                //System.out.println("CR is free ... sending OK message to "+iD+"at "+dateFormat.format(date));
            }
            setCRInUse(false);



            return true;
        }
       
            
        

    }
}
