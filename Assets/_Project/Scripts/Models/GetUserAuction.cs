using System.Collections.Generic;

namespace Bitszer
{
    public class GetUserAuction
    {
        public GetUserAuctionsData data { get; set; }
    }

    public class GetUserAuctionsData
    {
        public GetUserAuctions getUserAuctions { get; set; }
    }

    public class GetUserAuctions
    {
        public string nextToken { get; set; }
        public List<Auction> auctions { get; set; }
    }
}