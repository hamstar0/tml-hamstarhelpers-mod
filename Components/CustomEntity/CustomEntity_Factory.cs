using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.PlayerHelpers;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity {
	public abstract partial class CustomEntity : PacketProtocolData {
		protected abstract class CustomEntityFactory {
			public readonly Player OwnerPlayer;
			
			protected CustomEntityFactory( Player ownerPlr=null ) {
				this.OwnerPlayer = ownerPlr;
			}
		}



		////////////////

		protected override Tuple<object, Type> _MyFactoryType => Tuple.Create( (object)this, typeof(CustomEntityFactory) );



		////////////////
		
		protected sealed override void OnFactoryCreate( object factory ) {
			var myfactory = factory as CustomEntityFactory;

			if( this.OwnerPlayer != null ) {
				this.OwnerPlayerWho = this.OwnerPlayer.whoAmI;
				this.OwnerPlayerUID = PlayerIdentityHelpers.GetProperUniqueId( this.OwnerPlayer );
			} else {
				this.OwnerPlayerWho = -1;
				this.OwnerPlayerUID = "";
			}

			this.Core = this.CreateCore( myfactory );
			this.Components = this.CreateComponents( myfactory );
		}


		////////////////

		private new static CustomEntity CreateRawUninitialized( Type mytype ) {
			if( !mytype.IsSubclassOf( typeof( CustomEntity ) ) ) {
				throw new NotImplementedException( mytype.Name+" is not a CustomEntity subclass." );
			}
			
			var ent = (CustomEntity)Activator.CreateInstance( mytype,
				BindingFlags.Instance | BindingFlags.NonPublic,
				null,
				new object[] { new PacketProtocolDataConstructorLock() },
				null
			);

			return ent;
		}

		internal static CustomEntity CreateRaw( Type mytype, CustomEntityCore core, IList<CustomEntityComponent> components ) {
			var ent = CustomEntity.CreateRawUninitialized( mytype );
			ent.Core = core;
			ent.Components = components;
			ent.OwnerPlayerWho = -1;
			ent.OwnerPlayerUID = "";

			ent.OnInitialize();

			return ent;
		}

		internal static CustomEntity CreateRaw( Type mytype, CustomEntityCore core, IList<CustomEntityComponent> components, string ownerUid="" ) {
			var ent = CustomEntity.CreateRawUninitialized( mytype );
			ent.Core = core;
			ent.Components = components;
			ent.OwnerPlayerWho = -1;
			ent.OwnerPlayerUID = ownerUid;
			
			Player plr = PlayerIdentityHelpers.GetPlayerByProperId( ownerUid );
			if( plr != null ) {
				ent.OwnerPlayerWho = plr.whoAmI;
			}

			ent.OnInitialize();

			return ent;
		}
	}
}
