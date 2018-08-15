using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.TmlHelpers;
using HamstarHelpers.Services.Messages;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;


namespace HamstarHelpers {
	partial class HamstarHelpersMod : Mod {
		public override void PostDrawInterface( SpriteBatch sb ) {
			if( this.LoadHelpers != null ) {
				this.LoadHelpers.IsClientPlaying_Hackish = true;  // Ugh!
			}

			try {
				if( !Main.mapFullscreen && ( Main.mapStyle == 1 || Main.mapStyle == 2 ) ) {
					this.DrawMiniMapForAll( sb );
				}
			} catch( Exception e ) {
				ErrorLogger.Log( "HamstarHelpersMod.PostDrawInterface - " + e.ToString() );
				throw e;
			}
		}

		public override void PostDrawFullscreenMap( ref string mouseText ) {
			try {
				this.DrawFullMapForAll( Main.spriteBatch );
			} catch( Exception e ) {
				ErrorLogger.Log( "HamstarHelpersMod.PostDrawFullscreenMap: " + e.ToString() );
				throw e;
			}
		}


		////////////////

		public override void ModifyInterfaceLayers( List<GameInterfaceLayer> layers ) {
			if( this.LoadHelpers != null && !LoadHelpers.IsWorldBeingPlayed() ) { return; }

			int idx = layers.FindIndex( layer => layer.Name.Equals( "Vanilla: Mouse Text" ) );
			if( idx == -1 ) { return; }

			GameInterfaceDrawMethod debug_layer_draw = delegate {
				var sb = Main.spriteBatch;

				this.PlayerMessages.Draw( sb );
				SimpleMessage.DrawMessage( sb );

				DebugHelpers.PrintAll( sb );
				DebugHelpers.Once = false;
				DebugHelpers.OnceInAWhile--;
				return true;
			};

			GameInterfaceDrawMethod cp_layer_draw = delegate {
				var sb = Main.spriteBatch;
				
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

//sb.DrawString( Main.fontDeathText, "ALERT", new Vector2(128, 128), this.AnimatedColors.Alert.CurrentColor );
//sb.DrawString( Main.fontDeathText, "STROBE", new Vector2(128, 256), this.AnimatedColors.Strobe.CurrentColor );
//sb.DrawString( Main.fontDeathText, "FIRE", new Vector2(128, 320), this.AnimatedColors.Fire.CurrentColor );
//sb.DrawString( Main.fontDeathText, "WATER", new Vector2(128, 384), this.AnimatedColors.Water.CurrentColor );
//sb.DrawString( Main.fontDeathText, "AIR", new Vector2(128, 448), this.AnimatedColors.Air.CurrentColor );
				return true;
			};

			GameInterfaceDrawMethod modlock_layer_draw = delegate {
				this.ModLockHelpers.DrawWarning( Main.spriteBatch );
				return true;
			};


			var debug_layer = new LegacyGameInterfaceLayer( "ModHelpers: Debug Display",
				debug_layer_draw, InterfaceScaleType.UI );
			layers.Insert( idx, debug_layer );

			var modlock_layer = new LegacyGameInterfaceLayer( "ModHelpers: Mod Lock",
				modlock_layer_draw, InterfaceScaleType.UI );
			layers.Insert( idx, modlock_layer );

			if( !this.Config.DisableControlPanel ) {
				var cp_layer = new LegacyGameInterfaceLayer( "ModHelpers: Control Panel",
					cp_layer_draw, InterfaceScaleType.UI );
				layers.Insert( idx, cp_layer );
			}
		}
	}
}
