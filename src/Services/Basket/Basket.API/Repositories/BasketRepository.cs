﻿using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.API.Repositories
{
	public class BasketRepository : IBasketRepository
	{
		private readonly IDistributedCache _distributedCache;

		public BasketRepository(IDistributedCache distributedCache)
		{
			_distributedCache = distributedCache;
		}

		public async Task<ShoppingCart> GetBasket(string UserName)
		{
			var basket = await _distributedCache.GetStringAsync(UserName);

			if (string.IsNullOrEmpty(basket))
			{
				return null;
			}

			return JsonConvert.DeserializeObject<ShoppingCart>(basket);
		}

		public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
		{
			await _distributedCache.SetStringAsync(basket.UserName, JsonConvert.SerializeObject(basket));

			return await GetBasket(basket.UserName);
		}

		public async Task DeleteBasket(string UserName)
		{
			await _distributedCache.RemoveAsync(UserName);
		}
	}
}
