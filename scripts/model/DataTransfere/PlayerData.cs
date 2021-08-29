using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class PlayerData
{
    public int id;
    public string login;
    public string password;

    #region connect
    public int matchId;
    public Vector3 position;
    public Quaternion rotation;
    public int matchPosition = 0;
    public int countStart = 0;
    public int countEnd = 0;
    public bool endGame = false;
    public bool startGame = false;
    #endregion
    public PlayerData()
    {
    }
    public PlayerData(string login, string password)
    {
        this.login = login;
        this.password = password;
    }
}