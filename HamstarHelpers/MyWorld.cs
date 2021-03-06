﻿using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.World;
using HamstarHelpers.Helpers.XNA;
using HamstarHelpers.Internals.Logic;
using HamstarHelpers.Services.Hooks.Draw;
using HamstarHelpers.Services.Hooks.LoadHooks;


namespace HamstarHelpers {
	/// @private
	class ModHelpersWorld : ModWorld {
		private readonly static object MyValidatorKey;
		internal readonly static CustomLoadHookValidator<object> LoadValidator;
		internal readonly static CustomLoadHookValidator<object> SaveValidator;


		////////////////

		static ModHelpersWorld() {
			ModHelpersWorld.MyValidatorKey = new object();
			ModHelpersWorld.LoadValidator = new CustomLoadHookValidator<object>( ModHelpersWorld.MyValidatorKey );
			ModHelpersWorld.SaveValidator = new CustomLoadHookValidator<object>( ModHelpersWorld.MyValidatorKey );
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
			this.ObsoleteId2 = WorldHelpers.GetUniqueIdForCurrentWorld( false );
			this.HasObsoleteId = false;  // 'Load()' decides if no pre-existing one is found

			this.WorldLogic = new WorldLogic();

			if( String.IsNullOrEmpty( this.ObsoleteId2 ) ) {
				throw new ModHelpersException( "UID not defined." );
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

			CustomLoadHooks.TriggerHook( ModHelpersWorld.LoadValidator, ModHelpersWorld.MyValidatorKey );

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

			CustomLoadHooks.TriggerHook( ModHelpersWorld.SaveValidator, ModHelpersWorld.MyValidatorKey );
			
//DataStore.Add( DebugHelpers.GetCurrentContext()+"_B", 1 );
			return tags;
		}


		////////////////

		public override void PreUpdate() {
//DataStore.Add( DebugHelpers.GetCurrentContext()+"_A", 1 );
			var mymod = (ModHelpersMod)this.mod;

			if( this.WorldLogic != null ) {
				if( Main.netMode == NetmodeID.SinglePlayer ) { // Single
					this.WorldLogic.PreUpdateSingle();
				} else if( Main.netMode == NetmodeID.Server ) {
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
			XNAHelpers.DrawBatch(
				(sb) => {
					DebugHelpers.DrawAllRects( sb );
					ModContent.GetInstance<DrawHooksInternal>()?.RunPostDrawTilesActions();
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
