using CsvHelper;
using CsvHelper.Configuration;
using Domain;
using FluentValidation;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace Data
{
    public class DataContext
    {
        private readonly ILogger<DataContext> _logger;
        protected readonly IConfiguration Configuration;
        private readonly IValidator<GoldenRaspberryCSV> _validator;
        private SqliteConnection? InMemoryDbConnection = null;

        public DataContext(IConfiguration configuration
                            , ILogger<DataContext> logger
                            , IValidator<GoldenRaspberryCSV> validator)
        {
            Configuration = configuration;
            _logger = logger;
            _validator = validator;

            StartSQLite();
        }

        public SqliteConnection GetInMemoryDbConnection()
        {
            if (InMemoryDbConnection == null)
            {
                InMemoryDbConnection = new SqliteConnection(Configuration.GetConnectionString("SQLiteDB"));
                InMemoryDbConnection.Open();
                return InMemoryDbConnection;
            }
            return InMemoryDbConnection;
        }

        public void StartSQLite()
        {
            try
            {
                CreateGoldenRasperryTable();
                FromCSVToDB();

                _logger.LogInformation("SQLite Started !");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }

        private IEnumerable<GoldenRaspberryCSV> ReadGoldenRaspberryFromCSV(string filePath)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                Mode = CsvMode.NoEscape
            };

            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, config);

            csv.Read();

            csv.ReadHeader();

            while (csv.Read())
            {
                csv.TryGetField<string>("year", out string? year);
                csv.TryGetField<string>("title", out string? title);
                csv.TryGetField<string>("studios", out string? studio);
                csv.TryGetField<string>("producers", out string? producer);
                csv.TryGetField<string>("winner", out string? winner);
                var rowinfile = csv.Parser.Row;

                yield return new GoldenRaspberryCSV()
                {
                    Year = year != "" ? Convert.ToInt32(year) : 0,
                    Title = title ?? "",
                    Studio = studio ?? "",
                    Producer = producer ?? "",
                    Winner = winner == "yes",
                    RowinFile = rowinfile
                };
            }
        }

        private async void FromCSVToDB()
        {
            var csvFilePath = Configuration.GetSection("PathCSV").Value ?? "";

            var sql = "INSERT INTO GoldenRaspberry(year, title, studio, producer, winner) VALUES (@year, @title, @studio, @producer, @winner)";

            var connection = GetInMemoryDbConnection();

            foreach (var GR in ReadGoldenRaspberryFromCSV(csvFilePath))
            {
                var validationResult = await _validator.ValidateAsync(GR);

                if (validationResult.IsValid)
                {
                    using var command = new SqliteCommand(sql, connection);
                    command.Parameters.AddWithValue("@year", GR.Year);
                    command.Parameters.AddWithValue("@title", GR.Title);
                    command.Parameters.AddWithValue("@studio", GR.Studio);
                    command.Parameters.AddWithValue("@producer", GR.Producer);
                    command.Parameters.AddWithValue("@winner", GR.Winner);
                    command.ExecuteNonQuery();
                }
                else
                {
                    foreach (var error in validationResult.Errors)
                    {
                        _logger.LogWarning($"CSV FILE - Error in Line {GR.RowinFile} | Message: {error.ErrorMessage}");
                    }
                }
            }
        }

        private void CreateGoldenRasperryTable()
        {
            var sql = @"CREATE TABLE IF NOT EXISTS GoldenRaspberry(
                            id INTEGER PRIMARY KEY,
                            year INTEGER NOT NULL,
                            title TEXT NOT NULL,
                            studio TEXT NOT NULL,
                            producer TEXT NOT NULL,
                            winner INTEGER NOT NULL
                        )";

            var connection = GetInMemoryDbConnection();

            using var command = new SqliteCommand(sql, connection);
            command.ExecuteNonQuery();

            _logger.LogInformation("Table 'GoldenRaspberry' has created.");
        }
    }
}