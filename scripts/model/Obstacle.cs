using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public ObstacleData oData;
    private TcpClient client;
    private StreamReader reader;
    private StreamWriter writer;
    private int tcpClienteId;
    private bool ready;
    private Task current = null;

    internal void Enter(TcpClient tcpClient)
    {
        this.client = tcpClient;

        NetworkStream stream = this.client.GetStream();

        reader = new StreamReader(stream);
        writer = new StreamWriter(stream);

        var data = reader.ReadLine();

        while (data == null)
        {
            data = reader.ReadLine();
        }
        tcpClienteId = Int32.Parse(data);

        ready = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            oData.ready = false;
            writer.WriteLine(JsonUtility.ToJson(oData));
            writer.Flush();
        }
    }

    internal void Load(ObstacleData obstacleData)
    {
        this.oData = obstacleData;
    }


    internal void Speech()
    {
        oData.ready = true;
        writer.WriteLine(JsonUtility.ToJson(oData));
        writer.Flush();
    }

    internal void Listening()
    {
        if (current == null)
        {
            current = reader.ReadLineAsync();
        }
        if (current.IsCompleted)
        {
            var result = (current as Task<string>).Result.Split('$');
            if (Int32.Parse(result[0]) % GameManager.MAX_ELEMENTS == (tcpClienteId + GameManager.MAX_ELEMENTS) % GameManager.MAX_ELEMENTS)
            {
                oData = JsonUtility.FromJson<ObstacleData>(result[1]);
                gameObject.SetActive(oData.ready);
                transform.position = oData.pData.position;
            }
            current = null;
        }
    }
}
