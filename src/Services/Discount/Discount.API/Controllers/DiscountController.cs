﻿using Discount.API.Entities;
using Discount.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Discount.API.Controllers
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class DiscountController : ControllerBase
	{
		private readonly IDiscountRepository _discountRepository;

		public DiscountController(IDiscountRepository discountRepository)
		{
			_discountRepository = discountRepository;
		}

		[HttpGet("{productName}", Name = nameof(GetDiscount))]
		[ProducesResponseType(typeof(Coupon), StatusCodes.Status200OK)]
		public async Task<ActionResult<Coupon>> GetDiscount(string productName)
		{
			var coupon = await _discountRepository.GetDiscount(productName);

			return Ok(coupon);
		}

		[HttpPost]
		[ProducesResponseType(typeof(Coupon), StatusCodes.Status200OK)]
		public async Task<ActionResult<Coupon>> CreateDiscount([FromBody]Coupon coupon)
		{
			await _discountRepository.CreateDiscount(coupon);

			return CreatedAtRoute(nameof(GetDiscount), new { productName = coupon.ProductName }, coupon);
		}

		[HttpPut]
		[ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
		public async Task<ActionResult<Coupon>> UpdateDiscount([FromBody] Coupon coupon)
		{
			return Ok(await _discountRepository.UpdateDiscount(coupon));
		}

		[HttpDelete("{productName}", Name = nameof(DeleteDiscount))]
		[ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
		public async Task<ActionResult<bool>> DeleteDiscount(string productName)
		{
			return Ok(await _discountRepository.DeleteDiscount(productName));
		}
	}
}