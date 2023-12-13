using System.Collections.Generic;
using System.Linq;

public class Agent
{
    public int ID {get; private set;}
    public List<CurrencyAmount> Account {get; private set;}
    public List<Exchange> Exchanges {get; private set;}

    public Agent(int id)
    {
        ID = id;
        Account = new();
        Exchanges = new();
    }

    public void AddCurrency(CurrencyAmount currencyAmount) //начисление денег
    {
        CurrencyAmount? existingCurrency = Account.Find(x => x.Currency == currencyAmount.Currency);
        if (existingCurrency != null) existingCurrency.Amount += currencyAmount.Amount;
        else Account.Add(currencyAmount);
    }

    public void WithdrawCurrency(CurrencyAmount currencyAmount) //списание денег
    {
        CurrencyAmount existingCurrency = Account.Single(x => x.Currency == currencyAmount.Currency);
        existingCurrency.Amount -= currencyAmount.Amount;

    }

    public Agent? FindBestOption() //выбор лучшего варинта для обмена
    {
        List<Agent> potentialAgents = Auction.Instance.Agents
                                            .Where(x => x.Account.Single(y => y.Currency == x.Exchanges[0].From).Amount > 0) //у партнёра должны быть средства на счету
                                            .Where(x => x.Exchanges[0].To == Exchanges[0].From) //совпадают ли валюты
                                            .Where(x => 1/x.Exchanges[0].Rate <= Exchanges[0].Rate) //проверяем минимальную ставку для обмена
                                            .OrderBy(x => x.Exchanges[0].Rate).ToList(); //сортируем в соответствии с выгодой от обмена
        if (potentialAgents.Count == 0) return null; //если такого варинта не нашлось
        return potentialAgents[0];
    }

    public Agent? Exchange()
    {
        Agent? agent = FindBestOption();
        if (agent == null) return agent; //если такого варинта не нашлось

        var possibleAmount = Account.Single(x => x.Currency == Exchanges[0].From).Amount * Exchanges[0].Rate;
        if (possibleAmount > agent.Account.Single(x => x.Currency == Exchanges[0].To).Amount) //выясняем максимальный объём средств для обмена
            possibleAmount = agent.Account.Single(x => x.Currency == Exchanges[0].To).Amount;
        //производим обмен
        AddCurrency(new CurrencyAmount(Exchanges[0].To, possibleAmount));
        WithdrawCurrency(new CurrencyAmount(Exchanges[0].From, possibleAmount/Exchanges[0].Rate));
        agent.AddCurrency(new CurrencyAmount(Exchanges[0].From, possibleAmount/Exchanges[0].Rate));
        agent.WithdrawCurrency(new CurrencyAmount(Exchanges[0].To, possibleAmount));

        return agent;
    }
}