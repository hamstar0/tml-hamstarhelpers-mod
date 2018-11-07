using HamstarHelpers.Components.CustomEntity.Templates;
using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.PlayerHelpers;
using HamstarHelpers.Internals.NetProtocols;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity {
	public partial class CustomEntity : PacketProtocolData {
		[PacketProtocolIgnore]
		public string OwnerPlayerUID = "";
		[JsonIgnore]
		public int OwnerPlayerWho = -1;

		public CustomEntityCore Core;
		public IList<CustomEntityComponent> Components = new List<CustomEntityComponent>();

		private IDictionary<string, int> ComponentsByTypeName = new Dictionary<string, int>();
		private IDictionary<string, int> AllComponentsByTypeName = new Dictionary<string, int>();

		////

		[JsonIgnore]
		public int TypeID { get; internal set; }

		[JsonProperty]
		private string[] ComponentNames => this.Components.Select( c => c.GetType().Name ).ToArray();

		[JsonIgnore]
		[PacketProtocolIgnore]
		public bool IsInitialized => this.Components.All( c => c.IsInitialized );



		////////////////

		private CustomEntity( PacketProtocolDataConstructorLock ctor_lock ) : this() { }

		[JsonConstructor]
		internal CustomEntity() {
			this.TypeID = -1;
		}

		internal CustomEntity( CustomEntityCore core, IList<CustomEntityComponent> components ) {
			this.Initialize( "", -1, core, components );
		}

		internal CustomEntity( string owner_uid, CustomEntityCore core, IList<CustomEntityComponent> components ) {
			Player owner = PlayerIdentityHelpers.GetPlayerByProperId( owner_uid );
			if( owner == null ) {
				throw new HamstarException( "!ModHelpers.CustomEntity.CTor_3 - Could not verify if entity's owner (by id '"+owner_uid+"') is present or absent." );
			}

			int owner_who = owner == null ? -1 : owner.whoAmI;

			this.Initialize( owner_uid, owner_who, core, components );
		}

		internal CustomEntity( Player owner, CustomEntityCore core, IList<CustomEntityComponent> components ) {
			string id = PlayerIdentityHelpers.GetProperUniqueId( owner );

			this.Initialize( id, owner.whoAmI, core, components );
		}


		////////////////

		private void Initialize( string owner_uid, int owner_who, CustomEntityCore core, IList<CustomEntityComponent> components ) {
			this.TypeID = CustomEntityTemplateManager.GetTemplateID( components );
			if( this.TypeID == -1 ) {
				string comp_str = string.Join( ", ", components.Select( c => c.GetType().Name ) );
				throw new NotImplementedException( "!ModHelpers.CustomEntity.Initialize - No custom entity ID found to match to new entity called "
					+ core.DisplayName + ". Components: " + comp_str );
			}

			this.OwnerPlayerUID = owner_uid;
			this.OwnerPlayerWho = owner_who;
			this.Core = core;
			this.Components = components;
		}


		////////////////

		internal void RefreshOwnerWho() {
			if( Main.netMode == 1 ) {
				throw new HamstarException( "No client." );
			}

			if( string.IsNullOrEmpty( this.OwnerPlayerUID ) ) {
				this.OwnerPlayerWho = -1;
				return;
			}
			
			Player owner = PlayerIdentityHelpers.GetPlayerByProperId( this.OwnerPlayerUID );

			this.OwnerPlayerWho = owner == null ? -1 : owner.whoAmI;
		}


		////////////////

		public void CopyChangesFrom( CustomEntity copy ) {	// TODO: Actually copy changes only!
			if( this.TypeID == -1 ) {
				this.Core = new CustomEntityCore();
			}
			this.TypeID = copy.TypeID;
			this.OwnerPlayerWho = copy.OwnerPlayerWho;
			//this.OwnerPlayerUID = copy.OwnerPlayerUID;

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
			string typeid = "type "+this.TypeID;
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
				typeid = typeid + ":"+this.Components.Count();
			}

			return basename + " ("+ typeid + who + owner + ")";
		}
	}
}
