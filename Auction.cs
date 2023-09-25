using System;
using System.Collections.Generic;

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
        _auctionSerializer = new AuctionSerializer(this);
        Agents = new List<Agent>();
        for (int i = 0; i < _agentsCount; i++)
        {
            Agents.Add(new Agent(CreateAccountExample()));
            Agents[i].Exchanges.Add(CreateExchangeExample());
        }
    }

    List<CurrencyAmount> CreateAccountExample()
    {
            List<CurrencyAmount> account = new List<CurrencyAmount>();
            account.Add(new CurrencyAmount(Currency.Rubles, rnd.Next(100, 1001)));
            account.Add(new CurrencyAmount(Currency.Virts, rnd.Next(100, 1001)));
            return account;
    }

    Exchange CreateExchangeExample()
    {
        Currency from = rnd.Next(0,2) == 0? Currency.Rubles: Currency.Virts;
        Currency to = from == Currency.Rubles? Currency.Virts: Currency.Virts;
        return new Exchange(from, to, rnd.Next(5, 15)/10);
    }

    public void StartExchange() //
    {
        _auctionSerializer.LogIteration(0);
        for (int i = 0; i < _iterations; i++)
        {
            var AgentsCopy = Agents;
            var loopsCount = _agentsCount;
            for (int j = 0; j < loopsCount; j++) 
            {
                AgentsCopy.Remove(AgentsCopy[j].Exchange(Agents));
                AgentsCopy.Remove(AgentsCopy[j]);
                loopsCount--;
            }
            _auctionSerializer.LogIteration(i);
        }
    }
}