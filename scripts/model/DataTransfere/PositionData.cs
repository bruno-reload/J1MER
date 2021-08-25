using System;
using UnityEngine;

[Serializable]
public class PositionData
{
    public int id;
    [SerializeField] public Vector3 position;

    public PositionData(int id, Vector3 position)
    {
        this.id = id;
        this.position = position;
    }
}
