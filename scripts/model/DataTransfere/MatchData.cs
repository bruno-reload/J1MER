using System;
using UnityEngine;
[Serializable]
class MatchData
{
    public int id;
    [SerializeField] public Vector3 position;
    public MatchData()
    {
    }
    public MatchData(int id, Vector3 position)
    {
        this.id = id;
        this.position = position;
    }
}