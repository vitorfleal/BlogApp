using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Tests.Helpers;

public class IntegrationTestBase<TStartup> where TStartup : class
{
    protected IServiceProvider? ServiceProvider;

    private TestingWebApplicationFactory<TStartup>? _webApplicationFactory;
    private HttpClient? TestAppClient;

    public IntegrationTestBase(bool configureServer = true)
    {
        if (configureServer)
            ConfigureServer().GetAwaiter().GetResult();
    }

    protected async Task ConfigureServer()
    {
        _webApplicationFactory = new TestingWebApplicationFactory<TStartup>();
        TestAppClient = _webApplicationFactory.CreateClient();

        ServiceProvider = _webApplicationFactory.Server.Services;
    }

    protected HttpClient GetTestAppClient() => TestAppClient ?? new HttpClient();

    public void Dispose()
    {
        TestAppClient?.Dispose();
    }
}