using System.Collections.Generic;

namespace Bitszer
{
    public class Profile
    {
        public GetMyProfileData data { get; set; }
    }

    public class GetMyProfileData
    {
        public GetMyProfile getMyProfile { get; set; }
    }

    public class GetMyProfile
    {
        public double activeBidAmount { get; set; }
        public int activeBids { get; set; }
        public string backgroundImageUrl { get; set; }
        public double balance { get; set; }
        public double buyAmount { get; set; }
        public int buyCount { get; set; }
        public string createdAt { get; set; }
        public string id { get; set; }
        public string imageUrl { get; set; }
        public string name { get; set; }
        public int postedAuctionsCount { get; set; }
        public string screenName { get; set; }
        public double soldAmount { get; set; }
        public int soldCount { get; set; }
        public string title { get; set; }
        public List<string> titles { get; set; }
        public string role { get; set; }
    }
}