using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data.SqlClient;
using System;
using System.Data;

public class DAO 
{
    protected SqlCommand command;
    protected SqlDataReader reader;
    protected SqlParameter parameter;

    public SqlConnection Connection()
    {
        const string strConnection = "Data Source=(local); Initial Catalog=APNP; Persist Security Info=true; User ID=Admin; Password=123123";
        try

        {
            SqlConnection connection = new SqlConnection(strConnection);
            connection.Open();
            if (connection.State == ConnectionState.Open)
            {
                Debug.Log("conectado");
            }

            return connection;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: {0}", e);
        }
        return null;
    }
}
