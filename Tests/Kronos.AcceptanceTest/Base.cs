﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Kronos.Client;
using Xunit;

namespace Kronos.AcceptanceTest
{
    [Collection("AcceptanceTest")]
    public abstract class Base
    {
        static Base()
        {
            Trace.Listeners.Add(new ConsoleLogger());
        }

        [Fact]
        public async Task RunAsync()
        {
            const int port = 5000;

            LogMessage($"Creating kronos client with port {port}");
            IKronosClient client = KronosClientFactory.FromLocalhost(port);

            LogMessage($"Creating server with port {port}");
            Task server = Server.Program.StartAsync(port);

            LogMessage($"Waiting for server warnup");
            await Task.Delay(2000);

            try
            {
                LogMessage("Processing internal test");
                await ProcessAsync(client).AwaitWithTimeout(5000);
                LogMessage("Processing internal finished");
            }
            catch (Exception ex)
            {
                LogMessage($"EXCEPTION: {ex}");
                Assert.False(true, ex.Message);
            }
            finally
            {
                try
                {
                    LogMessage("Stopping server");
                    Server.Program.Stop();

                    LogMessage("Waiting for server task to finish");
                    await server.AwaitWithTimeout(5000);

                    LogMessage("Server stopped");
                }
                catch (Exception ex)
                {
                    LogMessage($"EXCEPTION: {ex}");
                    Assert.False(true, ex.Message);
                }
            }
        }

        protected void LogMessage(string message)
        {
            Trace.WriteLine($"{GetType()} - {message}");
        }

        protected abstract Task ProcessAsync(IKronosClient client);
    }

    public static class TaskExtensions
    {
        public static async Task AwaitWithTimeout(this Task task, int miliseconds)
        {
            if (await Task.WhenAny(task, Task.Delay(miliseconds)) == task)
            {
                await task;
            }
        }
    }
}
