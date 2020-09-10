namespace PaymentGateway.Validators
{
    public interface IValidate<T>
    {
        void Validate(T input);
    }


}