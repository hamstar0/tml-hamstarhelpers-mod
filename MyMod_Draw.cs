using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using HamstarHelpers.Helpers.Debug;


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
				this.DrawDebug( Main.spriteBatch );
				return true;
			};

			GameInterfaceDrawMethod cpDrawCallback = () => {
				this.DrawCP( Main.spriteBatch );
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


		////////////////

		private void DrawDebug( SpriteBatch sb ) {
			try {
				this.PlayerMessages.Draw( sb );
				this.DrawMouseData( sb );
				DebugHelpers.PrintAll( sb );
			} catch( Exception e ) {
				LogHelpers.Warn( e.ToString() );
			}
		}

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


		////

		private void DrawMouseData( SpriteBatch sb ) {
			if( !ModHelpersConfig.Instance.DebugModeMouseInfo ) {
				return;
			}

			Vector2 wldMouse = Main.MouseWorld / 16f;
			int tileX = (int)wldMouse.X;
			int tileY = (int)wldMouse.Y;
			Tile tile = Framing.GetTileSafely( tileX, tileY );

			var data = new List<string>();

			string tileData = tileX + ", " + tileY;

			if( tile != null ) {
				tileData += " - ";
				if( tile.active() ) {
					tileData += TileID.GetUniqueKey( tile.type ) + " " + tile.frameX + ":"+tile.frameY;
				} else {
					tileData += "Air";
				}
				if( tile.wall > 0 ) {
					tileData += ", wall: " + WallID.GetUniqueKey( tile.wall ) + " " + tile.wallFrameX()+":"+tile.wallFrameY();
				} else {
					tileData += ", wall: None";
				}
			}

			data.Add( tileData );

			for( int i=0; i<Main.npc.Length; i++ ) {
				NPC npc = Main.npc[i];
				if( npc?.active != true ) { continue; }
				if( !npc.getRect().Contains((int)Main.MouseWorld.X, (int)Main.MouseWorld.Y) ) { continue; }

				string npcData = npc.FullName+" ("+npc.type+")"
					+", who:"+npc.whoAmI
					+", ai:"+string.Join(",", npc.ai)
					+", localAI:"+string.Join(",", npc.localAI);
				data.Add( npcData );
			}

			for( int i=0; i<Main.projectile.Length; i++ ) {
				Projectile proj = Main.projectile[i];
				if( proj?.active != true ) { continue; }
				if( !proj.getRect().Contains((int)Main.MouseWorld.X, (int)Main.MouseWorld.Y) ) { continue; }

				string projData = proj.Name+" ("+ proj.type+")"
					+", who:"+ proj.whoAmI
					+", ai:"+string.Join(",", proj.ai)
					+", localAI:"+string.Join(",", proj.localAI);
				data.Add( projData );
			}

			for( int i=0; i<data.Count; i++ ) {
				Utils.DrawBorderStringFourWay(
					sb,
					Main.fontMouseText,
					data[i],
					Main.mouseX + 24,
					Main.mouseY + (i * 16) + 36,
					Color.White,
					Color.Black,
					Vector2.Zero
				);
			}
		}
	}
}
