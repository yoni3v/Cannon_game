using Microsoft.IdentityModel.Tokens;
using System;
using Thirdweb;
using Thirdweb.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WalletConnector : MonoBehaviour
{
    public static WalletConnector Instance;

    //events
    public UnityEvent OnWalletConnectedSucessfully;
    public UnityEvent OnWalletConnecting;
    public UnityEvent OnWalletConnectionFailed;


    [SerializeField] private GameObject connectWalletPanel;
    [SerializeField] private Button connectWalletButton, connectviaEmailButton, play_as_guest;
    [SerializeField] private TMP_InputField email;
    [SerializeField] private TextMeshProUGUI disclaimer_text;

    [Header("Wallet Settings")]
    [SerializeField] private ulong chainId = 56; // BSC Mainnet
    [SerializeField] private bool webglForceMetamaskExtension = false;

    public event Action OnWalletConnected;

    private IThirdwebWallet _connectedWallet;

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
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            disclaimer_text.gameObject.SetActive(true);
            connectWalletButton.interactable = false;
            connectviaEmailButton.interactable = false;
        }
        else
        {
            disclaimer_text.gameObject.SetActive(false);
            connectWalletButton.interactable = true;
            connectviaEmailButton.interactable = true;
        }

        if (play_as_guest != null)
        {
            play_as_guest.onClick.AddListener(PlayAsGuest);
        }
        else
        {
            Debug.LogError("Play as Guest Button is not assigned!");
        }

        if (connectWalletButton != null)
        {
            connectWalletButton.onClick.RemoveAllListeners();
            connectWalletButton.onClick.AddListener(OnWalletConnectEvent);
        }
        else
        {
            Debug.LogError("Connect Wallet Button is not assigned!");
        }

        if (connectviaEmailButton != null)
        {
            connectviaEmailButton.onClick.RemoveAllListeners();
            connectviaEmailButton.onClick.AddListener(OnEmailConnectEvent);
        }
        else
        {
            Debug.LogError("Connect email Button is not assigned!");
        }

        // Ensure the connect panel is visible initially
        if (connectWalletPanel != null)
        {
            connectWalletPanel.SetActive(true);
        }
    }

    bool walletAllocated = false;
    private void Update()
    {
        if (!walletAllocated)
        {
            if (ThirdwebManager.Instance.GetActiveWallet() != null)
            {
                _connectedWallet = ThirdwebManager.Instance.GetActiveWallet();
                OnWalletConnectedSucessfully?.Invoke();
                OnWalletConnected?.Invoke();
                connectWalletPanel.SetActive(false);
                walletAllocated = true;
            }
        }
    }

    public async void OnEmailConnectEvent()
    {
        string email_address = email.text;

        if (email_address.IsNullOrEmpty())
        {
            OnWalletConnectionFailed?.Invoke();
            return;
        }

        //proper events
        OnWalletConnecting?.Invoke();

        try
        {
            Debug.Log("Attempting to connect email...");

            // Create wallet options
            var walletOptions = GetEmailConnectOptions(email_address);

            // Connect the wallet
            _connectedWallet = await ThirdwebManager.Instance.ConnectWallet(walletOptions);

            if (_connectedWallet == null)
            {
                OnWalletConnectionFailed?.Invoke();
                Debug.LogError("Failed to connect wallet - result is null");
                return;
            }

            Debug.Log("Wallet connected successfully!");
            OnWalletConnected?.Invoke();
            OnWalletConnectedSucessfully?.Invoke();

            // Get wallet address
            var address = await _connectedWallet.GetAddress();
            Debug.Log($"Wallet address: {address}");

            // Hide the connect panel
            if (connectWalletPanel != null)
            {
                connectWalletPanel.SetActive(false);
            }

            connectWalletButton.interactable = false;

        }
        catch (Exception e)
        {
            OnWalletConnectionFailed?.Invoke();
            Debug.LogError($"Error connecting wallet: {e.Message}");
        }
    }

    public async void OnEmailConnectEvent(string email_address)
    {
        if (email_address.IsNullOrEmpty())
        {
            OnWalletConnectionFailed?.Invoke();
            return;
        }

        //proper events
        OnWalletConnecting?.Invoke();

        try
        {
            Debug.Log("Attempting to connect email...");

            // Create wallet options
            var walletOptions = GetEmailConnectOptions(email_address);

            // Connect the wallet
            _connectedWallet = await ThirdwebManager.Instance.ConnectWallet(walletOptions);

            if (_connectedWallet == null)
            {
                OnWalletConnectionFailed?.Invoke();
                Debug.LogError("Failed to connect wallet - result is null");
                return;
            }

            Debug.Log("Wallet connected successfully!");
            OnWalletConnected?.Invoke();
            OnWalletConnectedSucessfully?.Invoke();

            // Get wallet address
            var address = await _connectedWallet.GetAddress();
            Debug.Log($"Wallet address: {address}");

            // Hide the connect panel
            if (connectWalletPanel != null)
            {
                connectWalletPanel.SetActive(false);
            }

            connectWalletButton.interactable = false;

        }
        catch (Exception e)
        {
            OnWalletConnectionFailed?.Invoke();
            Debug.LogError($"Error connecting wallet: {e.Message}");
        }
    }

    private void PlayAsGuest()
    {
        connectWalletPanel.SetActive(false);
        _connectedWallet = null;
        Debug.Log("Playing as guest");
    }

    public async void OnWalletConnectEvent()
    {
        //proper events
        OnWalletConnecting?.Invoke();

        try
        {
            Debug.Log("Attempting to connect wallet...");

            // Create wallet options
            var walletOptions = GetWalletConnectOptions();

            try
            {
                // Connect the wallet
                _connectedWallet = await ThirdwebManager.Instance.ConnectWallet(walletOptions);
            }
            catch (Exception e)
            {
                OnWalletConnectionFailed?.Invoke();
                Debug.Log(e);
            }

            if (_connectedWallet == null)
            {
                OnWalletConnectionFailed?.Invoke();
                Debug.LogError("Failed to connect wallet - result is null");
                return;
            }

            Debug.Log("Wallet connected successfully!");
            OnWalletConnected?.Invoke();
            OnWalletConnectedSucessfully?.Invoke();

            // Get wallet address
            var address = await _connectedWallet.GetAddress();
            Debug.Log($"Wallet address: {address}");

            // Hide the connect panel
            if (connectWalletPanel != null)
            {
                connectWalletPanel.SetActive(false);
            }

            connectWalletButton.interactable = false;

        }
        catch (Exception e)
        {
            OnWalletConnectionFailed?.Invoke();
            Debug.LogError($"Error connecting wallet: {e.Message}");
        }
    }

    private WalletOptions GetWalletConnectOptions()
    {
        // Handle WebGL platform specifics
        var provider = WalletProvider.WalletConnectWallet;

        if (Application.platform == RuntimePlatform.WebGLPlayer && webglForceMetamaskExtension)
        {
            provider = WalletProvider.MetaMaskWallet;
        }

        return new WalletOptions(
            provider: provider,
            chainId: chainId
        );
    }

    private WalletOptions GetEmailConnectOptions(string email)
    {
        // Handle WebGL platform specifics
        var provider = WalletProvider.InAppWallet;

        if (Application.platform == RuntimePlatform.WebGLPlayer && webglForceMetamaskExtension)
        {
            provider = WalletProvider.MetaMaskWallet;
        }

        return new WalletOptions(
            provider: provider,
            chainId: chainId,
            inAppWalletOptions: new InAppWalletOptions(email)
        );
    }

    // Optional: Add disconnect functionality
    public void DisconnectWallet()
    {
        try
        {
            if (_connectedWallet != null)
            {
                string address = GetConnectedWallet().GetAddress().ToString();
                ThirdwebManager.Instance.RemoveWallet(address);
                _connectedWallet = null;

                // Show connect panel again
                if (connectWalletPanel != null)
                {
                    connectWalletPanel.SetActive(true);
                }

                connectWalletButton.interactable = true;

                Debug.Log("Wallet disconnected");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error disconnecting wallet: {e.Message}");
        }
    }

    // Optional: Method to check if wallet is connected
    public bool IsWalletConnected()
    {
        return _connectedWallet != null;
    }

    // Optional: Get connected wallet instance
    public IThirdwebWallet GetConnectedWallet()
    {
        return _connectedWallet;
    }

    private void OnDestroy()
    {
        OnWalletConnected = null;
    }
}