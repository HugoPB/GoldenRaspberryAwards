﻿namespace Domain
{
    public class GoldenRaspberryAward
    {
        public int id {  get; set; }
        public int Year { get; set; }
        public string Title { get; set; } = "";
        public string Studio { get; set; } = "";
        public List<GoldenRaspberryProducer> Producers { get; set; } = [];
        public bool Winner { get; set; }
    }
}
