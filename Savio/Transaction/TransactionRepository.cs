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

namespace Transaction
{
    public interface ITransactionRepository : IDisposable
    {
        List<TransactionModel> GetAllTransactions();
        int InsertTransaction(TransactionModel user);
        TransactionModel GetTransactionById(int id);
        int DeleteTransactionById(int id);
    }
    public class TransactionRepository : ITransactionRepository
    {
        private readonly string _connString;

        public TransactionRepository(IConfiguration configuration)
        {
            _connString = configuration.GetConnectionString("Postgres");
        }

        public TransactionRepository(string connection)
        {
            _connString = connection;
        }

        public List<TransactionModel> GetAllTransactions()
        {
            try
            {
                var users = new List<TransactionModel>();

                using (var conn = new NpgsqlConnection(_connString))
                {
                    conn.Open();

                    using (var cmd = new NpgsqlCommand("SELECT * FROM transaction_get_all()", conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        var properties = typeof(TransactionModel).GetProperties();

                        while (reader.Read())
                        {
                            var item = new TransactionModel();

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
                return new List<TransactionModel>();
            }
        }


        private static void InternalProcess(PropertyInfo property, NpgsqlDataReader reader, TransactionModel item)
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

        public int InsertTransaction(TransactionModel txn)
        {
            try
            {
                using (var conn = new NpgsqlConnection(_connString))
                {
                    conn.Open();

                    using (var cmd = new NpgsqlCommand(
                            "SELECT transaction_upsert(@p_id, @p_user_id, @p_category_id, @p_type, @p_member_id, @p_value, @p_action)", conn))
                    {
                        cmd.Parameters.AddWithValue("p_id", txn.id);
                        cmd.Parameters.AddWithValue("p_user_id", txn.user_id);
                        cmd.Parameters.AddWithValue("p_category_id", txn.category_id);
                        cmd.Parameters.AddWithValue("p_type", txn.type);
                        cmd.Parameters.AddWithValue("p_member_id", txn.member_id);
                        cmd.Parameters.AddWithValue("p_value", txn.value);
                        cmd.Parameters.AddWithValue("p_action", txn.action);

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


        public TransactionModel GetTransactionById(int id)
        {
            try
            {
                var conn = new NpgsqlConnection(_connString);
                conn.Open();

                var cmd = new NpgsqlCommand("SELECT * FROM transaction_get_by_id(@p_id)", conn);
                cmd.Parameters.AddWithValue("p_id", id);

                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new TransactionModel
                    {
                        id = reader.GetInt32(reader.GetOrdinal("id")),
                        user_id = reader.GetInt32(reader.GetOrdinal("user_id")),
                        category_id = reader.GetInt32(reader.GetOrdinal("category_id")),
                        type = reader.GetString(reader.GetOrdinal("type")),
                        member_id = reader.GetInt32(reader.GetOrdinal("member_id")),
                        value = reader.GetDecimal(reader.GetOrdinal("value")),
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

        public int DeleteTransactionById(int id)
        {
            try
            {
                using (var conn = new NpgsqlConnection(_connString))
                {
                    conn.Open();

                    using (var cmd = new NpgsqlCommand("CALL transaction_delete(@p_id)", conn))
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
