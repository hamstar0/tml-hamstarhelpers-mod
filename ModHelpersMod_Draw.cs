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
	partial class ModHelpersMod : Mod {
		public override void PostDrawInterface( SpriteBatch sb ) {
Services.DataStore.DataStore.Add( DebugHelpers.GetCurrentContext()+"_A", 1 );
			if( this.LoadHelpers != null ) {
				this.LoadHelpers.IsClientPlaying_Hackish = true;  // Ugh!
			}

			try {
				if( !Main.mapFullscreen && ( Main.mapStyle == 1 || Main.mapStyle == 2 ) ) {
					this.DrawMiniMapForAll( sb );
				}
			} catch( Exception e ) {
				LogHelpers.Warn( e.ToString() );
				throw e;
			}
Services.DataStore.DataStore.Add( DebugHelpers.GetCurrentContext()+"_B", 1 );
		}

		public override void PostDrawFullscreenMap( ref string mouseText ) {
Services.DataStore.DataStore.Add( DebugHelpers.GetCurrentContext()+"_A", 1 );
			try {
				this.DrawFullMapForAll( Main.spriteBatch );
			} catch( Exception e ) {
				LogHelpers.Warn( e.ToString() );
				throw e;
			}
Services.DataStore.DataStore.Add( DebugHelpers.GetCurrentContext()+"_B", 1 );
		}


		////////////////

		public override void ModifyInterfaceLayers( List<GameInterfaceLayer> layers ) {
Services.DataStore.DataStore.Add( DebugHelpers.GetCurrentContext()+"_A", 1 );
			if( this.LoadHelpers != null && !LoadHelpers.IsWorldBeingPlayed() ) { return; }

			int idx = layers.FindIndex( layer => layer.Name.Equals( "Vanilla: Mouse Text" ) );
			if( idx == -1 ) { return; }

			GameInterfaceDrawMethod debugLayerDraw = delegate {
				var sb = Main.spriteBatch;

				try {
					this.PlayerMessages.Draw( sb );
					SimpleMessage.DrawMessage( sb );

					DebugHelpers.PrintAll( sb );
					DebugHelpers.Once = false;
					DebugHelpers.OnceInAWhile--;
				} catch( Exception e ) {
					LogHelpers.Warn( "debugLayerDraw - " + e.ToString() );
				}
				return true;
			};

			GameInterfaceDrawMethod cpLayerDraw = delegate {
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

//sb.DrawString( Main.fontDeathText, "ALERT", new Vector2(128, 128), this.AnimatedColors.Alert.CurrentColor );
//sb.DrawString( Main.fontDeathText, "STROBE", new Vector2(128, 256), this.AnimatedColors.Strobe.CurrentColor );
//sb.DrawString( Main.fontDeathText, "FIRE", new Vector2(128, 320), this.AnimatedColors.Fire.CurrentColor );
//sb.DrawString( Main.fontDeathText, "WATER", new Vector2(128, 384), this.AnimatedColors.Water.CurrentColor );
//sb.DrawString( Main.fontDeathText, "AIR", new Vector2(128, 448), this.AnimatedColors.Air.CurrentColor );
				return true;
			};

			GameInterfaceDrawMethod modlockLayerDraw = delegate {
				try {
					this.ModLockHelpers.DrawWarning( Main.spriteBatch );
				} catch( Exception e ) {
					LogHelpers.Warn( "modlockLayerDraw - " + e.ToString() );
				}
				return true;
			};


			var debugLayer = new LegacyGameInterfaceLayer( "ModHelpers: Debug Display",
				debugLayerDraw, InterfaceScaleType.UI );
			layers.Insert( idx, debugLayer );

			var modlockLayer = new LegacyGameInterfaceLayer( "ModHelpers: Mod Lock",
				modlockLayerDraw, InterfaceScaleType.UI );
			layers.Insert( idx, modlockLayer );

			if( !this.Config.DisableControlPanel ) {
				var cpLayer = new LegacyGameInterfaceLayer( "ModHelpers: Control Panel",
					cpLayerDraw, InterfaceScaleType.UI );
				layers.Insert( idx, cpLayer );
			}
Services.DataStore.DataStore.Add( DebugHelpers.GetCurrentContext()+"_B", 1 );
		}
	}
}
