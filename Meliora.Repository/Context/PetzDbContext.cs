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

        // Configuring Many-to-Many relationship between People and Pets
        modelBuilder.Entity<Person>()
            .HasMany(p => p.Pets)
            .WithMany(p => p.People)
            .UsingEntity<PersonPet>(
                j => j
                    .HasOne(pp => pp.Pet)
                    .WithMany()
                    .HasForeignKey(pp => pp.PetId)
                    .HasConstraintName("FK_PersonPets_Pets"),
                j => j
                    .HasOne(pp => pp.Person)
                    .WithMany()
                    .HasForeignKey(pp => pp.PersonId)
                    .HasConstraintName("FK_PersonPets_People"),
                j =>
                {
                    j.ToTable("PersonPet");
                    j.Property(pp => pp.PersonId).HasColumnName("person_id");
                    j.Property(pp => pp.PetId).HasColumnName("pet_id");
                    j.HasKey(pp => new { pp.PersonId, pp.PetId });
                }
            );

        // People Entity
        modelBuilder.Entity<Person>()
            .ToTable("People")
            .HasKey(p => p.Id);
        modelBuilder.Entity<Person>()
            .Property(p => p.FirstName)
            .IsRequired()
            .HasMaxLength(255)
            .HasColumnName("first_name");
        modelBuilder.Entity<Person>()
            .Property(p => p.LastName)
            .IsRequired()
            .HasMaxLength(255)
            .HasColumnName("last_name");
        modelBuilder.Entity<Person>()
            .HasMany(p => p.Pets)
            .WithMany(p => p.People)
            .UsingEntity(j => j.ToTable("PersonPet"));

        // Pets Entity
        modelBuilder.Entity<Pet>()
            .ToTable("Pets")
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
    }
}