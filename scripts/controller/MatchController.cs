using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using UnityEngine;

class MatchController : DAO
{
    //String sql = "INSERT INTO MATCH (RANKING) OUTPUT inserted.ID VALUES(@RANKING);";
    public int Save(int plaierId)
    {
        String sqlMath = "INSERT INTO MATCH (PM) OUTPUT inserted.ID VALUES( @PM);";

        command = new SqlCommand(sqlMath, Connection());
        
        parameter = new SqlParameter("@PM", plaierId);
        parameter.SqlDbType = System.Data.SqlDbType.Int;
        command.Parameters.Add(parameter);

        int id = Int32.Parse(command.ExecuteScalar().ToString());
        Debug.Log("o ide gerado pela cena salva é " + id);

        return id;
    }
   
    public void Delete(int Id)
    {
        String sql = "DELETE FROM POSITION WHERE ID = @ID;";

        command = new SqlCommand(sql, Connection());

        parameter = new SqlParameter("@ID", Id);
        parameter.SqlDbType = System.Data.SqlDbType.Int;
        command.Parameters.Add(parameter);

        command.ExecuteNonQuery();
    }
    public MatchData Load(int id)
    {
        String sql = "SELECT X, Y, Z FROM POSITION WHERE ID = @ID;";

        command = new SqlCommand(sql, Connection());

        parameter = new SqlParameter("@ID", id);
        parameter.SqlDbType = System.Data.SqlDbType.Int;
        command.Parameters.Add(parameter);

        reader = command.ExecuteReader();

        MatchData md = new MatchData();

        if (reader.Read())
        {
            md.id = Int32.Parse(reader.GetValue(reader.GetOrdinal("ID")).ToString());
            md.position.x = float.Parse(reader.GetValue(reader.GetOrdinal("X")).ToString());
            md.position.y = float.Parse(reader.GetValue(reader.GetOrdinal("y")).ToString());
            md.position.z = float.Parse(reader.GetValue(reader.GetOrdinal("z")).ToString());
        }
        return md;
    }

    public List<MatchData> Spawner(int value)
    {
        String sql = "SELECT TOP(@VALUE) * FROM POSITION ORDER BY NEWID();";

        command = new SqlCommand(sql, Connection());

        parameter = new SqlParameter("@VALUE", value);
        parameter.SqlDbType = System.Data.SqlDbType.Int;
        command.Parameters.Add(parameter);

        reader = command.ExecuteReader();

        List<MatchData> list = new List<MatchData>();

        while (reader.Read())
        {
            MatchData ob = new MatchData(
               Int32.Parse(reader.GetValue(reader.GetOrdinal("id")).ToString()),
               new Vector3(
                    float.Parse(reader.GetValue(reader.GetOrdinal("X")).ToString()),
                    float.Parse(reader.GetValue(reader.GetOrdinal("y")).ToString()),
                    float.Parse(reader.GetValue(reader.GetOrdinal("z")).ToString())
                    )
                );
            list.Add(ob);
        }
        foreach (var a in list)
        {
            Debug.Log(a.position);
        }
        return list;
    }

    internal void Change(PlayerData p)
    {
        string sql = "UPDATE MATCH SET POSITION = @POSITION WHERE ID = @ID;";

        command = new SqlCommand(sql, Connection());

        parameter = new SqlParameter("@ID", p.matchId);
        parameter.SqlDbType = System.Data.SqlDbType.Int;
        command.Parameters.Add(parameter);

        parameter = new SqlParameter("@POSITION", p.matchPosition);
        parameter.SqlDbType = System.Data.SqlDbType.Int;
        command.Parameters.Add(parameter);

        reader = command.ExecuteReader();
    }
}
