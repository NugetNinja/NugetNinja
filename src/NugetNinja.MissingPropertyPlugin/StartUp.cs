﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.NugetNinja.Core;

namespace Microsoft.NugetNinja.MissingPropertyPlugin;

public class StartUp : IStartUp
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<MissingPropertyDetector>();
    }
}
