using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.PlayerHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity {
	public abstract partial class CustomEntity : PacketProtocolData {
		protected abstract class CustomEntityFactory : Factory<CustomEntity> {
			public CustomEntityFactory( out CustomEntity ent ) : base( out ent ) {
				ent.Core = this.InitializeCore();
				ent.Components = this.InitializeComponents();

				ent.OwnerPlayerWho = -1;
				ent.OwnerPlayerUID = "";
			}

			public CustomEntityFactory( Player owner_plr, out CustomEntity ent ) : base( out ent ) {
				ent.Core = this.InitializeCore();
				ent.Components = this.InitializeComponents();

				ent.OwnerPlayerWho = owner_plr.whoAmI;
				ent.OwnerPlayerUID = PlayerIdentityHelpers.GetProperUniqueId( owner_plr );
			}

			protected abstract CustomEntityCore InitializeCore();
			protected abstract IList<CustomEntityComponent> InitializeComponents();
		}



		////////////////

		protected CustomEntity( PacketProtocolDataConstructorLock ctor_lock ) : base( ctor_lock ) {
			if( ctor_lock.Context == typeof(PacketProtocolData.Factory<>) ) {
				throw new NotImplementedException();
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
			
			Player owner = PlayerIdentityHelpers.GetPlayerByProperId( this.OwnerPlayerUID );

			this.OwnerPlayerWho = owner == null ? -1 : owner.whoAmI;
		}


		////////////////

		private void CopyChangesFrom( CustomEntityCore core, IList<CustomEntityComponent> components, Player owner_plr ) {   // TODO: Actually copy changes only!
			//Type my_factory_type = this.GetMainFactoryType();
			//ConstructorInfo ctor = my_factory_type.GetConstructor( new Type[] {
			//	typeof(CustomEntityCore), typeof(IList<CustomEntityComponent>), typeof(string), typeof(CustomEntity)
			//} );
			//var new_ent = (CustomEntity)ctor.Invoke( new object[] { core, components, player_uid } );

			this.Core = new CustomEntityCore( core );
			this.OwnerPlayerWho = owner_plr != null ? owner_plr.whoAmI : -1;
			//this.OwnerPlayerUID = owner_plr != null ? PlayerIdentityHelpers.GetProperUniqueId(owner_plr) : "";

			this.Components = components.Select( c => c.InternalClone() ).ToList();

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
	}
}
