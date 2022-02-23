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

    [Header("Balance")]
    public Text balanceTextUI;

    [Header("Function - Approve")]
    public InputField approveToInput;
    public InputField approveAmountInput;

    [Header("Function - Transfer")]
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
                        await Balance();
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

    public async void ExecuteBalance()
    {
        await Balance();
    }

    public async Task Balance()
    {
        print("Getting Balance");
        BigInteger balanceOfFromWei = await testSL3PContract.BalanceOf(LeprichainMainnet.name, LeprichainMainnet.network, account, LeprichainMainnet.rpc, true);
        balanceTextUI.text = (balanceOfFromWei).ToString();
        print("Received Balance Number " + balanceOfFromWei);
    }

    public async void ExecuteApprove()
    {
        await Approve();
    }

    public async Task Approve()
    {
        float tokenAmount = 0;

        // Send back to contract address for testing
        string toAddress = testSL3PContract.Address;

        // Manual input
        if (approveToInput.text != "" && !approveToInput.text.Contains("0x"))
        {
            toAddress = approveAmountInput.text;
        }
        else
        {
            Debug.LogWarning("to address not found, you must input a valid erc20 address.");
            return;
        }

        if (!float.TryParse(approveToInput.text, out tokenAmount))
        {
            Debug.LogWarning("token amount format error.");
            return;
        }

        print("start approve");
        string transaction = await testSL3PContract.Approve(
            toAddress,
            new BigInteger(tokenAmount * Math.Pow(10, testSL3PContract.Decimals)).ToString()
        );
        
        print("result: " + transaction);
        if (transaction == "")
        {
            Debug.LogWarning("approve canceled");
            return;
        }

        print("Getting approve tx");
        bool txConfirmed = await EVMExtensions.WaitForTxFinished(LeprichainMainnet.name, LeprichainMainnet.network, transaction, LeprichainMainnet.rpc);
        print(txConfirmed);
    }

    public async void ExecuteTransfer()
    {
        await Transfer();
        // Renew balance
        await Balance();
    }

    public async Task Transfer()
    {
        float tokenAmount = 0;

        // Send back to contract address for testing
        string toAddress = testSL3PContract.Address;

        // Manual input
        if(transferToInput.text != "" && !transferToInput.text.Contains("0x"))
        {
            toAddress = transferAmountInput.text;
        } 
        else
        {
            Debug.LogWarning("to address not found, you must input a valid erc20 address.");
            return;
        }

        if (!float.TryParse(transferToInput.text, out tokenAmount))
        {
            Debug.LogWarning("token amount format error.");
            return;
        }

        print("start transfer");
        string transaction = await testSL3PContract.Transfer(
            testSL3PContract.Address,
            new BigInteger(1 * Math.Pow(10, testSL3PContract.Decimals)).ToString()
        );

        print("result: " + transaction);
        if (transaction == "")
        {
            Debug.LogWarning("transfer canceled");
            return;
        }

        print("Getting transfer tx");
        bool txConfirmed = await EVMExtensions.WaitForTxFinished(LeprichainMainnet.name, LeprichainMainnet.network, transaction, LeprichainMainnet.rpc);
        print(txConfirmed);

    }

}
