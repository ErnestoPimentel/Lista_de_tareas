using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TaskManager.Identity.Data;

namespace TaskManager.Identity.IntegrationTests;

public sealed class IdentityApiFactory : WebApplicationFactory<Program>
{
    private readonly string _databaseName = Guid.NewGuid().ToString();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureAppConfiguration((_, configuration) =>
        {
            configuration.AddInMemoryCollection(
                new Dictionary<string, string?>
                {
                    ["Jwt:Key"] =
                        "Integration-Test-Signing-Key-2026-At-Least-32-Bytes",
                    ["Jwt:Issuer"] = "TaskManager.Identity",
                    ["Jwt:Audience"] = "TaskManager",
                    ["Jwt:ExpirationMinutes"] = "60"
                });
        });

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<IdentityDbContext>>();
            services.RemoveAll<IDbContextOptionsConfiguration<IdentityDbContext>>();
            services.RemoveAll<IdentityDbContext>();

            services.AddDbContext<IdentityDbContext>(options =>
                options.UseInMemoryDatabase(_databaseName));
        });
    }
}
