using System;
using System.IO;
using UnityEngine;

public class LogTest : MonoBehaviour
{
    // Start is called before the first frame update
    public string LogPath;
    private void Awake()
    {
        LogPath = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf("/"));
    }
    void Start()
    {
        Application.logMessageReceived += LogCallback;
        try
        {
            GameObject n = null;
            n.transform.position = Vector3.zero;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
    void LogCallback(string condition, string stackTrace, LogType type)
    {
        if (type == LogType.Error || type == LogType.Exception || type == LogType.Assert)
        {
            string logPath = LogPath + "\\" + DateTime.Now.ToString("yyyy_MM_dd HH_mm_ss") + ".log";
            //¥Ú”°»’÷æ
            if (Directory.Exists(LogPath))
            {
                File.AppendAllText(logPath, "[time]:" + DateTime.Now.ToString() + "\r\n");
                File.AppendAllText(logPath, "[type]:" + type.ToString() + "\r\n");
                File.AppendAllText(logPath, "[exception message]:" + condition + "\r\n");
                File.AppendAllText(logPath, "[stack trace]:" + stackTrace + "\r\n");
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
