﻿using MediatR;
using Microsoft.Extensions.Logging;

namespace Ordering.Application.Behaviours
{
	public class UnHandledExceptionBehaviour<TRequest, TResponse>
		: IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
	{
		private readonly ILogger<TRequest> _logger;

		public UnHandledExceptionBehaviour(ILogger<TRequest> logger)
		{
			_logger = logger;
		}

		public async Task<TResponse> Handle(TRequest request,
			CancellationToken cancellationToken,
			RequestHandlerDelegate<TResponse> next)
		{
			try
			{
				return await next();
			}
			catch (Exception ex)
			{
				var requestName = typeof(TRequest).Name;

				_logger.LogError(ex,
					"Application Request: Unhandled exception for request {Name} {@Request}", requestName, request);

				throw;
			}
		}
	}
}
