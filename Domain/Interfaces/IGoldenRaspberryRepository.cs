
namespace Domain.Interfaces
{
    public interface IGoldenRaspberryRepository
    {
        public Task<IEnumerable<GoldenRaspberryAward>> GetAll();
        public Task<IEnumerable<GoldenRaspberryAward>> GetAllWinners();
    }
}