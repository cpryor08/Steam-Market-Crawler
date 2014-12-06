using System;
namespace SteamMarketCrawler
{
	public class MarketItem
	{
		public bool success
		{
			get;
			set;
		}
		public string lowest_price
		{
			get;
			set;
		}
		public string volume
		{
			get;
			set;
		}
		public string median_price
		{
			get;
			set;
		}
	}
}
