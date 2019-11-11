﻿using NUnit.Framework;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using NewRelic.Telemetry.Spans;
using NewRelic.Telemetry.Transport;

namespace NewRelic.Telemetry.Tests
{
    public class SpanBatchSenderTests
    {
        [Test]
        public void SendAnEmptySpanBatch()
        {
            var traceId = "123";
            var spanBatch = new SpanBatch(new List<Span>(), new Dictionary<string, object>(), traceId);

            var spanBatchMarshaller = new SpanBatchMarshaller();

            var mockBatchDataSender = new Mock<IBatchDataSender>();
            mockBatchDataSender.Setup(x => x.SendBatchAsync(It.IsAny<string>())).Returns(Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.OK)));

            var spanBatchSender = new SpanBatchSender(mockBatchDataSender.Object, spanBatchMarshaller);

            var response = spanBatchSender.SendDataAsync(spanBatch).Result;

            Assert.AreEqual(false, response.DidSend);
        }

        [Test]
        public void SendANonEmptySpanBatch()
        {
            var traceId = "123";
            var spanBatch = new SpanBatch(new List<Span>() { new Mock<Span>().Object }, new Dictionary<string, object>(), traceId);
            var spanBatchMarshaller = new SpanBatchMarshaller();

            var mockBatchDataSender = new Mock<IBatchDataSender>();
            mockBatchDataSender.Setup(x => x.SendBatchAsync(It.IsAny<string>())).Returns(Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.OK)));

            var spanBatchSender = new SpanBatchSender(mockBatchDataSender.Object, spanBatchMarshaller);

            var response = spanBatchSender.SendDataAsync(spanBatch).Result;

            Assert.AreEqual(true, response.DidSend);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
        }
    }
}