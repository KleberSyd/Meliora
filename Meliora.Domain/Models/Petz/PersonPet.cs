namespace Meliora.Domain.Models.Petz;

public class PersonPet
{
    protected PersonPet()
    {
        
    }
    public int PersonId { get; set; }
    public int PetId { get; set; }

    // Navigation properties
    public required Person Person { get; set; }
    public required Pet Pet { get; set; }
}
