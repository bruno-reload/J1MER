using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranking : MonoBehaviour
{
    public GameObject cell;
    public GameObject container;
    private PlayerController pc;
    private const int topFive = 5;
    private List<GameObject> list = new List<GameObject>();


    private void OnEnable()
    {
        pc = new PlayerController();
        int position = 0;
     
        foreach (CellData go in pc.Ranking(topFive))
        {
            go.position = position + 1;
            list[position].GetComponent<Cell>().cData = go;
            position++;
        }
        GetComponentInChildren<RectTransform>().ForceUpdateRectTransforms();
    }
    private void Awake()
    {
        while (list.Count <= topFive)
        {
            GameObject instace = Instantiate(cell);
            instace.GetComponent<Cell>().Clear();
            list.Add(instace);
            list[list.Count - 1].transform.SetParent(container.transform);
        }
        gameObject.SetActive(false);
    }
}
