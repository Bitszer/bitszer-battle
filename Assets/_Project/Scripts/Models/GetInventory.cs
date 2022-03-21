using System.Collections.Generic;

namespace Bitszer
{
    public class GetInventory
    {
        public GetInventoriesData data { get; set; }
    }

    public class GetInventoriesData
    {
        public GetInventories getInventory { get; set; }
    }

    public class GetInventories
    {
        public string nextToken { get; set; }
        public List<Inventory> inventory { get; set; }
    }
}