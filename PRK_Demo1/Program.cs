using System;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Flurl.Http;

public class Program
{
    private static readonly string _clientId = "0dff9ce3-dd7a-4fe0-920a-18daf1c6ad5a";

    public static async Task Main(string[] args)
    {
        var PuneethApp = PublicClientApplicationBuilder.Create(_clientId)
            .WithRedirectUri("http://localhost")
            .Build();

        string[] scopes = new string[] { "User.Read", "Files.ReadWrite.ALL", "Sites.ReadWrite.ALL" };

        var result = await PuneethApp.AcquireTokenInteractive(scopes)
            .ExecuteAsync();

        var token = result.AccessToken;

        string MyInfo = await "https://graph.microsoft.com/v1.0/me/"
            .WithOAuthBearerToken(token)
            .GetStringAsync();
        Console.WriteLine();
        Console.WriteLine(MyInfo);

        Console.WriteLine();
        Console.WriteLine();

         string PuonedriveJson = await "https://graph.microsoft.com/beta/me/drive"
            .WithOAuthBearerToken(token)
            .GetStringAsync();

        Console.WriteLine();
        Console.WriteLine(PuonedriveJson);
   
    }
}