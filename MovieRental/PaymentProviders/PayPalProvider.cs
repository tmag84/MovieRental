namespace MovieRental.PaymentProviders
{
    public class PayPalProvider : IPaymentProvider
    {
        public string GetProviderName() => "Paypal";
        public Task<bool> Pay(double price)
        {
            //ignore this implementation
            return Task.FromResult<bool>(true);
        }
    }
}
