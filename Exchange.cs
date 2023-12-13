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

public static class ExchangeRates
{
    public static Dictionary<Currency, float> сoefficients = new Dictionary<Currency, float>();
    static ExchangeRates()
    {
        var rnd = new Random();
        var currencyList = Enum.GetValues(typeof(Currency)).Cast<Currency>().ToList();
        //генерируем коэффициенты при делении которых будет образовываться ставка по курсу
        for (int i = 0; i < currencyList.Count; i++)
            сoefficients.Add(currencyList[i], rnd.Next(1, 10));
    }
}