using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.WorldHelpers;
using HamstarHelpers.Internals.Logic;
using HamstarHelpers.Services.Promises;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace HamstarHelpers {
	class HamstarHelpersWorld : ModWorld {
		internal static readonly object WorldLoad = new object();
		internal static readonly object WorldSave = new object();
		private static object MyLock = new object();


		////////////////

		public string ObsoleteID2 { get; private set; }
		
		internal string ObsoleteID;
		public bool HasCorrectID { get; internal set; }  // Workaround for tml bug?

		internal WorldLogic WorldLogic { get; private set; }
		


		////////////////
		
		public override void Initialize() {
			var mymod = (HamstarHelpersMod)this.mod;

			this.ObsoleteID2 = WorldHelpers.GetUniqueId();
			this.ObsoleteID = Guid.NewGuid().ToString( "D" );
			this.HasCorrectID = false;  // 'Load()' decides if no pre-existing one is found

			this.WorldLogic = new WorldLogic( mymod );

			if( String.IsNullOrEmpty(this.ObsoleteID2) ) {
				throw new Exception( "UID not defined." );
			}
		}


		internal void OnWorldExit() {
			this.HasCorrectID = false;
		}

		////////////////

		public override void Load( TagCompound tags ) {
			var mymod = (HamstarHelpersMod)this.mod;

			if( tags.ContainsKey( "world_id" ) ) {
				this.ObsoleteID = tags.GetString( "world_id" );
			}

			//mymod.UserHelpers.Load( mymod, tags );
			mymod.ModLockHelpers.Load( mymod, tags );

			this.WorldLogic.LoadForWorld( mymod, tags );

			mymod.ModLockHelpers.PostLoad( mymod, this );
			//mymod.UserHelpers.OnWorldLoad( this );

			Promises.TriggerCustomPromiseForObject( HamstarHelpersWorld.WorldLoad );

			this.HasCorrectID = true;
		}

		public override TagCompound Save() {
			var mymod = (HamstarHelpersMod)this.mod;
			TagCompound tags = new TagCompound();

			tags.Set( "world_id", this.ObsoleteID );

			//mymod.UserHelpers.Save( mymod, tags );
			mymod.ModLockHelpers.Save( mymod, tags );

			this.WorldLogic.SaveForWorld( mymod, tags );

			Promises.TriggerCustomPromiseForObject( HamstarHelpersWorld.WorldSave );

			return tags;
		}


		////////////////

		public override void PreUpdate() {
			var mymod = (HamstarHelpersMod)this.mod;
			
			if( this.WorldLogic != null ) {
				if( Main.netMode == 0 ) { // Single
					this.WorldLogic.PreUpdateSingle( mymod );
				} else if( Main.netMode == 2 ) {
					this.WorldLogic.PreUpdateServer( mymod );
				}
			}
		}


		////////////////

		public override void PostDrawTiles() {
			Player player = Main.LocalPlayer;
			var mymod = (HamstarHelpersMod)this.mod;
			var myplayer = player.GetModPlayer<HamstarHelpersPlayer>( mymod );

			try {
				lock( HamstarHelpersWorld.MyLock ) {
					//Main.spriteBatch.Begin();
					RasterizerState rasterizer = Main.gameMenu ||
						(double)player.gravDir == 1.0 ?
							RasterizerState.CullCounterClockwise :
							RasterizerState.CullClockwise;
					Main.spriteBatch.Begin( SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, rasterizer, (Effect)null, Main.GameViewMatrix.TransformationMatrix );

					mymod.CustomEntMngr.DrawAll( Main.spriteBatch );

					Main.spriteBatch.End();
				}
			} catch( Exception e ) {
				LogHelpers.Log( "HamstarHelpers.HamstarHelpersWorld.PostDrawTiles - " + e.ToString() );
			}
		}
	}
}
