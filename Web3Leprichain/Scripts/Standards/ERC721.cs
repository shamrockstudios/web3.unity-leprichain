using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using UnityEngine;

namespace Ethereum
{
    public class ERC721 : Contract
    {

        public ERC721() { _name = "ERC-721"; }

        public ERC721(string tokenName, string contract, string abi) : base(tokenName, contract, abi)
        {
        }

        public virtual async Task<string> Name(string _chain, string _network, string _rpc = "")
        {
            string method = "name";
            string[] obj = { };
            string args = JsonConvert.SerializeObject(obj);
            string response = await EVM.Call(_chain, _network, _contract, _abi, method, args, _rpc);
            return response;
        }

        public virtual async Task<BigInteger> TotalSupply(string _chain, string _network, string _rpc = "")
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

        public virtual async Task<int> BalanceOf(string _chain, string _network, string _account, string _rpc = "")
        {
            string method = "balanceOf";
            string[] obj = { _account };
            string args = JsonConvert.SerializeObject(obj);
            string response = await EVM.Call(_chain, _network, _contract, _abi, method, args, _rpc);
            try
            {
                return int.Parse(response);
            }
            catch
            {
                Debug.LogError(response);
                throw;
            }
        }

        public virtual async Task<string> OwnerOf(string _chain, string _network, string _tokenId, string _rpc = "")
        {
            string method = "ownerOf";
            string[] obj = { _tokenId };
            string args = JsonConvert.SerializeObject(obj);
            string response = await EVM.Call(_chain, _network, _contract, _abi, method, args, _rpc);
            return response;
        }

        public virtual async Task<List<string>> OwnerOfBatch(string _chain, string _network, string[] _tokenIds, string _multicall = "", string _rpc = "")
        {
            string method = "ownerOf";
            // build array of args
            string[][] obj = new string[_tokenIds.Length][];
            for (int i = 0; i < _tokenIds.Length; i++)
            {
                obj[i] = new string[1] { _tokenIds[i] };
            };
            string args = JsonConvert.SerializeObject(obj);
            string response = await EVM.MultiCall(_chain, _network, _contract, _abi, method, args, _multicall, _rpc);
            try
            {
                string[] responses = JsonConvert.DeserializeObject<string[]>(response);
                List<string> owners = new List<string>();
                for (int i = 0; i < responses.Length; i++)
                {
                    // clean up address
                    string address = "0x" + responses[i].Substring(responses[i].Length - 40);
                    owners.Add(address);
                }
                return owners;
            }
            catch
            {
                Debug.LogError(response);
                throw;
            }
        }

        public virtual async Task<string> URI(string _chain, string _network, string _tokenId, string _rpc = "")
        {
            string method = "tokenURI";
            string[] obj = { _tokenId };
            string args = JsonConvert.SerializeObject(obj);
            string response = await EVM.Call(_chain, _network, _contract, _abi, method, args, _rpc);
            return response;
        }


#if UNITY_WEBGL
        public virtual async Task<string> Approve(string _spender, string _tokenId, string _gasLimit = "200000", string _gas = "0")
        {
            string method = "approve";
            string[] obj = { _spender, _tokenId };
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

        public virtual async Task<string> Transfer(string _toAccount, string _tokenId, string _gasLimit = "200000", string _gas = "0")
        {
            string method = "transfer";
            string[] obj = { _toAccount, _tokenId };
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

