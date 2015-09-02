using System;
using System.Data;
using Afrodite.Connection;
using Apache.NMS;

namespace Afrodite.Common
{
    public static class Extensions
    {
        public static bool IsAcknowledge(this IMessage message)
        {
            MessageType type;
            if (!Enum.TryParse(message.NMSType, true, out type))
            {
                return false;
            }
            return type == MessageType.Affirmation;
        }

        public static bool TableExists(this IDbConnection connection,string tableName)
        {
            bool exists;
            try
            {
                connection.Open();
                // ANSI SQL
                var cmd = connection.CreateCommand();
                cmd.CommandText = string.Format("select case when exists((select * from information_schema.tables " +
                                                "where table_name = '{0}')) then 1 else 0 end", tableName);
                exists = (int) cmd.ExecuteScalar() == 1;
            }
            catch
            {
                try
                {
                    // Other RDBMS.
                    exists = true;
                    var cmd = connection.CreateCommand();
                    cmd.CommandText = string.Format("select 1 from {0} where 1 = 0", tableName);
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    exists = false;
                }
            }
            finally
            {
                connection.Close();
            }
            return exists;
        }

        public static int ExecuteNonQuery(this IDbConnection connection, string query)
        {
            try
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = query;
                return cmd.ExecuteNonQuery();
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
