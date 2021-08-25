using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public CollectableData cData;
    private bool ready;
    private TcpClient client;
    private StreamReader reader;
    private StreamWriter writer;
    private int tcpClienteId;
    private bool listening;
    private bool update;
    private Task current = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            cData.ready = false;
            writer.WriteLine(JsonUtility.ToJson(cData));
            writer.Flush();
        }
    }
    public void Enter(TcpClient tcpClient)
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

    public void Load(CollectableData collectableData)
    {
        this.cData = collectableData;
    }

    private void Update()
    {
        //if (current == null)
        //{
        //    current = reader.ReadLineAsync();
        //}
        //if (current.IsCompleted)
        //{
        //    var list = (current as Task<string>).Result.Split('$');
        //    if (tcpClienteId == Int32.Parse(list[0]))
        //    {
        //        if (list[1].Equals("true"))
        //            gameObject.SetActive(true);
        //    }
        //    current = null;
        //}
    }

    public void Speech()
    {
        cData.ready = true;
        writer.WriteLine(JsonUtility.ToJson(cData));
        writer.Flush();
    }

    public void Listening()
    {
        if (current == null)
        {
            current = reader.ReadLineAsync();
        }
        if (current.IsCompleted)
        {
            Debug.Log(current);
            var result = (current as Task<string>).Result.Split('$');
            if (Int32.Parse(result[0]) % GameManager.MAX_ELEMENTS == (tcpClienteId + GameManager.MAX_ELEMENTS) % GameManager.MAX_ELEMENTS)
            {
                cData = JsonUtility.FromJson<CollectableData>(result[1]);
                gameObject.SetActive(cData.ready);
                Debug.Log(cData.pData.position);
                transform.position = cData.pData.position;
            }
            current = null;
        }
    }
}
