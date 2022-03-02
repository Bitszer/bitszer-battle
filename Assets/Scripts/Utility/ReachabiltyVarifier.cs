using System;
using System.Net;

public class ReachabiltyVarifier
{
    private const bool   allowCarrierDataNetwork = true;
    private const string pingAddress             = "8.8.8.8"; // Google Public DNS server
    private const float  waitingTime             = 1.0f;

    private float pingStartTime;

    public static bool checkInternet()
    {
        var result = true;
        var webRequest = WebRequest.Create("http://www.google.com/");
        //var webRequest = WebRequest.Create("http://35.163.237.56/baseball/images/test.png");

        webRequest.Timeout = 5000; // milliseconds
        webRequest.Method = "HEAD";

        try
        {
            webRequest.GetResponse();
        }
        catch (Exception)
        {
            result = false;
        }

        return result;
    }
}