﻿using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Respositories;
using Ordering.Application.Models;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
	public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
	{
		private readonly IOrderRepository _orderRepository;
		private readonly IMapper _mapper;
		private readonly IEmailService _emailService;
		private readonly ILogger<CheckoutOrderCommand> _logger;

		public CheckoutOrderCommandHandler(IOrderRepository orderRepository,
			IMapper mapper,
			IEmailService emailService,
			ILogger<CheckoutOrderCommand> logger)
		{
			_orderRepository = orderRepository;
			_mapper = mapper;
			_emailService = emailService;
			_logger = logger;
		}

		public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Order for {user} is creating.", request.UserName);

			var orderEntity = _mapper.Map<Order>(request);
			var newOrder = await _orderRepository.AddAsync(orderEntity);

			_logger.LogInformation($"Order {newOrder.Id} is successfully created.");

			try
			{
				await _emailService.SendEmail(new Email()
				{
					To = "hamedkhatami@outlook.com",
					Subject = "Order was created",
					Body = "Order was created"
				});
			}
			catch (Exception ex)
			{
				_logger.LogError($"Order {newOrder.Id} failed due to an error with the mail service: {ex.Message}");
			}

			return newOrder.Id;
		}
	}
}
