using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;


namespace HamstarHelpers {
	/// @private
	partial class ModHelpersMod : Mod {
		public override void PostDrawInterface( SpriteBatch sb ) {
//Services.DataStore.DataStore.Add( DebugHelpers.GetCurrentContext()+"_A", 1 );
			if( this.LoadHelpers != null ) {
				this.LoadHelpers.IsClientPlaying_Hackish = true;  // Ugh!
			}

			try {
				if( !Main.mapFullscreen && ( Main.mapStyle == 1 || Main.mapStyle == 2 ) ) {
					//this.DrawMiniMapForAll( sb );
				}
			} catch( Exception e ) {
				LogHelpers.Warn( e.ToString() );
				throw e;
			}
//Services.DataStore.DataStore.Add( DebugHelpers.GetCurrentContext()+"_B", 1 );
		}

		public override void PostDrawFullscreenMap( ref string mouseText ) {
//Services.DataStore.DataStore.Add( DebugHelpers.GetCurrentContext()+"_A", 1 );
			try {
				//this.DrawFullMapForAll( Main.spriteBatch );
			} catch( Exception e ) {
				LogHelpers.Warn( e.ToString() );
				throw e;
			}
//Services.DataStore.DataStore.Add( DebugHelpers.GetCurrentContext()+"_B", 1 );
		}


		////////////////

		public override void ModifyInterfaceLayers( List<GameInterfaceLayer> layers ) {
//Services.DataStore.DataStore.Add( DebugHelpers.GetCurrentContext()+"_A", 1 );
			if( this.LoadHelpers != null && !LoadHelpers.IsWorldBeingPlayed() ) { return; }

			int idx = layers.FindIndex( layer => layer.Name.Equals( "Vanilla: Mouse Text" ) );
			if( idx == -1 ) { return; }

			GameInterfaceDrawMethod debugDrawCallback = () => {
				var sb = Main.spriteBatch;

				try {
					this.PlayerMessages.Draw( sb );

					DebugHelpers.PrintAll( sb );
				} catch( Exception e ) {
					LogHelpers.Warn( "debugLayerDraw - " + e.ToString() );
				}
				return true;
			};

			GameInterfaceDrawMethod cpDrawCallback = () => {
				var sb = Main.spriteBatch;

				try {
					if( !this.Config.DisableControlPanel ) {
						this.ControlPanel.UpdateToggler();
						this.ControlPanel.DrawToggler( sb );
					}
					if( this.LastSeenCPScreenWidth != Main.screenWidth || this.LastSeenCPScreenHeight != Main.screenHeight ) {
						this.LastSeenCPScreenWidth = Main.screenWidth;
						this.LastSeenCPScreenHeight = Main.screenHeight;
						this.ControlPanel.RecalculateMe();
					}

					this.Inbox.Draw( sb );
				} catch( Exception e ) {
					LogHelpers.Warn( "cpLayerDraw - " + e.ToString() );
				}

				return true;
			};

			GameInterfaceDrawMethod modlockDrawCallback = delegate {
				try {
					this.ModLock.DrawWarning( Main.spriteBatch );
				} catch( Exception e ) {
					LogHelpers.Warn( "modlockLayerDraw - " + e.ToString() );
				}
				return true;
			};

			////

			var debugLayer = new LegacyGameInterfaceLayer( "ModHelpers: Debug Display",
				debugDrawCallback,
				InterfaceScaleType.UI );
			layers.Insert( idx, debugLayer );

			var modlockLayer = new LegacyGameInterfaceLayer( "ModHelpers: Mod Lock",
				modlockDrawCallback,
				InterfaceScaleType.UI );
			layers.Insert( idx, modlockLayer );

			var cpLayer = new LegacyGameInterfaceLayer( "ModHelpers: Control Panel",
				cpDrawCallback,
				InterfaceScaleType.UI );
			layers.Insert( idx, cpLayer );
//Services.DataStore.DataStore.Add( DebugHelpers.GetCurrentContext()+"_B", 1 );
		}
	}
}
