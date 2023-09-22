using System;
using System.Collections.Generic;

public class Auction
{
    int _iterations;
    int _agentsCount;
    List<Agent> _agents = new List<Agent>();
    Random rnd = new Random();

    public Game(int iterations, int agentsCount)
    {
        _iterations = iterations;
        _agentsCount = agentsCount;
        for (int i = 0; i < _agentsCount; i++)
        {
            bool rndFromOneToAnother = Math.Round(rnd.Next(0,1));
            float rndOneCurrency = rnd.Next();
            float rndAnotherCurrency = rnd.Next();
            float rndMinRate = rnd.Next();
            float rndMaxRate = rnd.Next();
            _agents.Add(new Agent(rndOneCurrency, rndAnotherCurrency, rndMinRate, rndMaxRate, rndFromOneToAnother));
        }
    }

    public void StartExchange() //
    {
        var loopsCount = _agentsCount;
        for (int i = 0; i < loopsCount; i++) //скорее всего не работает
        {
            _agents.Remove(_agents[i].Exchange());
            _agents.Remove(_agents[i]);
            loopsCount--;
        }
    }
}