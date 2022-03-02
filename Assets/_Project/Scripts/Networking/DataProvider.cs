using UnityEngine;
using Newtonsoft.Json;
using GraphQlClient.Core;
using System.Threading.Tasks;

public class DataProvider : Singleton<DataProvider>
{
    [Header("Scriptable Object")]
    public GraphApi graphApi;
    public Configuration configuration;

    [Space]
    [Space]
    public GameObject loadingPanel;

    #region QUERIES
    public async Task<GetUserAuction> GetUserAuctions(string userId, int limit, string nextToken)
    {
        GraphApi.Query getUserActionsQuery = graphApi.GetQueryByName("getUserAuctions", GraphApi.Query.Type.Query);

        if (string.IsNullOrEmpty(nextToken))
            getUserActionsQuery.SetArgs(new { userId, limit, });
        else
            getUserActionsQuery.SetArgs(new { userId, limit, nextToken, });

        var www = await graphApi.Post(getUserActionsQuery);
        var data = JsonConvert.DeserializeObject<GetUserAuction>(www.downloadHandler.text);
        return data;
    }

    public async void GetMyProfile()
    {
        loadingPanel.SetActive(true);

        var www = await graphApi.Post("getMyProfile", GraphApi.Query.Type.Query);

        while (!www.isDone)
            return;

        var data = JsonConvert.DeserializeObject<Profile>(www.downloadHandler.text);

        if (data != null)
            Events.OnProfileReceived.Invoke(data);
    }

    public async void GetProfile(string screenName)
    {
        GraphApi.Query getProfileQuery = graphApi.GetQueryByName("getProfile", GraphApi.Query.Type.Query);

        getProfileQuery.SetArgs(new { screenName, });

        await graphApi.Post(getProfileQuery);
    }

    public async Task<GetAuction> GetAuctions(string itemName, int limit, string nextToken)
    {
        GraphApi.Query getAuctionsQuery = graphApi.GetQueryByName("getAuctions", GraphApi.Query.Type.Query);

        if (string.IsNullOrEmpty(nextToken))
            getAuctionsQuery.SetArgs(new { itemName, limit, });
        else
            getAuctionsQuery.SetArgs(new { itemName, limit, nextToken, });

        var www = await graphApi.Post(getAuctionsQuery);
        var data = JsonConvert.DeserializeObject<GetAuction>(www.downloadHandler.text);
        return data;
    }

    public async Task<GetInventory> GetInventory(int limit, string nextToken)
    {
        GraphApi.Query getInventoryQuery = graphApi.GetQueryByName("getInventory", GraphApi.Query.Type.Query);

        if (string.IsNullOrEmpty(nextToken))
            getInventoryQuery.SetArgs(new { limit, });
        else
            getInventoryQuery.SetArgs(new { limit, nextToken, });

        var www = await graphApi.Post(getInventoryQuery);
        var data = JsonConvert.DeserializeObject<GetInventory>(www.downloadHandler.text);
        return data;
    }

    public async void GetMyInventoryByGame(int limit, string nextToken)
    {
        GraphApi.Query getMyInventorybyGameQuery = graphApi.GetQueryByName("getMyInventorybyGame", GraphApi.Query.Type.Query);

        if (string.IsNullOrEmpty(nextToken))
            getMyInventorybyGameQuery.SetArgs(new { limit, configuration.gameId, });
        else
            getMyInventorybyGameQuery.SetArgs(new { limit, nextToken, configuration.gameId, });

        await graphApi.Post(getMyInventorybyGameQuery);
    }

    public async Task<GetAuctionbyGame> GetAuctionsByGame(int limit, string nextToken)
    {
        GraphApi.Query getAuctionsbyGameQuery = graphApi.GetQueryByName("getAuctionsbyGame", GraphApi.Query.Type.Query);

        if (string.IsNullOrEmpty(nextToken))
            getAuctionsbyGameQuery.SetArgs(new { configuration.gameId, limit, });
        else
            getAuctionsbyGameQuery.SetArgs(new { configuration.gameId, limit, nextToken, });

        var www = await graphApi.Post(getAuctionsbyGameQuery);
        var data = JsonConvert.DeserializeObject<GetAuctionbyGame>(www.downloadHandler.text);
        return data;
    }

    public async void GetGameItemsByGame(int limit, string nextToken)
    {
        GraphApi.Query getGameItemsbyGameQuery = graphApi.GetQueryByName("getGameItemsbyGame", GraphApi.Query.Type.Query);

        if (string.IsNullOrEmpty(nextToken))
            getGameItemsbyGameQuery.SetArgs(new { configuration.gameId, limit, });
        else
            getGameItemsbyGameQuery.SetArgs(new { configuration.gameId, limit, nextToken, });

        await graphApi.Post(getGameItemsbyGameQuery);
    }
    #endregion

    #region MUTATIONS
    public async void CancelAuction(string auctionId)
    {
        GraphApi.Query cancelAuctionMutation = graphApi.GetQueryByName("cancelAuction", GraphApi.Query.Type.Mutation);

        cancelAuctionMutation.SetArgs(new { auctionId, });

        await graphApi.Post(cancelAuctionMutation);
    }

    public async void Buyout(string auctionId)
    {
        GraphApi.Query buyoutMutation = graphApi.GetQueryByName("buyout", GraphApi.Query.Type.Mutation);

        buyoutMutation.SetArgs(new { auctionId, });

        await graphApi.Post(buyoutMutation);
    }

    public async void Bid(string auctionId, float bid)
    {
        GraphApi.Query bidMutation = graphApi.GetQueryByName("bid", GraphApi.Query.Type.Mutation);

        bidMutation.SetArgs(new { auctionId, bid, });

        await graphApi.Post(bidMutation);
    }

    public async void CreateAuction(AuctionInput newAuction)
    {
        GraphApi.Query createAuctionMutation = graphApi.GetQueryByName("createAuction", GraphApi.Query.Type.Mutation);

        createAuctionMutation.SetArgs(new { newAuction, });

        await graphApi.Post(createAuctionMutation);
    }
    #endregion
}