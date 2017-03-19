﻿using DependencyInjectionWithOptions;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class GreetingServiceCollectionExtensions
    {
        public static IServiceCollection AddGreetingService(this IServiceCollection collection, Action<GreetingServiceOptions> setupAction)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (setupAction == null) throw new ArgumentNullException(nameof(setupAction));

            collection.Configure(setupAction);
            return collection.AddTransient<IGreetingService, GreetingService>();
        }
    }
}
