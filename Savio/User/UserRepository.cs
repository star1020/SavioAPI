using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NLog;
using Npgsql;
using Savio.Core.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace User
{
    public interface IUserRepository : IDisposable
    {
        List<UserModel> GetAllUsers();
        int InsertUser(UserModel user);
    }
    public class UserRepository : IUserRepository
    {
        private readonly string _connString;

        public UserRepository(IConfiguration configuration)
        {
            _connString = configuration.GetConnectionString("Postgres");
        }

        public UserRepository(string connection)
        {
            _connString = connection;
        }

        public List<UserModel> GetAllUsers()
        {
            try
            {
                var users = new List<UserModel>();

                using (var conn = new NpgsqlConnection(_connString))
                {
                    conn.Open();

                    using (var cmd = new NpgsqlCommand("SELECT * FROM get_all_users()", conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        var properties = typeof(UserModel).GetProperties();

                        while (reader.Read())
                        {
                            var item = new UserModel();

                            foreach (var property in properties)
                            {
                                InternalProcess(property, reader, item);
                            }

                            users.Add(item);
                        }
                    }
                }

                return users;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                LogManager.GetCurrentClassLogger().Error(ex);
                return new List<UserModel>();
            }
        }


        private static void InternalProcess(PropertyInfo property, NpgsqlDataReader reader, UserModel item)
        {
            var v = reader[property.Name];

            if (v == null || v == DBNull.Value) return;
            
            if(property.PropertyType == typeof(IDictionary<string, object>))
            {
                var data = JsonConvert.DeserializeObject<IDictionary<string, object>>(v.ToString());
                property.SetValue(item, data);
                return;
            }

            property.SetValue(item, v);
        }

        public int InsertUser(UserModel user)
        {
            try
            {
                using (var conn = new NpgsqlConnection(_connString))
                {
                    conn.Open();

                    using (var cmd = new NpgsqlCommand("SELECT insert_user(@Username, @Password, @Email, @Role, @CreatedAt)", conn))
                    {
                        cmd.Parameters.AddWithValue("Username", user.Username ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("Password", user.Password ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("Email", user.Email ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("Role", user.Role ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("CreatedAt", DateTime.Now);

                        cmd.ExecuteNonQuery();
                    }
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                LogManager.GetCurrentClassLogger().Error(ex);
                return -1;
            }
        }
        public void Dispose()
        {
        }

    }

}
