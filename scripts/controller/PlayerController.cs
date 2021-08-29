using System;
using System.Collections.Generic;
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

    public List<CellData> Ranking(int value)
    {
        string sql = "SELECT TOP(@VALUE) R.LOGIN, R.TOTAL, R.MATCH_ID FROM RESULT AS R ORDER BY R.TOTAL DESC;";

        command = new SqlCommand(sql, Connection());

        parameter = new SqlParameter("@VALUE", value);
        parameter.SqlDbType = System.Data.SqlDbType.Int;
        command.Parameters.Add(parameter);

        reader = command.ExecuteReader();

        List<CellData> list = new List<CellData>();

        while (reader.Read())
        {
            CellData p = new CellData();
            p.login = reader.GetValue(reader.GetOrdinal("LOGIN")).ToString();
            if (p.login== null) p.login = "";
            Int32.TryParse(reader.GetValue(reader.GetOrdinal("TOTAL")).ToString(), out p.pontuation);
            Int32.TryParse(reader.GetValue(reader.GetOrdinal("MATCH_ID")).ToString(), out p.match_id);

            list.Add(p);
        }
        return list;
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
    public void DeleteElementInList(int Mc)
    {
        String sql = "DELETE FROM COLLIDE WHERE MC = @MC;" +
                     "DELETE FROM COLLECT WHERE MC = @MC;" +
                     "DELETE FROM MATCH WHERE ID = @MC";

        command = new SqlCommand(sql, Connection());

        parameter = new SqlParameter("@MC", Mc);
        parameter.SqlDbType = System.Data.SqlDbType.Int;
        command.Parameters.Add(parameter);
        Debug.Log("deletar elemento da lista");
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
