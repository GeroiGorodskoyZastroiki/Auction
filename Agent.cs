using System.Collections.Generic;

public class Agent
{
    List<CurrencyAmount> _account = new List<CurrencyAmount>();
    public List<Exchange> _exchanges {get; private set;}

    public Agent(List<CurrencyAmount> account)
    {
        _account = account;
        _exchanges = new List<Exchange>();
    }

    void AddCurrency(CurrencyAmount currencyAmount)
    {
        CurrencyAmount? existingCurrency = _account.Find(x => x.Currency == currencyAmount.Currency);
        if (existingCurrency != null) existingCurrency.Amount += currencyAmount.Amount;
        else _account.Add(currencyAmount);
    }

    public Agent Exchange(List<Agent> agents)
    {
        Agent partner = FindBestOption(agents);
        return partner;
    }

    Agent FindBestOption(List<Agent> agents)
    {
        Agent bestOption = agents[0];
        foreach (var agent in agents)
        {
            if (this == agent) continue;
            if (agent._fromOneToAnother == this._fromOneToAnother) continue;
        }
        return bestOption;
    }
}