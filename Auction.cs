using System;
using System.Collections.Generic;

public class Auction
{
    int _iterations;
    int _agentsCount;
    List<Agent> _agents = new List<Agent>();
    Random rnd = new Random();

    public Auction(int iterations, int agentsCount)
    {
        _iterations = iterations;
        _agentsCount = agentsCount;
        for (int i = 0; i < _agentsCount; i++)
        {
            List<CurrencyAmount> account = new List<CurrencyAmount>();
            _agents.Add(new Agent(account));
        }
    }

    public void StartExchange() //
    {
        for (int i = 0; i < _iterations; i++)
        {
            var loopsCount = _agentsCount;
            for (int j = 0; j < loopsCount; j++) 
            {
                _agents.Remove(_agents[j].Exchange(_agents));
                _agents.Remove(_agents[j]);
                loopsCount--;
            }
        }
    }
}