using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using UnityEngine;

namespace Ethereum
{
    public class ERC20 : Contract
    {
        protected int _decimals = 18;
        public int Decimals { get { return _decimals; } }

        public ERC20() { _name = "ERC-20"; }

        public ERC20(string tokenName, string contract, string abi, int decimals) : base (tokenName, contract, abi)
        {
            _decimals = decimals;
        }

        public virtual async Task<BigInteger> BalanceOf(string _chain, string _network, string _account, string _rpc = "", bool _removeDecimals = false)
        {
            string method = "balanceOf";
            string[] obj = { _account };
            string args = JsonConvert.SerializeObject(obj);
            string response = await EVM.Call(_chain, _network, _contract, _abi, method, args, _rpc);
            Debug.Log("Balance: ");
            Debug.Log("123: ");
            Debug.Log("Balance: " + response);
            try
            {
                if (_removeDecimals)
                {
                    return (BigInteger.Parse(response) / new BigInteger(Math.Pow(10, Decimals)));
                }
                return BigInteger.Parse(response);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                Debug.LogError(response);
                throw e;
            }
        }

        public virtual async Task<string> Name(string _chain, string _network, string _contract, string _rpc = "")
        {
            string method = "name";
            string[] obj = { };
            string args = JsonConvert.SerializeObject(obj);
            string response = await EVM.Call(_chain, _network, _contract, _abi, method, args, _rpc);
            return response;
        }

        public virtual async Task<string> Symbol(string _chain, string _network, string _contract, string _rpc = "")
        {
            string method = "symbol";
            string[] obj = { };
            string args = JsonConvert.SerializeObject(obj);
            string response = await EVM.Call(_chain, _network, _contract, _abi, method, args, _rpc);
            return response;
        }

        /*
        public virtual async Task<BigInteger> Decimals(string _chain, string _network, string _contract, string _rpc = "")
        {
            string method = "decimals";
            string[] obj = { };
            string args = JsonConvert.SerializeObject(obj);
            string response = await EVM.Call(_chain, _network, _contract, ABI, method, args, _rpc);
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
        */

        public virtual async Task<BigInteger> TotalSupply(string _chain, string _network, string _contract, string _rpc = "")
        {
            string method = "totalSupply";
            string[] obj = { };
            string args = JsonConvert.SerializeObject(obj);
            string response = await EVM.Call(_chain, _network, _contract, _abi, method, args, _rpc);
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
        public virtual async Task<string> Approve(string _spender, string _amount, string _gasLimit = "100000", string _gas = "0")
        {
            string method = "approve";
            string[] obj = { _spender, _amount };
            string args = JsonConvert.SerializeObject(obj);
            try
            {
                string response = await Web3GL.SendContract(method, _abi, _contract, args, "0", _gasLimit, _gas);
                Debug.Log(response);
                return response;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            return "";
        }

        public virtual async Task<string> Transfer(string _toAccount, string _amount, string _gasLimit = "100000", string _gas = "0")
        {
            string method = "transfer";
            string[] obj = { _toAccount, _amount };
            string args = JsonConvert.SerializeObject(obj);
            try
            {
                string response = await Web3GL.SendContract(method, _abi, _contract, args, "0", _gasLimit, _gas);
                Debug.Log(response);
                return response;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            return "";
        }
        #endif
    }
}

