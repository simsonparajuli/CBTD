namespace CBTD.Models.Interfaces
{
    public interface IUnitOfWork
    {
        public IGenericRepository<Category> Category { get; }

        public IGenericRepository<Manufacturer> Manufacturer { get; }

        //ADD other Models/Tables here as you create them

        //save changes to the data source
        int Commit();

        Task<int> CommitAsync();

    }
}
