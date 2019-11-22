using HamstarHelpers.Helpers.Debug;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;


namespace HamstarHelpers {
	/// @private
	partial class ModHelpersMod : Mod {
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
				//Services.DataStore.DataStore.Add( DebugHelpers.GetCurrentContext()+"_B", 1 );
			}
		}
	}
}
