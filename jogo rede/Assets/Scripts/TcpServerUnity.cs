using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class TcpServerUnity : MonoBehaviour
{
    TcpListener server;
    Thread serverThread;
    public TMP_Text texto;
    private string msg = "";
    void Start()
    {
        serverThread = new Thread(new ThreadStart(StartServer));
        serverThread.IsBackground = true;
        serverThread.Start();
    }

    void StartServer()
    {
        server = new TcpListener(IPAddress.Any, 8080);
        server.Start();
        Debug.Log("Servidor ouvindo na porta 8080...");

        while (true)
        {
            TcpClient client = server.AcceptTcpClient();
            NetworkStream stream = client.GetStream();

            byte[] buffer = new byte[1024];
            int len = stream.Read(buffer, 0, buffer.Length);
            msg = Encoding.UTF8.GetString(buffer, 0, len);

        
            
            Debug.Log($"[Servidor] Mensagem recebida: {msg}");

            stream.Close();
            client.Close();    
            
           
        }
        
    }
    
    void Update()
    {
        if (msg != "")
        {
            texto.text = msg;
            msg = "";
        }
    }

    void OnApplicationQuit()
    {
        server?.Stop();
        serverThread?.Abort();
    }
}
