﻿using System;
using System.Threading;
using Elasticsearch.Net;
using FluentAssertions;
using Nest;
using Tests.Framework;
using Tests.Framework.Integration;
using Tests.Framework.ManagedElasticsearch.Clusters;
using Tests.Framework.MockData;

namespace Tests.XPack.MachineLearning.GetBuckets
{
	public class GetBucketsApiTests : MachineLearningIntegrationTestBase<IGetBucketsResponse, IGetBucketsRequest, GetBucketsDescriptor, GetBucketsRequest>
	{
		public GetBucketsApiTests(XPackMachineLearningCluster cluster, EndpointUsage usage) : base(cluster, usage) { }

		protected override void IntegrationSetup(IElasticClient client, CallUniqueValues values)
		{
			foreach (var callUniqueValue in values)
			{
				PutJob(client, callUniqueValue.Value);
			}
		}

		protected override LazyResponses ClientUsage() => Calls(
			fluent: (client, f) => client.GetBuckets(CallIsolatedValue, f),
			fluentAsync: (client, f) => client.GetBucketsAsync(CallIsolatedValue, f),
			request: (client, r) => client.GetBuckets(r),
			requestAsync: (client, r) => client.GetBucketsAsync(r)
		);

		protected override bool ExpectIsValid => true;
		protected override int ExpectStatusCode => 200;
		protected override HttpMethod HttpMethod => HttpMethod.POST;
		protected override string UrlPath => $"_xpack/ml/anomaly_detectors/{CallIsolatedValue}/results/buckets";
		protected override bool SupportsDeserialization => true;
		protected override GetBucketsDescriptor NewDescriptor() => new GetBucketsDescriptor(CallIsolatedValue);
		protected override object ExpectJson => null;
		protected override Func<GetBucketsDescriptor, IGetBucketsRequest> Fluent => f => f;
		protected override GetBucketsRequest Initializer => new GetBucketsRequest(CallIsolatedValue);

		protected override void ExpectResponse(IGetBucketsResponse response)
		{
			response.ShouldBeValid();
			response.Buckets.Should().BeEmpty();
			response.Count.Should().Be(0);
		}
	}

	public class GetBucketsWithTimestampApiTests : MachineLearningIntegrationTestBase<IGetBucketsResponse, IGetBucketsRequest, GetBucketsDescriptor, GetBucketsRequest>
	{
		public GetBucketsWithTimestampApiTests(XPackMachineLearningCluster cluster, EndpointUsage usage) : base(cluster, usage) { }

		protected override void IntegrationSetup(IElasticClient client, CallUniqueValues values)
		{
			foreach (var callUniqueValue in values)
			{
				PutJob(client, callUniqueValue.Value);
			}
		}

		protected override LazyResponses ClientUsage() => Calls(
			fluent: (client, f) => client.GetBuckets(CallIsolatedValue, f),
			fluentAsync: (client, f) => client.GetBucketsAsync(CallIsolatedValue, f),
			request: (client, r) => client.GetBuckets(r),
			requestAsync: (client, r) => client.GetBucketsAsync(r)
		);

		protected override bool ExpectIsValid => true;
		protected override int ExpectStatusCode => 200;
		protected override HttpMethod HttpMethod => HttpMethod.POST;
		protected override string UrlPath => $"_xpack/ml/anomaly_detectors/{CallIsolatedValue}/results/buckets/1454943900000";
		protected override bool SupportsDeserialization => true;
		protected override GetBucketsDescriptor NewDescriptor() => new GetBucketsDescriptor(CallIsolatedValue);
		protected override object ExpectJson => null;
		protected override Func<GetBucketsDescriptor, IGetBucketsRequest> Fluent => f => f.Timestamp(new DateTime(2017, 08, 09));
		protected override GetBucketsRequest Initializer => new GetBucketsRequest(CallIsolatedValue, new DateTime(2017, 08, 09));

		protected override void ExpectResponse(IGetBucketsResponse response)
		{
			response.ShouldBeValid();
			response.Buckets.Should().BeEmpty();
			response.Count.Should().Be(0);
		}
	}
}
