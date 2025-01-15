using Domain;
using Domain.Interfaces;
using Dapper;

namespace Data.Repositories
{
    public class GoldenRaspberryRepository(DataContext context) : IGoldenRaspberryRepository
    {
        private readonly DataContext _context = context;

        public async Task<IEnumerable<GoldenRaspberryAward>> GetAll(bool winners)
        {
            var connection = _context.GetInMemoryDbConnection();
            var sql = @"SELECT GR.id, year, title, studio, winner , AP.goldenraspberryid, AP.Producer
                        FROM GoldenRaspberry GR
                        INNER JOIN AwardProducer AP ON AP.goldenraspberryid = GR.id";

            if (winners)
            {
                sql += " WHERE GR.winner == 1";
            }

            var regs = await connection.QueryAsync<GoldenRaspberryAward, GoldenRaspberryProducer, GoldenRaspberryAward>(sql, (goldenraspberry, awardproducer) =>
            {
                goldenraspberry.Producers.Add(awardproducer);
                return goldenraspberry;
            },
            splitOn: "goldenraspberryid");

            var result = regs.GroupBy(gr => gr.id).Select(ggr =>
            {
                var groupedGR = ggr.First();
                groupedGR.Producers = ggr.Select(p => p.Producers.Single()).ToList();
                return groupedGR;
            });

            return result.ToList();
        }

        public async Task<IEnumerable<Producer>> GetAllProducersWinners()
        {
            var connection = _context.GetInMemoryDbConnection();
            var sql = @"SELECT AP.ID, AP.producer ProducerName, GR.id, GR.year, GR.title, GR.studio, GR.winner
                        FROM AwardProducer AP
                        RIGHT JOIN GoldenRaspberry GR ON GR.id = AP.goldenraspberryid
                        WHERE GR.winner = 1";

            var regs = await connection.QueryAsync<Producer, ProducerAwards, Producer>(sql, (producer, produceraward) =>
            {
                producer.Awards.Add(produceraward);
                return producer;
            },
            splitOn: "id");

            var result = regs.GroupBy(pa => pa.ProducerName).Select(a =>
            {
                var awards = a.First();
                awards.Awards = a.Select(p => p.Awards.Single()).ToList();
                return awards;
            });

            return result.ToList();
        }
    }
}