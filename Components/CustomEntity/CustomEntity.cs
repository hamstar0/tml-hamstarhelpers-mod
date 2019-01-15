using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.Helpers.PlayerHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity {
	public abstract partial class CustomEntity : PacketProtocolData {
		public CustomEntityCore Core;
		[JsonRequired]
		protected IList<CustomEntityComponent> Components = new List<CustomEntityComponent>();

		private IDictionary<string, int> ComponentsByTypeName = new Dictionary<string, int>();
		private IDictionary<string, int> AllComponentsByTypeName = new Dictionary<string, int>();

		////

		[JsonRequired]
		[PacketProtocolWriteIgnoreClient]
		protected string OwnerPlayerUID = "";
		[JsonIgnore]
		protected int OwnerPlayerWho = -1;

		[PacketProtocolIgnore]
		[JsonIgnore]
		public string MyOwnerPlayerUID => this.OwnerPlayerUID;
		[PacketProtocolIgnore]
		[JsonIgnore]
		public int MyOwnerPlayerWho => this.OwnerPlayerWho;

		////////////////

		[PacketProtocolIgnore]
		[JsonIgnore]
		internal IList<CustomEntityComponent> InternalComponents => this.Components;



		////////////////

		protected CustomEntity( PacketProtocolDataConstructorLock ctorLock ) : base( ctorLock ) { }

		////

		protected sealed override void OnInitialize() {
			for( int i = 0; i < this.Components.Count; i++ ) {
				this.Components[i].InternalOnEntityInitialize( this );
			}
		}
		
		internal void InternalOnAddToWorld() {
			for( int i=0; i<this.Components.Count; i++ ) {
				this.Components[i].InternalOnAddToWorld( this );
			}
		}


		////////////////

		internal void CopyChangesFrom( CustomEntity ent ) { // TODO: Copy changes only!
			if( !ent.IsInitialized ) {
				//throw new HamstarException( "!ModHelpers.CustomEntity.CopyChangesFrom(CustomEntity) - Parameter not initialized." );
				throw new HamstarException( "Parameter not initialized." );
			}

			this.CopyChangesFrom( ent.Core, ent.Components, ent.OwnerPlayer );

			if( ModHelpersMod.Instance.Config.DebugModeCustomEntityInfo ) {
				LogHelpers.Alert( "Synced from " + ent.ToString() + " for " + this.ToString() );
			}
		}


		internal void CopyChangesFrom( CustomEntityCore core, IList<CustomEntityComponent> components, Player ownerPlr = null ) {
			this.Core = new CustomEntityCore( core );
			this.OwnerPlayerWho = ownerPlr != null ? ownerPlr.whoAmI : -1;
			this.OwnerPlayerUID = ownerPlr != null ? PlayerIdentityHelpers.GetProperUniqueId( ownerPlr ) : "";

			this.Components = components.SafeSelect( c => c.InternalClone() ).ToList();
			this.ClearComponentCache();

			if( !this.IsInitialized ) {
				//throw new HamstarException( "!ModHelpers."+this.GetType().Name+".CopyChangesFrom - Not initialized post-copy." );
				throw new HamstarException( "Not initialized post-copy." );
			}
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

			Player ownerPlr = PlayerIdentityHelpers.GetPlayerByProperId( this.OwnerPlayerUID );
			if( ownerPlr == null ) {
				LogHelpers.Alert( "No player found with UID " + this.OwnerPlayerUID );
			}

			this.OwnerPlayerWho = ownerPlr == null ? -1 : ownerPlr.whoAmI;
		}

		////////////////

		private void RefreshComponentTypeNames() {
			int compCount = this.Components.Count;

			this.ComponentsByTypeName.Clear();
			this.AllComponentsByTypeName.Clear();

			for( int i = 0; i < compCount; i++ ) {
				Type compType = this.Components[i].GetType();
				string compName = compType.Name;

				do {
					if( this.ComponentsByTypeName.ContainsKey( compName ) ) {
						//throw new HamstarException( "!ModHelpers.CustomEntity.RefreshComponentTypeNames - "+this.GetType().Name+" component "+compName+" already exists." );
						throw new HamstarException( "Component " + compName + " already exists." );
					}

					this.AllComponentsByTypeName[compName] = i;

					compType = compType.BaseType;
					compName = compType.Name;
				} while( compType.Name != "CustomEntityComponent" );
			}
		}


		////////////////

		internal void Update() {
			int propCount = this.Components.Count;

			switch( Main.netMode ) {
			case 0:
				if( !Main.dedServ ) {
					for( int i = 0; i < propCount; i++ ) {
						this.Components[i].UpdateSingle( this );
					}
				}
				break;
			case 1:
				for( int i = 0; i < propCount; i++ ) {
					this.Components[i].UpdateClient( this );
				}
				break;
			case 2:
				for( int i = 0; i < propCount; i++ ) {
					this.Components[i].UpdateServer( this );
				}
				break;
			}
		}


		////////////////

		public override string ToString() {
			string basename = "";
			string typeid = "type " + CustomEntityManager.GetIdByTypeName( this.GetType().Name );
			string who = "";
			string owner = ", owner:";

			if( this.Core == null ) {
				basename = "Undefined entity";
			} else {
				basename = this.Core.DisplayName;
				who = ", who " + this.Core.whoAmI;
			}

			if( !string.IsNullOrEmpty(this.OwnerPlayerUID) ) {
				owner += " " + this.OwnerPlayerUID.Substring( 0, 8 ) + "...";
			}
			if( this.OwnerPlayerWho != -1 ) {
				owner += " " + (("'"+Main.player[this.OwnerPlayerWho]?.name+"'") ?? "MISSING_PLAYER") + "':" + this.OwnerPlayerWho;
			}
			if( this.OwnerPlayerUID == "" && this.OwnerPlayerWho == -1 ) {
				owner += " none";
			}

			if( this.Components != null ) {
				typeid = typeid + ":" + this.Components.Count();
			}

			return basename + " ["+this.GetType().Name+"] (" + typeid + who + owner + ")";
		}
	}
}
