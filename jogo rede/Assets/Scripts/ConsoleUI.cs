using UnityEngine;
using TMPro;

public class ConsoleUI : MonoBehaviour
{
    public TextMeshProUGUI consoleText; // Arrastar o Text aqui
    private string logMessages = "";

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        logMessages += logString + "\n";
        consoleText.text = logMessages;
    }
}