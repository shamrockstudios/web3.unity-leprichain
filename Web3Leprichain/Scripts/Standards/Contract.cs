using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ethereum
{
    public class Contract
    {
        protected string _name = "";
        protected string _contract = "";
        protected string _abi = "";

        public string ContractName { get { return _name; } }
        public string Address { get { return _contract; } }
        public string ABI { get { return _abi; } }


        public Contract() { }

        public Contract(string name, string contract, string abi)
        {
            _name = name;
            _contract = contract;
            _abi = abi;
        }
    }
}