﻿// Copyright 2020 New Relic, Inc. All rights reserved.
// SPDX-License-Identifier: Apache-2.0

using System.Collections.Generic;
#if NETFRAMEWORK
using Newtonsoft.Json;
#else
using System.Text.Json.Serialization;
#endif

namespace NewRelic.Telemetry.Tracing
{
#if INTERNALIZE_TELEMETRY_SDK
    internal
#else
    public
#endif
    readonly struct NewRelicSpan
    {
        public string Id { get; }

#if NETFRAMEWORK
        [JsonProperty("trace.id")]
#else
        [JsonPropertyName("trace.id")]
#endif
        public string? TraceId { get; }

        public long Timestamp { get; }

        public Dictionary<string, object>? Attributes { get; }

        public NewRelicSpan(string? traceId, string spanId, long timestamp, string? parentSpanId, Dictionary<string, object>? attributes)
        {
            Id = spanId;
            TraceId = traceId;
            Timestamp = timestamp;

            if (parentSpanId != null && !string.IsNullOrWhiteSpace(parentSpanId))
            {
                (attributes ??= new Dictionary<string, object>())[NewRelicConsts.Tracing.AttribNameParentId] = parentSpanId;
            }

            Attributes = attributes;
        }
    }
}
