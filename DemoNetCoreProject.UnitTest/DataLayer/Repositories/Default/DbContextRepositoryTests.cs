using AutoFixture;
using AutoFixture.AutoMoq;
using DemoNetCoreProject.DataLayer.Entities;
using DemoNetCoreProject.DataLayer.Repositories.Default;
using DemoNetCoreProject.DataLayer.Services;
using DemoNetCoreProject.UnitTest.Helper;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;

namespace DemoNetCoreProject.UnitTest.DataLayer.Repositories.Default;

[TestClass]
public class DbContextRepositoryTests
{
    private readonly IFixture _fixture;
    private readonly Mock<DefaultDbContext> _mockDbContext;
    //private readonly Mock<DbSet<Person>> _mockDbSetPerson;
    //private readonly Mock<DbSet<Address>> _mockDbSetAddress;
    private readonly DbContextRepository _repository;

    public DbContextRepositoryTests()
    {
        _fixture = new Fixture()
            .Customize(new CommonCustomization())
            .Customize(new AutoMoqCustomization());

        var options = new DbContextOptionsBuilder<DefaultDbContext>().Options;
        _mockDbContext = new Mock<DefaultDbContext>(options);

        //_mockDbSetPerson = new Mock<DbSet<Person>>();
        //_mockDbContext.Setup(x => x.People).Returns(_mockDbSetPerson.Object);

        //_mockDbSetAddress = new Mock<DbSet<Address>>();
        //_mockDbContext.Setup(x => x.Addresses).Returns(_mockDbSetAddress.Object);

        _fixture.Register(() => _mockDbContext.Object);

        _repository = _fixture.Create<DbContextRepository>();
    }

    [TestMethod]
    public async Task GetPerson_ShouldReturnPersonWithName()
    {
        // Arrange
        var testData = new List<Person>
        {
            _fixture.Build<Person>().With(p => p.Name, "Name1").Create(),
            _fixture.Build<Person>().With(p => p.Name, "Name1").Create(),
            _fixture.Build<Person>().With(p => p.Name, "Name2").Create()
        };

        var mockDbSet = testData.AsQueryable().BuildMockDbSet();
        _mockDbContext.Setup(x => x.People).Returns(mockDbSet.Object);

        // Act
        var result1 = await _repository.GetPerson("Name1");
        var result2 = await _repository.GetPerson("Name2");

        // Assert
        Assert.AreEqual(2, result1.Count);
        Assert.AreEqual(1, result2.Count);
        Assert.IsTrue(result1.All(p => p.Name == "Name1"));
        Assert.IsTrue(result2.All(p => p.Name == "Name2"));
    }

    [TestMethod]
    public async Task GetPerson_ShouldReturnListOfPerson()
    {
        // Arrange
        var expectedPeople = _fixture.Build<Person>()
            .With(p => p.Name, "Name")
            .CreateMany(3)
            .ToList();
        var mockDbSet = expectedPeople.AsQueryable().BuildMockDbSet();
        _mockDbContext.Setup(x => x.People).Returns(mockDbSet.Object);

        // Act
        var result = await _repository.GetPerson();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(expectedPeople.Count, result.Count);
        CollectionAssert.AreEqual(expectedPeople, result);
    }

    [TestMethod]
    public async Task GetPersonWithAddress_ShouldReturnListOfPersonWithAddresses()
    {
        // Arrange
        var expectedPeople = _fixture.Build<Person>()
            .With(p => p.Name, "Name")
            .With(p => p.Addresses, _fixture.Build<Address>()
                .With(p => p.Text, "Name")
                .CreateMany(2)
                .ToList())
            .CreateMany(3)
            .ToList();
        var mockDbSet = expectedPeople.AsQueryable().BuildMockDbSet();
        _mockDbContext.Setup(x => x.People).Returns(mockDbSet.Object);

        // Act
        var result = await _repository.GetPersonWithAddress();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(expectedPeople.Count, result.Count);
        foreach (var person in result)
        {
            Assert.IsNotNull(person.Addresses);
            Assert.AreEqual(2, person.Addresses.Count);
        }
    }

    [TestMethod]
    public async Task GetAddress_ShouldReturnListOfAddress()
    {
        // Arrange
        var expectedAddresses = _fixture.Build<Address>()
            .With(p => p.Text, "Text")
            .CreateMany(3)
            .ToList();
        var mockDbSet = expectedAddresses.AsQueryable().BuildMockDbSet();
        _mockDbContext.Setup(x => x.Addresses).Returns(mockDbSet.Object);

        // Act
        var result = await _repository.GetAddress();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(expectedAddresses.Count, result.Count);
        CollectionAssert.AreEqual(expectedAddresses, result);
    }

    [TestMethod]
    public async Task GetAddressWithPerson_ShouldReturnListOfAddressWithPerson()
    {
        // Arrange
        var expectedAddresses = _fixture.Build<Address>()
            .With(p => p.Text, "Text")
            .With(p => p.Person, _fixture.Build<Person>()
                .With(p => p.Name, "Name")
                .Create())
            .CreateMany(3)
            .ToList();
        var mockDbSet = expectedAddresses.AsQueryable().BuildMockDbSet();
        _mockDbContext.Setup(x => x.Addresses).Returns(mockDbSet.Object);

        // Act
        var result = await _repository.GetAddressWithPerson();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(expectedAddresses.Count, result.Count);
        foreach (var address in result)
        {
            Assert.IsNotNull(address.Person);
        }
    }
} 