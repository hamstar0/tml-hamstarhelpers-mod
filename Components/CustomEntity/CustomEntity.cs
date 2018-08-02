using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
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
		public IList<CustomEntityComponent> Components = new List<CustomEntityComponent>();

		private IDictionary<string, int> ComponentsByTypeName = new Dictionary<string, int>();
		private IDictionary<string, int> AllComponentsByTypeName = new Dictionary<string, int>();

		[JsonProperty]
		private string[] ComponentNames {
			get {
				return this.Components.Select( t => t.GetType().Name ).ToArray();
			}
		}



		////////////////

		[JsonConstructor]
		internal CustomEntity() { }

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

		internal CustomEntity( CustomEntityCore core, IList<CustomEntityComponent> components ) {
			this.ID = CustomEntityManager.Instance.GetTemplateID( components );
			this.Core = core;
			this.Components = components;
		}

		////////////////

		public void CopyFrom( CustomEntity copy ) {
			this.ID = copy.ID;
			this.Core = copy.Core.Clone();
			this.Components = copy.Components.Select( c => c.Clone() ?? c ).ToList();

			this.ComponentsByTypeName.Clear();
			this.AllComponentsByTypeName.Clear();
		}


		internal CustomEntity Clone() {
			var copy = (CustomEntity)this.MemberwiseClone();

			copy.Core = copy.Core.Clone();
			copy.Components = copy.Components.Select( c => c.Clone() ?? c ).ToList();

			return copy;
		}


		////////////////

		private void RefreshComponentTypeNames() {
			int comp_count = this.Components.Count;

			this.ComponentsByTypeName.Clear();
			this.AllComponentsByTypeName.Clear();

			for( int i = 0; i < comp_count; i++ ) {
				Type comp_type = this.Components[i].GetType();
				string comp_name = comp_type.Name;

				this.ComponentsByTypeName[comp_name] = i;

				do {
					this.AllComponentsByTypeName[comp_name] = i;

					comp_type = comp_type.BaseType;
					comp_name = comp_type.Name;
				} while( comp_type.Name != "CustomEntityComponent" );
			}
		}


		public T GetComponentByType<T>() where T : CustomEntityComponent {
			if( this.ComponentsByTypeName.Count != this.Components.Count ) {
				this.RefreshComponentTypeNames();
			}

			int idx;

			if( !this.AllComponentsByTypeName.TryGetValue( typeof(T).Name, out idx ) ) {
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

			switch( Main.netMode ) {
			case 0:
				if( !Main.dedServ ) {
					for( int i = 0; i < prop_count; i++ ) {
						this.Components[i].UpdateSingle( this );
					}
				}
				break;
			case 1:
				for( int i = 0; i < prop_count; i++ ) {
					this.Components[i].UpdateClient( this );
				}
				break;
			case 2:
				for( int i = 0; i < prop_count; i++ ) {
					this.Components[i].UpdateServer( this );
				}
				break;
			}
		}
	}
}
