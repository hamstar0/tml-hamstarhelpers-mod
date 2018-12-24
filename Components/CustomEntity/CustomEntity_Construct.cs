using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.Helpers.PlayerHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity {
	public abstract partial class CustomEntity : PacketProtocolData {
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

			this.Components = components.SafeSelect( c => c.InternalClone() ).ToList();
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
