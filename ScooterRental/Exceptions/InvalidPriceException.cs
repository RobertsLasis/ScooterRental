namespace ScooterRental.Exceptions
{
    public class InvalidPriceException: Exception
    {
        public InvalidPriceException(): base ("Scooter price can not be negative.")
        {
            
        }
    }
}
