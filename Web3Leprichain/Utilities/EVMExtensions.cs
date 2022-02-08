using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EVMExtensions : EVM
{
    public static async Task<bool> WaitForTxFinished(string _chain, string _network, string _transaction, string _rpc = "")
    {
        try
        {
            string txStatus = "pending";
            while(txStatus == "pending")
            {
                await new WaitForSeconds(5.0f);
                txStatus = await TxStatus(_chain, _network, _transaction, _rpc);
            }
            
            // Check Status
            if(txStatus == "success")
            {
                return true;
            } 
            else
            {
                return false;
            }
        }
        catch(Exception e)
        {
            Debug.LogError(e);
        }
        return false;
    }
}
