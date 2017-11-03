using System;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace WalmartTest
{
	public class Walmart
	{
		public string Name { get; set; }
		public string City { get; set; }
		public string StateProvCode { get; set; }
		public string Zip { get; set; }
		public string PhoneNumber { get; set; }
	}

	class MainClass
	{
		static void Main(string[] args)
		{
			var client = new RestClient("http://api.walmartlabs.com/v1/");
			var request = new RestRequest("stores?format=json&apiKey=3gtu6jdh6a9q8335qzmer5dt", Method.GET);
			request.AddParameter("zip", "98001");

			var response = new RestResponse();

			Task.Run(async () =>
			{
				response = await GetResponseContentAsync(client, request) as RestResponse;
			}).Wait();

            JArray jsonResponse = JsonConvert.DeserializeObject<JArray>(response.Content);
		
            var walmartList = JsonConvert.DeserializeObject<List<Walmart>>(jsonResponse.ToString());
           
			foreach (var store in walmartList)
			{
				Console.WriteLine("Name: {0}", store.Name);
				Console.WriteLine("City: {0}", store.City);
				Console.WriteLine("State: {0}", store.StateProvCode);
				Console.WriteLine("Zip: {0}", store.Zip);
                Console.WriteLine("PhoneNumber: {0}", store.PhoneNumber);
			}
			Console.ReadLine();
		}


		public static Task<IRestResponse> GetResponseContentAsync(RestClient theClient, RestRequest theRequest)
		{
			var tcs = new TaskCompletionSource<IRestResponse>();
			theClient.ExecuteAsync(theRequest, response => {
				tcs.SetResult(response);
			});
			return tcs.Task;
		}
	}
}
