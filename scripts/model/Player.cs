using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

[Serializable]
public class Player : MonoBehaviour
{
    public static int playerOwnerId;
    public bool listening;
    public PlayerData pData;

    protected StreamReader reader;
    protected StreamWriter writer;

    private TcpClient client;
    private Task current = null;
    private bool ready = false;
    public GameManager gm;
    public GameObject pontuation;

    public bool Started { get; set; }
    public int direction { get; set; }
    public int tcpClienteId { get; set; }
    public bool update { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        var value = Int32.Parse(pontuation.GetComponent<TextMeshProUGUI>().text);
        if (other.CompareTag("collectable"))
        {
            gm.AddCollectable(other.gameObject.GetComponent<Collectable>().cData);
            Int32.Parse(pontuation.GetComponent<TextMeshProUGUI>().text = (value + other.gameObject.GetComponent<Collectable>().cData.value).ToString());
        }
        if (other.CompareTag("obstacle"))
        {
            gm.AddObstacle(other.gameObject.GetComponent<Obstacle>().oData);
            Int32.Parse(pontuation.GetComponent<TextMeshProUGUI>().text = (value + other.gameObject.GetComponent<Obstacle>().oData.value).ToString());

        }
    }
    private void Start()
    {
        if (this.listening)
            playerOwnerId = GetInstanceID();
        tcpClienteId = -1;

    }
    public void Enter(TcpClient client)
    {
        Enter(client, new PlayerData());
    }
    public void Enter(TcpClient client, PlayerData pData)
    {
        this.client = client;

        NetworkStream stream = this.client.GetStream();

        reader = new StreamReader(stream);
        writer = new StreamWriter(stream);

        var data = reader.ReadLine();

        while (data == null)
        {
            data = reader.ReadLine();
        }
        tcpClienteId = Int32.Parse(data);

        this.pData = pData;

        ready = true;
    }
    private void Update()
    {
        if (ready)
        {
            pData.position = transform.position;
            pData.rotation = transform.rotation;

            string pd = JsonUtility.ToJson(pData);

            if (!listening)
            {
                playerOwnerId = tcpClienteId;

                if (update)
                {
                    writer.WriteLine(pd);
                    writer.Flush();
                }
            }
            else
            {
                try
                {
                    if (current == null)
                    {
                        current = reader.ReadLineAsync();
                    }
                    if (current.IsCompleted)
                    {
                        var list = (current as Task<string>).Result.Split('$');
                        if (Int32.Parse(list[0]) != playerOwnerId)
                        {
                            pData = JsonUtility.FromJson<PlayerData>(list[1]);

                            transform.position = pData.position;
                            transform.rotation = pData.rotation;
                        }
                        current = null;
                        reader.DiscardBufferedData();
                    }
                }
                catch (Exception e) {
                    Debug.Log(e);
                }
            }
        }
    }
    public void Speech(string msg) {
        writer.WriteLine(msg);
        writer.Flush();
    }
}



