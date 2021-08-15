using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;

public class Player : MonoBehaviour
{
    private TcpClient client;
    protected StreamReader reader;
    protected StreamWriter writer;
    private float speed = 3;
    public bool listening;

    public static int playerId;
    private Task current = null;
    public int id { get; set; }

    private void Start()
    {
        if (this.listening)
            playerId = GetInstanceID();
    }
    public string login { get; set; }
    public string password { get; set; }

    public void Enter(TcpClient client)
    {
        this.client = client;
        NetworkStream stream = this.client.GetStream();

        reader = new StreamReader(stream);
        writer = new StreamWriter(stream);
    }
    private void Update()
    {
        if (writer != null || writer != null)
        {
            if (!listening)
            {
                if (Input.GetKey(KeyCode.A))
                {
                    transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
                }
                if (Input.GetKey(KeyCode.D))
                {
                    transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
                }
                writer.WriteLineAsync(GetInstanceID() + "," + transform.position.x + ",");
                writer.Flush();
            }
            else
            {
                if (current == null)
                {
                    current = reader.ReadLineAsync();
                }
                if (current.IsCompleted)
                {
                    var list = (current as Task<string>).Result.Split(',');
                    Debug.Log("adversary:" + GetInstanceID() + "  playerid:" + playerId);
                    float x = float.Parse(list[1]);
                    transform.position = new Vector3(x, transform.position.y, transform.position.z);
                    current = null;
                }
            }
        }
    }
}


