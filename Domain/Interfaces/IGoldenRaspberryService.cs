
namespace Domain.Interfaces
{
    public interface IGoldenRaspberryService
    {
        public Task<IEnumerable<GoldenRaspberryAward>> GetAllAwards();
        public Task<GoldenRaspberryAwardMinMaxInterval> GetMinMaxWinnersInterval();
    }
}
