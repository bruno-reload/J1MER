using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ObstacleData
{
    public int id;
    public string name;
    public int value;

    #region connect
    public bool ready;
    [SerializeField] public PositionData pData;
    #endregion
    public ObstacleData()
    {
    }
    public ObstacleData(int id, string name, int value)
    {
        this.id = id;
        this.name = name;
        this.value = value;
    }
}
