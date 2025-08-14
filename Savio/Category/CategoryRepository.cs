using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NLog;
using Npgsql;
using Savio.Core.Data;

namespace Category
{
    public interface ICategoryRepository : IDisposable
    {
        List<CategoryModel> GetAllCategories();
        int InsertCategory(CategoryModel user);
        CategoryModel GetCategoryById(int id);
        int DeleteCategoryById(int id);
    }
    public class CategoryRepository : ICategoryRepository
    {
        private readonly string _connString;

        public CategoryRepository(IConfiguration configuration)
        {
            _connString = configuration.GetConnectionString("Postgres");
        }

        public CategoryRepository(string connection)
        {
            _connString = connection;
        }

        public List<CategoryModel> GetAllCategories()
        {
            try
            {
                var users = new List<CategoryModel>();

                using (var conn = new NpgsqlConnection(_connString))
                {
                    conn.Open();

                    using (var cmd = new NpgsqlCommand("SELECT * FROM category_get_all()", conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        var properties = typeof(CategoryModel).GetProperties();

                        while (reader.Read())
                        {
                            var item = new CategoryModel();

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
                return new List<CategoryModel>();
            }
        }


        private static void InternalProcess(PropertyInfo property, NpgsqlDataReader reader, CategoryModel item)
        {
            var v = reader[property.Name];

            if (v == null || v == DBNull.Value) return;

            if (property.PropertyType == typeof(IDictionary<string, object>))
            {
                var data = JsonConvert.DeserializeObject<IDictionary<string, object>>(v.ToString());
                property.SetValue(item, data);
                return;
            }

            property.SetValue(item, v);
        }

        public int InsertCategory(CategoryModel category)
        {
            try
            {
                using (var conn = new NpgsqlConnection(_connString))
                {
                    conn.Open();

                    using (var cmd = new NpgsqlCommand(
                            "CALL category_upsert(@p_id, @p_icon, @p_name, @p_action)", conn))
                    {
                        cmd.Parameters.AddWithValue("p_id", category.id);
                        cmd.Parameters.AddWithValue("p_icon", category.icon);
                        cmd.Parameters.AddWithValue("p_name", category.name);
                        cmd.Parameters.AddWithValue("p_action", category.action);

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


        public CategoryModel GetCategoryById(int id)
        {
            try
            {
                var conn = new NpgsqlConnection(_connString);
                conn.Open();

                var cmd = new NpgsqlCommand("SELECT * FROM category_get_by_id(@p_id)", conn);
                cmd.Parameters.AddWithValue("p_id", id);

                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new CategoryModel
                    {
                        id = reader.GetInt32(reader.GetOrdinal("id")),
                        icon = reader.GetString(reader.GetOrdinal("icon")),
                        name = reader.GetString(reader.GetOrdinal("name")),
                        action = reader.GetString(reader.GetOrdinal("action")),
                        created_at = reader.IsDBNull(reader.GetOrdinal("created_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("created_at")),
                        updated_at = reader.IsDBNull(reader.GetOrdinal("updated_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated_at"))
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                LogManager.GetCurrentClassLogger().Error(ex);
                return null;
            }
        }

        public int DeleteCategoryById(int id)
        {
            try
            {
                using (var conn = new NpgsqlConnection(_connString))
                {
                    conn.Open();

                    using (var cmd = new NpgsqlCommand("CALL category_delete(@p_id)", conn))
                    {
                        cmd.Parameters.AddWithValue("p_id", id);
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
