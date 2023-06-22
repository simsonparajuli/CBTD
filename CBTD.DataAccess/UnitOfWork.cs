using CBTD.ApplicationCore.Interfaces;
using CBTD.ApplicationCore.Models;

namespace CBTD.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;  //dependency injection of Data Source

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private IGenericRepository<Category> _Category;

        private IGenericRepository<Manufacturer> _Manufacturer;

        private IGenericRepository<Product> _Product;

		private IGenericRepository<ApplicationUser> _ApplicationUser;

        private IGenericRepository<ShoppingCart> _ShoppingCart;

        private IOrderHeaderRepository<OrderHeader> _OrderHeader;

        private IGenericRepository<OrderDetails> _OrderDetails;


		//ADD ADDITIONAL MODELS HERE

		public IGenericRepository<Category> Category
        {
            get
            {
                if (_Category == null)
                {
                    _Category = new GenericRepository<Category>(_dbContext);
                }

                return _Category;
            }
        }

        //ADD ADDITIONAL METHODS FOR EACH MODEL (similar to Category) HERE

        public IGenericRepository<Manufacturer> Manufacturer
        {
            get
            {

                if (_Manufacturer == null)
                {
                    _Manufacturer = new GenericRepository<Manufacturer>(_dbContext);
                }

                return _Manufacturer;
            }
        }

        public IGenericRepository<Product> Product
        {
            get
            {
                if (_Product == null)
                {
                    _Product = new GenericRepository<Product>(_dbContext);
                }

                return _Product;
            }
        }


		public IGenericRepository<ApplicationUser> ApplicationUser
		{
			get
			{
				if (_ApplicationUser == null) _ApplicationUser = new GenericRepository<ApplicationUser>(_dbContext);
				return _ApplicationUser;
			}
		}

        public IGenericRepository<ShoppingCart> ShoppingCart
        {
            get
            {
                if (_ShoppingCart == null)
                {
                    _ShoppingCart = new GenericRepository<ShoppingCart>(_dbContext);
                }

                return _ShoppingCart;
            }
        }

        public IOrderHeaderRepository<OrderHeader> OrderHeader
        {
            get
            {
                if (_OrderHeader == null)
                {
                    _OrderHeader = new OrderHeaderRepository(_dbContext);
                }

                return _OrderHeader;
            }
        }

        public IGenericRepository<OrderDetails> OrderDetails
        {
            get
            {
                if (_OrderDetails == null)
                {
                    _OrderDetails = new GenericRepository<OrderDetails>(_dbContext);
                }

                return _OrderDetails;
            }
        }

        public int Commit()
        {
            return _dbContext.SaveChanges();
        }

        public async Task<int> CommitAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        //additional method added for garbage disposal

        public void Dispose()
        {
            _dbContext.Dispose();
        }

		int IUnitOfWork.Commit()
		{
			throw new NotImplementedException();
		}

		Task<int> IUnitOfWork.CommitAsync()
		{
			throw new NotImplementedException();
		}
	}
}
