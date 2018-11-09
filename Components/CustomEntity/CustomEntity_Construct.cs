using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.PlayerHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity {
	abstract public partial class CustomEntity : PacketProtocolData {
		protected CustomEntity( Player owner_plr ) {
			this.TypeId = CustomEntityManager.GetId( this.GetType() );

			this.OwnerPlayerWho = owner_plr == null ? -1 : owner_plr.whoAmI;
			this.OwnerPlayerUID = owner_plr == null ? "" : PlayerIdentityHelpers.GetProperUniqueId( owner_plr );
		}

		////

		[JsonConstructor]
		internal CustomEntity() {
			this.TypeId = CustomEntityManager.GetId( this.GetType() );
		}

		internal CustomEntity( CustomEntityCore core, IList<CustomEntityComponent> components ) {	// Deserializer 1
			this.FinishCtor( "", -1, core, components );
		}

		internal CustomEntity( string owner_uid, CustomEntityCore core, IList<CustomEntityComponent> components ) { // Deserializer 2
			Player owner = PlayerIdentityHelpers.GetPlayerByProperId( owner_uid );
			if( owner == null ) {
				throw new HamstarException( "!ModHelpers.CustomEntity.CTor_3 - Could not verify if entity's owner (by id '"+owner_uid+"') is present or absent." );
			}

			int owner_who = owner == null ? -1 : owner.whoAmI;

			this.FinishCtor( owner_uid, owner_who, core, components );
		}

		internal CustomEntity( Player owner, CustomEntityCore core, IList<CustomEntityComponent> components ) { // Deserializer 3
			string owner_uid = PlayerIdentityHelpers.GetProperUniqueId( owner );

			this.FinishCtor( owner_uid, owner.whoAmI, core, components );
		}
		
		////

		private void FinishCtor( string owner_uid, int owner_who, CustomEntityCore core, IList<CustomEntityComponent> components ) {
			this.TypeId = CustomEntityManager.GetId( this.GetType() );

			if( this.TypeId == -1 ) {
				string comp_str = string.Join( ", ", components.Select( c => c.GetType().Name ) );
				throw new NotImplementedException( "!ModHelpers.CustomEntity.FinishCtor - No ID found to match to new entity "
					+ core.DisplayName + ". Components: " + comp_str );
			}

			this.OwnerPlayerUID = owner_uid;
			this.OwnerPlayerWho = owner_who;
			this.Core = core;
			this.Components = components;
		}

		////////////////

		internal CustomEntity CloneAsType( Type ent_type ) {
			if( !ent_type.IsSubclassOf(typeof(CustomEntity)) ) {
				throw new HamstarException( ent_type.Name+" is not a valid CustomEntity." );
			}

			var args = this.OwnerPlayerWho == -1 ?
				new object[] { this.Core, this.Components } :
				new object[] { this.OwnerPlayerUID, this.Core, this.Components };
			
			return (CustomEntity)Activator.CreateInstance( ent_type,
				BindingFlags.NonPublic | BindingFlags.Instance,
				null,
				args,
				null );
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

		public void CopyChangesFrom( CustomEntity copy ) {	// TODO: Actually copy changes only!
			if( this.TypeId == -1 ) {
				this.Core = new CustomEntityCore( copy.Core );
			}
			this.TypeId = copy.TypeId;
			this.OwnerPlayerWho = copy.OwnerPlayerWho;
			//this.OwnerPlayerUID = copy.OwnerPlayerUID;

			this.Components = copy.Components.Select( c => c.InternalClone() ).ToList();

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
