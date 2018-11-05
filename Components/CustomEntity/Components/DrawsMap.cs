using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
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
		public string ModName;
		[PacketProtocolIgnore]
		public string TexturePath;
		[PacketProtocolIgnore]
		public int FrameCount;
		[PacketProtocolIgnore]
		public float Scale;
		[PacketProtocolIgnore]
		public bool Zooms;

		[PacketProtocolIgnore]
		[JsonIgnore]
		public Texture2D Texture { get; protected set; }



		////////////////

		private DrawsOnMapEntityComponent( PacketProtocolDataConstructorLock ctor_lock ) { }

		public DrawsOnMapEntityComponent( string src_mod_name, string rel_texture_path, int frame_count, float scale, bool zooms ) {
			this.ModName = src_mod_name;
			this.TexturePath = rel_texture_path;
			this.FrameCount = frame_count;
			this.Scale = scale;
			this.Zooms = zooms;

			this.ConfirmLoad();
		}

		////////////////

		protected override void ConfirmLoad() {
			if( string.IsNullOrEmpty( this.ModName ) || string.IsNullOrEmpty( this.TexturePath ) || this.FrameCount == 0 || this.Scale == 0 ) {
				this.IsInitialized = false;
				return;
			}

			var src_mod = ModLoader.GetMod( this.ModName );
			if( src_mod == null ) {
				this.IsInitialized = false;
				return;
			}

			if( !Main.dedServ ) {
				if( this.Texture == null ) {
					this.Texture = src_mod.GetTexture( this.TexturePath );
				}
			}

			base.ConfirmLoad();
		}


		////////////////

		public void DrawMiniMap( SpriteBatch sb, CustomEntity ent ) {
			if( !this.PreDrawMiniMap( sb, ent ) ) { return; }

			Entity core = ent.Core;
			float scale = (this.Zooms ? Main.mapMinimapScale : 1f) * this.Scale;

			int tile_x = (int)core.Center.X - (int)( (float)this.Texture.Width * this.Scale * 8 );
			int tile_y = (int)core.Center.Y - (int)( (float)this.Texture.Height * this.Scale * 8 );

			var map_rect_origin = new Rectangle( tile_x, tile_y, this.Texture.Width, this.Texture.Height );
			var mini_map_data = HudMapHelpers.GetMiniMapScreenPosition( map_rect_origin );

			if( mini_map_data.Item2 ) {
				sb.Draw( this.Texture, mini_map_data.Item1, null, Color.White, 0f, default(Vector2), scale, SpriteEffects.None, 1f );
			}

			this.PostDrawMiniMap( sb, ent );
		}

		public void DrawOverlayMap( SpriteBatch sb, CustomEntity ent ) {
			if( !this.PreDrawOverlayMap( sb, ent ) ) { return; }

			Entity core = ent.Core;
			float scale = (this.Zooms ? Main.mapOverlayScale : 1f) * this.Scale;

			int tile_x = (int)core.Center.X - (int)( (float)this.Texture.Width * this.Scale * 8 );
			int tile_y = (int)core.Center.Y - (int)( (float)this.Texture.Height * this.Scale * 8 );

			var map_rect_origin = new Rectangle( tile_x, tile_y, this.Texture.Width, this.Texture.Height );
			var over_map_data = HudMapHelpers.GetOverlayMapScreenPosition( map_rect_origin );

			if( over_map_data.Item2 ) {
				sb.Draw( this.Texture, over_map_data.Item1, null, Color.White, 0f, default( Vector2 ), scale, SpriteEffects.None, 1f );
			}

			this.PostDrawOverlayMap( sb, ent );
		}

		public void DrawFullscreenMap( SpriteBatch sb, CustomEntity ent ) {
			if( !this.PreDrawFullscreenMap( sb, ent) ) { return; }

			Entity core = ent.Core;
			float scale = (this.Zooms ? Main.mapFullscreenScale : 1f) * this.Scale;

			int tile_x = (int)core.Center.X - (int)( (float)this.Texture.Width * this.Scale * 8 );
			int tile_y = (int)core.Center.Y - (int)( (float)this.Texture.Height * this.Scale * 8 );

			var map_rect_origin = new Rectangle( tile_x, tile_y, this.Texture.Width, this.Texture.Height );
			var over_map_data = HudMapHelpers.GetFullMapScreenPosition( map_rect_origin );

			if( over_map_data.Item2 ) {
				sb.Draw( this.Texture, over_map_data.Item1, null, Color.White, 0f, default( Vector2 ), scale, SpriteEffects.None, 1f );
			}

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
