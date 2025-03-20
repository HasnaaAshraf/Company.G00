namespace Company.G00.PL.Services
{
    public interface ITransentService
    {
        public Guid Guid { get; set; }

        string GetGuid();
    }
}
