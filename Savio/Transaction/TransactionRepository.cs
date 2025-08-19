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
        List<TransactionModel> GetAllTransactionsWithData(TransactionModel txn);
        int InsertTransaction(TransactionModel user);
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

        public List<TransactionModel> GetAllTransactionsWithData(TransactionModel txn)
        {
            try
            {
                var txns = new List<TransactionModel>();

                using (var conn = new NpgsqlConnection(_connString))
                {
                    conn.Open();

                    using (var cmd = new NpgsqlCommand(
                        "SELECT * FROM transaction_get_all_with_data(@p_id, @p_user_id, @p_category_id, @p_type, @p_member_id, @p_record_date)", conn))
                    {
                        cmd.Parameters.AddWithValue("p_id", txn.id);
                        cmd.Parameters.AddWithValue("p_user_id", txn.user_id);
                        cmd.Parameters.AddWithValue("p_category_id", txn.category_id);
                        cmd.Parameters.Add("p_type", NpgsqlTypes.NpgsqlDbType.Varchar).Value = (object)txn.type ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("p_member_id", txn.member_id);
                        cmd.Parameters.Add("p_record_date", NpgsqlTypes.NpgsqlDbType.Date).Value = txn.record_date == null
                                                    ? (object)DBNull.Value
                                                    : txn.record_date?.Date;

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

                                txns.Add(item);
                            }
                        }
                    }
                }

                return txns;
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
                            "SELECT transaction_upsert(@p_id, @p_user_id, @p_category_id, @p_type, @p_member_id, @p_value, @p_record_date, @p_action)", conn))
                    {
                        cmd.Parameters.AddWithValue("p_id", txn.id);
                        cmd.Parameters.AddWithValue("p_user_id", txn.user_id);
                        cmd.Parameters.AddWithValue("p_category_id", txn.category_id);
                        cmd.Parameters.AddWithValue("p_type", txn.type);
                        cmd.Parameters.AddWithValue("p_member_id", txn.member_id);
                        cmd.Parameters.AddWithValue("p_value", txn.value);
                        cmd.Parameters.AddWithValue("p_record_date", txn.record_date);
                        cmd.Parameters.AddWithValue("p_action", NpgsqlTypes.NpgsqlDbType.Text).Value = txn.action ?? (object)DBNull.Value;

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

        public int DeleteTransactionById(int id)
        {
            try
            {
                using (var conn = new NpgsqlConnection(_connString))
                {
                    conn.Open();

                    using (var cmd = new NpgsqlCommand("SELECT transaction_delete(@p_id)", conn))
                    {
                        cmd.Parameters.AddWithValue("p_id", id);
                        var result = (int)cmd.ExecuteScalar();
                        if (result == 0)
                        {
                            throw new Exception($"Transaction with id {id} not found.");
                        }
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
