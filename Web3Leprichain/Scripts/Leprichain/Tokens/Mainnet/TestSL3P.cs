using Newtonsoft.Json;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System;
using System.Numerics;
using System.Threading.Tasks;

public class TestSL3P : Ethereum.ERC20
{
    public TestSL3P() : base("Staked testL3P", "0x80edc5ae6de1415b878d234b24568250a95cc9fb", JsonConvert.DeserializeObject<JObject>(Resources.Load<TextAsset>("Data/Leprichain/Token/TestSL3P").text).GetValue("abi").ToString(), 18) { }

}

