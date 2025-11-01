#region

using AICore.Entities;

#endregion

namespace AICore.Repositories;

public static class UserRepositoryExt
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