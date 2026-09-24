using System.Text.Json;
using PeopleVille.Core.Models;
using PeopleVille.Core.Models.Home;
using PeopleVille.Server.Models;

namespace PeopleVille.Server.Services;

public sealed class SaveService(IWebHostEnvironment environment)
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };

    public async Task<string> SaveAsync(World world)
    {
        string fileName = $"peopleville-{DateTime.UtcNow:dd-MM-yyyy-HH-mm}.json";
        SaveGameDto saveGame = CreateSaveGame(world, fileName);
        string saveDirectory = GetSaveDirectory();
        Directory.CreateDirectory(saveDirectory);

        string filePath = Path.Combine(saveDirectory, fileName);
        string json = JsonSerializer.Serialize(saveGame, JsonOptions);

        await File.WriteAllTextAsync(filePath, json);
        return fileName;
    }

    public SaveGameDto CreateSaveGame(World world, string fileName)
    {
        return new SaveGameDto
        {
            SaveNumber = GetSaveNumber(fileName),
            LastModifiedDate = DateTime.UtcNow,
            GameSave = new WorldSaveDto
            {
                CurrentDateTime = world.currentDateTime,
                Citizens = [.. world.Citizens.Select(c => new CitizenSaveDto
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Birth = c.Birth,
                    Gender = c.Gender,
                    HomeAddress = GetHomeAddress(world, c.HomeId),
                    HomeId = c.HomeId,
                    BankAccount = ToBankAccountDto(c.BankAccount),
                    CurrentLocation = c.CurrentLocation,
                    JobWorkplaceAddress = c.Job?.Workplace.Address,
                    SchoolAddress = c.School?.Address
                })],
                BankAccounts = [.. world.BankAccount.Select(account => new BankAccountSaveDto
                {
                    Balance = account.Balance,
                    Transactions = account.Transactions is null ? null : [.. account.Transactions]
                })],
                Jobs = [.. world.Jobs.Select(job => new JobSaveDto
                {
                    WorkplaceAddress = job.Workplace.Address
                })],
                Workplaces = [.. world.Workplaces.Select(workplace => new WorkplaceSaveDto
                {
                    Address = workplace.Address,
                    JobTitle = workplace.JobTitle,
                    JobCapacity = workplace.JobCapacity,
                    Salary = workplace.Salary,
                    WorkStartTime = workplace.WorkStartTime,
                    WorkEndTime = workplace.WorkEndTime
                })],
                ShoppingCenters = [.. world.ShoppingCenters.Select(center => new ShoppingCenterSaveDto
                {
                    Address = center.Address,
                    JobTitle = center.JobTitle,
                    JobCapacity = center.JobCapacity,
                    Salary = center.Salary,
                    WorkStartTime = center.WorkStartTime,
                    WorkEndTime = center.WorkEndTime,
                    FoodPrice = center.FoodPrice,
                    WaterPrice = center.WaterPrice
                })],
                Houses = [.. world.Houses.Select(house => new HouseSaveDto
                {
                    Address = house.Address,
                    CitizenCapacity = house.CitizenCapacity,
                    FoodInventory = house.FoodInventory,
                    WaterInventory = house.WaterInventory,
                    HomeId = house.HomeId,
                    BankAccount = ToBankAccountDto(house.HouseholdFunds)
                })],
                Apartments = [.. world.Apartments.Select(apartment => new ApartmentSaveDto
                {
                    Address = apartment.Address,
                    Rent = apartment.Rent,
                    Floors = apartment.Floors,
                    CitizenCapacity = apartment.CitizenCapacity,
                    FoodInventory = apartment.FoodInventory,
                    WaterInventory = apartment.WaterInventory,
                    HomeId = apartment.HomeId,
                    BankAccount = ToBankAccountDto(apartment.HouseholdFunds)
                })],
                Schools = [.. world.Schools.Select(school => new SchoolSaveDto
                {
                    Address = school.Address,
                    CitizenCapacity = school.CitizenCapacity,
                    StartTime = school.StartTime,
                    EndTime = school.EndTime,
                    FoodInventory = school.FoodInventory,
                    WaterInventory = school.WaterInventory
                })]
            }
        };
    }

    private string GetSaveDirectory()
    {
        return Path.GetFullPath(Path.Combine(
            environment.ContentRootPath,
            "..",
            "PeopleVille.Core",
            "saves"));
    }

    private static int GetSaveNumber(string fileName)
    {
        string name = Path.GetFileNameWithoutExtension(fileName);
        return int.TryParse(name, out int saveNumber) ? saveNumber : 1;
    }

    private static BankAccountSaveDto ToBankAccountDto(BankAccount account)
    {
        return new BankAccountSaveDto
        {
            Balance = account.Balance,
            Transactions = account.Transactions is null ? null : [.. account.Transactions]
        };
    }

    private static string GetHomeAddress(World world, int? homeId)
    {
        return world.Houses.Cast<Building>()
            .Concat(world.Apartments)
            .FirstOrDefault(home => home.HomeId == homeId)?.Address ?? string.Empty;
    }
}
