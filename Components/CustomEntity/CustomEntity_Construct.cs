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

				this.InitializeEntity( data );

				data.InternalPostInitialize();
			}

			////

			public abstract CustomEntityCore InitializeCore();
			public abstract IList<CustomEntityComponent> InitializeComponents();
			protected abstract void InitializeEntity( T ent );
		}



		////////////////

		private new static CustomEntity CreateRaw( Type mytype ) {
			if( !mytype.IsSubclassOf( typeof( CustomEntity ) ) ) {
				throw new NotImplementedException( mytype.Name+" is not a CustomEntity subclass." );
			}

			var data = (CustomEntity)Activator.CreateInstance( mytype,
				BindingFlags.Instance | BindingFlags.NonPublic,
				null,
				new object[] { new PacketProtocolDataConstructorLock( typeof( CustomEntity ) ) },
				null
			);

			return data;
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

		internal static CustomEntity CreateRaw( Type mytype, CustomEntityCore core, IList<CustomEntityComponent> components, Player owner_plr ) {
			var ent = CustomEntity.CreateRaw( mytype );
			ent.Core = core;
			ent.Components = components;
			ent.OwnerPlayerWho = owner_plr.whoAmI;
			ent.OwnerPlayerUID = PlayerIdentityHelpers.GetProperUniqueId( owner_plr );

			ent.InternalPostInitialize();

			return ent;
		}

		internal static CustomEntity CreateRaw( Type mytype, CustomEntityCore core, IList<CustomEntityComponent> components, string owner_uid="" ) {
			var ent = CustomEntity.CreateRaw( mytype );
			ent.Core = core;
			ent.Components = components;
			ent.OwnerPlayerWho = -1;
			ent.OwnerPlayerUID = owner_uid;
			
			Player plr = PlayerIdentityHelpers.GetPlayerByProperId( owner_uid );
			if( plr != null ) {
				ent.OwnerPlayerWho = plr.whoAmI;
			}

			ent.InternalPostInitialize();

			return ent;
		}



		////////////////

		protected CustomEntity( PacketProtocolDataConstructorLock ctor_lock ) : base( ctor_lock ) {
			if( !DotNetHelpers.IsSubclassOfRawGeneric( typeof( CustomEntityFactory<> ), ctor_lock.FactoryType ) ) {
				if( ctor_lock.FactoryType != typeof( CustomEntity ) && ctor_lock.FactoryType != typeof( PacketProtocolData ) ) {
					throw new NotImplementedException( "CustomEntity " + this.GetType().Name + " uses invalid factory " + ctor_lock.FactoryType.Name );
				}
			}
		}


		////////////////

		protected abstract CustomEntityCore CreateCoreTemplate();
		protected abstract IList<CustomEntityComponent> CreateComponentsTemplate();


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

		private void CopyChangesFrom( CustomEntityCore core, IList<CustomEntityComponent> components, Player owner_plr ) { // TODO: Copy changes only!
			this.Core = new CustomEntityCore( core );
			this.OwnerPlayerWho = owner_plr != null ? owner_plr.whoAmI : -1;
			//this.OwnerPlayerUID = owner_plr != null ? PlayerIdentityHelpers.GetProperUniqueId(owner_plr) : "";

			this.Components = components.Select( c => c.InternalClone() ).ToList();

			this.ComponentsByTypeName.Clear();
			this.AllComponentsByTypeName.Clear();

			this.InternalPostInitialize();
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
