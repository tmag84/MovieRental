namespace MovieRental.PaymentProviders
{
    public class MbWayProvider : IPaymentProvider
    {
        public string GetProviderName() => "MbWay";
        public Task<bool> Pay(double price)
        {
            //ignore this implementation
            return Task.FromResult<bool>(true);
        }
    }
}
