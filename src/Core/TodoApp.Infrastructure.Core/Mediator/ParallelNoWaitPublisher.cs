﻿using MediatR;

namespace TodoApp.Infrastructure.Core.Mediator;

public class ParallelNoWaitPublisher : INotificationPublisher
{
    public Task Publish(IEnumerable<NotificationHandlerExecutor> handlerExecutors, INotification notification, CancellationToken cancellationToken)
    {
        foreach (var handler in handlerExecutors)
        {
            Task.Run(() => handler.HandlerCallback(notification, cancellationToken));
        }

        return Task.CompletedTask;
    }
}
