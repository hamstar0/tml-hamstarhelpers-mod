using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Internals.NetProtocols;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity {
	/// <summary>
	/// Implements extensible custom game entities. Uses Components to implement functionality.
	/// </summary>
	public partial class CustomEntity : Entity {
		public string DisplayName = "";

		/// <summary>
		/// Lists all Components in order.
		/// </summary>
		public IList<CustomEntityComponent> ComponentsInOrder;

		private IDictionary<string, int> ComponentsByTypeName = new Dictionary<string, int>();



		////////////////

		public CustomEntity( string name, IList<CustomEntityComponent> components ) {
			this.DisplayName = name;
			this.ComponentsInOrder = components;
		}

		
		internal void SetComponents( IList<CustomEntityComponent> components ) {
			this.ComponentsInOrder = components;
			this.ComponentsByTypeName.Clear();
		}


		////////////////

		public T GetComponentByType<T>() where T : CustomEntityComponent {
			int comp_count = this.ComponentsInOrder.Count;

			if( this.ComponentsByTypeName.Count != comp_count ) {
				this.ComponentsByTypeName.Clear();

				for( int i = 0; i < comp_count; i++ ) {
					Type comp_type = this.ComponentsInOrder[i].GetType();
					do {
						string comp_name = comp_type.Name;

						this.ComponentsByTypeName[ comp_name ] = i;

						comp_type = comp_type.BaseType;
					} while( comp_type.Name != "CustomEntityComponent" );
				}
			}

			int idx;

			if( !this.ComponentsByTypeName.TryGetValue( typeof(T).Name, out idx ) ) {
				return null;
			}
			return (T)this.ComponentsInOrder[ idx ];
		}


		////////////////

		public void Sync() {
			if( Main.netMode == 2 ) {
				CustomEntityProtocol.SendToClients( this );
			} else if( Main.netMode == 1 ) {
				CustomEntityProtocol.SyncToAll( this );
			} else {
				throw new Exception( "Multiplayer only." );
			}
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
