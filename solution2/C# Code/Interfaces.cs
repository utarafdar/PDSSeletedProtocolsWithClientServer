using CookComputing.XmlRpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node1
{    //joinNetwork
    public interface joinNetw : IXmlRpcProxy
    {
        [XmlRpcMethod("DCIT.joinNetwork")]
        String joinNetwork(String iD, String address);

    }
    //updateAddressMap
    public interface updateAddrMap : IXmlRpcProxy
    {
        [XmlRpcMethod("DCIT.updateAddressMap")]
        Boolean updateAddressMap(String iD, String address);

    }

    //setMasterAddressId

    public interface setMasterAddrId : IXmlRpcProxy
    {
        [XmlRpcMethod("DCIT.setMasterAddressId")]
        Boolean setMasterAddressId(String iD, String address);

    }

    //distributedRead

    public interface distribRead : IXmlRpcProxy
    {
        [XmlRpcMethod("DCIT.distributedRead")]
        String distributedRead(String iD);

    }

    //distributedWrite
    public interface distribWrite : IXmlRpcProxy
    {
        [XmlRpcMethod("DCIT.distributedWrite")]
        Boolean distributedWrite(String iD, String message);

    }

    //requestCMEDistributiveOperation
    public interface reqCMEDistributiveOperation : IXmlRpcProxy
    {
        [XmlRpcMethod("DCIT.requestCMEDistributiveOperation")]
        String requestCMEDistributiveOperation(String iD);

    }

    //CMEreleaseResource
    public interface CMErelResource : IXmlRpcProxy
    {
        [XmlRpcMethod("DCIT.CMEreleaseResource")]
        Boolean CMEreleaseResource(String iD);

    }

    //electMasterNode
    public interface elctMasterNode : IXmlRpcProxy
    {
        [XmlRpcMethod("DCIT.electMasterNode")]
        String electMasterNode(String iD);

    }
    //nodeSignOff
    public interface nodeSignoff : IXmlRpcProxy
    {
        [XmlRpcMethod("DCIT.nodeSignOff")]
        Boolean nodeSignOff(String iD);

    }
    //callBullyAfterSignOff
    public interface callBullyAfterSignoff : IXmlRpcProxy
    {
        [XmlRpcMethod("DCIT.callBullyAfterSignOff")]
        String callBullyAfterSignOff(String iD);

    }

    //removerNodeFromNetwork
    public interface rmvNodeFromNetwork : IXmlRpcProxy
    {
        [XmlRpcMethod("DCIT.removerNodeFromNetwork")]
        Boolean removerNodeFromNetwork(String iD);

    }
    // public Boolean recieveOkFromRicartAgarwala(String oK)

    public interface rcvOkFromRA : IXmlRpcProxy
    {
        [XmlRpcMethod("DCIT.recieveOkFromRicartAgarwala")]
        Boolean recieveOkFromRicartAgarwala(String oK, String timeStamp);

    }

    // public String RA_sendRequest(String iD, String timeStamp)

    public interface RA_sendReq : IXmlRpcProxy
    {
        [XmlRpcMethod("DCIT.RA_sendRequest")]
        String RA_sendRequest(String iD, String timeStamp);

    }

    //Boolean recieveOkFromRicartAgarwala(String oK)
 //   public interface recieveOkFromRA : IXmlRpcProxy
 //   {
   //     [XmlRpcMethod("DCIT.recieveOkFromRicartAgarwala")]
     //   Boolean recieveOkFromRicartAgarwala(String oK);

    //}

    //Boolean distributedProcessDone(String iD)
    public interface distdProcessDone : IXmlRpcProxy
    {
        [XmlRpcMethod("DCIT.distributedProcessDone")]
        Boolean distributedProcessDone(String iD);

    }

    // Boolean resetDistributedMasterString(String iD)
    public interface resetDistributedMasterStr : IXmlRpcProxy
    {
        [XmlRpcMethod("DCIT.resetDistributedMasterString")]
        Boolean resetDistributedMasterString(String iD);

    }

    //Boolean startDistributedOperations(String approach)
    public interface strtDistributedOperations : IXmlRpcProxy
    {
        [XmlRpcMethod("DCIT.startDistributedOperations")]
        Boolean startDistributedOperations(String approach);

    }



    class Interfaces
    {
    }
}
