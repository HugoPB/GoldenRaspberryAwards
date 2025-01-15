
namespace Domain
{
    public class ProducerAwards
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public string Title { get; set; } = "";
        public string Studio { get; set; } = "";
        public bool Winner { get; set; }
    }
}