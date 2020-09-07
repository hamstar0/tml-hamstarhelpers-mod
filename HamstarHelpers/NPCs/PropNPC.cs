using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.UI;
using HamstarHelpers.Helpers.Draw;


namespace HamstarHelpers.NPCs {
	/// <summary>
	/// Implements an NPC able to have its texture and size adjusted dynamically. Is completely passive.
	/// </summary>
	public class PropNPC : ModNPC {
		/// <summary></summary>
		public override string Texture => this._Texture;

		private string _Texture = "HamstarHelpers/NPCs/PropNPC";



		////////////////

		/// @private
		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "A Prop" );
		}

		/// @private
		public override void SetDefaults() {
			this.npc.aiStyle = 0;
			this.npc.width = 16;
			this.npc.height = 16;
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

		/// @private
		public override bool CheckActive() {
			return false;
		}


		////////////////

		/// @private
		public override void FindFrame( int frameHeight ) {
			this.npc.frame = new Rectangle( 0, 0, this.npc.width, this.npc.height );
		}


		////////////////

		/// @private
		public override bool PreDraw( SpriteBatch sb, Color drawColor ) {
			Texture2D tex = ModContent.GetTexture( this.Texture );
			Vector2 pos = UIZoomHelpers.ConvertToScreenPosition( npc.Center, null, true );

			sb.Draw(
				texture: tex,
				position: pos,
				sourceRectangle: null,
				color: drawColor,
				rotation: npc.rotation,
				origin: new Vector2( tex.Width / 2, tex.Height / 2 ),
				scale: npc.scale,
				effects: npc.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
				layerDepth: 0f
			);

			if( ModHelpersConfig.Instance.DebugModeMiscInfo ) {
				var rect = new Rectangle(
					(int)( pos.X - ((npc.scale * (float)tex.Width) / 2f) ),
					(int)( pos.Y - ((npc.scale * (float)tex.Height) / 2f) ),
					(int)( npc.scale * (float)tex.Width ),
					(int)( npc.scale * (float)tex.Height )
				);
				DrawHelpers.DrawBorderedRect( sb, Color.Transparent, Color.Red, rect, 2 );
			}

			return false;
		}


		////////////////

		/// <summary></summary>
		/// <param name="texturePath"></param>
		public void SetTexture( string texturePath ) {
			this._Texture = texturePath;
		}
	}
}
