using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class TestSL3PExample : MonoBehaviour
{
    public Text networkIDTextUI;
    public Text accountTextUI;
    public Text balanceTextUI;
    public InputField transferToInput;
    public InputField transferAmountInput;

    private string account = "";
    private int chainId = 0;

    private TestSL3P testSL3PContract;


    // Start is called before the first frame update
    void Start()
    {

        account = PlayerPrefs.GetString("Account");
        accountTextUI.text = account.ToString();

        CheckChainID();
    }

    // Update is called once per frame
    void Update()
    {


    }

    public async void CheckChainID()
    {

        while (true)
        {
            try
            {
                if (chainId != Web3GL.Network())
                {
                    chainId = Web3GL.Network();
                    networkIDTextUI.text = chainId.ToString();
                    if (networkIDTextUI.text != "49777")
                    {
                        print("wrong network");
                    }
                    else
                    {
                        print("connected to leprichain network");
                        await GetAllBalance();
                    }
                }
            }
            catch (Exception e)
            {
                print(e.StackTrace.ToString());
            }

            await new WaitForSeconds(1.0f);
        }
       
    }

    public void GetBalance()
    {
        GetAllBalance();
    }

    public async Task GetAllBalance()
    {
        print("I'm getting balance");
        BigInteger balanceOfFromWei = await testSL3PContract.BalanceOf(LeprichainMainnet.name, LeprichainMainnet.network, account, LeprichainMainnet.rpc, true);
        print(balanceOfFromWei);
        balanceTextUI.text = (balanceOfFromWei).ToString();
        print("I'm getting balance x1");
    }

    public async void Approve()
    {  
        print("approve");
        string transaction = await testSL3PContract.Approve(
            testSL3PContract.Address,
            new BigInteger(1 * Math.Pow(10, testSL3PContract.Decimals)).ToString()
        );
        
        print("result: " + transaction);
        await GetAllBalance();
        if (transaction == "")
        {
            return;
        }
        print("Getting tx");
        bool txConfirmed = await EVMExtensions.WaitForTxFinished(LeprichainMainnet.name, LeprichainMainnet.network, transaction, LeprichainMainnet.rpc);
        print(txConfirmed);
        //print(balanceOf / TestSL3P.decimals * 10);
        //balanceTextUI.text = (balanceOf / TestSL3P.decimals * 10).ToString();
    }

    public async void Transfer()
    {
        int tokenAmount = 0;

        // Send back to contract address for testing
        string toAddress = testSL3PContract.Address;

        // Manual input
        if(transferAmountInput.text != "")
        {
            toAddress = transferAmountInput.text;
        }

        if (int.TryParse(transferToInput.text, out tokenAmount))
        {
            print("transfer");
            string transaction = await testSL3PContract.Transfer(
                testSL3PContract.Address,
                new BigInteger(1 * Math.Pow(10, testSL3PContract.Decimals)).ToString()
            );

            print("result: " + transaction);
            await GetAllBalance();
            if (transaction == "")
            {
                return;
            }
            print("Getting tx");
            bool txConfirmed = await EVMExtensions.WaitForTxFinished(LeprichainMainnet.name, LeprichainMainnet.network, transaction, LeprichainMainnet.rpc);
            print(txConfirmed);
        }


    }

}
