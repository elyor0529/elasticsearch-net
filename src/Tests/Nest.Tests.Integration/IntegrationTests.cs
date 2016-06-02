﻿using System;
using System.Collections.Generic;
using System.Linq;
using Elasticsearch.Net;
using Nest.Tests.MockData.Domain;
using NUnit.Framework;

namespace Nest.Tests.Integration
{
	public class IntegrationTests
	{
		protected virtual IElasticClient Client { get { return ElasticsearchConfiguration.Client.Value; } }
		protected virtual IElasticClient ClientThatThrows { get { return ElasticsearchConfiguration.ClientThatThrows.Value; } }
		protected virtual IElasticClient ClientNoRawResponse { get { return ElasticsearchConfiguration.ClientNoRawResponse.Value; } }
		protected virtual ElasticClient ThriftClient { get { return ElasticsearchConfiguration.ThriftClient.Value; } }
		protected virtual IConnectionSettingsValues Settings { get { return ElasticsearchConfiguration.Settings(); } }

		protected ISearchResponse<T> SearchRaw<T>(string query) where T : class
		{
			var index = this.Client.Infer.IndexName<T>();
			var typeName = this.Client.Infer.TypeName<T>();

			var connectionStatus = this.Client.Raw.Search<SearchResponse<T>>(index, typeName, query);
			var serializer = connectionStatus.Serializer as INestSerializer;
			return connectionStatus.Response;
		}

		public void DoFilterTest(Func<FilterDescriptor<ElasticsearchProject>, FilterContainer> filter, ElasticsearchProject project, bool queryMustHaveResults)
		{
			var filterId = Filter<ElasticsearchProject>.Term(e => e.Id, project.Id);

			var results = this.Client.Search<ElasticsearchProject>(
			  s => s.Filter(ff => ff.And(
				  f => f.Term(e => e.Id, project.Id),
				  filter
				))
			  );

			var rawResponse = results.ConnectionStatus.ResponseRaw.Utf8String();

			Assert.True(results.IsValid, rawResponse);
			Assert.True(results.ConnectionStatus.Success, rawResponse);
			Assert.AreEqual(queryMustHaveResults ? 1 : 0, results.Total);
		}

	}
}
