using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace ExcelCRUD;

[ApiController]
[Route("api/[controller]")]
public class PeopleController(CsvService csvService) : ControllerBase
{
    private readonly CsvService _csvService = csvService;

    // READ
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_csvService.GetAll());
    }

    // CREATE
    [HttpPost]
    public IActionResult Create(Person person)
    {
        var people = _csvService.GetAll();
        person.Id = people.Any() ? people.Max(x => x.Id) + 1 : 1;
        people.Add(person);
        _csvService.SaveAll(people);
        return Ok(person);
    }

    // UPDATE
    [HttpPut("{id}")]
    public IActionResult Update(int id, Person person)
    {
        var people = _csvService.GetAll();
        var existing = people.FirstOrDefault(x => x.Id == id);

        if (existing == null)
            return NotFound();

        existing.Name = person.Name;
        existing.Age = person.Age;
        _csvService.SaveAll(people);

        return Ok(existing);
    }

    // DELETE
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var people = _csvService.GetAll();
        var person = people.FirstOrDefault(x => x.Id == id);

        if (person == null)
            return NotFound();

        people.Remove(person);
        _csvService.SaveAll(people);

        return Ok();
    }

    [HttpGet("export")]
    public IActionResult ExportCsv()
    {
        var people = _csvService.GetAll();
        var sb = new StringBuilder();
        sb.AppendLine("Id,Name,Age");

        foreach (var p in people)
            sb.AppendLine($"{p.Id},{p.Name},{p.Age}");

        var bytes = Encoding.UTF8.GetBytes(sb.ToString());

        return File(bytes, "text/csv", "people.csv");
    }
}