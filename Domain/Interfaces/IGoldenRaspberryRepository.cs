
namespace Domain.Interfaces
{
    public interface IGoldenRaspberryRepository
    {
        public Task<IEnumerable<GoldenRaspberryAward>> GetAll(bool winners);
        public Task<IEnumerable<Producer>> GetAllProducersWinners();
    }
}