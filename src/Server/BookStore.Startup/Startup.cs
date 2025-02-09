﻿namespace BookStore.Startup;

using Application;
using Domain;
using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Web;
using Web.Extensions;
using Web.Middleware;

public class Startup
{
    public Startup(IConfiguration configuration)
        => this.Configuration = configuration;

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
        => services
            .AddDomain()
            .AddApplication(this.Configuration)
            .AddInfrastructure(this.Configuration)
            .AddWebComponents();

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        => app
            .UseSwagger(env)
            .UseExceptionHandling(env)
            .UseValidationExceptionHandler()
            .UseHttpsRedirection()
            .UseRouting()
            .UseCors(options => options
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod())
            .UseAuthentication()
            .UseAuthorization()
            .UseEndpoints(endpoints => endpoints
                .MapHealthChecks()
                .MapControllers())
            .Initialize();
}