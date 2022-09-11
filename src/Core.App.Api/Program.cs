using CsvHelper;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Core.App.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/csvFileCreated", async () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new DummyData
        (
            DateTime.Now.AddDays(index),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
    ))
        .ToArray();

    await using var writer = new StreamWriter("CSVFileCreated.csv");
    await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
    csv.WriteHeader<DummyData>();
    csv.NextRecord();
    foreach (var record in forecast)
    {
        csv.WriteRecord(record);
        csv.NextRecord();
    }

    return forecast;
});

app.Run();