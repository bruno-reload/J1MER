using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject collectable;
    public GameObject obstacle;

    private List<GameObject> collectables;
    private List<GameObject> obstacles;

    private ObstacleController oc;
    private CollectableController cc;
    private PositionController pc;
    private MatchController mc;

    private Match match;

    private List<TcpClient> clientCollectable;
    private List<TcpClient> clientObstacle;

    private List<CollectableData> collectablesData;
    private List<ObstacleData> obstaclesData;

    //esse valor foi escolhido pos no servidor o maximo de cada instancia é 3, caso eu mude aqui terei que mudar la tbm
    public const int MAX_ELEMENTS = 3;

    private string server = "127.0.0.1";
    private int portCollectable = 5001;
    private int portObstacle = 5002;

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
        match = new Match();
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
    public void CreateElements()
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

            for (int i = 0; i < MAX_ELEMENTS; i++)
            {
                collectables.Add(Instantiate(collectable));
                collectables[i].GetComponent<Collectable>().Enter(clientCollectable[i]);
            }
            for (int i = 0; i < MAX_ELEMENTS; i++)
            {
                obstacles.Add(Instantiate(obstacle));
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
    private void Update()
    {
        Read();
    }
    //nesse ponto devo colocar todos elementos coletaveis e colidiveis em suas posições
    public void PositionRandomDraw()
    {
        List<PositionData> position = pc.Spawner(GameManager.MAX_ELEMENTS * 2);
        int i = 0;
        while (i < GameManager.MAX_ELEMENTS)
        {
            collectables[i].GetComponent<Collectable>().cData.pData = position[i];
            collectables[i].transform.position = position[i].position;
            i++;
        }
        while (i < GameManager.MAX_ELEMENTS * 2)
        {
            obstacles[i % GameManager.MAX_ELEMENTS].GetComponent<Obstacle>().oData.positionData = position[i];
            obstacles[i % GameManager.MAX_ELEMENTS].transform.position = position[i].position;
            i++;
        }
        Write();
    }
    public void SaveElements(PlayerData p)
    {
        foreach (CollectableData collectableData in collectablesData)
        {
            cc.Save(p.matchId, collectableData);
            //cc.SavePosition(collectableData);
        }
        foreach (ObstacleData obstacleData in obstaclesData)
        {
            oc.Save(p.matchId, obstacleData);
            //oc.SavePosition(obstacleData);
        }
        mc.Change(p);
    }
}

