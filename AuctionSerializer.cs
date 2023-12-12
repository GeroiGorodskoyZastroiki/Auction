using System.IO;

public class AuctionSerializer
{
    Auction _auction;

    public AuctionSerializer(Auction auction)
    {
        _auction = auction;
    }

    public void LogIteration(int iteration)
    {
        using (StreamWriter writer = new StreamWriter($"Logs/{iteration}"))
        {
            writer.WriteLine($"Iteration {iteration}");
            for (int i = 0; i < _auction.Agents.Count; i++)
            {
                writer.WriteLine($"Agent {i}:");
                foreach (var account in _auction.Agents[i].Account)
                {
                    writer.WriteLine($"   {account.Currency.ToString()}: {account.Amount}");
                }
                writer.WriteLine("\n");
            }
        }
    }
}