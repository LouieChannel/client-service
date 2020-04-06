﻿using Ascalon.ClientService.Hubs;
using Ascalon.Kafka;
using Ascalon.Kafka.Dtos;
using Confluent.Kafka;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ascalon.ClientService.Kafka.Services
{
    public class DumperStateConsumerService : Consumer<NeuralNeworkPredict>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly DumperStateConsumerServiceOptions _options;
        private readonly ILogger<DumperStateConsumerService> _logger;
        private readonly IHubContext<LogistHub> _logistHub;
        private readonly IMemoryCache _memoryCache;
        private ConcurrentDictionary<int, Dictionary<int, int>> _neuralNetworkPredicts = new ConcurrentDictionary<int, Dictionary<int, int>>();

        public static ConcurrentDictionary<int, int> DriversTasks { get; set; } = new ConcurrentDictionary<int, int>();

        public static ConcurrentDictionary<int, int> NotificationTasks { get; set; } = new ConcurrentDictionary<int, int>();

        public DumperStateConsumerService(
            IServiceProvider serviceProvider,
            IMemoryCache memoryCache,
            IOptions<DumperStateConsumerServiceOptions> options,
            ILogger<DumperStateConsumerService> logger,
            IHubContext<LogistHub> logistHub) : base(logger)
        {
            _serviceProvider = serviceProvider;
            _options = options.Value;
            _logger = logger;
            _memoryCache = memoryCache;
            _logistHub = logistHub;
        }

        public override async Task ProcessMessage(string key, NeuralNeworkPredict neuralNeworkPredict, TopicPartitionOffset offset)
        {
            try
            {
                var neuralPredict = _neuralNetworkPredicts.GetOrAdd(neuralNeworkPredict.Id, x => new Dictionary<int, int>
                {
                    { neuralNeworkPredict.Result, 1 }
                });

                if (neuralPredict.Count < 25)
                {
                    int value = neuralPredict.GetValueOrDefault(neuralNeworkPredict.Result, 1);
                    neuralPredict[neuralNeworkPredict.Result] = ++value;
                    return;
                }

                int predict = neuralPredict.Where(i => i.Value.Equals(neuralPredict.Max(i => i.Value))).Select(i => i.Key).First();

                _memoryCache.TryGetValue(neuralNeworkPredict.Id, out int oldPredict);

                if (oldPredict != 0 && predict == oldPredict)
                    return;

                _memoryCache.Set(neuralNeworkPredict.Id, predict);

                if (DriversTasks.Count == 0)
                {
                    NotificationTasks.AddOrUpdate(neuralNeworkPredict.Id, predict, (key, value) => value = predict);
                    return;
                }

                DriversTasks.TryGetValue(neuralNeworkPredict.Id, out int taskId);

                if (taskId == 0)
                    return;

                await _logistHub.Clients.Group("Logist").SendAsync("DumperStatus", new DumperStatus()
                {
                    Id = taskId,
                    State = predict
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in method: {nameof(ProcessMessage)}", ex);
            }
        }

        protected override KafkaConsumerConfig BuildConfiguration() => _options.Config;
    }
}