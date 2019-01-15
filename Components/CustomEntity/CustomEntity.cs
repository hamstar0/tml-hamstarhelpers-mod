using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
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

		[PacketProtocolIgnore]
		[JsonIgnore]
		private Player OwnerPlayer => this.OwnerPlayerWho == -1 ? null : Main.player[ this.OwnerPlayerWho ];

		[JsonProperty]
		private string[] ComponentNames => this.Components.Select( c => c.GetType().Name ).ToArray();

		[PacketProtocolIgnore]
		[JsonIgnore]
		public abstract bool SyncFromClient { get; }
		[PacketProtocolIgnore]
		[JsonIgnore]
		public abstract bool SyncFromServer { get; }

		[PacketProtocolIgnore]
		[JsonIgnore]
		public virtual bool IsInitialized {
			get {
				if( this.Core == null ) { return false; }
				if( this.Components.Count == 0 ) { return false; }
				return true;
			}
		}



		////////////////
		
		internal void InternalWorldInitialize() {
			for( int i=0; i<this.Components.Count; i++ ) {
				this.Components[i].InternalOnAddToWorld( this );
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
