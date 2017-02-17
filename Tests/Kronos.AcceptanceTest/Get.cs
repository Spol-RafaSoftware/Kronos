﻿using System;
using System.Threading.Tasks;
using Kronos.Client;
using Xunit;
using Xunit.Abstractions;

namespace Kronos.AcceptanceTest
{
    public class Get : Base
    {
        public Get(ITestOutputHelper output) : base(output)
        {
        }

        protected override async Task ProcessAsync(IKronosClient client)
        {
            // Arrange
            string key = Guid.NewGuid().ToString();
            byte[] data = new byte[1024];

            // Act
            await client.InsertAsync(key, data, DateTime.UtcNow.AddDays(5));
            byte[] received = await client.GetAsync(key);

            // Assert
            Assert.Equal(data, received);
        }
    }
}
