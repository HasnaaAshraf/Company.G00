namespace Company.G00.PL.Services
{
    public interface IScopedService
    {
        public Guid Guid { get; set; }

        string GetGuid();
    }
}
