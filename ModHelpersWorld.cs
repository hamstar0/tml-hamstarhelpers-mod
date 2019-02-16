using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.WorldHelpers;
using HamstarHelpers.Internals.Logic;
using HamstarHelpers.Services.DataStore;
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

			this.WorldLogic = new WorldLogic();

			if( String.IsNullOrEmpty( this.ObsoleteID2 ) ) {
				throw new HamstarException( "UID not defined." );
			}
		}


		internal void OnWorldExit() {
			this.HasObsoletedID = false;
		}

		////////////////

		public override void Load( TagCompound tags ) {
//DataStore.Add( DebugHelpers.GetCurrentContext()+"_A", 1 );
			var mymod = (ModHelpersMod)this.mod;

			if( tags.ContainsKey( "world_id" ) ) {
				this.ObsoletedID = tags.GetString( "world_id" );
			}

			//mymod.UserHelpers.Load( mymod, tags );
			mymod.ModLockHelpers.Load( tags );

			this.WorldLogic.LoadForWorld( tags );

			mymod.ModLockHelpers.PostLoad( this );
			//mymod.UserHelpers.OnWorldLoad( this );

			Promises.TriggerValidatedPromise( ModHelpersWorld.LoadValidator, ModHelpersWorld.MyValidatorKey, null );

			this.HasObsoletedID = true;
//DataStore.Add( DebugHelpers.GetCurrentContext()+"_B", 1 );
		}

		public override TagCompound Save() {
//DataStore.Add( DebugHelpers.GetCurrentContext()+"_A", 1 );
			var mymod = (ModHelpersMod)this.mod;
			TagCompound tags = new TagCompound();

			tags.Set( "world_id", this.ObsoletedID );

			//mymod.UserHelpers.Save( mymod, tags );
			mymod.ModLockHelpers.Save( tags );

			this.WorldLogic.SaveForWorld( tags );

			Promises.TriggerValidatedPromise( ModHelpersWorld.SaveValidator, ModHelpersWorld.MyValidatorKey, null );
			
//DataStore.Add( DebugHelpers.GetCurrentContext()+"_B", 1 );
			return tags;
		}


		////////////////

		public override void PreUpdate() {
//DataStore.Add( DebugHelpers.GetCurrentContext()+"_A", 1 );
			var mymod = (ModHelpersMod)this.mod;

			if( this.WorldLogic != null ) {
				if( Main.netMode == 0 ) { // Single
					this.WorldLogic.PreUpdateSingle();
				} else if( Main.netMode == 2 ) {
					this.WorldLogic.PreUpdateServer();
				}
			}
//DataStore.Add( DebugHelpers.GetCurrentContext()+"_B", 1 );
		}


		////////////////

		public override void PostDrawTiles() {
//DataStore.Add( DebugHelpers.GetCurrentContext()+"_A", 1 );
			Player player = Main.LocalPlayer;
			if( player == null ) { return; }
			var mymod = (ModHelpersMod)this.mod;
			RasterizerState rasterizer = Main.gameMenu ||
					(double)player.gravDir == 1.0 ?
					RasterizerState.CullCounterClockwise : RasterizerState.CullClockwise;

			lock( ModHelpersWorld.MyLock ) {
				Main.spriteBatch.Begin( SpriteSortMode.Deferred,
					BlendState.AlphaBlend,
					Main.DefaultSamplerState,
					DepthStencilState.None,
					rasterizer,
					(Effect)null,
					Main.GameViewMatrix.TransformationMatrix
				);

				try {
					mymod.CustomEntMngr.DrawPostTilesAll( Main.spriteBatch );
					DebugHelpers.DrawAllRects( Main.spriteBatch );
				} catch( Exception e ) {
					LogHelpers.Warn( e.ToString() );
				}

				Main.spriteBatch.End();
			}
//DataStore.Add( DebugHelpers.GetCurrentContext()+"_B", 1 );
		}
	}
}
