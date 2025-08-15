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

namespace Notification
{
    public interface INotificationRepository : IDisposable
    {
        List<NotificationModel> GetAllNotificationsWithData(NotificationModel notification);
        int InsertNotification(NotificationModel user);
        int DeleteNotificationById(int id);
    }
    public class NotificationRepository : INotificationRepository
    {
        private readonly string _connString;

        public NotificationRepository(IConfiguration configuration)
        {
            _connString = configuration.GetConnectionString("Postgres");
        }

        public NotificationRepository(string connection)
        {
            _connString = connection;
        }

        public List<NotificationModel> GetAllNotificationsWithData(NotificationModel notification)
        {
            try
            {
                var notifications = new List<NotificationModel>();

                using (var conn = new NpgsqlConnection(_connString))
                {
                    conn.Open();

                    using (var cmd = new NpgsqlCommand(
                        "SELECT * FROM notification_get_all_with_data(@p_id, @p_notification_type)", conn))
                    {
                        cmd.Parameters.AddWithValue("p_id", notification.id);
                        cmd.Parameters.AddWithValue("p_user_id", notification.notification_type);

                        using (var reader = cmd.ExecuteReader())
                        {
                            var properties = typeof(NotificationModel).GetProperties();

                            while (reader.Read())
                            {
                                var item = new NotificationModel();

                                foreach (var property in properties)
                                {
                                    InternalProcess(property, reader, item);
                                }

                                notifications.Add(item);
                            }
                        }
                    }
                }

                return notifications;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                LogManager.GetCurrentClassLogger().Error(ex);
                return new List<NotificationModel>();
            }
        }

        private static void InternalProcess(PropertyInfo property, NpgsqlDataReader reader, NotificationModel item)
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

        public int InsertNotification(NotificationModel notification)
        {
            try
            {
                using (var conn = new NpgsqlConnection(_connString))
                {
                    conn.Open();

                    using (var cmd = new NpgsqlCommand(
                            "SELECT notification_upsert(@p_id, @p_msg, @p_notification_type, @p_day_of_week, @p_day_of_month, @p_send_time, @p_action)", conn))
                    {
                        cmd.Parameters.AddWithValue("p_id", notification.id);
                        cmd.Parameters.AddWithValue("p_msg", notification.msg);
                        cmd.Parameters.AddWithValue("p_notification_type", notification.notification_type);
                        cmd.Parameters.AddWithValue("p_day_of_week", notification.day_of_week);
                        cmd.Parameters.AddWithValue("p_day_of_month", notification.day_of_month);
                        cmd.Parameters.AddWithValue("p_send_time", notification.send_time);
                        cmd.Parameters.AddWithValue("p_action", NpgsqlTypes.NpgsqlDbType.Text).Value = notification.action ?? (object)DBNull.Value;

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

        public int DeleteNotificationById(int id)
        {
            try
            {
                using (var conn = new NpgsqlConnection(_connString))
                {
                    conn.Open();

                    using (var cmd = new NpgsqlCommand("SELECT notification_delete(@p_id)", conn))
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
