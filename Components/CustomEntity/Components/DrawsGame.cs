using HamstarHelpers.Components.Errors;
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
	public class DrawsInGameEntityComponent : CustomEntityComponent {
		public static void DrawTexture( SpriteBatch sb, CustomEntity ent, Texture2D tex, int frame_count, Color color ) {
			var core = ent.Core;
			var world_scr_rect = new Rectangle( (int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth, Main.screenHeight );

			if( !core.Hitbox.Intersects( world_scr_rect ) ) { return; }

			var scr_scr_pos = core.position - Main.screenPosition;
			var tex_rect = new Rectangle( 0, 0, tex.Width, tex.Height / frame_count );

			float scale = 1f;

			SpriteEffects dir = DrawsInGameEntityComponent.GetOrientation( core );

			sb.Draw( tex, scr_scr_pos, tex_rect, color, 0f, default( Vector2 ), scale, dir, 1f );

			if( ModHelpersMod.Instance.Config.DebugModeCustomEntityInfo ) {
				var rect = new Rectangle( (int)( core.position.X - Main.screenPosition.X ), (int)( core.position.Y - Main.screenPosition.Y ), core.width, core.height );
				HudHelpers.DrawBorderedRect( sb, null, Color.Red, rect, 1 );
			}
		}
		
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

		private DrawsInGameEntityComponent( PacketProtocolDataConstructorLock ctor_lock ) { }

		public DrawsInGameEntityComponent( string src_mod_name, string rel_texture_path, int frame_count ) {
			this.ModName = src_mod_name;
			this.TexturePath = rel_texture_path;
			this.FrameCount = frame_count;
		}

		////////////////

		protected override void Initialize() {
			if( string.IsNullOrEmpty( this.ModName ) || string.IsNullOrEmpty( this.TexturePath ) || this.FrameCount == 0 ) {
				throw new HamstarException("!ModHelpers.DrawsInGameEntityComponent.Initialize - Invalid fields.");
			}

			var src_mod = ModLoader.GetMod( this.ModName );
			if( src_mod == null ) {
				throw new HamstarException( "!ModHelpers.DrawsInGameEntityComponent.Initialize - Invalid mod "+this.ModName );
			}

			if( !Main.dedServ ) {
				if( this.Texture == null ) {
					this.Texture = src_mod.GetTexture( this.TexturePath );
				}
			}

			base.Initialize();
		}


		////////////////

		public virtual void PreDraw( SpriteBatch sb, CustomEntity ent ) { }

		public virtual void Draw( SpriteBatch sb, CustomEntity ent ) {
			Color color = Lighting.GetColor( (int)(ent.Core.Center.X / 16), (int)(ent.Core.Center.Y / 16), Color.White );
			DrawsInGameEntityComponent.DrawTexture( sb, ent, this.Texture, this.FrameCount, color );
		}

		public virtual void PostDraw( SpriteBatch sb, CustomEntity ent ) { }
	}
}
