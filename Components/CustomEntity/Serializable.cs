using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.PlayerHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;


namespace HamstarHelpers.Components.CustomEntity {
	internal partial class SerializableCustomEntity : CustomEntity {
		public static string GetTypeName( CustomEntity ent ) {
			return ent.GetType().Name;
		}



		////////////////

		public string MyTypeName;

		////////////////

		[PacketProtocolIgnore]
		[JsonIgnore]
		public override bool IsInitialized {
			get {
				if( string.IsNullOrEmpty(this.MyTypeName) ) {
					return false;
				}
				return base.IsInitialized;
			}
		}



		////////////////

		protected SerializableCustomEntity( PacketProtocolDataConstructorLock ctorLock ) : base( ctorLock ) { }

		internal SerializableCustomEntity( CustomEntity ent )
				: base( new PacketProtocolDataConstructorLock( typeof( CustomEntity ) ) ) {
			this.MyTypeName = SerializableCustomEntity.GetTypeName( ent );
			this.Core = ent.Core;
			this.Components = ent.InternalComponents;
			this.OwnerPlayerUID = ent.MyOwnerPlayerUID;
			this.OwnerPlayerWho = ent.MyOwnerPlayerWho;
		}

		internal SerializableCustomEntity( string typeName, CustomEntityCore core, IList<CustomEntityComponent> components, string playerUid )
				: base( new PacketProtocolDataConstructorLock( typeof( CustomEntity ) ) ) {
			this.MyTypeName = typeName;
			this.Core = core;
			this.Components = components;
			this.OwnerPlayerUID = playerUid;
			this.OwnerPlayerWho = PlayerIdentityHelpers.GetPlayerByProperId( playerUid )?.whoAmI ?? -1;
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

			Type entType = CustomEntityManager.GetTypeByName( this.MyTypeName );

			if( entType == null ) {
				throw new HamstarException( this.MyTypeName + " does not exist." );
			}
			if( !entType.IsSubclassOf( typeof( CustomEntity ) ) ) {
				throw new HamstarException( entType.Name + " is not a valid CustomEntity." );
			}

			if( string.IsNullOrEmpty( this.OwnerPlayerUID ) ) {
				return CustomEntity.CreateRaw( entType, this.Core, this.Components );
			} else {
				return CustomEntity.CreateRaw( entType, this.Core, this.Components, this.OwnerPlayerUID );
			}
			//var args = this.OwnerPlayerUID == "" ?
			//	new object[] { this.Core, this.Components } :
			//	new object[] { this.OwnerPlayerUID, this.Core, this.Components };
			//return (CustomEntity)Activator.CreateInstance( entType,
			//	BindingFlags.NonPublic | BindingFlags.Instance,
			//	null,
			//	args,
			//	null );
		}
	}
}
