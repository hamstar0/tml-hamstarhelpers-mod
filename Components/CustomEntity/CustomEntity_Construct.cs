using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.PlayerHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity {
	abstract public partial class CustomEntity : PacketProtocolData {
		protected CustomEntity( PacketProtocolDataConstructorLock ctor_lock ) { }

		protected CustomEntity( CustomEntityCore core, IList<CustomEntityComponent> components, string player_uid = "" ) {
			this.OwnerPlayerUID = player_uid;
			this.OwnerPlayerWho = -1;

			if( player_uid != "" ) {
				Player plr = PlayerIdentityHelpers.GetPlayerByProperId( player_uid );
				if( plr == null ) {
					this.OwnerPlayerWho = plr.whoAmI;
				}
			}

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
			this.Core = new CustomEntityCore( copy.Core );
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
