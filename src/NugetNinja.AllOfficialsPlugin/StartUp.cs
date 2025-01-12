﻿using Microsoft.Extensions.DependencyInjection;
using Aiursoft.NugetNinja.Core;

namespace Aiursoft.NugetNinja.AllOfficialsPlugin;

public class StartUp : IStartUp
{
    public void ConfigureServices(IServiceCollection services)
    {
        new DeprecatedPackagePlugin.StartUp().ConfigureServices(services);
        new PossiblePackageUpgradePlugin.StartUp().ConfigureServices(services);
        new UselessPackageReferencePlugin.StartUp().ConfigureServices(services);
        new UselessProjectReferencePlugin.StartUp().ConfigureServices(services);
        new MissingPropertyPlugin.StartUp().ConfigureServices(services);
    }
}
