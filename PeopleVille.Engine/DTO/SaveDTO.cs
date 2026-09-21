public class GameSave
{
    public int SaveNumber { get; set; }
    public DateTime LastModifiedDate { get; set; }
    public string SaveName { get; set; }
    public WorldSaveData GameSave { get; set; } = new();
}

public class WorldSaveData
{
    public DateTime CurrentDateTime { get; set; }
    public List<CitizenSave> Citizens { get; set; } = [];
    public List<HouseSave> Houses { get; set; } = [];
    public List<ApartmentSave> Apartments { get; set; } = [];
    public List<JobSaves> Jobs { get; set; } = [];
}

public class CitizenSave {
    public int Id { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public DateTime Birth { get; set; }
    public int Gender { get; set; }
    public string HomeAddress { get; set; } = "";
    public string CurrentLocation { get; set; } = "";
    public int? JobId { get; set; }
}

public class HouseSave
{
    public int CitizenCapacity { get; set; }
    public override required string Address { get; set; } = "";
    public int FoodInventory { get; set; }
    public int WaterInventory { get; set; }
    public BankAccount BankAccount { get; set; } = new();
}

public class ApartmentSave
{
    public decimal Rent { get; set; }
    public int Floors { get; set; }
    public int CitizenCapacity { get; set; }
    public override required string Address { get; set; } = "";
    public int FoodInventory { get; set; }
    public int WaterInventory { get; set; }
    public BankAccount BankAccount { get; set; } = new();
}

public class JobSave
{
    public int Id { get; set; }
    public string WorkplaceAddress { get; set; } = "";
} 
