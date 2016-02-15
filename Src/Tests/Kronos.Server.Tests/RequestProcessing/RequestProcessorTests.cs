﻿using System;
using System.Net.Sockets;
using System.Text;
using Kronos.Server.RequestProcessing;
using Xunit;

namespace Kronos.Server.Tests.RequestProcessing
{
    public class RequestProcessorTests
    {
        [Fact]
        public void ThrowsAnExceptionWhenRequestTypeWillBeIncorrect()
        {
            RequestProcessor processor = new RequestProcessor();
            byte[] bytes = Encoding.UTF8.GetBytes("lorem ipsum");

            Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);

            Assert.ThrowsAny<InvalidOperationException>(() => processor.ProcessRequest(socket, bytes));
        }
    }
}