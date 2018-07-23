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
								comp_name = comp_type.Name;

								if( this.EntitiesByComponentName.ContainsKey( comp_name ) ) {
									this.EntitiesByComponentName[ comp_name ].Remove( idx );
								}

								comp_type = comp_type.BaseType;
							} while( comp_name != "CustomEntityComponent" );
						}

						this.EntitiesByIds.Remove( idx );
					}
				} else {
					foreach( CustomEntityComponent component in value.ComponentsInOrder ) {
						comp_type = component.GetType();
						do {
							comp_name = comp_type.Name;

							if( !this.EntitiesByComponentName.ContainsKey( comp_name ) ) {
								this.EntitiesByComponentName[ comp_name ] = new HashSet<int>();
							}
							this.EntitiesByComponentName[ comp_name ].Add( idx );

							comp_type = comp_type.BaseType;
						} while( comp_name != "CustomEntityComponent" );
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
			this.EntitiesByComponentName.Clear();
		}



		////////////////

		public ISet<CustomEntity> GetByComponentType<T>() where T : CustomEntityComponent {
			ISet<int> ent_idxs;

			if( !this.EntitiesByComponentName.TryGetValue( typeof( T ).Name, out ent_idxs ) ) {
				return new HashSet<CustomEntity>();
			}

			return new HashSet<CustomEntity>(
				ent_idxs.Select( i => (CustomEntity)this.EntitiesByIds[i] )
			);
		}
	}
}
