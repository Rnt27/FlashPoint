using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Net;
using System.IO;

public class Server : MonoBehaviour
{
    public int port = 6321;

    private List<ServerClient> clients;
    private List<ServerClient> disconnectList;

    private TcpListener server;
    private bool serverStarted;

    //Not Start() because its called manually
    public void Init()
    {
        //Dont destroy server when changing scenes. 
        //Logic: Host makes Server, waits for another Player, then starts the game WITHOUT destroying Server
        DontDestroyOnLoad(gameObject);
        clients = new List<ServerClient>();
        disconnectList = new List<ServerClient>();

       
        try //try to start server
        {
            //Listen for any connections to 6321 port
            server = new TcpListener(IPAddress.Any, port); 
            server.Start();

            StartListening();
        }
        catch (Exception e)
        {
            Debug.Log("Socker error: " + e.Message);
        }
    }
    private void Update()
    {
        if (!serverStarted)
            return;

        foreach (ServerClient c in clients)
        {
            //Client is disconnected
            if (!IsConnected(c.tcp))
            {
                c.tcp.Close();
                disconnectList.Add(c);
                continue;
            }
            else //client IS connected
            {
                NetworkStream s = c.tcp.GetStream();
                if (s.DataAvailable)
                {
                    StreamReader reader = new StreamReader(s, true);
                    string data = reader.ReadLine();

                    if (data != null)
                        OnIncomingData(c, data);
                }
            }
        }

        for (int i = 0; i < disconnectList.Count - 1; i++)
        {
            //TODO : Tell our player somebody has disconnected

            clients.Remove(disconnectList[i]);
            disconnectList.RemoveAt(i);
        }
    }

    //Ascertains what to do when there are incoming connections
    private void StartListening()
    {
        //Handshake
        server.BeginAcceptTcpClient(AcceptTcpClient, server); 
    }
    //AcceptTcpClient: acertains what to do after accepting the client
    private void AcceptTcpClient(IAsyncResult ar)
    {
        TcpListener listener = (TcpListener)ar.AsyncState;

        //create definition for that person and add to list of clients
        ServerClient sc = new ServerClient(listener.EndAcceptTcpClient(ar));
        clients.Add(sc);

        StartListening();//Repeat doing this as Server "forget" to continually listen to client

        //DEBUGGING PURPOSES. REMOVE LATER.
        Debug.Log("Somebody has connected!");
    }

    private bool IsConnected(TcpClient c)
    {
        try
        {
            if (c != null && c.Client != null & c.Client.Connected)
            {
                if (c.Client.Poll(0, SelectMode.SelectRead))
                {
                    return !(c.Client.Receive(new byte[1], SocketFlags.Peek) == 0);
                }
                return true;
            }
            else //input c is null or c.Client is null or c.Client is not connected
            {
                return false;
            }
        }
        catch //just disconnect it
        {
            return false;
        }
    }

    //Server Send
    private void Broadcast(string data, List<ServerClient> cl)
    {
        foreach (ServerClient sc in cl)
        {
            try
            {
                //grab clients stream
                StreamWriter writer = new StreamWriter(sc.tcp.GetStream());
                writer.WriteLine(data);
                writer.Flush();
            }
            catch(Exception e)
            {
                Debug.Log("Write error: " + e.Message);
            }
            
        }
    }
    //Server Read
    private void OnIncomingData(ServerClient c, string data)
    {
        Debug.Log(c.clientName + " : " + data);
    }
}

public class ServerClient
{
    public string clientName;
    public TcpClient tcp;

    public ServerClient(TcpClient tcp)
    {
        this.tcp = tcp;
    }
}
