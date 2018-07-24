using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace HamstarHelpers.Components.CustomEntity {
	public partial class CustomEntityManager : IEnumerable<CustomEntity> {
		public CustomEntity this[ int idx ] {
			get {
				CustomEntity ent = null;
				this.EntitiesByIds.TryGetValue( idx, out ent );
				return ent;
			}

			
			set {
				Type comp_type;
				string comp_name;

				if( value == null ) {
					if( this.EntitiesByIds.ContainsKey( idx ) ) {
						IList<CustomEntityComponent> ent_components = this.EntitiesByIds[idx].ComponentsInOrder;

						foreach( CustomEntityComponent component in ent_components ) {
							comp_type = component.GetType();
							do {
								if( this.EntitiesByComponentType.ContainsKey( comp_type ) ) {
									this.EntitiesByComponentType[ comp_type ].Remove( idx );
								}

								comp_type = comp_type.BaseType;
							} while( comp_type != typeof(CustomEntityComponent) );
						}

						this.EntitiesByIds.Remove( idx );
					}
				} else {
					foreach( CustomEntityComponent component in value.ComponentsInOrder ) {
						comp_type = component.GetType();
						do {
							if( !this.EntitiesByComponentType.ContainsKey( comp_type ) ) {
								this.EntitiesByComponentType[comp_type] = new HashSet<int>();
							}
							this.EntitiesByComponentType[comp_type].Add( idx );

							comp_type = comp_type.BaseType;
						} while( comp_type != typeof( CustomEntityComponent ) );
					}

					value.whoAmI = idx;
					this.EntitiesByIds[idx] = value;
				}
			}
		}

		////////////////

		public IEnumerator<CustomEntity> GetEnumerator() {
			return this.EntitiesByIds.Values.GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator() {
			return this.EntitiesByIds.Values.GetEnumerator();
		}

		////////////////

		public int Add( CustomEntity ent ) {
			int idx = this.EntitiesByIds.Count;
			
			this[ idx ] = ent;

			return idx;
		}

		public void Remove( CustomEntity ent ) {
			this[ ent.whoAmI ] = null;
		}
		public void Remove( int idx ) {
			this[ idx ] = null;
		}


		public void Clear() {
			this.EntitiesByIds.Clear();
			this.EntitiesByComponentType.Clear();
		}



		////////////////

		public ISet<CustomEntity> GetByComponentType<T>() where T : CustomEntityComponent {
			ISet<int> ent_idxs = new HashSet<int>();
			Type curr_type = typeof( T );

			if( !this.EntitiesByComponentType.TryGetValue( curr_type, out ent_idxs ) ) {
				foreach( Type comp_type in this.EntitiesByComponentType.Keys ) {
					if( comp_type.IsSubclassOf( curr_type ) ) {
						ent_idxs.UnionWith( this.EntitiesByComponentType[ comp_type ] );
					}
				}
			}

			return new HashSet<CustomEntity>(
				ent_idxs.Select( i => (CustomEntity)this.EntitiesByIds[i] )
			);
		}
	}
}
