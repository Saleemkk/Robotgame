using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Reflection;


public class UnityHTTPServer : MonoBehaviour
{
    [SerializeField]
    public int port;
    [SerializeField]
    public string SaveFolder;
    [SerializeField]

    public int bufferSize = 16;
    public static UnityHTTPServer Instance;

    public MonoBehaviour controller;
    SimpleHTTPServer myServer;
    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        if (myServer == null)
        {
            Init();
        }
    }
    void Init()
    {
        StartServer();
    }

    public void StartServer()
    {
        myServer = new SimpleHTTPServer(Application.persistentDataPath, port, controller, bufferSize);
        myServer.OnJsonSerialized += (result) =>
        {
#if UseLitJson
            return LitJson.JsonMapper.ToJson(result);
#else
            return JsonUtility.ToJson(result);
#endif
        };
    }
    public static string GetHttpUrl()
    {
        return $"http://{GetLocalIPAddress()}:" + Instance.myServer.Port + "/";
    }

    /// <summary>
    /// Get the Host IPv4 adress
    /// </summary>
    /// <returns>IPv4 address</returns>
    public static string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new Exception("No network adapters with an IPv4 address in the system!");
    }
    public void StopServer()
    {
        Application.Quit();
    }

    void OnApplicationQuit()
    {
        myServer.Stop();
    }

}
