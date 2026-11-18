using System.Data;
using Microsoft.Data.SqlClient;

namespace CPMTools;

public static class LocalDb
{
    private const string ConnectionString =
        @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\user\source\repos\CPMTools\CPMTools\Database1.mdf;Integrated Security=True";

    public static void Init()
    {
        using var connection = new SqlConnection(ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = """
            IF OBJECT_ID(N'dbo.Sessions', N'U') IS NULL
            BEGIN
                CREATE TABLE dbo.Sessions
                (
                    id INT NOT NULL PRIMARY KEY,
                    email NVARCHAR(320) NOT NULL,
                    password NVARCHAR(MAX) NOT NULL,
                    auth NVARCHAR(MAX) NOT NULL,
                    firebase_uid NVARCHAR(255) NOT NULL
                );
            END;
            """;
        command.ExecuteNonQuery();
    }

    public static void Save(int slot, string email, string pw, string auth, string uid)
    {
        using var connection = new SqlConnection(ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = """
            IF EXISTS (SELECT 1 FROM dbo.Sessions WHERE id = @id)
                UPDATE dbo.Sessions
                SET email = @email, password = @password, auth = @auth, firebase_uid = @uid
                WHERE id = @id;
            ELSE
                INSERT INTO dbo.Sessions (id, email, password, auth, firebase_uid)
                VALUES (@id, @email, @password, @auth, @uid);
            """;
        command.Parameters.Add("@id", SqlDbType.Int).Value = slot;
        command.Parameters.Add("@email", SqlDbType.NVarChar, 320).Value = email;
        command.Parameters.Add("@password", SqlDbType.NVarChar, -1).Value = pw;
        command.Parameters.Add("@auth", SqlDbType.NVarChar, -1).Value = auth;
        command.Parameters.Add("@uid", SqlDbType.NVarChar, 255).Value = uid;
        command.ExecuteNonQuery();
    }

    public static (string email, string password, string auth, string uid)? Load(int slot)
    {
        using var connection = new SqlConnection(ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText =
            "SELECT email, password, auth, firebase_uid FROM dbo.Sessions WHERE id = @id";
        command.Parameters.Add("@id", SqlDbType.Int).Value = slot;

        using var reader = command.ExecuteReader();
        if (!reader.Read())
        {
            return null;
        }

        return (
            reader.GetString(0),
            reader.GetString(1),
            reader.GetString(2),
            reader.GetString(3));
    }

    public static void Clear(int slot)
    {
        using var connection = new SqlConnection(ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM dbo.Sessions WHERE id = @id";
        command.Parameters.Add("@id", SqlDbType.Int).Value = slot;
        command.ExecuteNonQuery();
    }
}
