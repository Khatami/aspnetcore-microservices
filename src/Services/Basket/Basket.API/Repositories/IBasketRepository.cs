﻿using Basket.API.Entities;

namespace Basket.API.Repositories
{
	public interface IBasketRepository
	{
		Task<ShoppingCart> GetBasket(string UserName);

		Task<ShoppingCart> UpdateBasket(ShoppingCart basket);

		Task DeleteBasket(string UserName);
	}
}