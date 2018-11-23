using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity {
	internal partial class SerializableCustomEntity : CustomEntity {
		public string MyTypeName;

		////////////////

		[JsonIgnore]
		[PacketProtocolIgnore]
		public override bool IsInitialized {
			get {
				if( string.IsNullOrEmpty(this.MyTypeName) ) {
					return false;
				}
				return base.IsInitialized;
			}
		}



		////////////////

		protected SerializableCustomEntity( PacketProtocolDataConstructorLock ctor_lock ) : base( ctor_lock ) { }

		internal SerializableCustomEntity( CustomEntity ent )
				: base( new PacketProtocolDataConstructorLock( typeof( CustomEntity ) ) ) {
			this.MyTypeName = ent.GetType().Name;
			this.Core = ent.Core;
			this.Components = ent.Components;
			this.OwnerPlayerUID = ent.OwnerPlayerUID;
		}

		internal SerializableCustomEntity( string type_name, CustomEntityCore core, IList<CustomEntityComponent> components, string player_uid )
				: base( new PacketProtocolDataConstructorLock( typeof( CustomEntity ) ) ) {
			this.MyTypeName = type_name;
			this.Core = core;
			this.Components = components;
			this.OwnerPlayerUID = player_uid;
		}


		////////////////

		protected override IList<CustomEntityComponent> CreateComponents<T>( CustomEntityFactory<T> factory ) {
			throw new NotImplementedException( "CreateComponents not implemented" );
			//if( !this.IsInitialized ) { throw new NotImplementedException( "SerializableCustomEntity components not initialized." ); }
			//return this.Components.ToList();
		}
		protected override CustomEntityCore CreateCore<T>( CustomEntityFactory<T> factory ) {
			throw new NotImplementedException( "CreateCore not implemented" );
			//return new CustomEntityCore( this.Core );
		}
		public override CustomEntityCore CreateCoreTemplate() {
			throw new NotImplementedException( "CreateCoreTemplate not implemented" );
		}
		public override IList<CustomEntityComponent> CreateComponentsTemplate() {
			throw new NotImplementedException( "CreateComponentsTemplate not implemented" );
		}


		////////////////

		internal CustomEntity Convert() {
			if( !this.IsInitialized ) {
				throw new HamstarException( "!ModHelpers.SerializableCustomEntity.Convert - Not initialized." );
			}

			Type ent_type = CustomEntityManager.GetEntityType( this.MyTypeName );

			if( ent_type == null ) {
				throw new HamstarException( this.MyTypeName + " does not exist." );
			}
			if( !ent_type.IsSubclassOf( typeof( CustomEntity ) ) ) {
				throw new HamstarException( ent_type.Name + " is not a valid CustomEntity." );
			}

			if( string.IsNullOrEmpty( this.OwnerPlayerUID ) ) {
				return CustomEntity.CreateRaw( ent_type, this.Core, this.Components );
			} else {
				return CustomEntity.CreateRaw( ent_type, this.Core, this.Components, this.OwnerPlayerUID );
			}
			//var args = this.OwnerPlayerUID == "" ?
			//	new object[] { this.Core, this.Components } :
			//	new object[] { this.OwnerPlayerUID, this.Core, this.Components };
			//return (CustomEntity)Activator.CreateInstance( ent_type,
			//	BindingFlags.NonPublic | BindingFlags.Instance,
			//	null,
			//	args,
			//	null );
		}
	}
}
