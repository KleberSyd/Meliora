using Meliora.Repository.Context;
using Meliora.Services.Petz.Dto;
using Microsoft.EntityFrameworkCore;

namespace Meliora.Services.Petz;

public class PetzService(PetzDbContext context) : IPetzService
{
    public async Task<List<PetDto>?> GetPetsByRawSqlAsync()
    {
        const string sqlQuery = """
                                SELECT TOP (1000) [Petz].[dbo].[Pets].[id]
                                      ,[Petz].[dbo].[Pets].[name]
                                      ,[Petz].[dbo].[Pets].[breed]
                                      ,[Petz].[dbo].[Pets].[age]
                                      ,[Petz].[dbo].[Pets].[dead]
                                FROM [Petz].[dbo].[Pets]
                                JOIN [Petz].[dbo].[PersonPet] ON [Petz].[dbo].[PersonPet].[pet_id] = [Petz].[dbo].[Pets].[id]
                                JOIN [Petz].[dbo].[People] ON [Petz].[dbo].[PersonPet].[person_id] = [Petz].[dbo].[People].[id]
                                WHERE [Petz].[dbo].[Pets].[dead] = 0 and [Petz].[dbo].[People].[age] > 50
                                ORDER BY [Petz].[dbo].[People].[first_name]
                                                                
                                """;
        return await context.Pets.FromSqlRaw(sqlQuery).Select(p => new PetDto
        {
            Name = p.Name,
            Breed = p.Breed,
            Age = p.Age
        }).ToListAsync();
    }

    public async Task<List<PetDto>> GetPetsByLambdaAsync()
    {
        return await (from pet in context.Pets
            join personPet in context.PersonPets on pet.Id equals personPet.PetId
            join person in context.People on personPet.PersonId equals person.Id
            where !pet.Dead && person.Age > 50
            orderby person.FirstName
            select new PetDto
            {
                Name = pet.Name,
                Breed = pet.Breed,
                Age = pet.Age
            }).ToListAsync();
    }
}
