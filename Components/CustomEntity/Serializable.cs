using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.PlayerHelpers;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;


namespace HamstarHelpers.Components.CustomEntity {
	internal sealed partial class SerializableCustomEntity : CustomEntity {
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

		private SerializableCustomEntity() : base( null ) { }

		internal SerializableCustomEntity( CustomEntity ent ) : base( null ) {
			this.MyTypeName = SerializableCustomEntity.GetTypeName( ent );
			this.Core = ent.Core;
			this.Components = ent.InternalComponents;
			this.OwnerPlayerUID = ent.MyOwnerPlayerUID;
			this.OwnerPlayerWho = ent.MyOwnerPlayerWho;
		}

		internal SerializableCustomEntity( string typeName, CustomEntityCore core, IList<CustomEntityComponent> components, string playerUid )
				: base( null ) {
			this.MyTypeName = typeName;
			this.Core = core;
			this.Components = components;
			this.OwnerPlayerUID = playerUid;
			this.OwnerPlayerWho = PlayerIdentityHelpers.GetPlayerByProperId( playerUid )?.whoAmI ?? -1;
		}


		////////////////

		protected override IList<CustomEntityComponent> CreateComponents( CustomEntityConstructor factory ) {
			throw new HamstarException( "CreateComponents not implemented" );
		}
		protected override CustomEntityCore CreateCore( CustomEntityConstructor factory ) {
			throw new HamstarException( "CreateCore not implemented" );
		}
		public override CustomEntityCore CreateCoreTemplate() {
			return new CustomEntityCore( "", 0, 0, default(Vector2), 0 );
		}
		public override IList<CustomEntityComponent> CreateComponentsTemplate() {
			return new List<CustomEntityComponent>();
		}


		////////////////

		internal CustomEntity Convert() {
			if( !this.IsInitialized ) {
				throw new HamstarException( "Not initialized." );
			}

			CustomEntity ent;
			Type entType = CustomEntityManager.GetTypeByName( this.MyTypeName );

			if( entType == null ) {
				throw new HamstarException( this.MyTypeName + " does not exist." );
			}
			if( !entType.IsSubclassOf( typeof( CustomEntity ) ) ) {
				throw new HamstarException( entType.Name + " is not a valid CustomEntity." );
			}

			if( string.IsNullOrEmpty( this.OwnerPlayerUID ) ) {
				ent = CustomEntity.CreateRaw( entType, this.Core, this.Components );
			} else {
				ent = CustomEntity.CreateRaw( entType, this.Core, this.Components, this.OwnerPlayerUID );
			}

			ent.InternalOnClone();

			return ent;
		}
	}
}
