using Bitszer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetMyLogs
{
    public GetMyLogsByGameData data { get; set; }
}

public class GetMyLogsByGameData
{
    public GetUserLogs getMyLogs { get; set; }
}

public class GetUserLogs
{
    public string nextToken { get; set; }
    public List<Logs> logs { get; set; }
}
public class Logs
{
    public string action { get; set; }
    public auctionItem auctionItem { get; set; }
    public string timestamp { get; set; }
}
public class auctionItem
{
    public float bid { get; set; }
    public string buyout { get; set; }
    public string createdAt { get; set; }
    public string expiration { get; set; }
    public string quantity { get; set; }
    public gameItem gameItem { get; set; }
    public HigherBidderProfile highBidderProfile { get; set; }
}
public class gameItem
{
    public string gameName { get; set; }
    public string imageUrl { get; set; }
}
public class HigherBidderProfile
{
    public string name { get; set; }
}