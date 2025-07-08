using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Messanger : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] GameObject MessageBox;
    [SerializeField] TextMeshProUGUI Title;
    [SerializeField] TextMeshProUGUI MessageText;
    [SerializeField] Button[] Buttons;

    public static Messanger Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void Message_outbound(string title, string Message, string[] options, Action[] actions)
    {
        MessageBox.SetActive(true);
        Title.text = title;
        MessageText.text = Message;

        for (int i = 0; i < options.Length; i++)
        {
            Buttons[i].gameObject.SetActive(true);
            Buttons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = options[i];
            Buttons[i].onClick.AddListener(actions[i].Invoke);
        }
    }

    public void CloseMessageBox()
    {
        MessageBox.SetActive(false);
        Title.text = "";
        MessageText.text = "";

        foreach (var item in Buttons)
        {
            item.onClick.RemoveAllListeners();
            item.gameObject.SetActive(false);
        }
    }
}