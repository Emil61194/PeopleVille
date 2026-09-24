namespace PeopleVille.Core.Models.Home
{
    public abstract class Building
    {
        public virtual int HomeId { get; set; }
        public abstract required string Address { get; set; }
    }
}
