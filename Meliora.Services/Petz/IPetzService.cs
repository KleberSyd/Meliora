using Meliora.Services.Petz.Dto;

namespace Meliora.Services.Petz;

public interface IPetzService
{
    Task<List<PetDto>?> GetPetsByRawSqlAsync();
    Task<List<PetDto>> GetPetsByLambdaAsync();
}