using System;
using System.Collections.Concurrent;
using System.Linq;
using Metrics.Core;
using Metrics.MetricData;

namespace ServiceControl.Monitoring
{
    class NServiceBusReceivedMetricContext : ReadOnlyMetricsContext, MetricsDataProvider
    {
        public override MetricsDataProvider DataProvider => this;

        public MetricsData CurrentMetricsData
        {
            get
            {
                var contextsList = contexts.Select(pair => pair.Value).ToList();
                return new MetricsData("NServiceBus.Endpoints", DateTime.UtcNow, Enumerable.Empty<EnvironmentEntry>(), Enumerable.Empty<GaugeValueSource>(),
                    Enumerable.Empty<CounterValueSource>(), Enumerable.Empty<MeterValueSource>(), Enumerable.Empty<HistogramValueSource>(), 
                    Enumerable.Empty<TimerValueSource>(), contextsList);
            }
        }

        public void Consume(MetricsData data)
        {
            contexts.AddOrUpdate(data.Context, data, (context, currentData) => data);
        }

        ConcurrentDictionary<string, MetricsData> contexts = new ConcurrentDictionary<string, MetricsData>();
    }
}