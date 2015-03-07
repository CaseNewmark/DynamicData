﻿using System.Collections;
using System.Collections.Generic;
using DynamicData.Kernel;

namespace DynamicData
{
    /// <summary>
    /// A set of changes applied to the 
    /// </summary>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public  class ChangeSet<TObject, TKey> : IChangeSet<TObject, TKey>
    {
	    private int _adds;
        private int _removes;
        private int _evaluates;
        private int _updates;
        private int _moves;

        /// <summary>
        /// An empty change set
        /// </summary>
        public readonly static IChangeSet<TObject, TKey> Empty = new ChangeSet<TObject, TKey>();

		/// <summary>
		/// Initializes a new instance of the <see cref="ChangeSet{TObject, TKey}"/> class.
		/// </summary>
		public ChangeSet()
        {
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="ChangeSet{TObject, TKey}"/> class.
		/// </summary>
		/// <param name="items">The items.</param>
		public ChangeSet(IEnumerable<Change<TObject, TKey>> items)
        {
            foreach (var update in items)
            {
                Add(update);
            }
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="ChangeSet{TObject, TKey}"/> class.
		/// </summary>
		/// <param name="change">The change.</param>
		public ChangeSet(Change<TObject, TKey> change)
        {
            Add(change);
        }
		/// <summary>
		/// Initializes a new instance of the <see cref="ChangeSet{TObject, TKey}"/> class.
		/// </summary>
		/// <param name="reason">The reason.</param>
		/// <param name="key">The key.</param>
		/// <param name="current">The current.</param>
		public ChangeSet(ChangeReason reason, TKey key, TObject current)
            : this(reason, key, current, Optional.None<TObject>())
        {
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="ChangeSet{TObject, TKey}"/> class.
		/// </summary>
		/// <param name="reason">The reason.</param>
		/// <param name="key">The key.</param>
		/// <param name="current">The current.</param>
		/// <param name="previous">The previous.</param>
		public ChangeSet(ChangeReason reason,TKey key, TObject current, Optional<TObject> previous)
            : this()
        {
            Add(new Change<TObject, TKey>(reason, key, current, previous));
        }

		/// <summary>
		/// Adds the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		public void Add(Change<TObject, TKey> item)
        {
            switch (item.Reason)
            {
                case ChangeReason.Add:
                    _adds++;
                    break;
                case ChangeReason.Update:
                    _updates++;
                    break;
                case ChangeReason.Remove:
                    _removes++;
                    break;
                case ChangeReason.Evaluate:
                    _evaluates++;
                    break;
                case ChangeReason.Moved:
                    _moves++;
                    break;
            }
            Items.Add(item);
        }
        
        private List<Change<TObject, TKey>> Items { get; } = new List<Change<TObject, TKey>>();


		/// <summary>
		///     The total update count
		/// </summary>
		public int Count => Items.Count;

		/// <summary>
		///     Gets the number of additions
		/// </summary>
		public int Adds => _adds;

		/// <summary>
		///     Gets the number of updates
		/// </summary>
		public int Updates => _updates;

		/// <summary>
		///     Gets the number of removes
		/// </summary>
		public int Removes => _removes;

		/// <summary>
		///     The number of requeries
		/// </summary>
		public int Evaluates => _evaluates;

		/// <summary>
		///     Gets the number of moves
		/// </summary>
		public int Moves => _moves;


		#region Enumeration

		/// <summary>
		/// Gets the enumerator.
		/// </summary>
		/// <returns></returns>
		public IEnumerator<Change<TObject, TKey>> GetEnumerator()
		{
			return Items.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


		#endregion

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return string.Format("ChangeSet<{0}.{1}>. Count={2}", typeof(TObject).Name, typeof(TKey).Name, Count);
		}

	}
}