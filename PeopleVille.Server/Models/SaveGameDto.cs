using PeopleVille.Core.Data;
using PeopleVille.Core.Enum;

namespace PeopleVille.Server.Models;

public sealed class SaveGameDto
{
    public int SaveVersion { get; set; } = 1;
    public int SaveNumber { get; set; }
    public DateTime LastModifiedDate { get; set; }
    public WorldSaveDto GameSave { get; set; } = new();
}

public sealed class WorldSaveDto
{
    public DateTime CurrentDateTime { get; set; }
    public List<CitizenSaveDto> Citizens { get; set; } = [];
    public List<BankAccountSaveDto> BankAccounts { get; set; } = [];
    public List<JobSaveDto> Jobs { get; set; } = [];
    public List<WorkplaceSaveDto> Workplaces { get; set; } = [];
    public List<ShoppingCenterSaveDto> ShoppingCenters { get; set; } = [];
    public List<HouseSaveDto> Houses { get; set; } = [];
    public List<ApartmentSaveDto> Apartments { get; set; } = [];
    public List<SchoolSaveDto> Schools { get; set; } = [];
}

public sealed class CitizenSaveDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime Birth { get; set; }
    public Genders Gender { get; set; }
    public int? HomeId { get; set; }
    public BankAccountSaveDto BankAccount { get; set; } = new();
    public string HomeAddress { get; set; } = string.Empty;
    public string CurrentLocation { get; set; } = string.Empty;
    public string? JobWorkplaceAddress { get; set; }
    public string? SchoolAddress { get; set; }
}

public sealed class BankAccountSaveDto
{
    public string? OwnerAddress { get; set; }
    public decimal Balance { get; set; }
    public List<int>? Transactions { get; set; }
}

public sealed class JobSaveDto
{
    public string WorkplaceAddress { get; set; } = string.Empty;
}

public class WorkplaceSaveDto
{
    public string Address { get; set; } = string.Empty;
    public JobTitle JobTitle { get; set; }
    public int JobCapacity { get; set; }
    public int Salary { get; set; }
    public int WorkStartTime { get; set; }
    public int WorkEndTime { get; set; }
}

public sealed class ShoppingCenterSaveDto : WorkplaceSaveDto
{
    public decimal FoodPrice { get; set; }
    public decimal WaterPrice { get; set; }
}

public sealed class HouseSaveDto
{
    public int HomeId { get; set; }
    public string Address { get; set; } = string.Empty;
    public int CitizenCapacity { get; set; }
    public int FoodInventory { get; set; }
    public int WaterInventory { get; set; }
    public BankAccountSaveDto BankAccount { get; set; } = new();
}

public sealed class ApartmentSaveDto
{
    public int HomeId { get; set; }
    public string Address { get; set; } = string.Empty;
    public decimal Rent { get; set; }
    public int Floors { get; set; }
    public int CitizenCapacity { get; set; }
    public int FoodInventory { get; set; }
    public int WaterInventory { get; set; }
    public BankAccountSaveDto BankAccount { get; set; } = new();
}

public sealed class SchoolSaveDto
{
    public string Address { get; set; } = string.Empty;
    public int CitizenCapacity { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int FoodInventory { get; set; }
    public int WaterInventory { get; set; }
}