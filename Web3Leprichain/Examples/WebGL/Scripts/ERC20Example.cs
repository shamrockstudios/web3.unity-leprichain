using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ERC20Example : MonoBehaviour
{
    public Text networkIDTextUI;
    public Text accountTextUI;

    [Header("Name")]
    public Text nameTextUI;

    [Header("TotalSupply")]
    public Text totalSupplyTextUI;

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

    private readonly TestGameToken testGameTokenContract = new TestGameToken();

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
                    //if (networkIDTextUI.text != "49777")
                    if(networkIDTextUI.text != "49778")
                    {
                        print("wrong network");
                    }
                    else
                    {
                        print("connected to leprichain network");
                        ExecuteBalance();
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
        balanceTextUI.text = (await Balance()).ToString();  
    }

    public async Task<BigInteger> Balance()
    {
        print("Getting Balance");
        print(account);
        BigInteger balanceOfFromWei = await testGameTokenContract.BalanceOf(LeprichainTestnet.name, LeprichainTestnet.network, account, LeprichainTestnet.rpc, true);
        print("Received Balance Number " + balanceOfFromWei);
        return balanceOfFromWei;
    }

    public async void ExecuteApprove()
    {

        // Send back to contract address for testing
        string toAddress = testGameTokenContract.Address;

        // Manual input
        if (approveToInput.text != "" && approveToInput.text.Contains("0x"))
        {
            toAddress = approveAmountInput.text;
        }
        else
        {
            Debug.LogWarning("to address not found, you must input a valid erc20 address.");
            return;
        }

        float tokenAmount = 0;

        if (!float.TryParse(approveToInput.text, out tokenAmount))
        {
            Debug.LogWarning("token amount format error.");
            return;
        }

        await Approve(toAddress, tokenAmount);
    }

    public async Task<bool> Approve(string toAddress, float tokenAmount)
    {

        print("start approve");
        string transaction = await testGameTokenContract.Approve(
            toAddress,
            new BigInteger(tokenAmount * Math.Pow(10, testGameTokenContract.Decimals)).ToString()
        );
        
        print("result: " + transaction);
        if (transaction == "")
        {
            Debug.LogWarning("approve canceled");
            return false;
        }

        print("Getting approve tx");
        bool txConfirmed = await EVMExtensions.WaitForTxFinished(LeprichainTestnet.name, LeprichainTestnet.network, transaction, LeprichainTestnet.rpc);
        print(txConfirmed);
        return txConfirmed;

    }

    public async void ExecuteTransfer()
    {

        // Send back to contract address for testing
        string toAddress = testGameTokenContract.Address;

        // Manual input
        if (transferToInput.text != "" && transferToInput.text.Contains("0x"))
        {
            toAddress = transferAmountInput.text;
        }
        else
        {
            Debug.LogWarning("to address not found, you must input a valid erc20 address.");
            return;
        }

        float tokenAmount = 0;

        if (!float.TryParse(transferToInput.text, out tokenAmount))
        {
            Debug.LogWarning("token amount format error.");
            return;
        }

        await Transfer(toAddress, tokenAmount);
        // Renew balance
        ExecuteBalance();
    }

    public async Task<bool> Transfer(string toAddress, float tokenAmount)
    {

        print("start transfer");
        string transaction = await testGameTokenContract.Transfer(
            testGameTokenContract.Address,
            new BigInteger(1 * Math.Pow(10, testGameTokenContract.Decimals)).ToString()
        );

        print("result: " + transaction);
        if (transaction == "")
        {
            Debug.LogWarning("transfer canceled");
            return false;
        }

        print("Getting transfer tx");
        bool txConfirmed = await EVMExtensions.WaitForTxFinished(LeprichainTestnet.name, LeprichainTestnet.network, transaction, LeprichainTestnet.rpc);
        print(txConfirmed);
        return txConfirmed;
    }


    public async void ExecuteGetName()
    {
        nameTextUI.text = (await GetName());
    }

    public async Task<string> GetName()
    {
        string name = await ERC20.Name(LeprichainTestnet.name, LeprichainTestnet.network, testGameTokenContract.Address, LeprichainTestnet.rpc);
        print(name);
        return name;
    }

    public async void ExecuteGetTotalSupply()
    {

        totalSupplyTextUI.text = (await GetTotalSupply()).ToString();

    }

    public async Task<BigInteger> GetTotalSupply()
    {
        BigInteger totalSupply = await ERC20.TotalSupply(LeprichainTestnet.name, LeprichainTestnet.network, testGameTokenContract.Address, LeprichainTestnet.rpc);
        print(totalSupply);
        return (totalSupply);
    }
}
