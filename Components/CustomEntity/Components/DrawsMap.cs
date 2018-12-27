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
	public class DrawsOnMapEntityComponent : CustomEntityComponent {
		protected class DrawsOnMapEntityComponentFactory<T> : CustomEntityComponentFactory<T> where T : DrawsOnMapEntityComponent {
			public readonly string SourceModName;
			public readonly string RelativeTexturePath;
			public readonly int FrameCount;
			public readonly float Scale;
			public readonly bool Zooms;


			////////////////

			public DrawsOnMapEntityComponentFactory( string srcModName, string relTexturePath, int frameCount, float scale, bool zooms ) {
				this.SourceModName = srcModName;
				this.RelativeTexturePath = relTexturePath;
				this.FrameCount = frameCount;
				this.Scale = scale;
				this.Zooms = zooms;
			}

			////

			protected sealed override void InitializeComponent( T data ) {
				data.ModName = this.SourceModName;
				data.TexturePath = this.RelativeTexturePath;
				data.FrameCount = this.FrameCount;
				data.Scale = this.Scale;
				data.Zooms = this.Zooms;

				this.InitializeDerivedComponent( data );
			}
			
			protected virtual void InitializeDerivedComponent( T data ) { }
		}



		////////////////

		public static DrawsOnMapEntityComponent CreateDrawsOnMapEntityComponent( string srcModName, string relTexturePath, int frameCount, float scale, bool zooms ) {
			var factory = new DrawsOnMapEntityComponentFactory<DrawsOnMapEntityComponent>( srcModName, relTexturePath, frameCount, scale, zooms );
			return factory.Create();
		}



		////////////////

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

		protected DrawsOnMapEntityComponent( PacketProtocolDataConstructorLock ctorLock ) : base( ctorLock ) { }


		////////////////

		protected sealed override void Initialize() {
			if( string.IsNullOrEmpty( this.ModName ) || string.IsNullOrEmpty( this.TexturePath ) || this.FrameCount == 0 || this.Scale == 0 ) {
				throw new HamstarException( "!ModHelpers.DrawsOnMapEntityComponent.Initialize - Invalid fields." );
			}

			var srcMod = ModLoader.GetMod( this.ModName );
			if( srcMod == null ) {
				throw new HamstarException( "!ModHelpers.DrawsOnMapEntityComponent.Initialize - Invalid mod " + this.ModName );
			}

			if( !Main.dedServ ) {
				if( this.Texture == null ) {
					this.Texture = srcMod.GetTexture( this.TexturePath );
				}
			}

			this.PostPostInitialize();
		}

		protected virtual void PostPostInitialize() { }


		////////////////

		public virtual Color GetColor( CustomEntity ent ) {
			return Color.White;
		}
		

		////////////////

		public void DrawMiniMap( SpriteBatch sb, CustomEntity ent ) {
			if( !this.PreDrawMiniMap( sb, ent ) ) { return; }

			Entity core = ent.Core;
			float scale = (this.Zooms ? Main.mapMinimapScale : 1f) * this.Scale;

			int tileX = (int)core.Center.X - (int)( (float)this.Texture.Width * this.Scale * 8 );
			int tileY = (int)core.Center.Y - (int)( (float)this.Texture.Height * this.Scale * 8 );

			var mapRectOrigin = new Rectangle( tileX, tileY, this.Texture.Width, this.Texture.Height );
			var miniMapData = HudMapHelpers.GetMiniMapScreenPosition( mapRectOrigin );

			if( miniMapData.Item2 ) {
				sb.Draw( this.Texture, miniMapData.Item1, null, this.GetColor(ent), 0f, default(Vector2), scale, SpriteEffects.None, 1f );
			}

			this.PostDrawMiniMap( sb, ent );
		}

		public void DrawOverlayMap( SpriteBatch sb, CustomEntity ent ) {
			if( !this.PreDrawOverlayMap( sb, ent ) ) { return; }

			Entity core = ent.Core;
			float scale = (this.Zooms ? Main.mapOverlayScale : 1f) * this.Scale;

			int tileX = (int)core.Center.X - (int)( (float)this.Texture.Width * this.Scale * 8 );
			int tileY = (int)core.Center.Y - (int)( (float)this.Texture.Height * this.Scale * 8 );

			var mapRectOrigin = new Rectangle( tileX, tileY, this.Texture.Width, this.Texture.Height );
			var overMapData = HudMapHelpers.GetOverlayMapScreenPosition( mapRectOrigin );

			if( overMapData.Item2 ) {
				sb.Draw( this.Texture, overMapData.Item1, null, this.GetColor(ent), 0f, default( Vector2 ), scale, SpriteEffects.None, 1f );
			}

			this.PostDrawOverlayMap( sb, ent );
		}

		public void DrawFullscreenMap( SpriteBatch sb, CustomEntity ent ) {
			if( !this.PreDrawFullscreenMap( sb, ent) ) { return; }

			Entity core = ent.Core;
			float scale = (this.Zooms ? Main.mapFullscreenScale : 1f) * this.Scale;

			int tileX = (int)core.Center.X - (int)( (float)this.Texture.Width * this.Scale * 8 );
			int tileY = (int)core.Center.Y - (int)( (float)this.Texture.Height * this.Scale * 8 );

			var mapRectOrigin = new Rectangle( tileX, tileY, this.Texture.Width, this.Texture.Height );
			var overMapData = HudMapHelpers.GetFullMapScreenPosition( mapRectOrigin );

			if( overMapData.Item2 ) {
				sb.Draw( this.Texture, overMapData.Item1, null, this.GetColor(ent), 0f, default( Vector2 ), scale, SpriteEffects.None, 1f );
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
