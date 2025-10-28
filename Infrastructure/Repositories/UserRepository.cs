using AICore.Entities;
using AICore.Repositories;
using Infrastructure.Entities;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository(AppDbContext context, IMapper mapper) : IRepository<User, long>
{
    public async Task<User?> GetById(long id)
    {
        var userDto = await GetDtoById(id);
        return userDto is null ? null : mapper.Map<User>(userDto);
    }

    public async Task Add(User entity)
    {
        var userDto = mapper.Map<UserDto>(entity);
        await context.Users.AddAsync(userDto);

        await context.SaveChangesAsync();
    }

    public async Task Delete(long id)
    {
        await context.Users
            .Where(dto => dto.Id == id)
            .ExecuteDeleteAsync();

        await context.SaveChangesAsync();
    }

    public async Task Update(User entity)
    {
        var userDto = await GetDtoById(entity.Id);

        if (userDto is null)
            throw new ArgumentException($"User {entity} is not in database");

        mapper.Map(entity, userDto);
        await context.SaveChangesAsync();
    }

    private async Task<UserDto?> GetDtoById(long id)
    {
        return await context.Users
            // .Include(dto => dto.Models)
            .SingleOrDefaultAsync(dto => dto.Id == id);
    }
}