using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Components.CustomEntity.Components {
	public class DrawsInGameEntityComponent : CustomEntityComponent {
		public static SpriteEffects GetOrientation( Entity ent ) {
			SpriteEffects dir = ent.direction > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			//dir |= ( Main.LocalPlayer.gravDir < 0 ) ? SpriteEffects.FlipVertically : SpriteEffects.None;
			return dir;
		}


		////////////////

		[PacketProtocolIgnore]
		public string ModName;
		[PacketProtocolIgnore]
		public string TexturePath;
		[PacketProtocolIgnore]
		public int FrameCount;

		[PacketProtocolIgnore]
		[JsonIgnore]
		public Texture2D Texture { get; protected set; }



		////////////////

		public DrawsInGameEntityComponent( string src_mod_name, string rel_texture_path, int frame_count ) {
			var src_mod = ModLoader.GetMod( src_mod_name );

			this.ModName = src_mod_name;
			this.TexturePath = rel_texture_path;
			this.FrameCount = frame_count;
			
			this.Texture = src_mod.GetTexture( rel_texture_path );
		}


		////////////////

		public void Draw( SpriteBatch sb, CustomEntity ent ) {
			if( Main.netMode == 2 ) { throw new Exception( "Server cannot Draw." ); }

			var world_scr_rect = new Rectangle( (int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth, Main.screenHeight );
			if( !ent.Hitbox.Intersects( world_scr_rect ) ) { return; }

			if( !this.PreDraw( sb, ent ) ) { return; }

			var scr_scr_pos = ent.position - Main.screenPosition;
			var tex_rect = new Rectangle( 0, 0, this.Texture.Width, this.Texture.Height / this.FrameCount );

			Color color = Lighting.GetColor( (int)(ent.position.X / 16), (int)(ent.position.Y / 16), Color.White );
			float scale = 1f;

			SpriteEffects dir = DrawsInGameEntityComponent.GetOrientation( ent );

			sb.Draw( this.Texture, scr_scr_pos, tex_rect, color, 0f, default(Vector2), scale, dir, 1f );

			this.PostDraw( sb, ent );
		}


		public virtual bool PreDraw( SpriteBatch sb, CustomEntity ent ) { return true; }
		public virtual void PostDraw( SpriteBatch sb, CustomEntity ent ) { }
	}
}
