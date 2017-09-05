﻿using System;
using System.Linq;
using Elasticsearch.Net;
using FluentAssertions;
using Nest;
using Tests.Framework;
using Tests.Framework.Integration;
using Tests.Framework.ManagedElasticsearch.Clusters;

namespace Tests.XPack.MachineLearning.DeleteModelSnapshot
{
	public class DeleteModelSnapshotApiTests : MachineLearningIntegrationTestBase<IDeleteModelSnapshotResponse, IDeleteModelSnapshotRequest, DeleteModelSnapshotDescriptor, DeleteModelSnapshotRequest>
	{
		public DeleteModelSnapshotApiTests(XPackMachineLearningCluster cluster, EndpointUsage usage) : base(cluster, usage) { }

		protected override void IntegrationSetup(IElasticClient client, CallUniqueValues values)
		{
			foreach (var callUniqueValue in values)
			{
				PutJob(client, callUniqueValue.Value);

				var getModelSnapshotResponse = client.GetModelSnapshots(callUniqueValue.Value, f => f);

				Console.Write(getModelSnapshotResponse.ModelSnapshots.First().SnapshotId);

				if (!getModelSnapshotResponse.IsValid)
					throw new Exception("Problem setting up GetModelSnapshots for integration test");
			}
		}

		protected override LazyResponses ClientUsage() => Calls(
			fluent: (client, f) => client.DeleteModelSnapshot(CallIsolatedValue, CallIsolatedValue, f),
			fluentAsync: (client, f) => client.DeleteModelSnapshotAsync(CallIsolatedValue, CallIsolatedValue, f),
			request: (client, r) => client.DeleteModelSnapshot(r),
			requestAsync: (client, r) => client.DeleteModelSnapshotAsync(r)
		);

		protected override bool ExpectIsValid => true;
		protected override int ExpectStatusCode => 200;
		protected override HttpMethod HttpMethod => HttpMethod.DELETE;
		protected override string UrlPath => $"_xpack/ml/anomaly_detectors/{CallIsolatedValue}/model_snapshots/{CallIsolatedValue}";
		protected override bool SupportsDeserialization => true;
		protected override DeleteModelSnapshotDescriptor NewDescriptor() =>	new DeleteModelSnapshotDescriptor(CallIsolatedValue, CallIsolatedValue);
		protected override object ExpectJson => null;
		protected override Func<DeleteModelSnapshotDescriptor, IDeleteModelSnapshotRequest> Fluent => f => f;
		protected override DeleteModelSnapshotRequest Initializer => new DeleteModelSnapshotRequest(CallIsolatedValue, CallIsolatedValue);

		protected override void ExpectResponse(IDeleteModelSnapshotResponse response)
		{
			response.Acknowledged.Should().BeTrue();
		}
	}
}
