using DA.DinnerPlanner.Model.Contracts;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using ZstdSharp.Unsafe;

namespace DA.DinnerPlanner.Model.GeoCode
{
	/// <ChangeLog>
	/// <Create Datum="24.03.2025" Entwickler="DA" />
	/// </ChangeLog>
	public class OsmGeoCoder : IGeoCoder
	{
		private readonly string nominatimSearchApiURL = "https://nominatim.openstreetmap.org/search?";
		public async Task<GeoLocation> Address2LocationAsync(Address? address)
		{
			if (address != null)
			{
				using HttpClient client = new();
				StringBuilder sbApi = new(nominatimSearchApiURL);
				if (!string.IsNullOrEmpty(address.Street))
					sbApi.Append($"street={HttpUtility.UrlEncode(address.Street + " ")}{HttpUtility.UrlEncode(address.HouseNumber + address.HouseNumberExtension)}");
				if (!string.IsNullOrEmpty(address.City))
					sbApi.Append($"&city={HttpUtility.UrlEncode(address.City)}");
				if (!string.IsNullOrEmpty(address.Country.CountryName))
					sbApi.Append($"&country={HttpUtility.UrlEncode(address.Country.CountryName)}");
				if (!string.IsNullOrEmpty(address.ZipCode))
					sbApi.Append($"&postalcode={HttpUtility.UrlEncode(address.ZipCode)}");
				sbApi.Append("&format=json&limit=1");

				string? json = "";
				Uri uri = new(sbApi.ToString());
				if (uri == null)
					throw new NullReferenceException(nameof(uri));
				json = await GetNominationJssonAsync(uri);
				if (json != null)
				{
					JsonDocument jsonDocument = JsonDocument.Parse(json);
					GeoLocation loc = new();
					try
					{
						loc.Latitude = double.Parse(jsonDocument.RootElement[0].GetProperty("lat").GetString()!.Replace('.', ','));
						loc.Longitude = double.Parse(jsonDocument!.RootElement[0]!.GetProperty("lon")!.GetString()!.Replace('.', ','));
						loc.GeoCodeResult = GeoCodeResult.OK;
					}
					catch
					{
						loc.GeoCodeResult = GeoCodeResult.Error;
					}
					return loc;
				}
				else
					return new() { GeoCodeResult = GeoCodeResult.Error };
			}
			else
				return new() {GeoCodeResult = GeoCodeResult.NotFound };
		}

		private static async Task<string?> GetNominationJssonAsync(Uri uri)
		{
			Debug.WriteLine(uri);
			using HttpClient client = new();
			client.DefaultRequestHeaders.UserAgent.ParseAdd("DA.DinnerPlanner.Blazor");
			HttpRequestMessage requestMessage = new()
			{
				Method = HttpMethod.Get,
				RequestUri = uri,
				Content = new StringContent("", Encoding.UTF8, "application/json")
			};
			HttpResponseMessage? responseMessage = null;
			try
			{
				responseMessage = await client.SendAsync(requestMessage);
			}
			catch (Exception e /* we swallow all exceptions */ )
			{
				Debug.WriteLine(e.Message);
			}
			if (responseMessage != null)
				if (!responseMessage.IsSuccessStatusCode)
					return null;
				else
					return await responseMessage!.Content.ReadAsStringAsync();
			return null;
		}
	}
}