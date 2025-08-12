using System.Net.Sockets;
using System.Text;
using UnityEngine;
using TMPro; // ← Isso é importante!
using UnityEngine.UI;

public class TcpClientUnity : MonoBehaviour
{
    public TMP_InputField input;
    public Button sendButton;


    void Start()
    {
        sendButton.onClick.AddListener(SendMessageToServer);
    }

    public void SendMessageToServer()
    {
        string mensagem = input.text;
        if (string.IsNullOrWhiteSpace(mensagem)) return;

        Debug.Log(mensagem);
        
        TcpClient client = new TcpClient("10.57.10.22", 8080);
        NetworkStream stream = client.GetStream();
        byte[] data = Encoding.UTF8.GetBytes(mensagem);
        stream.Write(data, 0, data.Length);
        
        Debug.Log(stream.CanWrite);
        
        stream.Close();
        client.Close();
    }
}