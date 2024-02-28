namespace Meliora.Domain.Models.Petz;

public class Person
{
    protected Person()
    {
        
    }
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public int Age { get; set; }

    // Navigation property for the relationship to PersonPet
    public IEnumerable<Pet>? Pets { get; set; }
}
