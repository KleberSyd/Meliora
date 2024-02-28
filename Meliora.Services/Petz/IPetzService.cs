using Meliora.Services.Petz.Dto;

namespace Meliora.Services.Petz;

public interface IPetzService
{
    /// <summary>
    /// Retrieves a list of pets using raw SQL query.
    /// </summary>
    /// <returns>The list of pets.</returns>
    Task<List<PetDto>?> GetPetsByRawSqlAsync();

    /// <summary>
    /// Retrieves a list of pets using LINQ query.
    /// </summary>
    /// <returns>The list of pets.</returns>
    Task<List<PetDto>> GetPetsByLambdaAsync();

    /// <summary>
    /// Retrieves a list of breed counts for pets.
    /// </summary>
    /// <returns>The list of breed counts.</returns>
    Task<List<BreedCountDto>> GetBreedCountsAsync();
}