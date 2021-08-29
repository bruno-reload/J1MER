using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [HideInInspector]
    public CellData cData = null;
    public GameObject position;
    public new GameObject name;
    public GameObject pontuation;
    private int match_id;
    private PlayerController pc;

    void Start()
    {
        pc = new PlayerController();
    }
    private void OnEnable()
    {
        Clear();
    }
    void Update()
    {
        if (cData != null)
        {
            position.GetComponent<TextMeshProUGUI>().text = cData.position.ToString();
            name.GetComponent<TextMeshProUGUI>().text = cData.login;
            pontuation.GetComponent<TextMeshProUGUI>().text = cData.pontuation.ToString();
            this.match_id = cData.match_id;
            cData = null;
        }
    }

    internal void Clear()
    {
        position.GetComponent<TextMeshProUGUI>().text = "0";
        name.GetComponent<TextMeshProUGUI>().text = "";
        pontuation.GetComponent<TextMeshProUGUI>().text = "0";
        match_id = 0;
    }
    public void DeletCell()
    {
        pc.DeleteElementInList(this.match_id);
        Clear();
    }
}

