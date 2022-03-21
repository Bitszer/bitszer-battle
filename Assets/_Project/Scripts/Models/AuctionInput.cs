namespace Bitszer
{
    public class AuctionInput
    {
        public string gameId { get; set; }
        public string itemId { get; set; }
        public int auctionDuration { get; set; }
        public float bid { get; set; }
        public float buyout { get; set; }
        public int quantity { get; set; }
    }
}