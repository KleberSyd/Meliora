using Meliora.Domain.Models.Petz;
using Microsoft.EntityFrameworkCore;

namespace Meliora.Repository.Context;

public class PetzDbContext(DbContextOptions<PetzDbContext> options) : DbContext(options)
{
    public required DbSet<Person> People { get; set; }
    public required DbSet<Pet> Pets { get; set; }
    public required DbSet<PersonPet> PersonPets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // People Entity
        modelBuilder.Entity<Person>()
            .HasKey(p => p.Id);
        modelBuilder.Entity<Person>()
            .Property(p => p.FirstName)
            .IsRequired()
            .HasMaxLength(255);
        modelBuilder.Entity<Person>()
            .Property(p => p.LastName)
            .IsRequired()
            .HasMaxLength(255);

        // Pets Entity
        modelBuilder.Entity<Pet>()
            .HasKey(p => p.Id);
        modelBuilder.Entity<Pet>()
            .Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(255);
        modelBuilder.Entity<Pet>()
            .Property(p => p.Breed)
            .IsRequired()
            .HasMaxLength(255);
        modelBuilder.Entity<Pet>()
            .Property(p => p.Dead)
            .IsRequired();

        // PersonPet Entity (Configuring Many-to-Many relationship between People and Pets)
        modelBuilder.Entity<PersonPet>()
            .HasKey(pp => new { pp.PersonId, pp.PetId });
        modelBuilder.Entity<PersonPet>()
            .HasOne(pp => pp.Person)
            .WithMany(p => p.PersonPets)
            .HasForeignKey(pp => pp.PersonId);
        modelBuilder.Entity<PersonPet>()
            .HasOne(pp => pp.Pet)
            .WithMany(p => p.PersonPets)
            .HasForeignKey(pp => pp.PetId);
    }
}