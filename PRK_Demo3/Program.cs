using System;
using System.Linq;
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

        string[] scopes = new string[] { "Mail.Read" };

        var provider = new InteractiveAuthenticationProvider(app, scopes);

        var client = new GraphServiceClient(provider);
        
        var emails = await client.Me.Messages.Request()
            .Filter($"{nameof(Message.Subject)} eq 'Tribute to PRK'")
            .Expand(m => m.Attachments)
            .GetAsync();

        foreach (var email in emails)
        {
            Console.WriteLine($"Received:\t{email.ReceivedDateTime:G}");
            Console.WriteLine($"Subject:\t{email.Subject}");
            Console.WriteLine($"Attached:\t{email.Attachments.SingleOrDefault()?.Name}");
            Console.WriteLine();
        }
    }
}
