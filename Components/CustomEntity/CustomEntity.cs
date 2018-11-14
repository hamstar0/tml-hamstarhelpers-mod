using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Internals.NetProtocols;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity {
	public abstract partial class CustomEntity : PacketProtocolData {
		public CustomEntityCore Core;
		public IList<CustomEntityComponent> Components = new List<CustomEntityComponent>();

		private IDictionary<string, int> ComponentsByTypeName = new Dictionary<string, int>();
		private IDictionary<string, int> AllComponentsByTypeName = new Dictionary<string, int>();

		[PacketProtocolIgnore]
		public string OwnerPlayerUID = "";
		[JsonIgnore]
		public int OwnerPlayerWho = -1;

		////

		[JsonIgnore]
		[PacketProtocolIgnore]
		private Player OwnerPlayer => this.OwnerPlayerWho == -1 ? null : Main.player[ this.OwnerPlayerWho ];

		[JsonProperty]
		private string[] ComponentNames => this.Components.Select( c => c.GetType().Name ).ToArray();

		[JsonIgnore]
		[PacketProtocolIgnore]
		public bool IsInitialized {
			get {
				if( this.Core == null ) { return false; }
				if( this.Components.Count == 0 ) { return false; }
				return true;
			}
		}



		////////////////
		
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

		public CustomEntityComponent GetComponentByName( string name ) {
			if( this.ComponentsByTypeName.Count != this.Components.Count ) {
				this.RefreshComponentTypeNames();
			}

			int idx;

			if( !this.AllComponentsByTypeName.TryGetValue( name, out idx ) ) {
				return null;
			}
			return this.Components[idx];
		}


		////////////////

		public void SyncTo() {
			if( Main.netMode == 2 ) {
				CustomEntityProtocol.SendToClients( this );
			} else if( Main.netMode == 1 ) {
				CustomEntityProtocol.SyncToAll( this );
			} else {
				throw new HamstarException( "!ModHelpers.CustomEntity.SyncTo - Multiplayer only." );
			}
		}


		internal void SyncFrom( CustomEntity ent ) {
			if( ModHelpersMod.Instance.Config.DebugModeCustomEntityInfo ) {
				LogHelpers.Log( "ModHelpers.CustomEntity.SyncFrom - Synced from " + ent.ToString() + " for "+ this.ToString() );
			}

			this.CopyChangesFrom( ent.Core, ent.Components, ent.OwnerPlayer );
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
			string typeid = "type "+CustomEntityManager.GetIdByTypeName( this.GetType().Name );
			string who = "";
			string owner = ", owner:";

			if( this.Core == null ) {
				basename = "Undefined entity";
			} else {
				basename = this.Core.DisplayName;
				who = ", who " + this.Core.whoAmI;
			}

			if( this.OwnerPlayerUID != "" ) {
				owner += " "+this.OwnerPlayerUID.Substring( 0, 8 )+"...";
			}
			if( this.OwnerPlayerWho != -1 ) {
				owner += " '" + Main.player[this.OwnerPlayerWho].name + "':" + this.OwnerPlayerWho;
			}
			if( this.OwnerPlayerUID == "" && this.OwnerPlayerWho == -1 ) {
				owner += " none";
			}

			if( this.Components != null ) {
				typeid = typeid+":"+this.Components.Count();
			}

			return basename + " ("+ typeid + who + owner + ")";
		}
	}
}
