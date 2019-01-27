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
	public partial class DrawsInGameEntityComponent : CustomEntityComponent {
		[PacketProtocolIgnore]
		public string SourceModName;
		[PacketProtocolIgnore]
		public string TexturePath;
		[PacketProtocolIgnore]
		public int FrameCount;

		////////////////

		[PacketProtocolIgnore]
		[JsonIgnore]
		public Texture2D Texture { get; protected set; }



		////////////////

		private DrawsInGameEntityComponent() { }

		protected DrawsInGameEntityComponent( string srcModName, string relTexturePath, int frameCount ) {
			this.SourceModName = srcModName;
			this.TexturePath = relTexturePath;
			this.FrameCount = frameCount;

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
			if( string.IsNullOrEmpty( this.SourceModName ) || string.IsNullOrEmpty( this.TexturePath ) || this.FrameCount == 0 ) {
				//throw new HamstarException( "!ModHelpers.DrawsInGameEntityComponent.Initialize - Invalid fields. (" + this.ModName + ", " + this.TexturePath + ", " + this.FrameCount + ")" );
				throw new HamstarException( "Invalid fields. (" + this.SourceModName + ", " + this.TexturePath + ", " + this.FrameCount + ")" );
			}

			var srcMod = ModLoader.GetMod( this.SourceModName );
			if( srcMod == null ) {
				//throw new HamstarException( "!ModHelpers.DrawsInGameEntityComponent.Initialize - Invalid mod " + this.ModName );
				throw new HamstarException( "Invalid mod " + this.SourceModName );
			}
		}

		private void Initialize() {
			var srcMod = ModLoader.GetMod( this.SourceModName );

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

		public virtual Effect GetFx( CustomEntity ent ) {
			return null;
		}


		////////////////

		public virtual void DrawOverlay( SpriteBatch sb, CustomEntity ent ) { }

		public virtual void DrawPostTiles( SpriteBatch sb, CustomEntity ent ) {
			DrawsInGameEntityComponent.DrawTexture( sb, ent, this.Texture, this.FrameCount, this.GetLightColor(ent), 1f );
		}

		public virtual void DrawPostDraw( SpriteBatch sb, CustomEntity ent ) { }
	}
}
