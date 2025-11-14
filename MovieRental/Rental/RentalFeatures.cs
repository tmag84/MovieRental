using MovieRental.Data;
using MovieRental.PaymentProviders;

namespace MovieRental.Rental
{
	public class RentalFeatures : IRentalFeatures
	{
		private readonly MovieRentalDbContext _movieRentalDb;
		public RentalFeatures(MovieRentalDbContext movieRentalDb)
		{
			_movieRentalDb = movieRentalDb;
		}

		private IPaymentProvider? GetPaymentProvider(string? paymentMethod)
		{
			return paymentMethod switch
			{
				"MbWay" => new MbWayProvider(),
				"Paypal" => new PayPalProvider(),
				_ => null
			};
		}

		//TODO: make me async :(
		public async Task<Rental> Save(Rental rental)
		{
			var paymentProvider = GetPaymentProvider(rental?.PaymentMethod);
			if (paymentProvider != null)
			{
				if (await paymentProvider.Pay(rental!.RentalPrice)) 
				{
                    _movieRentalDb.Rentals.Add(rental);
                    await _movieRentalDb.SaveChangesAsync();
                    return rental;
                }
			}
			// this is a very generic exception, we can give much more detailed information regarding the failure
			throw new Exception($"The rental failed.");			
		}

		//TODO: finish this method and create an endpoint for it
		public IEnumerable<Rental> GetRentalsByCustomerName(int customerId)
		{
			return _movieRentalDb.Rentals
				.Where(r => r.CustomerId == customerId)
				.ToList();
		}



	}
}
