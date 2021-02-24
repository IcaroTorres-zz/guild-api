namespace Infrastructure.Persistence.Entities
{
    public class Guild : Domain.Models.Guild
    {
        public override string Name { get; protected set; }
    }
}