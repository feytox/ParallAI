using AICore;
using AICore.Repositories;
using Infrastructure.Entities;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

// TODO: deduplicate??? (issue #26)
public class AiModelRepository(AppDbContext context, IMapper mapper) : IRepository<AiModel, Guid>
{
    public async Task<AiModel?> GetById(Guid id)
    {
        var userDto = await context.Models.FindAsync(id);
        return userDto is null ? null : mapper.Map<AiModel>(userDto);
    }

    public async Task Add(AiModel entity)
    {
        var userDto = mapper.Map<AiModelDto>(entity);
        await context.Models.AddAsync(userDto);

        await context.SaveChangesAsync();
    }

    public async Task Delete(Guid id)
    {
        await context.Models
            .Where(dto => dto.Id == id)
            .ExecuteDeleteAsync();

        await context.SaveChangesAsync();
    }

    public async Task Update(AiModel entity)
    {
        var userDto = await context.Models.FindAsync(entity.Id);

        if (userDto is null)
            throw new ArgumentException($"AiModel {entity} is not in database");

        mapper.Map(entity, userDto);
        await context.SaveChangesAsync();
    }
}