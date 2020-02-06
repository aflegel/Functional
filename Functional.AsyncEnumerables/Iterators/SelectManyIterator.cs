﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Functional
{
	internal class SelectManyIterator<TSource, TCollection, TResult> : IAsyncEnumerator<TResult>
	{
		private readonly IAsyncEnumerator<TSource> _enumerator;
		private readonly Func<TSource, int, IAsyncEnumerable<TCollection>> _collectionSelector;
		private readonly Func<TSource, TCollection, TResult> _resultSelector;
		private int _count;
		private IAsyncEnumerator<TCollection> _subEnumerator;

		public TResult Current { get; private set; }

		public SelectManyIterator(IAsyncEnumerable<TSource> source, Func<TSource, int, IAsyncEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
		{
			_enumerator = (source ?? throw new ArgumentNullException(nameof(source))).GetAsyncEnumerator();
			_collectionSelector = collectionSelector ?? throw new ArgumentNullException(nameof(collectionSelector));
			_resultSelector = resultSelector ?? throw new ArgumentNullException(nameof(resultSelector));
		}

		public async ValueTask DisposeAsync()
		{
			if (_subEnumerator != null)
				await _subEnumerator.DisposeAsync();

			await _enumerator.DisposeAsync();
		}

		public async ValueTask<bool> MoveNextAsync()
		{
			while (_subEnumerator == null || !await _subEnumerator.MoveNextAsync())
			{
				if (!await _enumerator.MoveNextAsync())
					return false;

				if (_subEnumerator != null)
					await _subEnumerator.DisposeAsync();

				_subEnumerator = _collectionSelector.Invoke(_enumerator.Current, _count++).GetAsyncEnumerator();
			}

			Current = _resultSelector.Invoke(_enumerator.Current, _subEnumerator.Current);

			return true;
		}
	}
}
