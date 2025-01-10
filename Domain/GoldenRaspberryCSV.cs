using Domain.Validators;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class GoldenRaspberryCSV
    {
        public int Year;

        public string Title = "";

        public string Studio = "";

        public string Producer = "";

        public bool Winner;
        public int RowinFile;
    }
}
