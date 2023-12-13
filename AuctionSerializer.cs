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
        var sortedAgents = _auction.Agents.OrderBy(x => x.ID).ToList();
        using (StreamWriter writer = new StreamWriter($"Logs/{iteration}.txt"))
        {
            writer.WriteLine($"Iteration {iteration}");
            for (int i = 0; i < sortedAgents.Count; i++)
            {
                writer.WriteLine($"Agent {sortedAgents[i].ID}:");
                foreach (var account in sortedAgents[i].Account)
                {
                    writer.WriteLine($"   {account.Currency}: {account.Amount}");
                }
                writer.WriteLine("\n");
            }
        }
    }
}