using Domain;
using Domain.Interfaces;

namespace Services
{
    public class GoldenRaspberryService(IGoldenRaspberryRepository repository) : IGoldenRaspberryService
    {
        private readonly IGoldenRaspberryRepository _repository = repository;

        public async Task<IEnumerable<GoldenRaspberryAward>> GetAllAwards()
        {
            return await _repository.GetAll();
        }

        public async Task<GoldenRaspberryAwardMinMaxInterval> GetMinMaxWinnersInterval()
        {
            GoldenRaspberryAwardMinMaxInterval AwardsMinMaxInterval = new();
            List<IntervalAward> intervalAwards = [];

            var allWinners = await _repository.GetAllWinners();            
            //Separa os registros por Produtor e apenas os que tiverem mais de 1 vitória, caso contrário não existe intervalo a ser cálculado
            var producersWinners = allWinners.GroupBy(x => x.Producer).Where(x => x.Count() > 1);

            foreach (var producerAward in producersWinners)
            {
                var awards = producerAward.OrderByDescending(x => x.Year);

                foreach (var award in awards)
                {
                    var previousAward = awards.Where(x => x.Year < award.Year).FirstOrDefault();

                    if (previousAward != null) {
                        IntervalAward intervalAward = new()
                        {
                            Producer = award.Producer,
                            Interval = award.Year - previousAward.Year,
                            previousWin = previousAward.Year,
                            followingWin = award.Year,
                        };

                        intervalAwards.Add(intervalAward);
                    }
                }
            }

            var minInterval = intervalAwards.Min(x => x.Interval);
            var maxInterval = intervalAwards.Max(x => x.Interval);

            //Recupera o grupo de registro que possam ter o mesmo Intervalo minimo e máximo
            var minAwardInterval = intervalAwards.Where(x => x.Interval == minInterval);
            var maxAwardInterval = intervalAwards.Where(x => x.Interval == maxInterval);

            AwardsMinMaxInterval.Min.AddRange(minAwardInterval);
            AwardsMinMaxInterval.Max.AddRange(maxAwardInterval);

            return AwardsMinMaxInterval;
        }
    }
}