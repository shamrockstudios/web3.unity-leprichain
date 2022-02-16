using Newtonsoft.Json;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System;

public class BTest : Ethereum.ERC20
{
    public BTest() : base("BTest", "0x7df27878630EAF223984079B6AE08D046c243fdD", JsonConvert.DeserializeObject<JObject>(Resources.Load<TextAsset>("Data/Leprichain/Token/BTest").text).GetValue("abi").ToString(), 18) { }
}
