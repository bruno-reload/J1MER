using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class PositionController : DAO
{
    public void Save(Vector3 p)
    {
        String sql = "INSERT INTO MATCH (NAME, VALUE) VALUES (@X, @Y, @Z);";

        command = new SqlCommand(sql, Connection());

        parameter = new SqlParameter("@X", p.x);
        parameter.SqlDbType = System.Data.SqlDbType.Float;
        command.Parameters.Add(parameter);

        parameter = new SqlParameter("@Y", p.y);
        parameter.SqlDbType = System.Data.SqlDbType.Float;
        command.Parameters.Add(parameter);

        parameter = new SqlParameter("@Z", p.z);
        parameter.SqlDbType = System.Data.SqlDbType.Float;
        command.Parameters.Add(parameter);

        command.ExecuteNonQuery();
    }
    public void Change(Vector3 p)
    {
        String sql = "UPDATE MATCH SET X = @X, Y = @Y, Z = @Z  WHERE ID = @ID;";

        command = new SqlCommand(sql, Connection());


        parameter = new SqlParameter("@X", p.x);
        parameter.SqlDbType = System.Data.SqlDbType.Float;
        command.Parameters.Add(parameter);

        parameter = new SqlParameter("@Y", p.y);
        parameter.SqlDbType = System.Data.SqlDbType.Float;
        command.Parameters.Add(parameter);

        parameter = new SqlParameter("@Z", p.z);
        parameter.SqlDbType = System.Data.SqlDbType.Float;
        command.Parameters.Add(parameter);

        command.ExecuteNonQuery();
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

    public List<PositionData> Spawner(int value)
    {
        String sql = "SELECT TOP(@VALUE) * FROM POSITION ORDER BY NEWID();";

        command = new SqlCommand(sql, Connection());

        parameter = new SqlParameter("@VALUE", value);
        parameter.SqlDbType = System.Data.SqlDbType.Int;
        command.Parameters.Add(parameter);

        reader = command.ExecuteReader();

        List<PositionData> list = new List<PositionData>();

        while (reader.Read())
        {
            PositionData ob = new PositionData(
                Int32.Parse(reader.GetValue(reader.GetOrdinal("ID")).ToString()),
                new Vector3(
                    float.Parse(reader.GetValue(reader.GetOrdinal("X")).ToString()),
                    float.Parse(reader.GetValue(reader.GetOrdinal("y")).ToString()),
                    float.Parse(reader.GetValue(reader.GetOrdinal("z")).ToString())
                    ));
            list.Add(ob);
        }
        foreach (var a in list)
        {
            Debug.Log(a.id + " " + a.position);
        }
        return list;
    }
}

