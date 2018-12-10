﻿using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PerformanceLogger;
using PerformanceLogger.Extensions.Logging;
using System.Threading;

namespace Sample
{
    /// <summary>
    /// This is a sample console application demonstrating how to use the Performance Logger to track operation
    /// execution times, using the Logging extension (PerformanceLogger.Extensions.Logging).
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // Instanciate a console logger, implementing ILogger. Will be used to show logs on console.
            var serviceProvider = new ServiceCollection()
                .AddLogging(builder => {
                    builder.AddConsole();
                    builder.SetMinimumLevel(LogLevel.Debug);
                })
                .BuildServiceProvider();
            var consoleLogger = serviceProvider.GetService<ILogger<Program>>();
            
            consoleLogger.LogInformation("Application start");

            // Build a performance logger
            var performanceLogger = new PerformanceLoggerBuilder()
                .AddLogger(consoleLogger)   // Adds the console logger as a target for logging the performance results
                .Build();

            // Perform a long running operation and log its performance
            var perfTracker = performanceLogger.Start("my_long_task_01");

            // Simulating a long running task
            Thread.Sleep(1000);

            // End the tracking and log it
            perfTracker.End();            

            // Causes the loggers to flush their logs
            serviceProvider.Dispose();
        }
    }
}
