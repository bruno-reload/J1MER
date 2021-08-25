using System;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using UnityEngine;

public class PlayerController : DAO
{
    public void Save(PlayerData p)
    {
        String sql = "INSERT INTO PLAYER (LOGIN, PASSWORD) VALUES (@LOGIN, @PASSWORD);";

        command = new SqlCommand(sql, Connection());

        parameter = new SqlParameter("@LOGIN", p.login);
        parameter.SqlDbType = System.Data.SqlDbType.Char;
        command.Parameters.Add(parameter);

        parameter = new SqlParameter("@PASSWORD", p.password);
        parameter.SqlDbType = System.Data.SqlDbType.Char;
        command.Parameters.Add(parameter);

        command.ExecuteNonQuery();
    }
    public void Change(PlayerData p)
    {
        String sql = "UPDATE PLAYER SET LOGIN = @LOGIN, PASSWORD = @PASSWORD WHERE ID = @ID;";

        command = new SqlCommand(sql, Connection());

        parameter = new SqlParameter("@ID", p.id);
        parameter.SqlDbType = System.Data.SqlDbType.Int;
        command.Parameters.Add(parameter);

        parameter = new SqlParameter("@LOGIN", p.login);
        parameter.SqlDbType = System.Data.SqlDbType.Char;
        command.Parameters.Add(parameter);

        parameter = new SqlParameter("@PASSWORD", p.password);
        parameter.SqlDbType = System.Data.SqlDbType.Char;
        command.Parameters.Add(parameter);

        command.ExecuteNonQuery();
    }
    public void Delete(int Id)
    {
        String sql = "DELETE FROM PLAYER WHERE ID = @ID;";

        command = new SqlCommand(sql, Connection());

        parameter = new SqlParameter("@ID", Id);
        parameter.SqlDbType = System.Data.SqlDbType.Int;
        command.Parameters.Add(parameter);

        command.ExecuteNonQuery();
    }
    public PlayerData Load(string login, string password)
    {
        String sql = "SELECT ID, LOGIN, PASSWORD FROM PLAYER WHERE (LOGIN = @LOGIN AND PASSWORD = @PASSWORD);";

        command = new SqlCommand(sql, Connection());


        parameter = new SqlParameter("@LOGIN", login);
        parameter.SqlDbType = System.Data.SqlDbType.VarChar;
        command.Parameters.Add(parameter);

        parameter = new SqlParameter("@PASSWORD", password);
        parameter.SqlDbType = System.Data.SqlDbType.Char;
        command.Parameters.Add(parameter);

        reader = command.ExecuteReader();

        PlayerData p = new PlayerData();

        if (reader.Read())
        {
            p.id = Int32.Parse(reader.GetValue(reader.GetOrdinal("ID")).ToString());
            p.login = reader.GetValue(reader.GetOrdinal("LOGIN")).ToString();
            p.password = reader.GetValue(reader.GetOrdinal("PASSWORD")).ToString();
        }

        if (String.IsNullOrEmpty(p.login) || String.IsNullOrEmpty(p.password))
        {
            return null;
        }
        return p;
    }
}
