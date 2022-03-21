namespace Bitszer
{
    public class Auction
    {
        public double bid { get; set; }
        public double buyout { get; set; }
        public string createdAt { get; set; }
        public string expiration { get; set; }
        public HighBidderProfile highBidderProfile { get; set; }
        public SellerProfile sellerProfile { get; set; }
        public GameItem gameItem { get; set; }
        public string id { get; set; }
        public int quantity { get; set; }
    }
}