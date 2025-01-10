
namespace Domain
{
    public class IntervalAward
    {
        public string Producer { get; set; } = "";
        public int Interval { get; set; }
        public int previousWin {  get; set; }
        public int followingWin { get; set; }
    }
}