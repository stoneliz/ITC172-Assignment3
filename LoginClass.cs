using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data; //general data tools
using System.Data.SqlClient; //sql server data tools
using System.Configuration; //tools for talking to config file

/// <summary>
/// Summary description for LoginClass
/// </summary>
public class LoginClass
{
    SqlConnection connect;
    string user;
    string pass;
    public LoginClass(string userName, string password)
    {
        user = userName;
        pass = password;
        string connectString = 
            ConfigurationManager.ConnectionStrings["Community_AssistConnection"].ToString();
        connect = new SqlConnection(connectString);
    }

    public int ValidateLogin()
    {
        SqlCommand cmd = new SqlCommand(); //passes a sql string through the connection
        cmd.Connection = connect;
        cmd.CommandType = CommandType.StoredProcedure; //returns 1 if it's good and -1 if it fails
        cmd.CommandText = "usp_Login";
        cmd.Parameters.AddWithValue("@Email", user);
        cmd.Parameters.AddWithValue("@password", pass);
        connect.Open();
        int result = cmd.ExecuteNonQuery(); //
        connect.Close();
        int key = 0;
        if(result != -1)
        {
            key = GetUserKey();
        }
        return key;
    }

    private int GetUserKey()
    {
        string sql = "Select PersonKey from Person where PersonEmail =@user";
        SqlCommand cmd = new SqlCommand(sql, connect);
        cmd.Parameters.AddWithValue("@user", user);
        SqlDataReader reader = null;
        connect.Open();
        reader = cmd.ExecuteReader();
        int key = 0;
        while (reader.Read())
        {
             key= (int)reader["PersonKey"];
        }
        connect.Close();


        return key;
    }
}