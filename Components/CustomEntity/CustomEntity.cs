using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Internals.NetProtocols;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity {
	public abstract partial class CustomEntity : PacketProtocolData {
		public CustomEntityCore Core;
		public IList<CustomEntityComponent> Components = new List<CustomEntityComponent>();

		private IDictionary<string, int> ComponentsByTypeName = new Dictionary<string, int>();
		private IDictionary<string, int> AllComponentsByTypeName = new Dictionary<string, int>();

		[PacketProtocolWriteIgnoreClient]
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
		public virtual bool IsInitialized {
			get {
				if( this.Core == null ) { return false; }
				if( this.Components.Count == 0 ) { return false; }
				return true;
			}
		}



		////////////////

		protected virtual void PostInitialize() { }

		internal void InternalPostInitialize() {
			this.PostInitialize();
		}


		////////////////

		protected void ClearComponentCache() {
			this.ComponentsByTypeName.Clear();
			this.AllComponentsByTypeName.Clear();
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

		public void SyncToAll() {
			if( !this.IsInitialized ) {
				throw new HamstarException( "!ModHelpers.CustomEntity.SyncToAll ("+this.GetType().Name+") - Not initialized." );
			}
			if( !SaveableEntityComponent.HaveAllEntitiesLoaded ) {
				LogHelpers.Log( "!ModHelpers.CustomEntity.SyncToAll ("+this.GetType().Name+") - Entities not yet loaded." );
				return;
			}

			if( ModHelpersMod.Instance.Config.DebugModeCustomEntityInfo ) {
				LogHelpers.Log( "ModHelpers.CustomEntity.SyncToAll ("+this.GetType().Name+")" );
			}

			if( Main.netMode == 0 ) {
				throw new HamstarException( "!ModHelpers.CustomEntity.SyncToAll (" + this.GetType().Name + ") - Multiplayer only." );
			}

			if( Main.netMode == 2 ) {
				CustomEntityProtocol.SendToClients( this );
			} else if( Main.netMode == 1 ) {
				CustomEntityProtocol.SyncToAll( this );
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

		protected override void WriteStream( BinaryWriter writer ) {
			throw new NotImplementedException( "WriteStream not implemented." );
		}
		protected override void ReadStream( BinaryReader reader ) {
			throw new NotImplementedException( "ReadStream not implemented." );
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

			if( this.OwnerPlayerUID != "" ) {
				owner += " " + this.OwnerPlayerUID.Substring( 0, 8 ) + "...";
			}
			if( this.OwnerPlayerWho != -1 ) {
				owner += " '" + Main.player[this.OwnerPlayerWho].name + "':" + this.OwnerPlayerWho;
			}
			if( this.OwnerPlayerUID == "" && this.OwnerPlayerWho == -1 ) {
				owner += " none";
			}

			if( this.Components != null ) {
				typeid = typeid + ":" + this.Components.Count();
			}

			return basename + " ["+this.GetType().Name+"] " + " (" + typeid + who + owner + ")";
		}
	}
}
