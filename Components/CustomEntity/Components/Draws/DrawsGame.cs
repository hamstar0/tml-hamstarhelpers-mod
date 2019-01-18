using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Components.CustomEntity.Components {
	public partial class DrawsInGameEntityComponent : CustomEntityComponent {
		protected class DrawsInGameEntityComponentFactory {
			public readonly string SourceModName;
			public readonly string TexturePath;
			public readonly int FrameCount;
			
			public DrawsInGameEntityComponentFactory( string srcModName, string relTexturePath, int frameCount ) {
				this.SourceModName = srcModName;
				this.TexturePath = relTexturePath;
				this.FrameCount = frameCount;
			}
		}



		////////////////

		[PacketProtocolIgnore]
		public string ModName;
		[PacketProtocolIgnore]
		public string TexturePath;
		[PacketProtocolIgnore]
		public int FrameCount;

		////////////////

		[PacketProtocolIgnore]
		[JsonIgnore]
		public Texture2D Texture { get; protected set; }



		////////////////

		protected DrawsInGameEntityComponent( PacketProtocolDataConstructorLock ctorLock ) : base( ctorLock ) { }

		////////////////

		protected override Type GetMyFactoryType() {
			return typeof( DrawsInGameEntityComponentFactory );
		}

		////////////////

		protected sealed override void OnInitialize() {
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
