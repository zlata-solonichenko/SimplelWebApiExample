using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using FileHelper = System.IO.File;

namespace WebApplicationSuper.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonController : ControllerBase
{
    /// <summary>
    /// Добавление человека в файл
    /// </summary>
    /// <returns></returns>
     [HttpPost("create")]
     public async Task<IActionResult> CreateNewPerson([FromBody] Person input)
     {
         if (input == null)
         {
             return BadRequest();
         }
         
         input.Id = Guid.NewGuid();
         var json  = FileHelper.ReadAllText("Db.json");
         var persons = JsonSerializer.Deserialize<List<Person>>(json);
         persons.Add(input);

         var options = new JsonSerializerOptions
         {
             WriteIndented = true
         };
         json = JsonSerializer.Serialize(persons, options);
         FileHelper.WriteAllText("Db.json", json, Encoding.UTF8);
         
         return Ok();
     }

    [HttpGet("getAllPerson")]
    public async Task<String> GetAllPerson()
    {
        var json  = FileHelper.ReadAllText("Db.json");
        var persons = JsonSerializer.Deserialize<List<Person>>(json);
        
        var result = "";
        foreach (var person in persons)
        {
            result = $"{person.Id} - {person.Fio}";
        }
        return result;
    }

    [HttpGet("getPerson")]
    public async Task<String> GetPerson(Guid? id)
    {
        var json  = FileHelper.ReadAllText("Db.json");
        var persons = JsonSerializer.Deserialize<List<Person>>(json);
        
        var person = persons.FirstOrDefault(p => p.Id == id);
    
        if (person == null)
            return "Error";
        
        return $"{person.Id} - {person.Fio}";
    }
    
    [HttpPut("update")]
    public async Task Put(Guid? id, string newFio)
    {
        var json  = FileHelper.ReadAllText("Db.json");
        var persons = JsonSerializer.Deserialize<List<Person>>(json);
        
        if (persons != null)
        {
            var personForUpdate = persons.FirstOrDefault(u => u.Id == id);
            
            if (personForUpdate != null)
            {
                personForUpdate.Fio = newFio;
            }
            
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            json = JsonSerializer.Serialize(persons, options);
            FileHelper.WriteAllText("Db.json", json, Encoding.UTF8);
        }
    }
    
    [HttpDelete("delete")]
    public async Task<IActionResult> Delete(Guid? id)
    {
        var json  = FileHelper.ReadAllText("Db.json");
        var persons = JsonSerializer.Deserialize<List<Person>>(json);
        
        var person = persons.FirstOrDefault(p => p.Id == id);
    
        if (person != null) //если нашли пользователя, то удаляем
        {
            persons.Remove(person);
            
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            json = JsonSerializer.Serialize(persons, options);
            FileHelper.WriteAllText("Db.json", json, Encoding.UTF8);
        }
        else
        {
            return BadRequest();
        }

        return Ok();
    }
}