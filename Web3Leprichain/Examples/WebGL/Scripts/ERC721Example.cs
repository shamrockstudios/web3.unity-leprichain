using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ERC721Example : MonoBehaviour
{
    public Text networkIDTextUI;
    public Text accountTextUI;

    [Header("Name")]
    public Text nameTextUI;

    [Header("TotalSupply")]
    public Text totalSupplyTextUI;

    [Header("Balance Of")]
    public InputField balanceOfAddressInput;
    public Text balanceTextUI;

    [Header("Owner Of")]
    public InputField ownerOfTokenIdInput;
    public Text ownerOfTextUI;

    [Header("TokenURI")]
    public InputField tokenURIIdInput;
    public Text tokenURITextUI;

    [Header("Function - Approve")]
    public InputField approveToInput;
    public InputField approveTokenIdInput;

    [Header("Function - Transfer")]
    public InputField transferToInput;
    public InputField transferTokenIdInput;



    private string account = "";
    private int chainId = 0;

    private LepriFoxTest lepriFoxTextContract;


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

    public async void ExecuteTokenURI()
    {
        if (tokenURIIdInput.text == "")
        {
            Debug.LogWarning("token id not found");
            return;
        }

        tokenURITextUI.text = (await TokenURI(ownerOfTokenIdInput.text));
    }

    public async Task<string> TokenURI(string tokenId)
    {
        print("Getting TokenURI");
        string address = await lepriFoxTextContract.URI(LeprichainTestnet.name, LeprichainTestnet.network, tokenId, LeprichainTestnet.rpc);
        print("Received TokenURI NFT " + address);
        return address;
    }

    public async void ExecuteOwnerOf()
    {
        if (ownerOfTokenIdInput.text == "")
        {
            Debug.LogWarning("to address not found, you must input a valid erc20 address.");
            return;
        }

        ownerOfTextUI.text = (await OwnerOf(ownerOfTokenIdInput.text));
    }

    public async Task<string> OwnerOf(string tokenId)
    {
        print("Getting OwnerOf");
        string address = await lepriFoxTextContract.OwnerOf(LeprichainTestnet.name, LeprichainTestnet.network, tokenId, LeprichainTestnet.rpc);
        print("Received OwnerOf NFT " + address);
        return address;
    }

    public async void ExecuteBalance()
    {
        if (balanceOfAddressInput.text == "" || !balanceOfAddressInput.text.Contains("0x"))
        {
            Debug.LogWarning("to address not found, you must input a valid erc20 address.");
            return;
        }

        balanceTextUI.text = (await BalanceOf(balanceOfAddressInput.text)).ToString();
    }

    public async Task<BigInteger> BalanceOf(string address)
    {
        print("Getting Balance");
        BigInteger balanceOfFromWei = await lepriFoxTextContract.BalanceOf(LeprichainTestnet.name, LeprichainTestnet.network, address, LeprichainTestnet.rpc);
        print("Received Balance Number " + balanceOfFromWei);
        return balanceOfFromWei;
    }


    public async void ExecuteApprove()
    {
        // Send back to contract address for testing
        string toAddress = lepriFoxTextContract.Address;

        // Manual input
        if (approveToInput.text != "" && approveToInput.text.Contains("0x"))
        {
            toAddress = approveToInput.text;
        }
        else
        {
            Debug.LogWarning("to address not found, you must input a valid erc20 address.");
            return;
        }

        string tokenId = "";

        if (approveTokenIdInput.text != "")
        {
            tokenId = approveTokenIdInput.text;
            Debug.LogWarning("token id format error.");
            return;
        }

        await Approve(toAddress, tokenId);
    }

    public async Task<bool> Approve(string toAddress, string tokenId)
    {
        print("start approve");
        string transaction = await lepriFoxTextContract.Approve(
            toAddress,
            tokenId
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
        string toAddress = lepriFoxTextContract.Address;

        // Manual input
        if (transferToInput.text != "" && transferToInput.text.Contains("0x"))
        {
            toAddress = transferToInput.text;
        }
        else
        {
            Debug.LogWarning("to address not found, you must input a valid erc20 address.");
            return;
        }

        string tokenId = "";

        if (transferTokenIdInput.text != "")
        {
            tokenId = transferTokenIdInput.text;
            Debug.LogWarning("token id format error.");
            return;
        }

        await Transfer(toAddress, tokenId);
    }

    public async Task<bool> Transfer(string toAddress, string tokenId)
    {
        print("start transfer");
        string transaction = await lepriFoxTextContract.Transfer(
            lepriFoxTextContract.Address,
            tokenId
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
        string name = await ERC721.Name(LeprichainTestnet.name, LeprichainTestnet.network, lepriFoxTextContract.Address, LeprichainTestnet.rpc);
        nameTextUI.text = name;
    }

    public async void ExecuteGetTotalSupply()
    {
        BigInteger totalSupply = await ERC20.TotalSupply(LeprichainTestnet.name, LeprichainTestnet.network, lepriFoxTextContract.Address, LeprichainTestnet.rpc);
        print(totalSupply);
        totalSupplyTextUI.text = totalSupply.ToString();
    }

}
