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
            Agents.Add(new Agent());
            Agents[i].Exchanges.Add(CreateExampleExchange());
            Agents[i].Account.AddRange(CreateExampleAccount(new List<Currency>() { Agents[i].Exchanges[0].From, Agents[i].Exchanges[0].To }));
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
        rate += (float)(rnd.Next(1,5))/10;
        rate = Math.Abs(rate);
        return new Exchange(from, to, rate);
    }

    public void StartExampleExchange()
    {
        _auctionSerializer.LogIteration(0);
        for (int i = 0; i < _iterations; i++)
        {
            List<Agent[]> pairs = new List<Agent[]>();
            while (Agents.Count != 0)
            {
                Agent[] pair = new Agent[2];
                Agent agent = Agents[0];
                Agents.Remove(agent);
                List<Agent> potentialAgents = Agents.Where(x => x.Exchanges[0].Rate > 1 == agent.Exchanges[0].Rate > 1).Order().ToList(); //выгоднее когда меньше ставка
                pairs.Add(pair);
            }

            // var AgentsCopy = Agents;
            // var loopsCount = _agentsCount;
            // for (int j = 0; j < loopsCount; j++) 
            // {
            //     AgentsCopy.Remove(AgentsCopy[j].Exchange(Agents));
            //     AgentsCopy.Remove(AgentsCopy[j]);
            //     loopsCount--;
            // }
            _auctionSerializer.LogIteration(i);
        }
    }
}