namespace MovieRental.PaymentProviders
{
    public interface IPaymentProvider
    {
        string GetProviderName();
        Task<bool> Pay(double price);
    }
}
