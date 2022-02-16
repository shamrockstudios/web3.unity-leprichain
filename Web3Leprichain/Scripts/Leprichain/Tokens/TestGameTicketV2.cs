using System.Collections;
using System.Numerics;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System;

public class TestGameTicketV2 : Ethereum.ERC20
{
    public TestGameTicketV2() : base("TestGameTicketV2", "0xD56501293ed4b3252768b299b76b48D75E0F2dcA", JsonConvert.DeserializeObject<JObject>(Resources.Load<TextAsset>("Data/Leprichain/Token/TestGameTicketV2").text).GetValue("abi").ToString(), 18) { }

}
