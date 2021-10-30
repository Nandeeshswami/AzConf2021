# AzConf2021
This repo is for "The Power of Graph" session at AzConf 2021, azconf.dev. The session is intended to be a delivery mode, meaning we will run through the demos and this script can be used by the attendees to run through them again. There are assignments below which are not compulsory but to help provide you a challenge.

_**Script for the work Shop**_
=================================

1. Slides 1 to 13 provide you the base for the upcoming demos. You can download the slides attached to the repo to review and come to point 2.

2. Best way to learn Graph API is through Graph Explorer, go to https://aka.ms/ge and play around with the samples provided. 

![image](https://user-images.githubusercontent.com/3333558/139537626-094bc7d0-d691-4192-bc6c-ba1a76ab7d9e.png)

**Important points to note:**
-> Permission & consent are very centric to the graph conversation, for any API you would like to use ensure you review the Graph Explorer Permissions tab.

![image](https://user-images.githubusercontent.com/3333558/139537682-fd1ef951-3115-4a42-b77d-0cb4ea1256e3.png)

Like wise Request Body and Headers can be very useful in case of Post/Patch scenarios.

-> In the bottom pane, look at the tabs provided, Code Preview can be very handy in scenarios where you would like to get the code sample for implemntation, we will look at several examples ahead.

**Assignment: Get the emails with high importance for the last 10 days.**
Hint: Look at the sample queries for Outlook and combine couple of them.

-> Now let's look at a sample which calls into a graph API. In this sample we will use standard APIs to get a token and then using that token we do an http get call to get details of my profile and onedrive.
In the sample we are using Flurl, it's an excellent library that makes http calls and URL building extremly easy. Give it a try!

For any App whether Desktop, Web, SPA etc to utilize tons of services like Application authentication and authorization, User authentication and authorization, SSO using federation or password, User provisioning and synchronization and many more, you need to do an App Registration.
check out the link below for more on App Registration.
https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-register-app

For the purposes of this demo, we have set up an App. For my app, I have registered it as a Multi-Tenant App as shown and also added the platform as desktop app.
![image](https://user-images.githubusercontent.com/3333558/139538726-f4659979-5162-454f-84a3-8bea9c4fb2c8.png)

Desktop App

![image](https://user-images.githubusercontent.com/3333558/139538779-94d393db-2b74-4ca4-aa3c-71b2893a5b35.png)

Further added Redirect URI as http://localhost

Coming back to our sample, please download the samples as part of this repo and open PRK_Demo1

In the sample you can observe we are using the CLient ID for the App I have registered, you will get it in the portal as shown in the image below.
![image](https://user-images.githubusercontent.com/3333558/139538906-6c0a032d-5aa2-48cd-b69b-086b1e37101e.png)

Observe that I am creating using PublicClientApplicationBuilder and passing on the Redirect URI. Note that this has to match the Redirect URI in your App Registration, else you may end up with mismatch errors, etc.

var PuneethApp = **PublicClientApplicationBuilder**.Create(_clientId)
            .**WithRedirectUri**("http://localhost")
            .Build();

We pass in the scopes to AcquireTokenInteractive(scopes) to get the token. Further Flurl.http makes our lives easy and lets us do a My profile Graph API call with relative ease. Likewise we call the Onedrive API.

**Note about Permissions & Consent:** It's very important to always add the API permission that your app uses in the API Permissions blade. Note it has to be the minimum required permission to execute your tasks in the App. For instance if you need to read OneDrive read only request for it and do not request for Mail.Read etc.
Here's where you do it
![image](https://user-images.githubusercontent.com/3333558/139540169-7d04b1ee-4c7a-4e80-8c0c-8e8426c495ec.png)

Declaring here makes it easy for Admin to consent and also the User Experience is way better with pre consent . For more on Permissions & Consent, please check https://docs.microsoft.com/en-us/azure/active-directory/develop/v2-permissions-and-consent

To build the App, I use a terminal and **dotnet run**__.
YOu will be prompted to enter your credentials, once you have entered, you will get the below output.
![image](https://user-images.githubusercontent.com/3333558/139539248-d1b628a1-a767-4709-88d5-df459f153bb9.png)

3. **Graph SDK**:Now that we have seen how to graph API calls let's see what Graph SDK and Auth SDK have to offer. Go to https://Nuget.org and search for Graph. We will use Graph & Graph.Auth libraries. Let's see how these make our lives easy. Get PRKDemo2.

In our previous sample we made the below calls to get tokens but Graph.AUth makes lot of it seamless. We are replacing the below lines 
        
        var result = await PuneethApp.AcquireTokenInteractive(scopes)
            .ExecuteAsync();
        
with **InteractiveAuthenticationProvider.**
In this sample, we will will use Graph SDK to make Graph API calls. In our previous sample we called in https://graph.microsoft.com/v1.0/me/, it gives me tons of info like,

{"@odata.context":"https://graph.microsoft.com/v1.0/$metadata#users/$entity","businessPhones":["+1 412 555 010
9"],"displayName":"Megan Bowen","givenName":"Megan","jobTitle":"Marketing Manager","mail":"MeganB@applify1.onm
icrosoft.com","mobilePhone":null,"officeLocation":"12/1110","preferredLanguage":"en-US","surname":"Bowen","use
rPrincipalName":"MeganB@applify1.onmicrosoft.com","id":"55868a98-9f9a-4f0e-bed9-4405c0a6eb1f"}

What if I just want Display name & my job title and my profile pic. Imagine using Json classes to filter throgh the above that would be too much of plumbing code. Graph along with it's SDK saves your time. The below code wil get us what we desired above.

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
        
Here's the o/p.
![image](https://user-images.githubusercontent.com/3333558/139539868-ae9e473d-4791-4516-bb88-a14cb18ca188.png)

![image](https://user-images.githubusercontent.com/3333558/139539888-e44a5e7c-2429-46ed-8c4e-47c460424424.png)

4. Let's look at another demo to realize what we talked about in Slide 17. Specifically about Odata & SDK. Go to PRKDemo3 and look at the code file. In this sample we will use SDK to build advanced querying. Let's try to get an email with attachment and dump it's name. Sounds like we need a filter. Yes, and we can realize all of it with SDK.

var emails = await client.Me.Messages.Request()
            .Filter($"{nameof(Message.Subject)} eq 'Tribute to PRK'")
            .Expand(m => m.Attachments)
            .GetAsync();

The above line does the trick. Running this, provides us below.
![image](https://user-images.githubusercontent.com/3333558/139540494-13121e34-b74c-40e4-96da-f456d38fb472.png)

Here's a snip of my email.
![image](https://user-images.githubusercontent.com/3333558/139540515-0275e545-0968-431b-9860-fc60d5fbaebe.png)

5. Onto the next one, there may be scenarios where we may want to debug. I personally love the debugging options in VS and VS Code. But let's say we are using Graph SDK and building our Graph API and not getting the expected results. SDK provides you with code that can help in such scenarios. Open PRKDemo4.

Use Request Builder to print out what Graph API are we calling.
Here's the code,
 
        var requestBuilder = client.Me;
        Console.WriteLine($"Request URL:\t{requestBuilder.RequestUrl}");
        var requestBuilder1=  client.Me.Messages.AppendSegmentToRequestUrl("$filter=importance eq 'high'").ToString();

Here's the o/p:
![image](https://user-images.githubusercontent.com/3333558/139540680-fde4c3ac-2810-4882-89d5-cabac389cd75.png)

**Assignement: Create a sample that uses SDKs to create a Team's Team and add a channel to it.**
Hint: Look at Graph Explorer and find the query for creating a Team and channel. Use the code snippet from the Explorer.

6. **Pagination:** : Imagine scenarios where we query graph to list all the emails sent in 10 days, there may be tons of results. It's impossible to return all the data at once. Hence it uses Odata.NextLink and provides the link to next page. While there may be APIs now which may not have pagination but it is always important to take care of this in your code. Here's am example where we are getting all the groups that a user is part of. 

Here's the code,

  if (mygroups != null)
                {
                    do
                    {
                        // Page through results
                        foreach (var directoryObject in mygroups.**CurrentPage**)
                        {
                            if (directoryObject is Group)
                            {
                                Group group = directoryObject as Group;
                                ResultText += $"Group: { group.DisplayName} {group.Id}";
                                ResultText += Environment.NewLine;
                            }
                        }

                        **// are there more pages (Has a @odata.nextLink ?)**
                        if (mygroups.NextPageRequest != null)
                        {
                            mygroups = await mygroups.NextPageRequest.GetAsync();
                        }
                        else
                        {
                            mygroups = null;
                        }
                    } while (mygroups != null);

Observe the code where we keep checking for **NextPageRequest**. Once it is NULL, we come out of the loop.
Here's the output,
![image](https://user-images.githubusercontent.com/3333558/139542289-dadef025-d635-4a82-a635-57192b705dd0.png)

7. **Graph tool kit** makes it easy to build UI components based on Graph API. It has a collection of reusable, framework-agnostic components and authentication providers for accessing and working with Microsoft Graph. The components are fully functional right of out of the box, with built in providers that authenticate with and fetch data from Microsoft Graph. We can use these right of the box.

Visit https://mgt.dev/ to play around.

For detailed training on Graph Tool kit, check https://www.youtube.com/watch?v=tlZMt7vnUu4&ab_channel=MicrosoftDeveloper

Check out our Tips section to become a Graph Pro Dev.

Highly recommend the below session:
https://www.youtube.com/watch?v=Mc1ilblJLJw

https://github.com/piotrci/Microsoft-Graph-Efficient-Operations


That's All Folks!
Happy Coding :)

 
