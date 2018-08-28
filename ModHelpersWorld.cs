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
	class ModHelpersWorld : ModWorld {
		private readonly static object MyValidatorKey;
		internal readonly static PromiseValidator LoadValidator;
		internal readonly static PromiseValidator SaveValidator;

		private static object MyLock = new object();


		////////////////

		static ModHelpersWorld() {
			ModHelpersWorld.MyValidatorKey = new object();
			ModHelpersWorld.LoadValidator = new PromiseValidator( ModHelpersWorld.MyValidatorKey );
			ModHelpersWorld.SaveValidator = new PromiseValidator( ModHelpersWorld.MyValidatorKey );
		}



		////////////////

		public string ObsoleteID2 { get; private set; }
		
		internal string ObsoletedID;
		public bool HasObsoletedID { get; internal set; }  // Workaround for tml bug?

		internal WorldLogic WorldLogic { get; private set; }
		


		////////////////
		
		public override void Initialize() {
			var mymod = (ModHelpersMod)this.mod;

			this.ObsoleteID2 = WorldHelpers.GetUniqueId();
			this.ObsoletedID = Guid.NewGuid().ToString( "D" );
			this.HasObsoletedID = false;  // 'Load()' decides if no pre-existing one is found

			this.WorldLogic = new WorldLogic( mymod );

			if( String.IsNullOrEmpty(this.ObsoleteID2) ) {
				throw new Exception( "UID not defined." );
			}
		}


		internal void OnWorldExit() {
			this.HasObsoletedID = false;
		}

		////////////////

		public override void Load( TagCompound tags ) {
			var mymod = (ModHelpersMod)this.mod;

			if( tags.ContainsKey( "world_id" ) ) {
				this.ObsoletedID = tags.GetString( "world_id" );
			}

			//mymod.UserHelpers.Load( mymod, tags );
			mymod.ModLockHelpers.Load( mymod, tags );

			this.WorldLogic.LoadForWorld( mymod, tags );

			mymod.ModLockHelpers.PostLoad( mymod, this );
			//mymod.UserHelpers.OnWorldLoad( this );
			
			Promises.TriggerValidatedPromise( ModHelpersWorld.LoadValidator, ModHelpersWorld.MyValidatorKey, null );

			this.HasObsoletedID = true;
		}

		public override TagCompound Save() {
			var mymod = (ModHelpersMod)this.mod;
			TagCompound tags = new TagCompound();

			tags.Set( "world_id", this.ObsoletedID );

			//mymod.UserHelpers.Save( mymod, tags );
			mymod.ModLockHelpers.Save( mymod, tags );

			this.WorldLogic.SaveForWorld( mymod, tags );
			
			Promises.TriggerValidatedPromise( ModHelpersWorld.SaveValidator, ModHelpersWorld.MyValidatorKey, null );

			return tags;
		}


		////////////////

		public override void PreUpdate() {
			var mymod = (ModHelpersMod)this.mod;
			
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
			var mymod = (ModHelpersMod)this.mod;
			var myplayer = player.GetModPlayer<ModHelpersPlayer>( mymod );

			try {
				lock( ModHelpersWorld.MyLock ) {
					//Main.spriteBatch.Begin();
					RasterizerState rasterizer = Main.gameMenu ||
						(double)player.gravDir == 1.0 ?
							RasterizerState.CullCounterClockwise :
							RasterizerState.CullClockwise;
					Main.spriteBatch.Begin( SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, rasterizer, (Effect)null, Main.GameViewMatrix.TransformationMatrix );

					mymod.CustomEntMngr.DrawAll( Main.spriteBatch );

					DebugHelpers.DrawAllRects( Main.spriteBatch );

					Main.spriteBatch.End();
				}
			} catch( Exception e ) {
				LogHelpers.Log( "!ModHelpers.ModHelpersWorld.PostDrawTiles - " + e.ToString() );
			}
		}
	}
}
