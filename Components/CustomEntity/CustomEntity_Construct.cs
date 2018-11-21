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
		protected abstract class CustomEntityFactory<T> : Factory<T> where T : CustomEntity {
			protected readonly Player OwnerPlayer;


			////////////////
			
			protected CustomEntityFactory( Player owner_plr=null ) {
				this.OwnerPlayer = owner_plr;
			}

			////////////////

			public override void Initialize( T data ) {
				if( this.OwnerPlayer != null ) {
					data.OwnerPlayerWho = this.OwnerPlayer.whoAmI;
					data.OwnerPlayerUID = PlayerIdentityHelpers.GetProperUniqueId( this.OwnerPlayer );
				} else {
					data.OwnerPlayerWho = -1;
					data.OwnerPlayerUID = "";
				}

				data.Core = this.InitializeCore();
				data.Components = this.InitializeComponents();
				this.PostInitialize( data );
			}

			////

			protected abstract CustomEntityCore InitializeCore();
			protected abstract IList<CustomEntityComponent> InitializeComponents();
			protected abstract void PostInitialize( T ent );
		}



		////////////////

		internal static CustomEntity CreateRaw( Type mytype, CustomEntityCore core, IList<CustomEntityComponent> components ) {
			var ent = (CustomEntity)PacketProtocolData.CreateRaw( mytype );
			ent.Core = core;
			ent.Components = components;
			ent.OwnerPlayerWho = -1;
			ent.OwnerPlayerUID = "";
			return ent;
		}

		internal static CustomEntity CreateRaw( Type mytype, CustomEntityCore core, IList<CustomEntityComponent> components, Player owner_plr ) {
			var ent = (CustomEntity)PacketProtocolData.CreateRaw( mytype );
			ent.Core = core;
			ent.Components = components;
			ent.OwnerPlayerWho = owner_plr.whoAmI;
			ent.OwnerPlayerUID = PlayerIdentityHelpers.GetProperUniqueId( owner_plr );
			return ent;
		}

		internal static CustomEntity CreateRaw( Type mytype, CustomEntityCore core, IList<CustomEntityComponent> components, string owner_uid ) {
			Player plr = owner_uid != "" ? PlayerIdentityHelpers.GetPlayerByProperId( owner_uid ) : null;

			if( plr == null ) {
				return CustomEntity.CreateRaw( mytype, core, components );
			} else {
				return CustomEntity.CreateRaw( mytype, core, components, plr );
			}
		}



		////////////////

		protected CustomEntity( PacketProtocolDataConstructorLock ctor_lock ) : base( ctor_lock ) { }


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

				this.ComponentsByTypeName[ comp_name ] = i;

				do {
					this.AllComponentsByTypeName[ comp_name ] = i;

					comp_type = comp_type.BaseType;
					comp_name = comp_type.Name;
				} while( comp_type.Name != "CustomEntityComponent" );
			}
		}
	}
}
