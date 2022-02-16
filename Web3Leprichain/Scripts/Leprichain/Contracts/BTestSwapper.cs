using System.Collections;
using System.Numerics;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System;

public class BTestSwapper
{
    public static string tokenName = "BTest Swapper";
    public static string contract = "0xAED8c9Ef8EA7cF41B9Ffda5aA558936Bb63e62cA";
    public static int decimals = 18;
    public static string abi = JsonConvert.DeserializeObject<JObject>(Resources.Load<TextAsset>("Data/Leprichain/Token/BTestSwapper").text).GetValue("abi").ToString();

    
    public static async Task<BigInteger> BalanceOf(string _chain, string _network, string _account, string _rpc = "")
    {

        string method = "balanceOf";
        string[] obj = { _account };
        string args = JsonConvert.SerializeObject(obj);
        string response = await EVM.Call(_chain, _network, contract, abi, method, args, _rpc);
        Debug.Log(response);
        try
        {
            return BigInteger.Parse(response);
        }
        catch
        {
            Debug.LogError(response);
            throw;
        }
    }

    public static async Task<string> Name(string _chain, string _network, string _contract, string _rpc = "")
    {
        string method = "name";
        string[] obj = { };
        string args = JsonConvert.SerializeObject(obj);
        string response = await EVM.Call(_chain, _network, _contract, abi, method, args, _rpc);
        return response;
    }

    public static async Task<string> Symbol(string _chain, string _network, string _contract, string _rpc = "")
    {
        string method = "symbol";
        string[] obj = { };
        string args = JsonConvert.SerializeObject(obj);
        string response = await EVM.Call(_chain, _network, _contract, abi, method, args, _rpc);
        return response;
    }

    public static async Task<BigInteger> Decimals(string _chain, string _network, string _contract, string _rpc = "")
    {
        string method = "decimals";
        string[] obj = { };
        string args = JsonConvert.SerializeObject(obj);
        string response = await EVM.Call(_chain, _network, _contract, abi, method, args, _rpc);
        try
        {
            return BigInteger.Parse(response);
        }
        catch
        {
            Debug.LogError(response);
            throw;
        }
    }

    public static async Task<BigInteger> TotalSupply(string _chain, string _network, string _contract, string _rpc = "")
    {
        string method = "totalSupply";
        string[] obj = { };
        string args = JsonConvert.SerializeObject(obj);
        string response = await EVM.Call(_chain, _network, _contract, abi, method, args, _rpc);
        try
        {
            return BigInteger.Parse(response);
        }
        catch
        {
            Debug.LogError(response);
            throw;
        }
    }

#if UNITY_WEBGL
    public static async Task<bool> Swap(string _fromTokenAddress, string _amount, string _gasLimit = "100000", string _gas = "0")
    {

        string method = "swap";
        string[] obj = { _fromTokenAddress, _amount };
        string args = JsonConvert.SerializeObject(obj);
        try
        {
            string response = await Web3GL.SendContract(method, abi, contract, args, "0", _gasLimit, _gas);
            Debug.Log(response);
            return bool.Parse(response);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
        return false;
    }
#endif
}
