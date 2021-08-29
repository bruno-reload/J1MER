using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using UnityEngine;

public class CollectableController : DAO
{
    public void Save(int mathID, CollectableData p)
    {
        String sql = "INSERT INTO COLLECT (MC, CC) VALUES (@MC, @CC);";

        command = new SqlCommand(sql, Connection());

        parameter = new SqlParameter("@MC", mathID);
        parameter.SqlDbType = System.Data.SqlDbType.Int;
        command.Parameters.Add(parameter);

        parameter = new SqlParameter("@CC", p.id);
        parameter.SqlDbType = System.Data.SqlDbType.Int;
        command.Parameters.Add(parameter);

        command.ExecuteNonQuery();
    }
    public void Change(CollectableData p)
    {
        String sql = "UPDATE COLLECTABLE SET NAME = @NAME, VALUE = @VALUE WHERE ID = @ID;";

        command = new SqlCommand(sql, Connection());

        parameter = new SqlParameter("@ID", p.id);
        parameter.SqlDbType = System.Data.SqlDbType.Int;
        command.Parameters.Add(parameter);

        parameter = new SqlParameter("@NAME", p.name);
        parameter.SqlDbType = System.Data.SqlDbType.VarChar;
        command.Parameters.Add(parameter);

        parameter = new SqlParameter("@VALUE", p.value);
        parameter.SqlDbType = System.Data.SqlDbType.Int;
        command.Parameters.Add(parameter);

        command.ExecuteNonQuery();
    }
    public void Delete(int Id)
    {
        String sql = "DELETE FROM COLLECTABLE WHERE ID = @ID;";

        command = new SqlCommand(sql, Connection());

        parameter = new SqlParameter("@ID", Id);
        parameter.SqlDbType = System.Data.SqlDbType.Int;
        command.Parameters.Add(parameter);

        command.ExecuteNonQuery();
    }
    public CollectableData Load(int id)
    {
        String sql = "SELECT ID, LOGIN, PASSWORD FROM COLLECTABLE WHERE ID = @ID;";

        command = new SqlCommand(sql, Connection());

        parameter = new SqlParameter("@ID", id);
        parameter.SqlDbType = System.Data.SqlDbType.Int;
        command.Parameters.Add(parameter);

        reader = command.ExecuteReader();

        CollectableData od = new CollectableData();

        if (reader.Read())
        {
            od.id = Int32.Parse(reader.GetValue(reader.GetOrdinal("ID")).ToString());
            od.name = reader.GetValue(reader.GetOrdinal("NAME")).ToString();
            od.value = Int32.Parse(reader.GetValue(reader.GetOrdinal("VALUE")).ToString());
        }
        return od;
    }
    public List<CollectableData> RandomDraw(int value)
    {
        String sql = "SELECT TOP(@VALUE) * FROM COLLECTABLE ORDER BY NEWID();";

        command = new SqlCommand(sql, Connection());

        parameter = new SqlParameter("@VALUE", value);
        parameter.SqlDbType = System.Data.SqlDbType.Int;
        command.Parameters.Add(parameter);

        reader = command.ExecuteReader();

        List<CollectableData> list = new List<CollectableData>();

        while (reader.Read())
        {
            CollectableData ob = new CollectableData(
                Int32.Parse(reader.GetValue(reader.GetOrdinal("ID")).ToString()),
                reader.GetValue(reader.GetOrdinal("NAME")).ToString(),
                Int32.Parse(reader.GetValue(reader.GetOrdinal("VALUE")).ToString()));
            list.Add(ob);
        }
        foreach (var a in list)
        {
            Debug.Log(a.name + " " + a.value + " " + a.id);
        }
        return list;
    }
    internal void SavePosition(CollectableData collectableData)
    {
        String sql = "INSERT INTO REWARDS (CR, PR) VALUES (@CR, @PR);";

        command = new SqlCommand(sql, Connection());

        parameter = new SqlParameter("@CR", collectableData.id);
        parameter.SqlDbType = System.Data.SqlDbType.Int;
        command.Parameters.Add(parameter);

        parameter = new SqlParameter("@PR", collectableData.pData.id);
        parameter.SqlDbType = System.Data.SqlDbType.Int;
        command.Parameters.Add(parameter);

        command.ExecuteNonQuery();
    }
}
