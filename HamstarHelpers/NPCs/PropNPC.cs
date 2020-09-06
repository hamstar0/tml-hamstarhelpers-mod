using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace HamstarHelpers.NPCs {
	/// <summary>
	/// Implements an NPC able to have its texture and size adjusted dynamically. Is completely passive.
	/// </summary>
	public class PropNPC : ModNPC {
		/// <summary></summary>
		public override string Texture => this._Texture;

		private string _Texture = "Terraria/MapDeath";



		////////////////

		/// @private
		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "" );
		}

		/// @private
		public override void SetDefaults() {
			this.npc.width = 16;
			this.npc.height = 16;
			this.npc.aiStyle = 0;
			this.npc.damage = 0;
			this.npc.defense = 0;
			this.npc.lifeMax = 5;
			this.npc.HitSound = SoundID.NPCHit1;
			this.npc.DeathSound = SoundID.NPCDeath1;
			this.npc.npcSlots = 0f;
			this.npc.noGravity = true;

			this.npc.friendly = true;
			this.npc.lavaImmune = true;
			this.npc.dontTakeDamage = true;
		}

		////

		/// @private
		public override bool? CanBeHitByItem( Player player, Item item ) {
			return false;
		}

		/// @private
		public override bool? CanBeHitByProjectile( Projectile projectile ) {
			return false;
		}

		/// @private
		public override bool? CanHitNPC( NPC target ) {
			return false;
		}

		/// @private
		public override bool CanHitPlayer( Player target, ref int cooldownSlot ) {
			return false;
		}


		////////////////

		/// <summary></summary>
		/// <param name="texturePath"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public void SetTexture( string texturePath, int? width=null, int? height=null ) {
			this._Texture = texturePath;
			if( width.HasValue ) {
				this.npc.width = width.Value;
			}
			if( height.HasValue ) {
				this.npc.height = height.Value;
			}
		}
	}
}
