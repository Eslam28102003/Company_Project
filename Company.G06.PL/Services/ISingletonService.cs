namespace Company.G06.PL.Services
{
    public interface ISingletonService
    {
        public Guid Guid { get; set; }
        string GetGuid();
    }
}
