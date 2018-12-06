using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.Helpers.PlayerHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity {
	public abstract partial class CustomEntity : PacketProtocolData {
		protected abstract class CustomEntityFactory<T> : Factory<T> where T : CustomEntity {
			public readonly Player OwnerPlayer;


			////////////////
			
			protected CustomEntityFactory( Player ownerPlr=null ) {
				this.OwnerPlayer = ownerPlr;
			}

			////////////////

			protected sealed override void Initialize( T data ) {
				if( this.OwnerPlayer != null ) {
					data.OwnerPlayerWho = this.OwnerPlayer.whoAmI;
					data.OwnerPlayerUID = PlayerIdentityHelpers.GetProperUniqueId( this.OwnerPlayer );
				} else {
					data.OwnerPlayerWho = -1;
					data.OwnerPlayerUID = "";
				}

				data.Core = data.CreateCore<T>( this );
				data.Components = data.CreateComponents<T>( this );

				this.InitializeEntity( data );

				data.InternalPostInitialize();
			}

			////

			protected abstract void InitializeEntity( T ent );
		}



		////////////////

		private new static CustomEntity CreateRaw( Type mytype ) {
			if( !mytype.IsSubclassOf( typeof( CustomEntity ) ) ) {
				throw new NotImplementedException( mytype.Name+" is not a CustomEntity subclass." );
			}

			var ent = (CustomEntity)Activator.CreateInstance( mytype,
				BindingFlags.Instance | BindingFlags.NonPublic,
				null,
				new object[] { new PacketProtocolDataConstructorLock( typeof( CustomEntity ) ) },
				null
			);

			return ent;
		}

		internal static CustomEntity CreateRaw( Type mytype, CustomEntityCore core, IList<CustomEntityComponent> components ) {
			var ent = CustomEntity.CreateRaw( mytype );
			ent.Core = core;
			ent.Components = components;
			ent.OwnerPlayerWho = -1;
			ent.OwnerPlayerUID = "";

			ent.InternalPostInitialize();

			return ent;
		}

		internal static CustomEntity CreateRaw( Type mytype, CustomEntityCore core, IList<CustomEntityComponent> components, Player ownerPlr ) {
			var ent = CustomEntity.CreateRaw( mytype );
			ent.Core = core;
			ent.Components = components;
			ent.OwnerPlayerWho = ownerPlr.whoAmI;
			ent.OwnerPlayerUID = PlayerIdentityHelpers.GetProperUniqueId( ownerPlr );

			ent.InternalPostInitialize();

			return ent;
		}

		internal static CustomEntity CreateRaw( Type mytype, CustomEntityCore core, IList<CustomEntityComponent> components, string ownerUid="" ) {
			var ent = CustomEntity.CreateRaw( mytype );
			ent.Core = core;
			ent.Components = components;
			ent.OwnerPlayerWho = -1;
			ent.OwnerPlayerUID = ownerUid;
			
			Player plr = PlayerIdentityHelpers.GetPlayerByProperId( ownerUid );
			if( plr != null ) {
				ent.OwnerPlayerWho = plr.whoAmI;
			}

			ent.InternalPostInitialize();

			return ent;
		}



		////////////////

		protected CustomEntity( PacketProtocolDataConstructorLock ctorLock ) : base( ctorLock ) {
			if( !DotNetHelpers.IsSubclassOfRawGeneric( typeof( CustomEntityFactory<> ), ctorLock.FactoryType ) ) {
				if( ctorLock.FactoryType != typeof( CustomEntity ) && ctorLock.FactoryType != typeof( PacketProtocolData ) ) {
					throw new NotImplementedException( "CustomEntity " + this.GetType().Name + " uses invalid factory " + ctorLock.FactoryType.Name );
				}
			}
		}



		////////////////
		
		protected abstract CustomEntityCore CreateCore<T>( CustomEntityFactory<T> factory ) where T : CustomEntity;
		protected abstract IList<CustomEntityComponent> CreateComponents<T>( CustomEntityFactory<T> factory ) where T : CustomEntity;
		public abstract CustomEntityCore CreateCoreTemplate();
		public abstract IList<CustomEntityComponent> CreateComponentsTemplate();

		
		////////////////

		internal void CopyChangesFrom( CustomEntity ent ) { // TODO: Copy changes only!
			if( !ent.IsInitialized ) {
				throw new HamstarException( "!ModHelpers.CustomEntity.CopyChangesFrom(CustomEntity) - Parameter not initialized." );
			}

			this.CopyChangesFrom( ent.Core, ent.Components, ent.OwnerPlayer );

			if( ModHelpersMod.Instance.Config.DebugModeCustomEntityInfo ) {
				LogHelpers.Log( "ModHelpers.CustomEntity.CopyChangesFrom(CustomEntity) - Synced from " + ent.ToString() + " for " + this.ToString() );
			}
		}


		internal void CopyChangesFrom( CustomEntityCore core, IList<CustomEntityComponent> components, Player ownerPlr=null ) {
			this.Core = new CustomEntityCore( core );
			this.OwnerPlayerWho = ownerPlr != null ? ownerPlr.whoAmI : -1;
			this.OwnerPlayerUID = ownerPlr != null ? PlayerIdentityHelpers.GetProperUniqueId(ownerPlr) : "";

			this.Components = components.Select( c => c.InternalClone() ).ToList();
			this.ClearComponentCache();

			if( !this.IsInitialized ) {
				throw new HamstarException( "!ModHelpers."+this.GetType().Name+".CopyChangesFrom - Not initialized post-copy." );
			}

			this.InternalPostInitialize();
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
				LogHelpers.Log( "ModHelpers.CustomEntity.RefreshOwnerWho - No player found with UID "+this.OwnerPlayerUID );
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
					if( this.ComponentsByTypeName.ContainsKey(compName) ) {
						throw new HamstarException( "!ModHelpers.CustomEntity.RefreshComponentTypeNames - "+this.GetType().Name+" component "+compName+" already exists." );
					}

					this.AllComponentsByTypeName[ compName ] = i;

					compType = compType.BaseType;
					compName = compType.Name;
				} while( compType.Name != "CustomEntityComponent" );
			}
		}
	}
}
