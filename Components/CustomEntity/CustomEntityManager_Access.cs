using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace HamstarHelpers.Components.CustomEntity {
	public partial class CustomEntityManager : IEnumerable<CustomEntity> {
		public CustomEntity this[ int idx ] {
			get {
				CustomEntity ent = null;
				this.EntitiesToIds.TryGetValue( idx, out ent );
				return ent;
			}


			set {
				if( value == null ) {
					if( this.EntitiesToIds.ContainsKey( idx ) ) {
						IList<CustomEntityComponent> components = this.EntitiesToIds[idx].ComponentsInOrder;

						foreach( CustomEntityComponent component in components ) {
							string comp_name = component.GetType().Name;

							if( this.EntitiesByComponentName.ContainsKey( comp_name ) ) {
								this.EntitiesByComponentName[comp_name].Remove( idx );
							}
						}

						this.EntitiesToIds.Remove( idx );
					}
				} else {
					foreach( CustomEntityComponent component in value.ComponentsInOrder ) {
						string comp_name = component.GetType().Name;

						if( !this.EntitiesByComponentName.ContainsKey( comp_name ) ) {
							this.EntitiesByComponentName[comp_name] = new HashSet<int>();
						}
						this.EntitiesByComponentName[comp_name].Add( idx );
					}

					value.whoAmI = idx;
					this.EntitiesToIds[idx] = value;
				}
			}
		}

		////////////////

		public IEnumerator<CustomEntity> GetEnumerator() {
			return this.EntitiesToIds.Values.GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator() {
			return this.EntitiesToIds.Values.GetEnumerator();
		}

		////////////////

		public int Add( CustomEntity ent ) {
			int idx = this.EntitiesToIds.Count;
			
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
			this.EntitiesToIds.Clear();
			this.EntitiesByComponentName.Clear();
		}



		////////////////

		public ISet<CustomEntity> GetByComponentType<T>() where T : CustomEntityComponent {
			ISet<int> ent_idxs;

			if( !this.EntitiesByComponentName.TryGetValue( typeof( T ).Name, out ent_idxs ) ) {
				return new HashSet<CustomEntity>();
			}

			return new HashSet<CustomEntity>(
				ent_idxs.Select( i => (CustomEntity)this.EntitiesToIds[i] )
			);
		}
	}
}
