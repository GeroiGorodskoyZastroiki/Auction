using System;
using System.Collections.Generic;
using System.Linq;

public class Auction
{
    int _iterations;
    int _agentsCount;
    AuctionSerializer _auctionSerializer;
    public List<Agent> Agents {get; private set;}
    Random rnd = new Random();

    public Auction(int iterations, int agentsCount)
    {
        _iterations = iterations;
        _agentsCount = agentsCount;
        _auctionSerializer = new(this);
        Agents = new();
        for (int i = 0; i < _agentsCount; i++)
        {
            Agents.Add(new Agent(i));
            Agents[i].Exchanges.Add(CreateExampleExchange());
            Agents[i].Account.AddRange(CreateExampleAccount(new List<Currency>() { Agents[i].Exchanges[0].From, Agents[i].Exchanges[0].To }));
            Agents = Agents.OrderBy(x => x.Exchanges[0].Rate).ToList();
        }

        StartExampleExchange();
    }

    List<CurrencyAmount> CreateExampleAccount(List<Currency> currencies)
    {
        List<CurrencyAmount> account = new List<CurrencyAmount>();
        account.Add(new CurrencyAmount(currencies[0], rnd.Next(100, 1001)));
        account.Add(new CurrencyAmount(currencies[1], rnd.Next(100, 1001)));
        return account;
    }

    Exchange CreateExampleExchange()
    {
        var currencyList = Enum.GetValues(typeof(Currency)).Cast<Currency>().ToList();
        Currency from = currencyList[rnd.Next(0, currencyList.Count)];
        currencyList.Remove(from);
        Currency to = currencyList[rnd.Next(0, currencyList.Count())];
        float rate = ExchangeRates.сoefficients[from]/ExchangeRates.сoefficients[to];
        rate += rate > 1 ? (float)rnd.Next(10,50)/100 : (float)rnd.Next(10,50)/1000;
        return new Exchange(from, to, rate);
    }

    public void StartExampleExchange()
    {
        _auctionSerializer.LogIteration(0);
        for (int i = 1; i < _iterations + 1; i++)
        {
            List<Agent[]> pairs = new List<Agent[]>();
            for (int j = 0; j < _agentsCount/2; j++)
            {
                Agent[] pair = new Agent[2];
                Agents = Agents.Where(x => x.Account.Single(y => y.Currency == x.Exchanges[0].From).Amount > 0)
                                .OrderBy(x => x.Exchanges[0].Rate).ToList();
                pair[0] = Agents[0];
                List<Agent> potentialAgents = Agents.Where(x => x.Account.Single(y => y.Currency == x.Exchanges[0].From).Amount > 0)
                                                    .Where(x => x.Exchanges[0].To == pair[0].Exchanges[0].From)
                                                    .Where(x => 1/x.Exchanges[0].Rate <= pair[0].Exchanges[0].Rate)
                                                    .OrderBy(x => x.Exchanges[0].Rate).ToList();
                if (potentialAgents.Count == 0) break;
                pair[1] = potentialAgents[0];
                Agents.Remove(pair[0]);
                Agents.Remove(potentialAgents[0]);
                pairs.Add(pair);
            }

            foreach (var pair in pairs)
            {
                var possibleAmount = pair[0].Account.Single(x => x.Currency == pair[0].Exchanges[0].From).Amount * pair[0].Exchanges[0].Rate;
                if (possibleAmount > pair[1].Account.Single(x => x.Currency == pair[0].Exchanges[0].To).Amount)
                    possibleAmount = pair[1].Account.Single(x => x.Currency == pair[0].Exchanges[0].To).Amount;
                pair[0].AddCurrency(new CurrencyAmount(pair[0].Exchanges[0].To, possibleAmount));
                pair[0].WithdrawCurrency(new CurrencyAmount(pair[0].Exchanges[0].From, possibleAmount/pair[0].Exchanges[0].Rate));
                pair[1].AddCurrency(new CurrencyAmount(pair[0].Exchanges[0].From, possibleAmount/pair[0].Exchanges[0].Rate));
                pair[1].WithdrawCurrency(new CurrencyAmount(pair[0].Exchanges[0].To, possibleAmount));
                Agents.Add(pair[0]);
                Agents.Add(pair[1]);
            }

            _auctionSerializer.LogIteration(i);
            Agents = Agents.OrderBy(x => x.Exchanges[0].Rate).ToList();
        }
    }
}