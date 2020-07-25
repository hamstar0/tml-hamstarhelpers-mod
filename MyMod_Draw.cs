using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.UI.FreeHUD;


namespace HamstarHelpers {
	/// @private
	partial class ModHelpersMod : Mod {
		public override void UpdateUI( GameTime gameTime ) {
			FreeHUD.Instance?.UIContext.Update( Main._drawInterfaceGameTime );
		}


		////////////////

		public override void ModifyInterfaceLayers( List<GameInterfaceLayer> layers ) {
			//Services.DataStore.DataStore.Add( DebugHelpers.GetCurrentContext()+"_A", 1 );
			if( this.LoadHelpers == null ) { return; }

			int idx = layers.FindIndex( layer => layer.Name.Equals( "Vanilla: Mouse Text" ) );
			if( idx == -1 ) { return; }

			////

			GameInterfaceDrawMethod internalCallback = () => {
				if( this.LoadHelpers != null ) {
					this.LoadHelpers.IsLocalPlayerInGame_Hackish = true;  // Ugh!
				}
				return true;
			};

			GameInterfaceDrawMethod debugDrawCallback = () => {
				this.DrawDebug( Main.spriteBatch );
				return true;
			};

			GameInterfaceDrawMethod cpDrawCallback = () => {
				this.DrawCP( Main.spriteBatch );
				return true;
			};

			GameInterfaceDrawMethod hudDrawCallback = () => {
				FreeHUD.Instance?.UIContext.Draw( Main.spriteBatch, Main._drawInterfaceGameTime );
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

			if( !this.LoadHelpers.IsLocalPlayerInGame_Hackish ) {
				var internalLayer = new LegacyGameInterfaceLayer( "ModHelpers: Internal",
					internalCallback,
					InterfaceScaleType.UI );
				layers.Insert( 0, internalLayer );
			}

			if( this.LoadHelpers.IsLocalPlayerInGame_Hackish ) {
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

				var hudLayer = new LegacyGameInterfaceLayer( "ModHelpers: HUD UI",
					hudDrawCallback,
					InterfaceScaleType.UI );
				layers.Insert( idx, hudLayer );
			}
		}


		////////////////

		private void DrawCP( SpriteBatch sb ) {
			try {
				if( !ModHelpersConfig.Instance.DisableControlPanel ) {
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
				LogHelpers.Warn( e.ToString() );
			}
		}
	}
}
