namespace WebApplicationSuper;

public class Person
{
    public string Fio { get; set; }
    public Guid Id { get; set; }

    public Person(Guid id, string fio)
    {
        Id = id;
        Fio = fio;
    }

    public Person()
    {
        
    }
}