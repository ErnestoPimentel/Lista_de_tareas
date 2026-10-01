using TaskManager.Application.Interfaces.Tasks;
using TaskManager.Application.Services.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace TaskManager.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ITaskService, TaskService>();

        return services;
    }
}
