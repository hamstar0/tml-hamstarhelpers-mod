using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.WorldHelpers;
using HamstarHelpers.Helpers.XnaHelpers;
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


		////////////////

		static ModHelpersWorld() {
			ModHelpersWorld.MyValidatorKey = new object();
			ModHelpersWorld.LoadValidator = new PromiseValidator( ModHelpersWorld.MyValidatorKey );
			ModHelpersWorld.SaveValidator = new PromiseValidator( ModHelpersWorld.MyValidatorKey );
		}



		////////////////
		
		public bool HasObsoleteId { get; internal set; }  // Workaround for tml bug?

		public string ObsoleteId { get; internal set; }
		public string ObsoleteId2 { get; private set; }

		internal WorldLogic WorldLogic { get; private set; }



		////////////////

		public override void Initialize() {
			var mymod = (ModHelpersMod)this.mod;

			this.ObsoleteId = Guid.NewGuid().ToString( "D" );
			this.ObsoleteId2 = WorldHelpers.GetUniqueId( false );
			this.HasObsoleteId = false;  // 'Load()' decides if no pre-existing one is found

			this.WorldLogic = new WorldLogic();

			if( String.IsNullOrEmpty( this.ObsoleteId2 ) ) {
				throw new HamstarException( "UID not defined." );
			}
		}


		internal void OnWorldExit() {
			this.HasObsoleteId = false;
		}

		////////////////

		public override void Load( TagCompound tags ) {
//DataStore.Add( DebugHelpers.GetCurrentContext()+"_A", 1 );
			var mymod = (ModHelpersMod)this.mod;

			if( tags.ContainsKey( "world_id" ) ) {
				this.ObsoleteId = tags.GetString( "world_id" );
			}

			//mymod.UserHelpers.Load( mymod, tags );
			mymod.ModLock.Load( tags );

			this.WorldLogic.LoadForWorld( tags );

			mymod.ModLock.PostLoad( this );
			//mymod.UserHelpers.OnWorldLoad( this );

			Promises.TriggerValidatedPromise( ModHelpersWorld.LoadValidator, ModHelpersWorld.MyValidatorKey, null );

			this.HasObsoleteId = true;
//DataStore.Add( DebugHelpers.GetCurrentContext()+"_B", 1 );
		}

		public override TagCompound Save() {
//DataStore.Add( DebugHelpers.GetCurrentContext()+"_A", 1 );
			var mymod = (ModHelpersMod)this.mod;
			TagCompound tags = new TagCompound();

			tags["world_id"] = this.ObsoleteId;

			//mymod.UserHelpers.Save( mymod, tags );
			mymod.ModLock.Save( tags );

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
			RasterizerState rasterizer = Main.gameMenu ||
					(double)player.gravDir == 1.0 ?
					RasterizerState.CullCounterClockwise : RasterizerState.CullClockwise;

			bool _;
			XnaHelpers.DrawBatch(
				(sb) => {
					var mymod = (ModHelpersMod)this.mod;
					DebugHelpers.DrawAllRects( sb );
				},
				SpriteSortMode.Deferred,
				BlendState.AlphaBlend,
				Main.DefaultSamplerState,
				DepthStencilState.None,
				rasterizer,
				(Effect)null,
				Main.GameViewMatrix.TransformationMatrix,
				out _
			);
//DataStore.Add( DebugHelpers.GetCurrentContext()+"_B", 1 );
		}
	}
}
