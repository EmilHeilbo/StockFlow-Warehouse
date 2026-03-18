using Microsoft.EntityFrameworkCore;
using StockFlow_Warehouse.Model;
using StockFlow_Warehouse.Repositories;

namespace TestWarehouse;

public class WarehouseRepositoryTests
{
    private AppDbContext _db = null!;
    private WarehouseRepository _repo = null!;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _db = new AppDbContext(options);
        _repo = new WarehouseRepository(_db);
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
    public async Task GetAll_WithWarehouses_ReturnsAll()
    {
        _db.Warehouses.AddRange(
            new Warehouse { Name = "Warehouse A", Address = "Address A" },
            new Warehouse { Name = "Warehouse B", Address = "Address B" }
        );
        await _db.SaveChangesAsync();

        var result = await _repo.GetAll();

        Assert.That(result, Has.Count.EqualTo(2));
    }

    [Test]
    public async Task GetAll_IncludesProducts()
    {
        var product = new Product { Name = "Beans" };
        _db.Warehouses.Add(new Warehouse
        {
            Name = "Warehouse A",
            Address = "Address A",
            Products = [new ProductAmount { Product = product, Amount = 5 }]
        });
        await _db.SaveChangesAsync();

        var result = await _repo.GetAll();

        Assert.That(result[0].Products, Has.Count.EqualTo(1));
        Assert.That(result[0].Products[0].Amount, Is.EqualTo(5));
    }

    // GetById

    [Test]
    public async Task GetById_ExistingId_ReturnsWarehouse()
    {
        var warehouse = new Warehouse { Name = "Warehouse A", Address = "Address A" };
        _db.Warehouses.Add(warehouse);
        await _db.SaveChangesAsync();

        var result = await _repo.GetById(warehouse.Id);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Name, Is.EqualTo("Warehouse A"));
    }

    [Test]
    public async Task GetById_NonExistentId_ReturnsNull()
    {
        var result = await _repo.GetById(Guid.NewGuid());
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetById_IncludesProducts()
    {
        var product = new Product { Name = "Cola" };
        var warehouse = new Warehouse
        {
            Name = "Warehouse A",
            Address = "Address A",
            Products = [new ProductAmount { Product = product, Amount = 3 }]
        };
        _db.Warehouses.Add(warehouse);
        await _db.SaveChangesAsync();

        var result = await _repo.GetById(warehouse.Id);

        Assert.That(result!.Products, Has.Count.EqualTo(1));
        Assert.That(result.Products[0].Amount, Is.EqualTo(3));
    }

    // Create

    [Test]
    public async Task Create_AddsWarehouseToDatabase()
    {
        var warehouse = new Warehouse { Name = "Warehouse A", Address = "Address A" };

        await _repo.Create(warehouse);

        Assert.That(await _db.Warehouses.CountAsync(), Is.EqualTo(1));
    }

    [Test]
    public async Task Create_WarehouseIsRetrievableAfterCreation()
    {
        var warehouse = new Warehouse { Name = "Warehouse A", Address = "Address A" };

        await _repo.Create(warehouse);

        var stored = await _db.Warehouses.FindAsync(warehouse.Id);
        Assert.That(stored, Is.Not.Null);
        Assert.That(stored!.Name, Is.EqualTo("Warehouse A"));
    }

    // Delete

    [Test]
    public async Task Delete_ExistingWarehouse_RemovesItFromDatabase()
    {
        var warehouse = new Warehouse { Name = "Warehouse A", Address = "Address A" };
        _db.Warehouses.Add(warehouse);
        await _db.SaveChangesAsync();

        await _repo.Delete(warehouse.Id);

        Assert.That(await _db.Warehouses.CountAsync(), Is.EqualTo(0));
    }

    [Test]
    public async Task Delete_NonExistentId_DoesNotThrow()
    {
        Assert.DoesNotThrowAsync(() => _repo.Delete(Guid.NewGuid()));
    }

    [Test]
    public async Task Delete_OnlyDeletesTargetWarehouse()
    {
        var target = new Warehouse { Name = "Warehouse A", Address = "Address A" };
        var other = new Warehouse { Name = "Warehouse B", Address = "Address B" };
        _db.Warehouses.AddRange(target, other);
        await _db.SaveChangesAsync();

        await _repo.Delete(target.Id);

        Assert.That(await _db.Warehouses.CountAsync(), Is.EqualTo(1));
        Assert.That(await _db.Warehouses.FindAsync(other.Id), Is.Not.Null);
    }

    // Update

    [Test]
    public async Task Update_ChangesWarehouseName()
    {
        var warehouse = new Warehouse { Name = "Old Name", Address = "Address A" };
        _db.Warehouses.Add(warehouse);
        await _db.SaveChangesAsync();

        warehouse.Name = "New Name";
        await _repo.Update(warehouse);

        var updated = await _db.Warehouses.FindAsync(warehouse.Id);
        Assert.That(updated!.Name, Is.EqualTo("New Name"));
    }

    [Test]
    public async Task Update_ChangesWarehouseAddress()
    {
        var warehouse = new Warehouse { Name = "Warehouse A", Address = "Old Address" };
        _db.Warehouses.Add(warehouse);
        await _db.SaveChangesAsync();

        warehouse.Address = "New Address";
        await _repo.Update(warehouse);

        var updated = await _db.Warehouses.FindAsync(warehouse.Id);
        Assert.That(updated!.Address, Is.EqualTo("New Address"));
    }

    [Test]
    public async Task Update_NonExistentWarehouse_DoesNotThrow()
    {
        var ghost = new Warehouse { Name = "Ghost", Address = "Nowhere" };
        Assert.DoesNotThrowAsync(() => _repo.Update(ghost));
    }

    [Test]
    public async Task Update_DoesNotAffectOtherWarehouses()
    {
        var target = new Warehouse { Name = "Target", Address = "Address A" };
        var other = new Warehouse { Name = "Other", Address = "Address B" };
        _db.Warehouses.AddRange(target, other);
        await _db.SaveChangesAsync();

        target.Name = "Updated Target";
        await _repo.Update(target);

        var unchanged = await _db.Warehouses.FindAsync(other.Id);
        Assert.That(unchanged!.Name, Is.EqualTo("Other"));
    }
}
