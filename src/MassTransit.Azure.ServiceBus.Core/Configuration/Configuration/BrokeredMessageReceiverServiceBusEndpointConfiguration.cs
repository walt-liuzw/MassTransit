﻿namespace MassTransit.Azure.ServiceBus.Core.Configuration
{
    using System;
    using Context;
    using GreenPipes;
    using MassTransit.Configuration;
    using MassTransit.Pipeline;
    using MassTransit.Pipeline.Filters;
    using Transports;


    public class BrokeredMessageReceiverServiceBusEndpointConfiguration :
        ReceiveEndpointConfiguration
    {
        public BrokeredMessageReceiverServiceBusEndpointConfiguration(IHostConfiguration hostConfiguration, IEndpointConfiguration configuration)
            : base(hostConfiguration, configuration)
        {
            HostAddress = hostConfiguration.Host.Address;
            InputAddress = new Uri(hostConfiguration.Host.Address, "no-queue-specified");
        }

        public override Uri HostAddress { get; }

        public override Uri InputAddress { get; }

        public override IReceivePipe CreateReceivePipe()
        {
            return Receive.CreatePipe(ConsumePipe, Serialization.Deserializer, configurator =>
            {
                Receive.ErrorConfigurator.UseFilter(new GenerateFaultFilter());

                configurator.UseRescue(Receive.ErrorConfigurator.Build(), x =>
                {
                    x.Ignore<OperationCanceledException>();
                });
            });
        }

        protected override IReceiveEndpoint CreateReceiveEndpoint(string endpointName, IReceiveTransport receiveTransport,
            ReceiveEndpointContext topology)
        {
            throw new NotImplementedException();
        }

        public override IReceiveEndpoint Build()
        {
            throw new NotImplementedException();
        }
    }
}
