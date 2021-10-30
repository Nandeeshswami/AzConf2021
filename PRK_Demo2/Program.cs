using System;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Microsoft.Graph;
using Microsoft.Graph.Auth;

public class Program
{
    private static readonly string _clientId = "Add your Client ID";

    public static async Task Main(string[] args)
    {
        var app = PublicClientApplicationBuilder.Create(_clientId)
            .WithRedirectUri("http://localhost")
            .Build();

        string[] scopes = new string[] { "User.Read" };

        var PRKprovider = new InteractiveAuthenticationProvider(app, scopes);
        var client = new GraphServiceClient(PRKprovider);

        var me = await client.Me.Request().GetAsync();
            
        var PRKphotoMetadata = await client.Me.Photo.Request().GetAsync();
         Console.WriteLine($"[Job Title:]\t{me.JobTitle}");

        Console.WriteLine($"[Display Name:]\t{me.DisplayName}");
        Console.WriteLine();
        Console.WriteLine();

        Console.WriteLine($"Media:\t{PRKphotoMetadata.AdditionalData["@odata.mediaContentType"]}");
        Console.WriteLine();
        Console.WriteLine();

        var photo = await client.Me.Photo.Content.Request().GetAsync();

        using var stream = System.IO.File.Create(@"C:\Users\nsuhas\Documents\Imp_Docs\PRK\PRK_Demo2\profile.jpg");
        await photo.CopyToAsync(stream);
    }
}
