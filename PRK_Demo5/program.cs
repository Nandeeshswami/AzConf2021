using System;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Microsoft.Graph;
using Microsoft.Graph.Auth;

public class Program
{
    private static readonly string _clientId = "79c7060a-c87a-4b9e-8112-a15e08e1dca4";

  public static async Task Main(string[] args)
    {
        var app = PublicClientApplicationBuilder.Create(_clientId)
            .WithRedirectUri("http://localhost")
            .Build();

        string[] scopes = new string[] { "group.read.all" };

        var provider = new InteractiveAuthenticationProvider(app, scopes);

        var client = new GraphServiceClient(provider);
                         var mygroups = await client.Me.TransitiveMemberOf.Request().GetAsync();
                         String ResultText= null;
                if (mygroups != null)
                {
                    do
                    {
                        // Page through results
                        foreach (var directoryObject in mygroups.CurrentPage)
                        {
                            if (directoryObject is Group)
                            {
                                Group group = directoryObject as Group;
                                ResultText += $"Group: { group.DisplayName} {group.Id}";
                                ResultText += Environment.NewLine;
                            }
                        }

                        // are there more pages (Has a @odata.nextLink ?)
                        if (mygroups.NextPageRequest != null)
                        {
                            mygroups = await mygroups.NextPageRequest.GetAsync();
                        }
                        else
                        {
                            mygroups = null;
                        }
                    } while (mygroups != null);

                    Console.WriteLine("Below are all the groups you are part of");
                    Console.WriteLine(ResultText);
                }
				
				
				
				     
    
    }
}