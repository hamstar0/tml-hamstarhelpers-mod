using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Components.CustomEntity.Components {
	public partial class DrawsOnMapEntityComponent : CustomEntityComponent {
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

		protected sealed override void OnInitialize() {
			if( string.IsNullOrEmpty(this.ModName) || string.IsNullOrEmpty(this.TexturePath) || this.FrameCount == 0 || this.Scale == 0 ) {
				//throw new HamstarException( "!ModHelpers.DrawsOnMapEntityComponent.Initialize - Invalid fields." );
				throw new HamstarException( "Invalid fields." );
			}

			var srcMod = ModLoader.GetMod( this.ModName );
			if( srcMod == null ) {
				//throw new HamstarException( "!ModHelpers.DrawsOnMapEntityComponent.Initialize - Invalid mod " + this.ModName );
				throw new HamstarException( "Invalid mod " + this.ModName );
			}

			if( !Main.dedServ ) {
				if( this.Texture == null ) {
					this.Texture = srcMod.GetTexture( this.TexturePath );
				}
			}

			this.PostInitialize();
		}

		protected virtual void PostInitialize() { }
		

		////////////////

		public virtual Color GetColor( CustomEntity ent ) {
			return Color.White;
		}


		////////////////

		public void DrawMiniMap( SpriteBatch sb, CustomEntity ent ) {
			if( !this.PreDrawMiniMap( sb, ent ) ) { return; }
			DrawsOnMapEntityComponent.DrawToMiniMap( sb, this.Texture, this.GetColor(ent), ent.Core.Center, this.Zooms, this.Scale );
			this.PostDrawMiniMap( sb, ent );
		}

		public void DrawOverlayMap( SpriteBatch sb, CustomEntity ent ) {
			if( !this.PreDrawOverlayMap( sb, ent ) ) { return; }
			DrawsOnMapEntityComponent.DrawToOverlayMap( sb, this.Texture, this.GetColor(ent), ent.Core.Center, this.Zooms, this.Scale );
			this.PostDrawOverlayMap( sb, ent );
		}

		public void DrawFullscreenMap( SpriteBatch sb, CustomEntity ent ) {
			if( !this.PreDrawFullscreenMap( sb, ent) ) { return; }
			DrawsOnMapEntityComponent.DrawToFullMap( sb, this.Texture, this.GetColor(ent), ent.Core.Center, this.Zooms, this.Scale );
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
