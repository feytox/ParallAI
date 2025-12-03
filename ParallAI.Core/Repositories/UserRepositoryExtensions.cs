using ParallAI.Core.Entities;

namespace ParallAI.Core.Repositories;

public static class UserRepositoryExtensions
{
    public static async Task<User> GetOrCreate(this IRepository<User, long> repository, long id)
    {
        var user = await repository.GetById(id);
        if (user is not null)
            return user;

        user = new User(id);
        await repository.Add(user);
        return user;
    }
}