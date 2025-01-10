using Microsoft.Data.Sqlite;

namespace Domain.Interfaces
{
    public interface ISQLiteDBCon
    {
        SqliteConnection GetInMemoryDBCon();
    }
}