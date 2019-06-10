using HamstarHelpers.Components.DataStructures;
using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.Debug;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Services.EntityGroups {
	public partial class EntityGroups {
		private void ComputeGroups<T>( IList<Tuple<string, string[], Func<T, IDictionary<string, ISet<int>>, bool>>> matchers,
				ref IDictionary<string, ReadOnlySet<int>> groups,
				ref IDictionary<int, ReadOnlySet<string>> groupsPerEnt ) where T : Entity {
			var rawGroupsPerEnt = new Dictionary<int, ISet<string>>();
			
			IList<T> pool = this.GetPool<T>();

			for( int i=0; i<matchers.Count; i++ ) {
				string grpName = matchers[i].Item1;
				string[] dependencies = matchers[i].Item2;
				var matcherFunc = matchers[i].Item3;
				ISet<int> grp;

				if( !this.ComputeGroupMatch( pool, grpName, dependencies, matcherFunc, out grp ) ) {
					matchers.Add( matchers[i] );
					continue;
				}

				lock( EntityGroups.MyLock ) {
					groups[ grpName ] = new ReadOnlySet<int>( grp );
				}

				foreach( int idx in grp ) {
					if( !rawGroupsPerEnt.ContainsKey( idx ) ) {
						rawGroupsPerEnt[ idx ] = new HashSet<string>();
					}
					rawGroupsPerEnt[ idx ].Add( grpName );
				}
			}

			lock( EntityGroups.MyLock ) {
				foreach( var kv in rawGroupsPerEnt ) {
					groupsPerEnt[ kv.Key ] = new ReadOnlySet<string>( kv.Value );
				}
			}
		}


		private bool ComputeGroupMatch<T>( IList<T> entityPool,
				string groupName,
				string[] dependencies,
				Func<T, IDictionary<string, ISet<int>>, bool> matcherFunc,
				out ISet<int> entityIdsOfGroup )
				where T : Entity {
			entityIdsOfGroup = new HashSet<int>();
			IDictionary<string, ISet<int>> deps = this.GetGroups<T>( groupName, dependencies );

			for( int i = 1; i < entityPool.Count; i++ ) {
				try {
					lock( EntityGroups.MyLock ) {
						if( matcherFunc( entityPool[i], deps ) ) {
							entityIdsOfGroup.Add( i );
						}
					}
				} catch( Exception ) {
					LogHelpers.Alert( "Compute fail for '"+groupName+"' with ent ("+i+") "+(entityPool[i] == null ? "null" : entityPool[i].ToString()) );
				}
			}

			return true;
		}


		////////////////

		private ISet<string> _AlreadyRequeued = new HashSet<string>();

		private IDictionary<string, ISet<int>> GetGroups<T>( string groupName, string[] dependencies )
				where T : Entity {
			var deps = new Dictionary<string, ISet<int>>();
			if( dependencies == null ) { return deps; }

			IReadOnlyDictionary<string, ReadOnlySet<int>> entityGroups;

			switch( typeof( T ).Name ) {
			case "Item":
				entityGroups = this._ItemGroups;
				break;
			case "NPC":
				entityGroups = this._NPCGroups;
				break;
			case "Projectile":
				entityGroups = this._ProjGroups;
				break;
			default:
				throw new NotImplementedException( "Invalid Entity type " + typeof( T ).Name );
			}

			foreach( string dependency in dependencies ) {
				if( !entityGroups.ContainsKey( dependency ) ) {
					if( this._AlreadyRequeued.Contains( groupName ) ) {
						throw new HamstarException( "Entity group " + groupName + " could not find dependency " + dependency + "." );
					}
					this._AlreadyRequeued.Add( groupName );

					return deps;
				}

				deps[dependency] = entityGroups[dependency];
			}

			return deps;
		}
	}
}
