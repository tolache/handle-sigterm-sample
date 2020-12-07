﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace handle_sigterm_smaple
{
    public class ApplicationLifetimeHostedService : IHostedService
    {
        IApplicationLifetime appLifetime;  
        ILogger<ApplicationLifetimeHostedService> logger;  
        IHostingEnvironment environment;  
        IConfiguration configuration;  
        public ApplicationLifetimeHostedService(  
            IConfiguration configuration,  
            IHostingEnvironment environment,  
            ILogger<ApplicationLifetimeHostedService> logger,   
            IApplicationLifetime appLifetime)  
        {  
            this.configuration = configuration;  
            this.logger = logger;  
            this.appLifetime = appLifetime;  
            this.environment = environment;  
        }
        
        private string GetHomePath()
        {
            return Environment.OSVersion.Platform == PlatformID.Unix ||
                   Environment.OSVersion.Platform == PlatformID.MacOSX
                ? Environment.GetEnvironmentVariable("HOME")
                : Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
        }
  
        public Task StartAsync(CancellationToken cancellationToken)  
        {  
            logger.LogInformation("StartAsync method called.");  
  
            appLifetime.ApplicationStarted.Register(OnStarted);  
            appLifetime.ApplicationStopping.Register(OnStopping);  
            appLifetime.ApplicationStopped.Register(OnStopped);  
  
            return Task.CompletedTask;  
  
        }

        private void OnStarted()
        {
            logger.LogInformation("OnStarted method called.");
            string filePath = GetHomePath() + @"/sigterm-handler-started.txt";
            string[] lines = {"Process started."};
            
            System.IO.File.WriteAllLines(filePath,lines);
        }  
  
        private void OnStopping()  
        {  
            logger.LogInformation("OnStopping method called.");
        }  
  
        private void OnStopped()  
        {  
            logger.LogInformation("OnStopped method called.");  
            string filePath = GetHomePath() + @"/sigterm-handler-stopped.txt";
            string[] lines = {"Process stopped."};
            
            System.IO.File.WriteAllLines(filePath,lines);
        }  
  
  
        public Task StopAsync(CancellationToken cancellationToken)  
        {  
            logger.LogInformation("StopAsync method called.");  
  
            return Task.CompletedTask;  
        } 
    }
}