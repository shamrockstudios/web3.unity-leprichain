using Newtonsoft.Json;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System;
using System.Numerics;
using System.Threading.Tasks;

public class SL3P : Ethereum.ERC20
{
    public SL3P() : base("Staked L3P", "0x9fedef2647A356F545c13b27F9E0e9d7F08C87e9", JsonConvert.DeserializeObject<JObject>(Resources.Load<TextAsset>("Data/Leprichain/Token/TestSL3P").text).GetValue("abi").ToString(), 18) { }

}

