namespace Meliora.Domain.Models.Petz;

public class PersonPet
{
    protected PersonPet()
    {
        
    }
    public int PersonId { get; set; }
    public int PetId { get; set; }

    // Navigation properties
    public virtual required Person Person { get; set; }
    public virtual required Pet Pet { get; set; }
}
