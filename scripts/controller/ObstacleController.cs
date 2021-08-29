using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using UnityEngine;

public class ObstacleController : DAO
{
    public void Save(int matchId, ObstacleData p)
    {
        String sql = "INSERT INTO COLLIDE (MC, OC) VALUES (@MC, @OC);";

        command = new SqlCommand(sql, Connection());

        parameter = new SqlParameter("@MC", matchId);
        parameter.SqlDbType = System.Data.SqlDbType.Int;
        command.Parameters.Add(parameter);

        parameter = new SqlParameter("@OC", p.id);
        parameter.SqlDbType = System.Data.SqlDbType.Int;
        command.Parameters.Add(parameter);

        command.ExecuteNonQuery();
    }
    public void Change(ObstacleData p)
    {
        String sql = "UPDATE OBSTACLE SET NAME = @NAME, VALUE = @VALUE WHERE ID = @ID;";

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
        String sql = "DELETE FROM OBSTACLE WHERE ID = @ID;";

        command = new SqlCommand(sql, Connection());

        parameter = new SqlParameter("@ID", Id);
        parameter.SqlDbType = System.Data.SqlDbType.Int;
        command.Parameters.Add(parameter);

        command.ExecuteNonQuery();
    }
    public ObstacleData Load(int id)
    {
        String sql = "SELECT ID, LOGIN, PASSWORD FROM OBSTACLE WHERE ID = @ID;";

        command = new SqlCommand(sql, Connection());

        parameter = new SqlParameter("@ID", id);
        parameter.SqlDbType = System.Data.SqlDbType.Int;
        command.Parameters.Add(parameter);

        reader = command.ExecuteReader();

        ObstacleData od = new ObstacleData();

        if (reader.Read())
        {
            od.id = Int32.Parse(reader.GetValue(reader.GetOrdinal("ID")).ToString());
            od.name = reader.GetValue(reader.GetOrdinal("NAME")).ToString();
            od.value = Int32.Parse(reader.GetValue(reader.GetOrdinal("VALUE")).ToString());
        }
        return od;
    }
    public List<ObstacleData> RandomDraw(int value)
    {
        string sql = "SELECT TOP(@VALUE) * FROM OBSTACLE ORDER BY NEWID();";

        command = new SqlCommand(sql, Connection());

        parameter = new SqlParameter("@VALUE", value);
        parameter.SqlDbType = System.Data.SqlDbType.Int;
        command.Parameters.Add(parameter);

        reader = command.ExecuteReader();

        List<ObstacleData> list = new List<ObstacleData>();

        while (reader.Read())
        {
            ObstacleData ob = new ObstacleData(
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
    internal void SavePosition(ObstacleData obstacleData)
    {
        String sql = "INSERT INTO PENALTIES (OP, PP) VALUES (@OP, @PP);";

        command = new SqlCommand(sql, Connection());

        parameter = new SqlParameter("@OP", obstacleData.id);
        parameter.SqlDbType = System.Data.SqlDbType.Int;
        command.Parameters.Add(parameter);

        parameter = new SqlParameter("@PP", obstacleData.pData.id);
        parameter.SqlDbType = System.Data.SqlDbType.Int;
        command.Parameters.Add(parameter);

        command.ExecuteNonQuery();
    }
}
