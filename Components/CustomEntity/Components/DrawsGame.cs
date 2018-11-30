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
		protected class DrawsInGameEntityComponentFactory<T> : CustomEntityComponentFactory<T> where T : DrawsInGameEntityComponent {
			public readonly string SourceModName;
			public readonly string TexturePath;
			public readonly int FrameCount;


			////////////////

			public DrawsInGameEntityComponentFactory( string src_mod_name, string rel_texture_path, int frame_count ) {
				this.SourceModName = src_mod_name;
				this.TexturePath = rel_texture_path;
				this.FrameCount = frame_count;
			}

			////

			protected sealed override void InitializeComponent( T data ) {
				data.ModName = this.SourceModName;
				data.TexturePath = this.TexturePath;
				data.FrameCount = this.FrameCount;
				
				this.InitializeDerivedComponent( data );
			}

			protected virtual void InitializeDerivedComponent( T data ) { }
		}



		////////////////

		public static DrawsInGameEntityComponent CreateDrawsInGameEntityComponent( string src_mod_name, string rel_texture_path, int frame_count ) {
			var factory = new DrawsInGameEntityComponentFactory<DrawsInGameEntityComponent>( src_mod_name, rel_texture_path, frame_count );
			return factory.Create();
		}



		////////////////

		public static void DrawTexture( SpriteBatch sb, CustomEntity ent, Texture2D tex, int frame_count, Color color, float scale, float rotation=0f, Vector2 origin=default(Vector2) ) {
			var core = ent.Core;
			var world_scr_rect = new Rectangle( (int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth, Main.screenHeight );

			if( !core.Hitbox.Intersects( world_scr_rect ) ) { return; }

			var scr_scr_pos = core.position - Main.screenPosition;
			var tex_rect = new Rectangle( 0, 0, tex.Width, tex.Height / frame_count );
			
			SpriteEffects dir = DrawsInGameEntityComponent.GetOrientation( core );

			sb.Draw( tex, scr_scr_pos, tex_rect, color, rotation, origin, scale, dir, 1f );

			if( ModHelpersMod.Instance.Config.DebugModeCustomEntityInfo ) {
				var rect = new Rectangle(
					(int)(core.position.X - Main.screenPosition.X - ((float)origin.X * scale)),
					(int)(core.position.Y - Main.screenPosition.Y - ((float)origin.Y * scale)),
					(int)((float)core.width * scale),
					(int)((float)core.height * scale)
				);
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

		protected DrawsInGameEntityComponent( PacketProtocolDataConstructorLock ctor_lock ) : base( ctor_lock ) { }


		////////////////

		protected sealed override void PostInitialize() {
			if( string.IsNullOrEmpty(this.ModName) || string.IsNullOrEmpty(this.TexturePath) || this.FrameCount == 0 ) {
				throw new HamstarException( "!ModHelpers.DrawsInGameEntityComponent.Initialize - Invalid fields. (" + this.ModName + ", " + this.TexturePath + ", " + this.FrameCount + ")" );
			}

			var src_mod = ModLoader.GetMod( this.ModName );
			if( src_mod == null ) {
				throw new HamstarException( "!ModHelpers.DrawsInGameEntityComponent.Initialize - Invalid mod " + this.ModName );
			}

			if( !Main.dedServ ) {
				if( this.Texture == null ) {
					this.Texture = src_mod.GetTexture( this.TexturePath );
					if( this.Texture == null ) {
						throw new HamstarException( "!ModHelpers.DrawsInGameEntityComponent.Initialize - Invalid texture " + this.TexturePath );
					}
				}
			}

			this.PostPostInitialize();
		}

		protected virtual void PostPostInitialize() { }


		////////////////

		public virtual Color GetColor( CustomEntity ent ) {
			return Color.White;
		}

		public virtual Color GetLightColor( CustomEntity ent ) {
			return Lighting.GetColor( (int)( ent.Core.Center.X / 16 ), (int)( ent.Core.Center.Y / 16 ), this.GetColor( ent ) );
		}


		////////////////

		public virtual void PreDraw( SpriteBatch sb, CustomEntity ent ) { }

		public virtual void Draw( SpriteBatch sb, CustomEntity ent ) {
			DrawsInGameEntityComponent.DrawTexture( sb, ent, this.Texture, this.FrameCount, this.GetLightColor(ent), 1f );
		}

		public virtual void PostDraw( SpriteBatch sb, CustomEntity ent ) { }
	}
}
