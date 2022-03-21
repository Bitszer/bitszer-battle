using System.Collections.Generic;

namespace Bitszer
{
    public class GetAuctionbyGame
    {
        public GetAuctionsbyGameData data { get; set; }
    }

    public class GetAuctionsbyGameData
    {
        public GetAuctionsbyGame getAuctionsbyGame { get; set; }
    }

    public class GetAuctionsbyGame
    {
        public string nextToken { get; set; }
        public List<Auction> auctions { get; set; }
    }
}