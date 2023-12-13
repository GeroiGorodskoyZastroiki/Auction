using System.Collections.Generic;

public class Agent
{
    public int ID {get; private set;}
    public List<CurrencyAmount> Account {get; private set;}
    public List<Exchange> Exchanges {get; private set;}

    public Agent(int id)
    {
        this.ID = id;
        Account = new();
        Exchanges = new();
    }

    public void AddCurrency(CurrencyAmount currencyAmount)
    {
        CurrencyAmount? existingCurrency = Account.Find(x => x.Currency == currencyAmount.Currency);
        if (existingCurrency != null) existingCurrency.Amount += currencyAmount.Amount;
        else Account.Add(currencyAmount);
    }

    public void WithdrawCurrency(CurrencyAmount currencyAmount)
    {
        CurrencyAmount existingCurrency = Account.Single(x => x.Currency == currencyAmount.Currency);
        existingCurrency.Amount -= currencyAmount.Amount;
    }
}