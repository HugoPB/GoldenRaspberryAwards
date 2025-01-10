using Domain;
using Domain.Interfaces;
using Dapper;

namespace Data.Repositories
{
    public class GoldenRaspberryRepository(DataContext context) : IGoldenRaspberryRepository
    {
        private readonly DataContext _context = context;

        public async Task<IEnumerable<GoldenRaspberryAward>> GetAll()
        {
            var connection = _context.GetInMemoryDbConnection();
            var sql = "SELECT * FROM GoldenRaspberry";
            return await connection.QueryAsync<GoldenRaspberryAward>(sql);
        }

        public async Task<IEnumerable<GoldenRaspberryAward>> GetAllWinners()
        {
            var connection = _context.GetInMemoryDbConnection();
            var sql = "SELECT * FROM GoldenRaspberry WHERE winner = 1";
            return await connection.QueryAsync<GoldenRaspberryAward>(sql);
        }
    }
}