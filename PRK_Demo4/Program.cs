using System;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Microsoft.Graph;
using Microsoft.Graph.Auth;

public class Program
{
    private static readonly string _clientId = "Add your Client ID";

    public static void Main(string[] args)
    {
        var app = PublicClientApplicationBuilder.Create(_clientId)
            .WithRedirectUri("http://localhost")
            .Build();

        string[] scopes = new string[] { "Mail.Read" };

        var provider = new InteractiveAuthenticationProvider(app, scopes);

        var client = new GraphServiceClient(provider);
         var requestBuilder = client.Me;

        Console.WriteLine($"Request URL:\t{requestBuilder.RequestUrl}");
        var requestBuilder1=  client.Me.Messages.AppendSegmentToRequestUrl("$filter=importance eq 'high'").ToString();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine($"Request URL with filter:\t{requestBuilder1}");    
            Console.WriteLine();
              
    
    }
}
