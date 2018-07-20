using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Internals.NetProtocols;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity {
	public partial class CustomEntity : Entity {
		private IList<CustomEntityComponent> _ComponentsInOrder;
		private IDictionary<string, int> ComponentsByTypeName = new Dictionary<string, int>();
		
		public IReadOnlyList<CustomEntityComponent> ComponentsInOrder { get; private set; }
		

		////////////////

		public CustomEntity( IList<CustomEntityComponent> components ) {
			this.SetComponents( components );
		}


		////////////////

		public CustomEntityComponent GetComponentByType<T>() where T : CustomEntityComponent {
			int comp_count = this.ComponentsInOrder.Count;

			if( this.ComponentsByTypeName.Count != comp_count ) {
				this.ComponentsByTypeName.Clear();

				for( int i = 0; i < comp_count; i++ ) {
					string comp_name = this.ComponentsInOrder[i].GetType().Name;
					this.ComponentsByTypeName[comp_name] = i;
				}
			}

			int idx;

			if( !this.ComponentsByTypeName.TryGetValue( typeof(T).Name, out idx ) ) {
				return null;
			}
			return this.ComponentsInOrder[ idx ];
		}


		////////////////

		public void Sync() {
			if( Main.netMode != 2 ) { throw new Exception("Server only"); }
			CustomEntityProtocol.SendToClients( this );
		}

		internal void SetComponents( IList<CustomEntityComponent> components ) {
			this._ComponentsInOrder = components;
			this.ComponentsInOrder = new ReadOnlyCollection<CustomEntityComponent>( this._ComponentsInOrder );
		}


		////////////////

		internal void Update() {
			int prop_count = this.ComponentsInOrder.Count;
			
			for( int i=0; i<prop_count; i++ ) {
				this.ComponentsInOrder[ i ].Update( this );
			}
		}
	}
}
