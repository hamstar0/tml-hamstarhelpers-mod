using HamstarHelpers.Components.CustomEntity.Templates;
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

		private CustomEntity( PacketProtocolDataConstructorLock ctor_lock ) : this() { }

		[JsonConstructor]
		internal CustomEntity() {
			this.ID = -1;
		}

		internal CustomEntity( CustomEntityCore core, IList<CustomEntityComponent> components ) {
			this.ID = CustomEntityTemplateManager.GetID( components );
			if( this.ID == -1 ) {
				throw new NotImplementedException( "No custom entity ID found to match to new entity called " + core.DisplayName
					+ ". Components: " + string.Join( ", ", components.Select( c => c.GetType().Name ) ) );
			}

			this.Core = core;
			this.Components = components;
		}


		////////////////

		public void CopyChangesFrom( CustomEntity copy ) {	// TODO: Actually copy changes only!
			if( this.ID == -1 ) {
				this.Core = new CustomEntityCore();
			}
			this.ID = copy.ID;

			this.Core.CopyFrom( copy.Core );

			this.Components = copy.Components.Select( c => c.InternalClone() ).ToList();

			this.ComponentsByTypeName.Clear();
			this.AllComponentsByTypeName.Clear();
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

		public void SyncTo() {
			if( Main.netMode == 2 ) {
				CustomEntityProtocol.SendToClients( this );
			} else if( Main.netMode == 1 ) {
				CustomEntityProtocol.SyncToAll( this );
			} else {
				throw new Exception( "Multiplayer only." );
			}
		}


		internal void SyncFrom( CustomEntity ent ) {
			this.CopyChangesFrom( ent );
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


		////////////////

		public override string ToString() {
			string basename = "";
			string typeid = "type "+this.ID;
			string who = "";

			if( this.Core == null ) {
				basename = "Undefined entity";
			} else {
				basename = this.Core.DisplayName;
				who = ", who " + this.Core.whoAmI;
			}

			if( this.Components != null ) {
				typeid = typeid + ":"+this.Components.Count();
			}

			return basename + " ("+ typeid + who + ")";
		}
	}
}
