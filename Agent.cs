using System.Collections.Generic;

public class Agent
{
    float _oneCurrency;
    float _anotherCurrency;
    float _minRate;
    float _maxRate;
    bool _fromOneToAnother;

    public Agent(float oneCurrency, float anotherCurrency, float minRate, float maxRate, bool fromOneToAnother)
    {
        _oneCurrency = oneCurrency;
        _anotherCurrency = anotherCurrency;
        _minRate = minRate;
        _maxRate = maxRate;
        _fromOneToAnother = fromOneToAnother;
    }

    public Agent Exchange()
    {
        Agent partner = FindBestOption();
        return partner;
    }

    Agent FindBestOption(List<Agent> agents)
    {
        Agent bestOption;
        foreach (var agent in agents)
        {
            if (agent._fromOneToAnother == this._fromOneToAnother) continue;

        }
        return bestOption;
    }
}