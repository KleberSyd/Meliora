namespace Meliora.Domain.Models.Petz;

public class Pet
{
    protected Pet()
    {
        
    }
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Breed { get; set; }
    public int Age { get; set; }
    public bool Dead { get; set; }

    // Navigation property for the relationship to PersonPet
    public IEnumerable<Person>? People { get; set; }
}
