using FakeItEasy;
using ParallAI.Core.Entities;
using ParallAI.Core.Repositories;

namespace ParallAI.Core.Tests.UserTests;

[TestFixture]
public class UserRepositoryExtTests
{
    private IRepository<User, long> repository;
    private long id;
    private User user;

    [SetUp]
    public void Setup()
    {
        repository = A.Fake<IRepository<User, long>>();
        id = 1;
        user = new User(id);
    }

    [Test]
    public async Task GetOrCreate_GetExistsUser()
    {
        A.CallTo(() => repository.GetById(A<long>._)).Returns(user);
        var userFromRepo = await repository.GetOrCreate(id);
        Assert.That(userFromRepo, Is.EqualTo(user));
    }

    [Test]
    public async Task GetOrCreate_CreateNewUser()
    {
        User? returnValue = null;
        A.CallTo(() => repository.GetById(A<long>._)).Returns(returnValue);

        var createdUser = await repository.GetOrCreate(id);
        A.CallTo(() => repository.Add(createdUser)).MustHaveHappened();
        Assert.That(createdUser.Id, Is.EqualTo(id));
    }
}