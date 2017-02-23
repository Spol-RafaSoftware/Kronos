﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Kronos.Core.Storage;
using Xunit;

namespace Kronos.Core.Tests.Storage
{
    public class CleanerTests
    {
        [Fact]
        public async Task Start_CanDeleteObjectsFromStorage()
        {
            var data = new Dictionary<Key, byte[]>
            {
                [new Key("one", DateTime.UtcNow)] = new byte[0],
                [new Key("two", DateTime.MaxValue)] = new byte[0]
            };

            Cleaner provider = new Cleaner();
            provider.Start(data, CancellationToken.None);
            await Task.Delay(Cleaner.Timer + 100);

            Assert.Equal(data.Count, 1);
        }
    }
}