using System;

public static class Events
{
    public static readonly Evt<Profile> OnProfileReceived = new Evt<Profile>();
    public static readonly Evt<GetAuction> OnAuctionsReceived = new Evt<GetAuction>();
    public static readonly Evt<GetInventory> OnInventoryReceived = new Evt<GetInventory>();
    public static readonly Evt<GetAuctionbyGame> OnAuctionsByGameReceived = new Evt<GetAuctionbyGame>();

    public static readonly Evt<ScrollController.SCROLL_PANEL> OnScrolledToBottom = new Evt<ScrollController.SCROLL_PANEL>();
}

public class Evt
{
    private Action _action;

    public void Invoke() => _action?.Invoke();

    public void AddListener(Action listener) => _action += listener;

    public void RemoveListener(Action listener) => _action -= listener;
}

public class Evt<T>
{
    private Action<T> _action;

    public void Invoke(T param) => _action?.Invoke(param);

    public void AddListener(Action<T> listener) => _action += listener;

    public void RemoveListener(Action<T> listener) => _action -= listener;
}