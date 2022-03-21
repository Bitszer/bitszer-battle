using System.Collections.Generic;

namespace Bitszer
{
    public class GetGameItembyGame
    {
        public GetGameItemsData data { get; set; }
    }

    public class GetGameItemsData
    {
        public GetGameItems getGameItemsbyGame { get; set; }
    }

    public class GetGameItems
    {
        public string nextToken { get; set; }
        public List<GameItem> gameItems { get; set; }
    }
}