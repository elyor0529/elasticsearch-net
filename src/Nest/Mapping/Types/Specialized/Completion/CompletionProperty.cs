﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;

namespace Nest
{
	[JsonObject(MemberSerialization.OptIn)]
	public interface ICompletionProperty : IDocValuesProperty
	{
		[JsonProperty("analyzer")]
		string Analyzer { get; set; }

		[JsonProperty("contexts")]
		IList<ISuggestContext> Contexts { get; set; }

		[JsonProperty("max_input_length")]
		int? MaxInputLength { get; set; }

		[JsonProperty("preserve_position_increments")]
		bool? PreservePositionIncrements { get; set; }

		[JsonProperty("preserve_separators")]
		bool? PreserveSeparators { get; set; }

		[JsonProperty("search_analyzer")]
		string SearchAnalyzer { get; set; }
	}

	[JsonObject(MemberSerialization.OptIn)]
	[DebuggerDisplay("{DebugDisplay}")]
	public class CompletionProperty : DocValuesPropertyBase, ICompletionProperty
	{
		public CompletionProperty() : base(FieldType.Completion) { }

		public string Analyzer { get; set; }
		public IList<ISuggestContext> Contexts { get; set; }
		public int? MaxInputLength { get; set; }
		public bool? PreservePositionIncrements { get; set; }
		public bool? PreserveSeparators { get; set; }

		public string SearchAnalyzer { get; set; }
	}

	[DebuggerDisplay("{DebugDisplay}")]
	public class CompletionPropertyDescriptor<T>
		: DocValuesPropertyDescriptorBase<CompletionPropertyDescriptor<T>, ICompletionProperty, T>, ICompletionProperty
		where T : class
	{
		public CompletionPropertyDescriptor() : base(FieldType.Completion) { }

		string ICompletionProperty.Analyzer { get; set; }
		IList<ISuggestContext> ICompletionProperty.Contexts { get; set; }
		int? ICompletionProperty.MaxInputLength { get; set; }
		bool? ICompletionProperty.PreservePositionIncrements { get; set; }
		bool? ICompletionProperty.PreserveSeparators { get; set; }
		string ICompletionProperty.SearchAnalyzer { get; set; }

		public CompletionPropertyDescriptor<T> SearchAnalyzer(string searchAnalyzer) =>
			Assign(a => a.SearchAnalyzer = searchAnalyzer);

		public CompletionPropertyDescriptor<T> Analyzer(string analyzer) => Assign(a => a.Analyzer = analyzer);

		public CompletionPropertyDescriptor<T> PreserveSeparators(bool? preserveSeparators = true) =>
			Assign(a => a.PreserveSeparators = preserveSeparators);

		public CompletionPropertyDescriptor<T> PreservePositionIncrements(bool? preservePositionIncrements = true) =>
			Assign(a => a.PreservePositionIncrements = preservePositionIncrements);

		public CompletionPropertyDescriptor<T> MaxInputLength(int? maxInputLength) => Assign(a => a.MaxInputLength = maxInputLength);

		public CompletionPropertyDescriptor<T> Contexts(Func<SuggestContextsDescriptor<T>, IPromise<IList<ISuggestContext>>> contexts) =>
			Assign(a => a.Contexts = contexts?.Invoke(new SuggestContextsDescriptor<T>()).Value);
	}
}
