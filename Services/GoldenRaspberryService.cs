using Domain;
using Domain.Interfaces;

namespace Services
{
    public class GoldenRaspberryService(IGoldenRaspberryRepository repository) : IGoldenRaspberryService
    {
        private readonly IGoldenRaspberryRepository _repository = repository;

        public async Task<IEnumerable<GoldenRaspberryAward>> GetAllAwards()
        {
            return await _repository.GetAll(false);
        }

        public async Task<GoldenRaspberryAwardMinMaxInterval> GetMinMaxWinnersInterval()
        {
            GoldenRaspberryAwardMinMaxInterval AwardsMinMaxInterval = new();
            List<IntervalAward> intervalAwards = [];

            var allProducer = await _repository.GetAllProducersWinners();

            var producersWInterval = allProducer.Where(p => p.Awards.Count > 1).ToList();

            foreach (var producer in producersWInterval)
            {
                var awards = producer.Awards.OrderByDescending(x => x.Year);

                foreach (var award in awards)
                {
                    var previousAward = awards.Where(x => x.Year < award.Year).FirstOrDefault();

                    if (previousAward != null)
                    {
                        IntervalAward intervalAward = new()
                        {
                            Producer = producer.ProducerName,
                            Interval = award.Year - previousAward.Year,
                            previousWin = previousAward.Year,
                            followingWin = award.Year,
                        };

                        intervalAwards.Add(intervalAward);
                    }
                }
            }

            var minIntervalValue = intervalAwards.Min(x => x.Interval);
            var maxIntervalValue = intervalAwards.Max(x => x.Interval);

            //Recupera o grupo de registro que possam ter o mesmo Intervalo minimo e máximo
            var minAwardInterval = intervalAwards.Where(x => x.Interval == minIntervalValue);
            var maxAwardInterval = intervalAwards.Where(x => x.Interval == maxIntervalValue);

            AwardsMinMaxInterval.Min.AddRange(minAwardInterval);
            AwardsMinMaxInterval.Max.AddRange(maxAwardInterval);

            return AwardsMinMaxInterval;
        }
    }
}