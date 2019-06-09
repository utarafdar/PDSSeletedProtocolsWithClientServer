using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Node1
{
    class Controller
    {
        static void Main(string[] args)
        {
            int choice = 1;
            Console.WriteLine("Please enter the port number to start listening----");
            int port =Int32.Parse(Console.ReadLine());
            ServerOperations serveroperation = new ServerOperations();
            Thread thread = new Thread(() => serveroperation.start(port));
            thread.Start();
           
            CsClient csclient = new CsClient();
           // csclient.testing1();
            while (choice ==1)
            {
                Console.WriteLine("1.join \n2.view address list\n3.Trigger Bully election \n4.Reset central distributed variable");
		        Console.WriteLine("5.view master node\n6.Start Distributive Operation with Centralised mutual exclusion\n7.Start Distributive Operation with Ricart and Agarwala Algorithm\n8.signoff \n9.exit");
                int read = Int32.Parse(Console.ReadLine());

                switch (read)
                {
                    case 1:
                        {
                            Console.WriteLine("Please enter the  Address to join ");
                            String sendAddress = Console.ReadLine();
                            try
                            {
                                csclient.join(sendAddress);
                            }
                            catch (Exception e)
                            {
                                // TODO Auto-generated catch block
                                Console.WriteLine(e.StackTrace);
                            }

                            break;
                        }
                    case 2:
                        {
                            NetworkBean data = new NetworkBean();
                            data.diaplayIdAddress();
                            Console.WriteLine("own address");
                            Console.WriteLine(data.getpId() + ":" + data.getAddress());
                            break;
                        }

                    case 3:
                        {
                            csclient.bullyAlgorithm();
                            break;
                        }
                    case 4:
                        {
                            csclient.resetDistributedStringVariable();

                            break;
                        }
                    case 5:
                        {
                            NetworkBean data = new NetworkBean();
                            data.diaplayIdAddress();
                            Console.WriteLine("master");
                            Console.WriteLine(data.getMasterPid() + ":" + data.getMasterAddress());
                            break;
                        }
                    case 6:
                        {
                            csclient.startDistributedOpeartion("CME");
                            break;
                        }
                    case 7:
                        {
                            csclient.startDistributedOpeartion("RA");
                            break;
                        }
                    case 8:
                        {
                            csclient.signOff();
                            break;
                        }
                    case 9:
                        {
                            csclient.signOff();
                            System.Environment.Exit(1);
                            //choice = 0;
                            break;
                        }
                    default:
                        Console.WriteLine("Invalid input");
                        break;
                }
            }
            //csclient.testing();
            Console.Read();
        }
    }
}
