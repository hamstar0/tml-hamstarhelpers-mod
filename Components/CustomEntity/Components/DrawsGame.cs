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

			public DrawsInGameEntityComponentFactory( string srcModName, string relTexturePath, int frameCount ) {
				this.SourceModName = srcModName;
				this.TexturePath = relTexturePath;
				this.FrameCount = frameCount;
			}

			////

			protected sealed override void InitializeComponent( T data ) {
				data.ModName = this.SourceModName;
				data.TexturePath = this.TexturePath;
				data.FrameCount = this.FrameCount;
				
				this.InitializeDrawsInGameEntityComponent( data );
			}

			protected virtual void InitializeDrawsInGameEntityComponent( T data ) { }
		}



		////////////////

		public static DrawsInGameEntityComponent CreateDrawsInGameEntityComponent( string srcModName, string relTexturePath, int frameCount ) {
			var factory = new DrawsInGameEntityComponentFactory<DrawsInGameEntityComponent>( srcModName, relTexturePath, frameCount );
			return factory.Create();
		}



		////////////////

		public static void DrawTexture( SpriteBatch sb, CustomEntity ent, Texture2D tex, int frameCount, Color color, float scale, float rotation=0f, Vector2 origin=default(Vector2) ) {
			var core = ent.Core;
			var worldScrRect = new Rectangle( (int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth, Main.screenHeight );

			if( !core.Hitbox.Intersects( worldScrRect ) ) { return; }

			var scrScrPos = core.position - Main.screenPosition;
			var texRect = new Rectangle( 0, 0, tex.Width, tex.Height / frameCount );
			
			SpriteEffects dir = DrawsInGameEntityComponent.GetOrientation( core );

			sb.Draw( tex, scrScrPos, texRect, color, rotation, origin, scale, dir, 1f );

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

		protected DrawsInGameEntityComponent( PacketProtocolDataConstructorLock ctorLock ) : base( ctorLock ) { }
		
		////////////////

		public sealed override void OnInitialize() {
			if( string.IsNullOrEmpty(this.ModName) || string.IsNullOrEmpty(this.TexturePath) || this.FrameCount == 0 ) {
				//throw new HamstarException( "!ModHelpers.DrawsInGameEntityComponent.Initialize - Invalid fields. (" + this.ModName + ", " + this.TexturePath + ", " + this.FrameCount + ")" );
				throw new HamstarException( "Invalid fields. (" + this.ModName + ", " + this.TexturePath + ", " + this.FrameCount + ")" );
			}

			var srcMod = ModLoader.GetMod( this.ModName );
			if( srcMod == null ) {
				//throw new HamstarException( "!ModHelpers.DrawsInGameEntityComponent.Initialize - Invalid mod " + this.ModName );
				throw new HamstarException( "Invalid mod " + this.ModName );
			}

			if( !Main.dedServ ) {
				if( this.Texture == null ) {
					this.Texture = srcMod.GetTexture( this.TexturePath );
					if( this.Texture == null ) {
						//throw new HamstarException( "!ModHelpers.DrawsInGameEntityComponent.Initialize - Invalid texture " + this.TexturePath );
						throw new HamstarException( "Invalid texture " + this.TexturePath );
					}
				}
			}

			this.PostInitialize();
		}

		protected virtual void PostInitialize() { }


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
