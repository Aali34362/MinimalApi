using System.Text;

namespace ExcelCRUD;

public class CsvService
{
    private readonly string _filePath = "Data/people.csv";

    public CsvService(IWebHostEnvironment env)
    {
        _filePath = Path.Combine(
            env.WebRootPath,       // wwwroot
            "ReadExcelFile",       // folder
            "Person.csv"           // file
        );

        if (!File.Exists(_filePath))
        {
            Directory.CreateDirectory(
                Path.GetDirectoryName(_filePath)!
            );

            File.WriteAllText(_filePath, "Id,Name,Age\n");
        }
    }

    public List<Person> GetAll()
    {
        return File.ReadAllLines(_filePath)
            .Skip(1)
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .Select(line =>
            {
                var data = line.Split(',');
                return new Person
                {
                    Id = int.Parse(data[0]),
                    Name = data[1],
                    Age = int.Parse(data[2])
                };
            }).ToList();
    }

    public void SaveAll(List<Person> people)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Id,Name,Age");

        foreach (var p in people)
            sb.AppendLine($"{p.Id},{p.Name},{p.Age}");

        File.WriteAllText(_filePath, sb.ToString());
    }
}
