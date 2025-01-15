using Domain.Validators;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class GoldenRaspberryCSV
    {
        public int Year;

        public string Title = "";

        public string Studio = "";

        public bool Winner;

        public List<string> Producers = [];

        public int RowinFile;
    }
}
