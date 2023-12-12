using System.Collections.Generic;

public class Agent
{
    public List<CurrencyAmount> Account {get; private set;}
    public List<Exchange> Exchanges {get; private set;}

    public Agent()
    {
        Account = new();
        Exchanges = new();
    }

    void AddCurrency(CurrencyAmount currencyAmount)
    {
        CurrencyAmount? existingCurrency = Account.Find(x => x.Currency == currencyAmount.Currency);
        if (existingCurrency != null) existingCurrency.Amount += currencyAmount.Amount;
        else Account.Add(currencyAmount);
    }

    void WithdrawCurrency(CurrencyAmount currencyAmount)
    {
        CurrencyAmount existingCurrency = Account.Single(x => x.Currency == currencyAmount.Currency);
        existingCurrency.Amount -= currencyAmount.Amount;
    }

    public Agent Exchange(List<Agent> agents)
    {
        Agent partner = FindBestOption(agents);
        var agentAccount = this.Account.Single(x => x.Currency == this.Exchanges[0].From);
        var partnerAccount = partner.Account.Single(x => x.Currency == this.Exchanges[0].To);
        //if (partnerAccount.Amount * this.Exchanges[0].Rate < agentAccount.Amount)
        // this.WithdrawCurrency();
        // this.AddCurrency();
        // partner.WithdrawCurrency();
        // partner.AddCurrency();
        return partner;
    }

    Agent FindBestOption(List<Agent> agents)
    {
        Agent? bestOption = null;
        foreach (var agent in agents)
        {
            if (this == agent) continue;
            if (agent.Exchanges[0].From == this.Exchanges[0].To && agent.Exchanges[0].To == this.Exchanges[0].From)
                if (bestOption?.Exchanges[0].Rate < agent.Exchanges[0].Rate) bestOption = agent;
        }
        return bestOption;
    }
}