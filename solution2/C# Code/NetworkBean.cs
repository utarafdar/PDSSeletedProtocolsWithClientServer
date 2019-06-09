using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node1
{
    class NetworkBean
    {
        private static String pId;
        private static int port;
        private static String address;
        private static String masterAddress;
        private static String masterPid;
        private static Dictionary<String, String> pId_Address = new Dictionary<String, String>();

        private static String distributedStringVariable;
        private static Boolean startCME;
        private static Boolean startRicartAgarwala;
        private static String message;
        private static ArrayList addedWords = new ArrayList();
        private static int noOfNodesFinishedOperation;
        private static int rnd1;
        //getters and setters
        public int getRnd()
        {
          
            return rnd1;
        }

        public void setRnd()
        {
            Random rnd = new Random();
          rnd1=  rnd.Next(1000, 8000);
            

        }

        public String getDistributedStringVariable()
        {
            return distributedStringVariable;
        }
        public void setDistributedStringVariable(String distributedStringVariable)
        {
            NetworkBean.distributedStringVariable = distributedStringVariable;
        }

        public String getMessage()
        {
            return message;
        }
        public void setMessage(String message)
        {
            NetworkBean.message = message;
        }
        public Boolean isStartCME()
        {
            return startCME;
        }
        public void setStartCME(Boolean startCME)
        {
            NetworkBean.startCME = startCME;
        }
        public Boolean isStartRicartAgarwala()
        {
            return startRicartAgarwala;
        }
        public void setStartRicartAgarwala(Boolean startRicartAgarwala)
        {
            NetworkBean.startRicartAgarwala = startRicartAgarwala;
        }

        public int getNoOfNodesFinishedOperation()
        {
            return noOfNodesFinishedOperation;
        }
        public void setNoOfNodesFinishedOperation(int noOfNodesFinishedOperation)
        {
            NetworkBean.noOfNodesFinishedOperation = noOfNodesFinishedOperation;
        }
        public ArrayList getAddedWords()
        {
            return addedWords;
        }
        public void setAddedWords(ArrayList addedWords)
        {
            NetworkBean.addedWords = addedWords;
        }
        public String getMasterAddress()
        {
            return masterAddress;
        }
        public void setMasterAddress(String masterAddress)
        {
            NetworkBean.masterAddress = masterAddress;
        }
        public String getMasterPid()
        {
            return masterPid;
        }
        public void setMasterPid(String masterPid)
        {
            NetworkBean.masterPid = masterPid;
        }

        public int getPort()
        {
            return port;
        }
        public void setPort(int port)
        {
            NetworkBean.port = port;
        }
        public String getpId()
        {
            return pId;
        }
        public void setpId(String pId)
        {
            NetworkBean.pId = pId;
        }
        public String getAddress()
        {
            return address;
        }
        public void setAddress(String address)
        {
            NetworkBean.address = address;
        }
        public Dictionary<String, String> getpId_Address()
        {
            return pId_Address;
        }
        public void setpId_Address(Dictionary<String, String> pId_Address)
        {
            NetworkBean.pId_Address.Clear();
            //NetworkBean.pId_Address.All<pId_Address>;
            foreach (KeyValuePair<string, string> entry in pId_Address)
            {
                // do something with entry.Value or entry.Key
                NetworkBean.pId_Address.Add(entry.Key, entry.Value);
            }
        }
        public void diaplayIdAddress()
        {
          
            foreach (KeyValuePair<string, string> entry in pId_Address)
            {
                // do something with entry.Value or entry.Key
                Console.WriteLine(entry.Key + ":\t" + entry.Value);
            }

        }
        public void addValuesToDictionary(String iD, String address)
        {
            NetworkBean.pId_Address.Add(iD, address);
        }
    }
}
