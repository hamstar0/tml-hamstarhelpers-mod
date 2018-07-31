using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Internals.NetProtocols;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity {
	public partial class CustomEntity : PacketProtocolData {
		[JsonIgnore]
		public int ID { get; internal set; }

		public CustomEntityCore Core;
		public IList<CustomEntityComponent> Components;

		private IDictionary<string, int> ComponentsByTypeName = new Dictionary<string, int>();



		////////////////

		internal CustomEntity( int id, string name, int width, int height, IList<CustomEntityComponent> components ) {
			this.ID = id;
			this.Core = new CustomEntityCore( name, width, height );
			this.Components = components;

			for( int i=0; i<components.Count; i++ ) {
				CustomEntityComponent comp = components[i].Clone();

				if( comp != null ) {
					this.Components[i] = comp;
				}
			}
		}

		public void CopyFrom( CustomEntity copy ) {
			this.ID = copy.ID;
			this.Core = copy.Core.Clone();
			this.Components = copy.Components.Select( c => c.Clone() ?? c ).ToList();

			this.ComponentsByTypeName.Clear();
		}


		internal CustomEntity Clone() {
			var copy = (CustomEntity)this.MemberwiseClone();

			copy.Core = copy.Core.Clone();
			copy.Components = copy.Components.Select( c => c.Clone() ?? c ).ToList();

			return copy;
		}


		////////////////

		public T GetComponentByType<T>() where T : CustomEntityComponent {
			int comp_count = this.Components.Count;

			if( this.ComponentsByTypeName.Count != comp_count ) {
				this.ComponentsByTypeName.Clear();

				for( int i = 0; i < comp_count; i++ ) {
					Type comp_type = this.Components[i].GetType();
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
			return (T)this.Components[ idx ];
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
			int prop_count = this.Components.Count;
			
			for( int i=0; i<prop_count; i++ ) {
				this.Components[ i ].Update( this );
			}
		}
	}
}
