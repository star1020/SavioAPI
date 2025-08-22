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
using Savio.Core;
using Savio.Core.Data;

namespace Category
{
    public interface ICategoryRepository : IDisposable
    {
        List<CategoryModel> GetAllCategoriesWithData(CategoryModel category);
        int InsertCategory(CategoryModel category);
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

        public List<CategoryModel> GetAllCategoriesWithData(CategoryModel category)
        {
            try
            {
                var txns = new List<CategoryModel>();

                using (var conn = new NpgsqlConnection(_connString))
                {
                    conn.Open();

                    using (var cmd = new NpgsqlCommand(
                        "SELECT * FROM category_get_all_with_data(@p_id)", conn))
                    {
                        cmd.Parameters.AddWithValue("p_id", category.id);

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

                                txns.Add(item);
                            }
                        }
                    }
                }

                return txns;
            }
            catch (Exception ex)
            {
                LogHelper.LoggingException(ex);
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
                            "SELECT category_upsert(@p_id, @p_icon, @p_name, @p_action)", conn))
                    {
                        cmd.Parameters.AddWithValue("p_id", category.id);
                        cmd.Parameters.AddWithValue("p_icon", NpgsqlTypes.NpgsqlDbType.Text).Value = category.icon ?? (object)DBNull.Value;
                        cmd.Parameters.AddWithValue("p_name", category.name);
                        cmd.Parameters.AddWithValue("p_action", NpgsqlTypes.NpgsqlDbType.Text).Value = category.action ?? (object)DBNull.Value;

                        cmd.ExecuteNonQuery();
                    }

                    return 0;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LoggingException(ex);
                return -1;
            }
        }

        public int DeleteCategoryById(int id)
        {
            try
            {
                using (var conn = new NpgsqlConnection(_connString))
                {
                    conn.Open();

                    using (var cmd = new NpgsqlCommand("SELECT category_delete(@p_id)", conn))
                    {
                        cmd.Parameters.AddWithValue("p_id", id);
                        var result = (int)cmd.ExecuteScalar();
                        if (result == 0)
                        {
                            throw new Exception($"Category with id {id} not found.");
                        }
                    }

                    return 0;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LoggingException(ex);
                return -1;
            }
        }

        public void Dispose()
        {
        }

    }

}
