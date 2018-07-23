using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.HudHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Components.CustomEntity.Components {
	public class DrawsOnMapEntityComponent : CustomEntityComponent {
		[PacketProtocolIgnore]
		public string TexturePath;
		[PacketProtocolIgnore]
		public int FrameCount;
		[PacketProtocolIgnore]
		public float Scale;

		[PacketProtocolIgnore]
		[JsonIgnore]
		public Texture2D Texture { get; protected set; }



		////////////////

		public DrawsOnMapEntityComponent( string terraria_texture_path, int frame_count, float scale ) {
			this.TexturePath = terraria_texture_path;
			this.FrameCount = frame_count;
			this.Scale = scale;
			
			this.Texture = ModLoader.GetTexture( terraria_texture_path );
		}


		////////////////

		public void DrawMiniMap( SpriteBatch sb, CustomEntity ent ) {
			if( !this.PreDrawMiniMap( sb, ent ) ) { return; }

			float scale = Main.mapMinimapScale * this.Scale;

			var rect = new Rectangle( (int)ent.position.X, (int)ent.position.Y, this.Texture.Width, this.Texture.Height );
			Vector2? mini_map_pos = HudMapHelpers.GetMiniMapPosition( rect );

			if( mini_map_pos != null ) {
				sb.Draw( this.Texture, (Vector2)mini_map_pos, null, Color.White, 0f, default( Vector2 ), scale, SpriteEffects.None, 1f );
			}

			this.PostDrawMiniMap( sb, ent );
		}

		public void DrawOverlayMap( SpriteBatch sb, CustomEntity ent ) {
			if( !this.PreDrawOverlayMap( sb, ent ) ) { return; }

			float scale = Main.mapOverlayScale * this.Scale;

			var rect = new Rectangle( (int)ent.position.X, (int)ent.position.Y, this.Texture.Width, this.Texture.Height );
			Vector2? over_map_pos = HudMapHelpers.GetOverlayMapPosition( rect );

			if( over_map_pos != null ) {
				sb.Draw( this.Texture, (Vector2)over_map_pos, null, Color.White, 0f, default( Vector2 ), scale, SpriteEffects.None, 1f );
			}

			this.PostDrawOverlayMap( sb, ent );
		}

		public void DrawFullscreenMap( SpriteBatch sb, CustomEntity ent ) {
			if( !this.PreDrawFullscreenMap( sb, ent) ) { return; }

			float scale = Main.mapFullscreenScale * this.Scale;

			var rect = new Rectangle( (int)ent.position.X, (int)ent.position.Y, this.Texture.Width, this.Texture.Height );
			Vector2 pos = HudMapHelpers.GetFullMapPosition( rect );

			sb.Draw( this.Texture, pos, null, Color.White, 0f, default( Vector2 ), scale, SpriteEffects.None, 1f );

			this.PostDrawFullscreenMap( sb, ent );
		}


		////////////////

		public virtual bool PreDrawMiniMap( SpriteBatch sb, CustomEntity ent ) {
			return true;
		}

		public virtual bool PreDrawOverlayMap( SpriteBatch sb, CustomEntity ent ) {
			return true;
		}

		public virtual bool PreDrawFullscreenMap( SpriteBatch sb, CustomEntity ent ) {
			return true;
		}


		public virtual void PostDrawMiniMap( SpriteBatch sb, CustomEntity ent ) {
		}

		public virtual void PostDrawOverlayMap( SpriteBatch sb, CustomEntity ent ) {
		}

		public virtual void PostDrawFullscreenMap( SpriteBatch sb, CustomEntity ent ) {
		}
	}
}
