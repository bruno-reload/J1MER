using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player p1 { set; private get; }
    public Player p2 { set; private get; }

    public GameObject collectable;
    public GameObject obstacle;

    private List<GameObject> collectables;
    private List<GameObject> obstacles;

    private ObstacleController oc;
    private CollectableController cc;
    private PositionController pc;
    private MatchController mc;

    private List<TcpClient> clientCollectable;
    private List<TcpClient> clientObstacle;

    private List<CollectableData> collectablesData;
    private List<ObstacleData> obstaclesData;

    //esse valor foi escolhido pos no servidor o maximo de cada instancia é 3, caso eu mude aqui terei que mudar la tbm
    public const int MAX_ELEMENTS = 6;

    private string server = "127.0.0.1";
    private int portCollectable = 5001;
    private int portObstacle = 5002;

    private void Start()
    {
        collectables = new List<GameObject>();
        obstacles = new List<GameObject>();

        collectablesData = new List<CollectableData>();
        obstaclesData = new List<ObstacleData>();

        oc = new ObstacleController();
        cc = new CollectableController();
        pc = new PositionController();
        mc = new MatchController();

        clientCollectable = new List<TcpClient>();
        clientObstacle = new List<TcpClient>();
    }
    private void Update()
    {
        Read();
    }
    public void ActiveGroup()
    {
        try
        {
            foreach (var e in collectables)
            {
                e.gameObject.SetActive(true);
            }
            foreach (var e in obstacles)
            {
                e.gameObject.SetActive(true);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
    public void AddCollectable(CollectableData collectable)
    {
        Debug.Log("+1 collectable");
        collectablesData.Add(collectable);
    }
    public void AddObstacle(ObstacleData obstacle)
    {
        Debug.Log("+1 obstacle");
        obstaclesData.Add(obstacle);
    }
    public void LoadEllements()
    {
        try
        {
            List<ObstacleData> l_oc = oc.RandomDraw(MAX_ELEMENTS);
            List<CollectableData> l_cc = cc.RandomDraw(MAX_ELEMENTS);

            for (int i = 0; i < MAX_ELEMENTS; i++)
            {
                collectables[i].GetComponent<Collectable>().Load(l_cc[i]);
            }
            for (int i = 0; i < MAX_ELEMENTS; i++)
            {
                obstacles[i].GetComponent<Obstacle>().Load(l_oc[i]);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
    public void CreateConnections()
    {
        try
        {
            while (clientCollectable.Count < MAX_ELEMENTS)
            {
                clientCollectable.Add(new TcpClient(server, portCollectable));
            }
            while (clientObstacle.Count < MAX_ELEMENTS)
            {
                clientObstacle.Add(new TcpClient(server, portObstacle));
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
    public void CreateElements()
    {
        try
        {
            for (int i = 0; i < MAX_ELEMENTS; i++)
            {
                collectables.Add(Instantiate(collectable));
            }
            for (int i = 0; i < MAX_ELEMENTS; i++)
            {
                obstacles.Add(Instantiate(obstacle));
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
    public void DefineElements()
    {
        try
        {
            for (int i = 0; i < MAX_ELEMENTS; i++)
            {
                collectables[i].GetComponent<Collectable>().Enter(clientCollectable[i]);
            }
            for (int i = 0; i < MAX_ELEMENTS; i++)
            {
                obstacles[i].GetComponent<Obstacle>().Enter(clientObstacle[i]);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
    public void Read()
    {
        foreach (GameObject c in collectables)
        {
            c.GetComponent<Collectable>().Listening();
        }
        foreach (GameObject c in obstacles)
        {
            c.GetComponent<Obstacle>().Listening();
        }
    }
    public void Write()
    {
        foreach (GameObject c in collectables)
        {
            c.GetComponent<Collectable>().Speech();
        }
        foreach (GameObject c in obstacles)
        {
            c.GetComponent<Obstacle>().Speech();
        }
    }
    public void PositionRandomDraw()
    {
        List<PositionData> position = pc.Spawner(GameManager.MAX_ELEMENTS * 2);
        int i = 0;
        while (i < GameManager.MAX_ELEMENTS)
        {
            if (i < position.Count)
            {
                collectables[i].GetComponent<Collectable>().cData.pData = position[i];
                collectables[i].transform.position = position[i].position;
            }
            else
            {
                Destroy(collectables[i]);
            }
            i++;
        }
        while (i < GameManager.MAX_ELEMENTS * 2)
        {
            if (i < position.Count)
            {
                obstacles[i % GameManager.MAX_ELEMENTS].GetComponent<Obstacle>().oData.pData = position[i];
                obstacles[i % GameManager.MAX_ELEMENTS].transform.position = position[i].position;
            }
            else
            {
                Destroy(obstacles[i % GameManager.MAX_ELEMENTS]);
            }
            i++;
        }
        Write();
    }
    public void SaveElements(PlayerData p)
    {
        foreach (CollectableData collectableData in collectablesData)
        {
            Debug.Log(collectablesData.Count);
            cc.Save(p.matchId, collectableData);
            cc.SavePosition(collectableData);
        }
        foreach (ObstacleData obstacleData in obstaclesData)
        {
            Debug.Log(obstaclesData.Count);
            oc.Save(p.matchId, obstacleData);
            oc.SavePosition(obstacleData);
        }
        mc.Change(p);
    }
}

