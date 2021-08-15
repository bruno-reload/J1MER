using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Player : MonoBehaviour
{
    private TcpClient client;
    protected StreamReader reader;
    protected StreamWriter writer;

    public bool listening;

    public static int playerId;
    private Task current = null;
    private bool ready = false;
    public bool Started { get; set; }
    public int direction { get; set; }
    [HideInInspector]
    public int clienteId = -1;
    private PlayerData pData;

    private void Start()
    {
        if (this.listening)
            playerId = GetInstanceID();
        this.pData = new PlayerData();
        pData.position = Vector3.one;
        pData.rotation = Quaternion.identity;


    }
    public string login { get; set; }
    public string password { get; set; }
    [HideInInspector]
    public bool update;

    public void Enter(TcpClient client)
    {
        this.client = client;
        this.client.NoDelay = true;

        NetworkStream stream = this.client.GetStream();

        reader = new StreamReader(stream);
        writer = new StreamWriter(stream);

        var data = reader.ReadLine();

        while (data == null)
        {
            data = reader.ReadLine();
        }
        clienteId = Int32.Parse(data);

        ready = true;
    }
    private void Update()
    {
        pData.position = transform.position;
        pData.rotation = transform.rotation;

        string pd = JsonUtility.ToJson(pData);
        if (ready)
        {
            if (!listening)
            {
                playerId = clienteId;

                if (update)
                {
                    writer.WriteLine(pd);
                    writer.Flush();
                }
            }
            else
            {
                if (current == null)
                {
                    current = reader.ReadLineAsync();
                }
                if (current.IsCompleted)
                {
                    var list = (current as Task<string>).Result.Split('$');
                    if (Int32.Parse(list[0]) != playerId)
                    {
                        var temp = JsonUtility.FromJson<PlayerData>(list[1]);

                        transform.position = temp.position;
                        transform.rotation = temp.rotation;
                    }
                    current = null;
                }
            }
        }
    }
}
[Serializable]
public class PlayerData {
    public Vector3 position;
    public Quaternion rotation;
}


