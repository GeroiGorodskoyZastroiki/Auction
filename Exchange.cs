public enum Currency
{
    Rubles,
    Virts
}

public class CurrencyAmount
{
    public Currency Currency {get; private set;}
    public float Amount;

    public CurrencyAmount(Currency currency, float amount)
    {
        Currency = currency;
        Amount = amount;
    }
}

public class Exchange
{
    public Currency From {get; private set;}
    public Currency To {get; private set;}
    public float Rate {get; private set;}

    public Exchange(Currency from, Currency to, float rate)
    {
        From = from;
        To = to;
        Rate = rate;
    }
}