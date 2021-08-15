using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data.SqlClient;
using System;

public class DAO : MonoBehaviour
{
    protected SqlCommand command;
    protected SqlDataReader reader;
    protected SqlParameter parameter;

    public SqlConnection Connection()
    {
        string strConnection = "conexão com o banco de dados ainda não definido";
        try
        {
            Debug.LogWarning(strConnection);

            SqlConnection connection = new SqlConnection(strConnection);
            connection.Open();

            return connection;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: {0}", e) ;
        }
        return null;
    }
}
