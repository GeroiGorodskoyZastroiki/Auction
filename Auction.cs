using System;
using System.Collections.Generic;
using System.Linq;

public class Auction
{
    public static Auction Instance {get; private set;}
    int _iterations;
    int _agentsCount;
    AuctionSerializer _auctionSerializer;
    public List<Agent> Agents {get; private set;}
    Random rnd = new Random();

    public Auction(int iterations, int agentsCount)
    {
        Instance = this;

        _iterations = iterations;
        _agentsCount = agentsCount;
        _auctionSerializer = new(this);
        Agents = new();
        for (int i = 0; i < _agentsCount; i++) //создание популяции агентов
        {
            Agents.Add(new Agent(i));
            Agents[i].Exchanges.Add(CreateExampleExchange());
            Agents[i].Account.AddRange(CreateExampleAccount(new List<Currency>() { Agents[i].Exchanges[0].From, Agents[i].Exchanges[0].To }));
        }

        StartExampleExchange();
    }

    List<CurrencyAmount> CreateExampleAccount(List<Currency> currencies) //создание счетов агента
    {
        List<CurrencyAmount> account = new List<CurrencyAmount>() {
        new CurrencyAmount(currencies[0], rnd.Next(100, 1001)),
        new CurrencyAmount(currencies[1], rnd.Next(100, 1001)) };
        return account;
    }

    Exchange CreateExampleExchange() //создание предложения обмена агента
    {
        var currencyList = Enum.GetValues(typeof(Currency)).Cast<Currency>().ToList();
        Currency from = currencyList[rnd.Next(0, currencyList.Count)];
        currencyList.Remove(from);
        Currency to = currencyList[rnd.Next(0, currencyList.Count())];
        float rate = ExchangeRates.coefficients[from]/ExchangeRates.coefficients[to];
        rate += rate > 1 ? (float)rnd.Next(10,50)/100 : (float)rnd.Next(10,50)/1000;
        return new Exchange(from, to, rate);
    }

    public void StartExampleExchange()
    {
        _auctionSerializer.LogIteration(0);
        for (int i = 1; i < _iterations + 1; i++)
        {
            List<Agent> newAgents = new List<Agent>(); //буффер для агентов, которые уже осуществили итерацию
            Agents.Where(x => x.Account.Single(y => y.Currency == x.Exchanges[0].From).Amount == 0).ToList()
                    .ForEach(x => {newAgents.Add(x); Agents.Remove(x);}); //сразу перемещаем туда агентов без средств для обмена

            while (Agents.Count != 0)
            {
                Agents = Agents.OrderBy(x => x.Exchanges[0].Rate).ToList();
                var agent = Agents[0];
                newAgents.Add(agent);
                Agents.Remove(agent);
                var pairAgent = agent.Exchange(); //командуем автономным агентам начать обмен
                if (pairAgent == null) continue;
                newAgents.Add(pairAgent);
                Agents.Remove(pairAgent);
            }
            Agents = newAgents;
            _auctionSerializer.LogIteration(i);
        }
    }
}