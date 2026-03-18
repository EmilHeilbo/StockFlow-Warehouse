using Microsoft.EntityFrameworkCore;
using StockFlow_Warehouse.Model;
using StockFlow_Warehouse.Repositories;

namespace TestWarehouse;

public class ProductRepositoryTests
{
    private AppDbContext _db = null!;
    private ProductRepository _repo = null!;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _db = new AppDbContext(options);
        _repo = new ProductRepository(_db);
    }

    [TearDown]
    public void TearDown() => _db.Dispose();

    // GetAll

    [Test]
    public async Task GetAll_EmptyDatabase_ReturnsEmptyList()
    {
        var result = await _repo.GetAll();
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetAll_WithProducts_ReturnsAll()
    {
        _db.Products.AddRange(
            new Product { Name = "Beans" },
            new Product { Name = "Cola" }
        );
        await _db.SaveChangesAsync();

        var result = await _repo.GetAll();

        Assert.That(result, Has.Count.EqualTo(2));
        // TODO test names
    }

    [Test]
    public async Task GetAll_IncludesCategories()
    {
        var category = new Category { Name = "Food" };
        _db.Products.Add(new Product { Name = "Beans", Categories = [category] });
        await _db.SaveChangesAsync();

        var result = await _repo.GetAll();

        Assert.That(result[0].Categories, Has.Count.EqualTo(1));
        Assert.That(result[0].Categories[0].Name, Is.EqualTo("Food"));
    }

    // GetById

    [Test]
    public async Task GetById_ExistingId_ReturnsProduct()
    {
        var product = new Product { Name = "Beans" };
        _db.Products.Add(product);
        await _db.SaveChangesAsync();

        var result = await _repo.GetById(product.Id);

        Assert.That(result, Is.Not.Null);
        // It is allowed to assume result is not null because it was tested abouve
        // Therefrore we can safely use result!.Name
        Assert.That(result!.Name, Is.EqualTo("Beans"));
    }

    [Test]
    public async Task GetById_NonExistentId_ReturnsNull()
    {
        var result = await _repo.GetById(Guid.NewGuid());
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetById_IncludesCategories()
    {
        var category = new Category { Name = "Food" };
        var product = new Product { Name = "Beans", Categories = [category] };
        _db.Products.Add(product);
        await _db.SaveChangesAsync();

        var result = await _repo.GetById(product.Id);

        Assert.That(result!.Categories, Has.Count.EqualTo(1));
        // TODO: Name check
    }

    // GetAllInCategory

    [Test]
    public async Task GetAllInCategory_ReturnsOnlyProductsInThatCategory()
    {
        var food = new Category { Name = "Food" };
        var tech = new Category { Name = "Tech" };
        _db.Products.AddRange(
            new Product { Name = "Beans", Categories = [food] },
            new Product { Name = "Laptop", Categories = [tech] }
        );
        await _db.SaveChangesAsync();

        var result = await _repo.GetAllInCategory(food);

        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Name, Is.EqualTo("Beans"));
    }

    [Test]
    public async Task GetAllInCategory_NoMatchingProducts_ReturnsEmptyList()
    {
        var food = new Category { Name = "Food" };
        var tech = new Category { Name = "Tech" };
        _db.Products.Add(new Product { Name = "Laptop", Categories = [tech] });
        await _db.SaveChangesAsync();

        var result = await _repo.GetAllInCategory(food);

        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetAllInCategory_ProductInMultipleCategories_IsReturned()
    {
        var food = new Category { Name = "Food" };
        var organic = new Category { Name = "Organic" };
        _db.Products.Add(new Product { Name = "Beans", Categories = [food, organic] });
        await _db.SaveChangesAsync();

        var result = await _repo.GetAllInCategory(organic);

        Assert.That(result, Has.Count.EqualTo(1));
        // TODO: Name check
    }

    // Create

    [Test]
    public async Task Create_AddsProductToDatabase()
    {
        var product = new Product { Name = "Beans" };

        await _repo.Create(product);

        Assert.That(await _db.Products.CountAsync(), Is.EqualTo(1));
    }

    [Test]
    public async Task Create_ProductIsRetrievableAfterCreation()
    {
        var product = new Product { Name = "Beans" };

        await _repo.Create(product);

        var stored = await _db.Products.FindAsync(product.Id);
        Assert.That(stored, Is.Not.Null);
        Assert.That(stored!.Name, Is.EqualTo("Beans"));
    }

    // Delete

    [Test]
    public async Task Delete_ExistingProduct_RemovesItFromDatabase()
    {
        var product = new Product { Name = "Beans" };
        _db.Products.Add(product);
        await _db.SaveChangesAsync();

        await _repo.Delete(product.Id);

        Assert.That(await _db.Products.CountAsync(), Is.EqualTo(0));
    }

    [Test]
    public async Task Delete_NonExistentId_DoesNotThrow()
    {
        Assert.DoesNotThrowAsync(() => _repo.Delete(Guid.NewGuid()));
    }

    [Test]
    public async Task Delete_OnlyDeletesTargetProduct()
    {
        var target = new Product { Name = "Beans" };
        var other = new Product { Name = "Cola" };
        _db.Products.AddRange(target, other);
        await _db.SaveChangesAsync();

        await _repo.Delete(target.Id);

        // Assert that there is still a product in the database
        Assert.That(await _db.Products.CountAsync(), Is.EqualTo(1));
        // Assert that the product is other by finding product with id of other
        Assert.That(await _db.Products.FindAsync(other.Id), Is.Not.Null);
    }

    // Update

    [Test]
    public async Task Update_ChangesProductName()
    {
        var product = new Product { Name = "Old Name" };
        _db.Products.Add(product);
        await _db.SaveChangesAsync();

        product.Name = "New Name";
        await _repo.Update(product);

        var updated = await _db.Products.FindAsync(product.Id);
        Assert.That(updated!.Name, Is.EqualTo("New Name"));
    }

    [Test]
    public async Task Update_NonExistentProduct_DoesNotThrow()
    {
        var ghost = new Product { Name = "Ghost" };
        Assert.DoesNotThrowAsync(() => _repo.Update(ghost));
    }

    [Test]
    public async Task Update_DoesNotAffectOtherProducts()
    {
        var target = new Product { Name = "Target" };
        var other = new Product { Name = "Other" };
        _db.Products.AddRange(target, other);
        await _db.SaveChangesAsync();

        target.Name = "Updated Target";
        await _repo.Update(target);

        var unchanged = await _db.Products.FindAsync(other.Id);
        Assert.That(unchanged!.Name, Is.EqualTo("Other"));
    }
}
