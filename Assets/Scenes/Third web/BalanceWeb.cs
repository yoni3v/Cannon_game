using UnityEngine;
using Thirdweb;
using UnityEngine.Events;
using TMPro;
using Thirdweb.Unity;
using System;
using UnityEngine.UI;

public class BalanceWeb : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI LocalBalanceText, WalletBalanceText;
    [SerializeField] Button ClaimBalance;

    ThirdwebContract contract;

    public static BalanceWeb Instance { get; private set; }
    public int WaleltBalance { get; private set; }
    public int LocalBalance { get; private set; }
    WalletConnector walletConnector;
    IThirdwebWallet wallet;
    const string ChainAddress = "0x15193CaE98F968fC410b837227279e71bB2f45EC";

    // events
    public UnityEvent OnBalanceUpdating;
    public UnityEvent<int> OnBalanceUpdated;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        walletConnector = WalletConnector.Instance;
        walletConnector.OnWalletConnected += HandleWalletConnect;

        OnBalanceUpdating.AddListener(() =>
        {
            LocalBalanceText.text = "processing..";
            WalletBalanceText.text = "processing..";
        });

        OnBalanceUpdated.AddListener(new_value =>
        {
            LocalBalanceText.text = LocalBalance.ToString() + " PNN";
            WalletBalanceText.text = new_value.ToString() + " PNN";
        });

        ClaimBalance.onClick.AddListener(() =>
        {
            if (LocalBalance <= 0)
            {
                Messanger.Instance.Message_outbound("Failed", "Not enough PNN to send" , new string[]{"OK"}, new Action[] { () => {
                    Messanger.Instance.CloseMessageBox();
                } });

                Debug.Log("Not enough balance to claim");
                return;
            }

            ClaimAmount();
            Debug.Log("Claiming Amount");
        });
    }

    private void HandleWalletConnect()
    {
        wallet = walletConnector.GetConnectedWallet();

        if (wallet == null)
        {
            Debug.LogError("There was no wallet reference");
        }
        else
        {
            UpdateWalletBalance();
        }
    }

    private async void UpdateWalletBalance()
    {
        OnBalanceUpdating?.Invoke();
        WaleltBalance = (int) await wallet.GetBalance(56);
        OnBalanceUpdated?.Invoke(WaleltBalance);
    }

    private void UpdateLocalBalance()
    {
        LocalBalanceText.text = LocalBalance.ToString() + " PNN";
    }

    public void AppendBalance(int amount)
    {
        LocalBalance += amount;
        UpdateLocalBalance();
    }

    public void ClaimAmount()
    {
        Time.timeScale = 0;

        Messanger.Instance.Message_outbound("Claim amount",
            $"Are you sure you want to claim {LocalBalance} PNN? it will require some BNB in your wallet prior the transaction in order to complete the transaction",
            new string[] {"Confirm", "Not right now"},
            new Action[] {() =>
            {
                //if the user confirms
                ProcessTransaction();
                Messanger.Instance.CloseMessageBox();
            }, () => { 
                //If the user doesn't allows
                Messanger.Instance.CloseMessageBox();
                Time.timeScale = 1;
            } });
    }

    private async void ProcessTransaction()
    {
        try
        {
            var wallet = ThirdwebManager.Instance.GetActiveWallet();

            if (wallet != null)
            {
                if (contract == null)
                {
                    contract = await ThirdwebManager.Instance.GetContract(ChainAddress, 56);
                }

                await contract.DropERC20_Claim(wallet, await wallet.GetAddress(), LocalBalance.ToString());
                Debug.Log($"Amound added to user amount : {LocalBalance.ToString()}");
                LocalBalance = 0;
            }
            else
            {
                Debug.Log("wallet null");
            }
        }
        catch (Exception e)
        {
            if (e.Message.Contains("gas"))
            {
                Messanger.Instance.Message_outbound("Failed to claim PNN",
                    "Not enough funds in your wallet to pay for the gas fee\n" +
                    "Consider adding some BNB in your wallet", new string[] { "I understand" },
                    new Action[] { () => {
                    Messanger.Instance.CloseMessageBox();
                } });
            }
            else
            {
                Messanger.Instance.Message_outbound("Failed to claim PNN",
                    e.Message, new string[] { "I understand" },
                    new Action[] { () => {
                    Messanger.Instance.CloseMessageBox();
                } });
            }

            Debug.Log(e.ToString());
        }

        Time.timeScale = 1;

        UpdateWalletBalance();
        UpdateLocalBalance();
    }

    private void OnDestroy()
    {
        walletConnector.OnWalletConnected -= HandleWalletConnect;
    }
}