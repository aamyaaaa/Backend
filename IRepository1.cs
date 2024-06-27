using Npgsql;

namespace art_gallery_api.Persistence
{
    public interface IRepository1
    {
        List<T> ExecuteReader<T>(string sqlCommand, NpgsqlParameter[] dbParams = null) where T : class, new();
    }
}