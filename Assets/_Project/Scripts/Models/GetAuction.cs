using System.Collections.Generic;

namespace Bitszer
{
    public class GetAuction
    {
        public GetAuctionsData data { get; set; }
    }

    public class GetAuctionsData
    {
        public GetAuctions getAuctions { get; set; }
    }

    public class GetAuctions
    {
        public string nextToken { get; set; }
        public List<Auction> auctions { get; set; }
    }
}