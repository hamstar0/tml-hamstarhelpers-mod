using HamstarHelpers.Libraries.DotNET;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace HamstarHelpers.Classes.DataStructures {
	/// <summary>
	/// Implements an immutable dictionary whose values are sets.
	/// </summary>
	/// <typeparam name="K">Any key value.</typeparam>
	/// <typeparam name="V">Any value type to be mapped to a key's set.</typeparam>
	public class ReadOnlyDictionaryOfSets<K, V> : IReadOnlyDictionary<K, V> {
		private IDictionary<K, ISet<V>> Dict = new Dictionary<K, ISet<V>>();



		////////////////

		/// <param name="dict">Dictionary to translate into the current read-only set-dictionary.</param>
		public ReadOnlyDictionaryOfSets( IDictionary<K, V> dict ) {
			foreach( var kv in dict ) {
				this.Dict[ kv.Key ] = new HashSet<V>() { kv.Value };
			}
		}

		/// <param name="dictCopy">Set-dictionary to directly wrap.</param>
		public ReadOnlyDictionaryOfSets( IDictionary<K, ISet<V>> dictCopy ) {
			this.Dict = new Dictionary<K, ISet<V>>( dictCopy );
		}


		////////////////

		/// <summary>
		/// Default getter.
		/// </summary>
		/// <param name="key">Key of our dictionary.</param>
		/// <returns>First item of the set mapped to the given key.</returns>
		public V this[K key] => this.Dict[key].FirstOrDefault();	// First()?

		/// <summary>
		/// Set getter.
		/// </summary>
		/// <param name="key">Key of our dictionary.</param>
		/// <returns>The whole set mapped to the given key.</returns>
		public ISet<V> Get( K key ) {
			return this.Dict[key];
		}


		////////////////

		/// <summary>
		/// Count of sets in the dictionary.
		/// </summary>
		public int Count => this.Dict.Count;

		/// <summary>
		/// Count of all items of all sets in the dictionary.
		/// </summary>
		public int CountAll => this.Dict.SafeSelect( kv => kv.Value.Count ).Sum();


		////////////////

		/// <summary>
		/// All the keys of the dictionary as a plain Enumerable data structure.
		/// </summary>
		public IEnumerable<K> Keys => this.Dict.Keys;

		/// @private
		IEnumerable<K> IReadOnlyDictionary<K, V>.Keys => this.Dict.Keys;

		/// <summary>
		/// The first items of each set in the dictionary as a plain Enumerable data structure.
		/// </summary>
		public IEnumerable<V> Values => this.Dict.Values.SafeSelect( v => v.First() );

		/// @private
		IEnumerable<V> IReadOnlyDictionary<K, V>.Values => this.Dict.Values.SafeSelect( v => v.First() );




		////////////////

		/// <summary>
		/// Detects if the given key exists in the dictionary.
		/// </summary>
		/// <param name="key">Key to check for.</param>
		/// <returns>`true` if the key exists.</returns>
		public bool ContainsKey( K key ) {
			return this.Dict.ContainsKey( key );
		}

		////////////////

		/// <summary>
		/// Quickhand getter with safety checking.
		/// </summary>
		/// <param name="key">Key to find value for.</param>
		/// <param name="value">Output parameter. Returns the first value of the set mapped to the given key.</param>
		/// <returns>`true` if the key is exists.</returns>
		public bool TryGetValue( K key, out V value ) {
			ISet<V> valueSet;
			bool tried = this.Dict.TryGetValue( key, out valueSet );

			if( tried ) {
				value = valueSet.First();
			} else {
				value = default( V );
			}

			return tried;
		}

		/// <summary>
		/// Quickhand getter with safety checking.
		/// </summary>
		/// <param name="key">Key to find value for.</param>
		/// <param name="values">Output parameter. Returns the set mapped to the given key.</param>
		/// <returns>`true` if the key is exists.</returns>
		public bool TryGetValues( K key, out ISet<V> values ) {
			return this.Dict.TryGetValue( key, out values );
		}

		////////////////

		/// <summary>
		/// Gets a standard key/value enumerator.
		/// </summary>
		/// <returns>The enumerator.</returns>
		public IEnumerator<KeyValuePair<K, V>> GetEnumerator() {
			return this.Dict.SafeSelect(
				kv => new KeyValuePair<K, V>( kv.Key, kv.Value.FirstOrDefault() )   // First()?
			).GetEnumerator();
		}

		/// @private
		IEnumerator IEnumerable.GetEnumerator() {
			return this.Dict.SafeSelect(
				kv => new KeyValuePair<K, V>( kv.Key, kv.Value.FirstOrDefault() )   // First()?
			).GetEnumerator();
		}
	}
}
