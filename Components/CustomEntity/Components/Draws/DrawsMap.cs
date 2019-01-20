using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Components.CustomEntity.Components {
	public partial class DrawsOnMapEntityComponent : CustomEntityComponent {
		[PacketProtocolIgnore]
		public string SourceModName;
		[PacketProtocolIgnore]
		public string RelativeTexturePath;
		[PacketProtocolIgnore]
		public int FrameCount;
		[PacketProtocolIgnore]
		public float Scale;
		[PacketProtocolIgnore]
		public bool Zooms;

		////////////////

		[PacketProtocolIgnore]
		[JsonIgnore]
		public Texture2D Texture { get; protected set; }



		////////////////

		private DrawsOnMapEntityComponent() { }

		protected DrawsOnMapEntityComponent( string srcModName, string relTexturePath, int frameCount, float scale, bool zooms ) {
			this.SourceModName = srcModName;
			this.RelativeTexturePath = relTexturePath;
			this.FrameCount = frameCount;
			this.Scale = scale;
			this.Zooms = zooms;

			this.Validate();
			this.Initialize();
		}


		////////////////

		protected sealed override void OnClone() {
			this.Validate();
			this.Initialize();
		}

		////

		private void Validate() {
			if( string.IsNullOrEmpty( this.SourceModName ) || string.IsNullOrEmpty( this.RelativeTexturePath ) || this.FrameCount == 0 || this.Scale == 0 ) {
				//throw new HamstarException( "!ModHelpers.DrawsOnMapEntityComponent.Initialize - Invalid fields." );
				throw new HamstarException( "Invalid fields." );
			}

			var srcMod = ModLoader.GetMod( this.SourceModName );
			if( srcMod == null ) {
				//throw new HamstarException( "!ModHelpers.DrawsOnMapEntityComponent.Initialize - Invalid mod " + this.ModName );
				throw new HamstarException( "Invalid mod " + this.SourceModName );
			}
		}

		private void Initialize() {
			var srcMod = ModLoader.GetMod( this.SourceModName );

			if( !Main.dedServ ) {
				if( this.Texture == null ) {
					this.Texture = srcMod.GetTexture( this.RelativeTexturePath );
				}
			}

			this.PostInitialize();
		}

		////

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
