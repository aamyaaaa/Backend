using Npgsql;
using System.Collections.Generic;
using System.Linq;

namespace art_gallery_api.Persistence
{
    public interface IRepository
    {
        private const string CONNECTION_STRING = "Host=localhost;Username=postgres;Password=muffy12;Database=art_gallery";

        public List<T> ExecuteReader<T>(string sqlCommand, NpgsqlParameter[] dbParams = null) where T : class, new()
        {
            var entities = new List<T>();
            //List<T> entities = new List<T>();
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            using var cmd = new NpgsqlCommand(sqlCommand, conn);

            if (dbParams != null)
            {
                cmd.Parameters.AddRange(dbParams.Where(x => x.Value != null).ToArray());
            }

            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                var entity = new T();
                dr.MapTo(entity); // Ensure you have this extension method defined elsewhere.
                entities.Add(entity);
            }

            return entities;
        }
    }
}
