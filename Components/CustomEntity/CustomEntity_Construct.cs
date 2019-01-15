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
		protected CustomEntity( PacketProtocolDataConstructorLock ctorLock ) : base( ctorLock ) { }

		////

		protected sealed override void OnInitialize() {
			for( int i = 0; i < this.Components.Count; i++ ) {
				this.Components[i].InternalOnEntityInitialize( this );
			}
		}


		////////////////

		protected abstract CustomEntityCore CreateCore( CustomEntityFactory factory );

		protected abstract IList<CustomEntityComponent> CreateComponents( CustomEntityFactory factory );

		public abstract CustomEntityCore CreateCoreTemplate();

		public abstract IList<CustomEntityComponent> CreateComponentsTemplate();


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
	}
}
