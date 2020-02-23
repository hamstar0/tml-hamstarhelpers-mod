using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Classes.PlayerData;
using HamstarHelpers.Helpers.DotNET.Reflection;
using HamstarHelpers.Internals.ClassOverrides;


namespace HamstarHelpers.Services.TerrariaOverrides {
	class PlayerOverrideManager : CustomPlayerData {
		protected override void OnEnter( object data ) {
			var po = ModContent.GetInstance<PlayerOverrides>();
			po.AddPlayer( this.PlayerWho );
		}


		protected override object OnExit() {
			var po = ModContent.GetInstance<PlayerOverrides>();
			po.RemovePlayer( this.PlayerWho );
			return base.OnExit();
		}
	}




	/// <summary>
	/// 
	/// </summary>
	public class PlayerOverrides : ILoadable {
		public static bool SetOverride<T>( int playerWho, string memberName, T value ) {
			var po = ModContent.GetInstance<PlayerOverrides>();
			
			if( !po.Overrides.ContainsKey(playerWho) ) {
				return false;
			}

			T testValue;
			if( !ReflectionHelpers.Get(po.TemplatePlayer, memberName, out testValue) ) {
				throw new ModHelpersException( "No Player field or property named "+memberName+" found." );
			}
			if( testValue.GetType() != typeof(T) ) {
				throw new ModHelpersException( "Mismatched Player field or property type for " + memberName 
					+ ": Expectedd "+testValue.GetType().Name+", found "+typeof(T).Name );
			}

			po.Overrides[ playerWho ].Set( memberName, value );
			return true;
		}

		public static bool UnsetOverride( int playerWho, string memberName ) {
			var po = ModContent.GetInstance<PlayerOverrides>();

			if( !po.Overrides.ContainsKey( playerWho ) ) {
				return false;
			}

			return po.Overrides[playerWho].Unset( memberName );
		}

		////////////////

		internal static bool ApplyOverrides( Player player ) {
			var po = ModContent.GetInstance<PlayerOverrides>();
			if( !po.Overrides.ContainsKey(player.whoAmI) ) {
				throw new ModHelpersException( "Invalid player "+player.name+" ("+player.whoAmI+")" );
			}

			return po.Overrides[ player.whoAmI ]
				.ApplyOverrides( player );
		}



		////////////////

		private Player TemplatePlayer = new Player();
		private IDictionary<int, ClassOverrides> Overrides = new Dictionary<int, ClassOverrides>();



		////////////////

		void ILoadable.OnModsLoad() { }

		void ILoadable.OnModsUnload() { }

		void ILoadable.OnPostModsLoad() { }


		////////////////

		internal ClassOverrides AddPlayer( int playerWho ) {
			var co = new ClassOverrides();
			this.Overrides[ playerWho ] = co;
			return co;
		}

		internal void RemovePlayer( int playerWho ) {
			this.Overrides.Remove( playerWho );
		}

		internal void ClearPlayers() {
			this.Overrides.Clear();
		}
	}




	class OverridePlayer : ModPlayer {
		//public virtual void OnMissingMana( Item item, int neededMana );
		//public virtual bool PreHurt( bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource );
		//public virtual bool PreItemCheck();
		//public virtual bool PreKill( double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource );
		//public virtual void PreUpdate();
		//public virtual void PreUpdateBuffs();
		//public virtual void PreUpdateMovement();
		//public virtual void ProcessTriggers( TriggersSet triggersSet );
		//public virtual bool Shoot( Item item, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack );
		//public virtual void SyncPlayer( int toWho, int fromWho, bool newPlayer );
		//public virtual void UpdateEquips( ref bool wallSpeedBuff, ref bool tileSpeedBuff, ref bool tileRangeBuff );
		//public virtual void UpdateLifeRegen();
		//public virtual void UpdateVanityAccessories();
		//public virtual float UseTimeMultiplier( Item item );

		public override void ModifyDrawInfo( ref PlayerDrawInfo drawInfo ) {
			PlayerOverrides.ApplyOverrides( this.player );
		}

		//public override void ModifyDrawLayers( List<PlayerLayer> layers ) {
		//	base.ModifyDrawLayers( layers );
		//}

		//public override void ModifyDrawHeadLayers( List<PlayerHeadLayer> layers ) {
		//	base.ModifyDrawHeadLayers( layers );
		//}
	}
}
