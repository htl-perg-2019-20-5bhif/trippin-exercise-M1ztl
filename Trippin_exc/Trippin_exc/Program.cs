using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace Trippin_exc
{
    class Program
    {
        static HttpClient HttpClient = new HttpClient() { BaseAddress = new Uri("https://services.odata.org/TripPinRESTierService/(S(5dhay5wvj2x3o5h3oh1menja))/") };
        
        static async Task Main(string[] args)
        {

            var readFile = await File.ReadAllTextAsync("users.json");
            IEnumerable<TransferObejcts.User> users = JsonSerializer.Deserialize<IEnumerable<TransferObejcts.User>>(readFile);

            foreach (var user in users)
            {
                var response = await HttpClient.GetAsync("People('" + user.UserName + "')");
                if (!response.IsSuccessStatusCode)
                {
                    var newUser = new StringContent(JsonSerializer.Serialize(new TransferObejcts.Tripping(user)), Encoding.UTF8, "application/json");
                    await HttpClient.PostAsync("People", newUser);
                    Console.WriteLine("User inserted:"+user.UserName+" " +user.LastName);
                }
                else
                {
                    Console.WriteLine("Found user" + user.UserName);
                }
            }
        }
    }
}
