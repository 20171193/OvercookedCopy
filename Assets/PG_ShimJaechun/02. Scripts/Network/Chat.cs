using TMPro;
using UnityEngine;

public class Chat : MonoBehaviour
{
    [SerializeField] Client client;

    [SerializeField] TMP_InputField inputField;
    [SerializeField] RectTransform content;

    [SerializeField] TMP_Text chatTextPrefab;

    private void Awake()
    {
        inputField.onSubmit.AddListener(SendChat);
    }

    public void AddMessage(string message)
    {
        TMP_Text newMessage = Instantiate(chatTextPrefab, content);
        newMessage.text = message;
    }

    public void SendChat(string chat)
    {
        if (client != null)
        {
            client.SendChat(chat);
        }

        inputField.text = "";
        inputField.ActivateInputField();
    }
}
