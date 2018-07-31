using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;


namespace HamstarHelpers.Components.CustomEntity {
	public partial class CustomEntityManager {
		public CustomEntity Get( int idx ) {
			CustomEntity ent = null;
			this.EntitiesByIndexes.TryGetValue( idx, out ent );
			return ent;
		}


		public void Set( int idx, CustomEntity ent ) {
			if( ent == null ) { throw new HamstarException("Null ent not allowed."); }

			Type comp_type;
			Type base_type = typeof( CustomEntityComponent );
			
			foreach( CustomEntityComponent component in ent.Components ) {
				comp_type = component.GetType();
				do {
					if( !this.EntitiesByComponentType.ContainsKey( comp_type ) ) {
						this.EntitiesByComponentType[comp_type] = new HashSet<int>();
					}
					this.EntitiesByComponentType[comp_type].Add( idx );

					comp_type = comp_type.BaseType;
				} while( comp_type != base_type );
			}

			ent.Core.whoAmI = idx;
			this.EntitiesByIndexes[ idx ] = ent;
		}


		////////////////

		public int Add( CustomEntity ent ) {
			if( ent == null ) { throw new HamstarException( "Null ent not allowed." ); }

			int idx = this.EntitiesByIndexes.Count;
			
			this.Set( idx, ent );

			return idx;
		}


		public void Remove( CustomEntity ent ) {
			if( ent == null ) { throw new HamstarException( "Null ent not allowed." ); }

			this.Remove( ent.Core.whoAmI );
		}

		public void Remove( int idx ) {
			if( !this.EntitiesByIndexes.ContainsKey( idx ) ) { return; }

			Type comp_type;
			Type base_type = typeof( CustomEntityComponent );

			IList<CustomEntityComponent> ent_components = this.EntitiesByIndexes[ idx ].Components;

			foreach( CustomEntityComponent component in ent_components ) {
				comp_type = component.GetType();
				do {
					if( this.EntitiesByComponentType.ContainsKey( comp_type ) ) {
						this.EntitiesByComponentType[comp_type].Remove( idx );
					}

					comp_type = comp_type.BaseType;
				} while( comp_type != base_type );
			}

			this.EntitiesByIndexes.Remove( idx );
		}


		public void Clear() {
			this.EntitiesByIndexes.Clear();
			this.EntitiesByComponentType.Clear();
		}



		////////////////
		
		public ISet<CustomEntity> GetByComponentType<T>() where T : CustomEntityComponent {
			ISet<int> ent_idxs = new HashSet<int>();
			Type curr_type = typeof( T );
			
			if( !this.EntitiesByComponentType.TryGetValue( curr_type, out ent_idxs ) ) {
				foreach( var kv in this.EntitiesByComponentType ) {
					if( kv.Key.IsSubclassOf( curr_type ) ) {
						ent_idxs.UnionWith( kv.Value );
					}
				}

				if( ent_idxs == null ) {
					return new HashSet<CustomEntity>();
				}
			}
			
			return new HashSet<CustomEntity>(
				ent_idxs.Select( i => (CustomEntity)this.EntitiesByIndexes[i] )
			);
		}
	}
}
