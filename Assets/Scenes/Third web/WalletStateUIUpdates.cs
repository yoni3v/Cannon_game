using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WalletStateUIUpdates : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] TextMeshProUGUI wallet_address;
    [SerializeField] Button CloseButton;
    [SerializeField] Button change_wallet;
    [SerializeField] GameObject walletOptions;
    [SerializeField] TMP_Dropdown Method;
    [SerializeField] Button Done;
    [SerializeField] GameObject email_window;
    [SerializeField] TMP_InputField email;
    [SerializeField] Button VerifyEmail;

    bool EventAdded = false;

    private void Awake()
    {
        change_wallet.onClick.AddListener(HandleDisconnectRequest);
        Done.onClick.AddListener(HandleConnectRequest);
        VerifyEmail.onClick.AddListener(ProcessEmailRequest);
    }

    private async void Start()
    {
        await UpdateUI();

        if (!EventAdded)
        {
            WalletConnector.Instance.OnWalletConnected += () => UpdateUI();
            EventAdded = true;
        }
    }

    public async Task UpdateUI()
    {
        if (WalletConnector.Instance.IsWalletConnected())
        {
            CloseButton.interactable = true;

            //The wallet is connect
            wallet_address.gameObject.SetActive(true);
            change_wallet.interactable = true;

            wallet_address.text = await WalletConnector.Instance.GetConnectedWallet().GetAddress();
        }
        else
        {
            //There is no wallet connection
            wallet_address.gameObject.SetActive(false);
            change_wallet.interactable = false;
        }
    }

    private void HandleConnectRequest()
    {
        int value = Method.value;       // value = 0 {email} and value = 2 {wallet}

        if (value == 0)
        {
            ShowEmailRequest();
        }
        else
        {
            WalletConnector.Instance.OnWalletConnectEvent();
        }
    }

    private void ShowEmailRequest()
    {
        email_window.SetActive(true);
    }

    private void ProcessEmailRequest()
    {
        string _email = email.text;

        if (_email.IsNullOrEmpty())
        {
            return;
        }

        email.text = "";
        email_window.SetActive(false);
        walletOptions.SetActive(false);
        WalletConnector.Instance.OnEmailConnectEvent(_email);
    }

    private async void HandleDisconnectRequest()
    {
        CloseButton.interactable = false;
        WalletConnector.Instance.DisconnectWallet();
        await UpdateUI();
        //show connect options
        walletOptions.SetActive(true);
    }

    private void OnDisable()
    {
        WalletConnector.Instance.OnWalletConnected -= () => UpdateUI();
    }
}