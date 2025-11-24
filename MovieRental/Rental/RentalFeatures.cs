using MovieRental.Data;
using MovieRental.PaymentProviders;

namespace MovieRental.Rental
{
	public class RentalFeatures : IRentalFeatures
	{
		private readonly MovieRentalDbContext _movieRentalDb;
		private readonly IEnumerable<IPaymentProvider> _paymentProviders;
		public RentalFeatures(MovieRentalDbContext movieRentalDb, IEnumerable<IPaymentProvider> paymentProviders)
		{
			_movieRentalDb = movieRentalDb;
			_paymentProviders = paymentProviders;
		}

		private IPaymentProvider? GetPaymentProvider(string? paymentMethod)
		{
			return _paymentProviders.Where(pp => pp.GetProviderName().Equals(paymentMethod)).FirstOrDefault();
		}

		//TODO: make me async :(
		public async Task<Rental> Save(Rental rental)
		{
			var paymentProvider = GetPaymentProvider(rental?.PaymentMethod);
			using (var scope = _movieRentalDb.Database.BeginTransaction())
			{
				try
				{
                    if (paymentProvider != null && rental != null)
                    {
                        _movieRentalDb.Rentals.Add(rental);
                        if (await paymentProvider.Pay(rental!.RentalPrice))
                        {
                            await _movieRentalDb.SaveChangesAsync();
                            return rental;
                        }
                    }
                    throw new InvalidOperationException("Payment was not successfull.");
                }
				catch (Exception ex)
				{
					await scope.RollbackAsync();
                    throw new Exception($"{ex.Message}");
				}
            }
		}

		//TODO: finish this method and create an endpoint for it
		public IEnumerable<Rental> GetRentalsByCustomerName(int customerId)
		{
			return _movieRentalDb.Rentals
				.Where(r => r.Customer != null && r.Customer.Id == customerId)
				.ToList();
		}



	}
}
