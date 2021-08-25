using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class CollectableData
{
    public int id;
    public string name;
    public int value;

    #region connect
    public bool ready;
    [SerializeField] public PositionData pData;
    #endregion

    public CollectableData()
    {
    }
    public CollectableData(int id, string name, int value)
    {
        this.id = id;
        this.name = name;
        this.value = value;
    }
}
