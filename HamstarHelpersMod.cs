using HamstarHelpers.Components.Config;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Services.Messages;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;


namespace HamstarHelpers {
	partial class HamstarHelpersMod : Mod {
		public static HamstarHelpersMod Instance;

		public static string GithubUserName { get { return "hamstar0"; } }
		public static string GithubProjectName { get { return "tml-hamstarhelpers-mod"; } }

		public static string ConfigFileRelativePath {
			get { return JsonConfig<HamstarHelpersConfigData>.ConfigSubfolder + Path.DirectorySeparatorChar + HamstarHelpersConfigData.ConfigFileName; }
		}
		public static void ReloadConfigFromFile() {
			if( Main.netMode != 0 ) {
				throw new Exception( "Cannot reload configs outside of single player." );
			}
			if( HamstarHelpersMod.Instance != null ) {
				if( !HamstarHelpersMod.Instance.ConfigJson.LoadFile() ) {
					HamstarHelpersMod.Instance.ConfigJson.SaveFile();
				}
			}
		}
		public static void ResetConfigFromDefaults() {
			if( Main.netMode != 0 ) {
				throw new Exception( "Cannot reset to default configs outside of single player." );
			}
			HamstarHelpersMod.Instance.ConfigJson.SetData( new HamstarHelpersConfigData() );
			HamstarHelpersMod.Instance.ConfigJson.SaveFile();
		}


		////////////////

		public override void HandlePacket( BinaryReader reader, int player_who ) {
			try {
				int protocol_code = reader.ReadInt32();

				if( Main.netMode == 1 ) {
					if( protocol_code >= 0 ) {
						PacketProtocol.HandlePacketOnClient( protocol_code, reader, player_who );
					} else {
						Utilities.Network.OldPacketProtocol.HandlePacketOnClient( protocol_code, reader, player_who );
					}
				} else if( Main.netMode == 2 ) {
					if( protocol_code >= 0 ) {
						PacketProtocol.HandlePacketOnServer( protocol_code, reader, player_who );
					} else {
						Utilities.Network.OldPacketProtocol.HandlePacketOnServer( protocol_code, reader, player_who );
					}
				}
			} catch( Exception e ) {
				DebugHelpers.LogHelpers.Log( "(Mod Helpers) HandlePacket - " + e.ToString() );
			}
		}


		////////////////

		public override void PostDrawInterface( SpriteBatch sb ) {
			var modworld = this.GetModWorld<HamstarHelpersWorld>();

			if( this.LoadHelpers != null ) {
				this.LoadHelpers.IsClientPlaying = true;  // Ugh!
			}
		}


		public override void ModifyInterfaceLayers( List<GameInterfaceLayer> layers ) {
			if( this.LoadHelpers != null && !TmlHelpers.LoadHelpers.IsWorldBeingPlayed() ) { return; }

			int idx = layers.FindIndex( layer => layer.Name.Equals( "Vanilla: Mouse Text" ) );
			if( idx == -1 ) { return; }

			GameInterfaceDrawMethod debug_layer_draw = delegate {
				var sb = Main.spriteBatch;

				this.PlayerMessages.Draw( sb );
				SimpleMessage.DrawMessage( sb );

				DebugHelpers.DebugHelpers.PrintToBatch( sb );
				DebugHelpers.DebugHelpers.Once = false;
				DebugHelpers.DebugHelpers.OnceInAWhile--;
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


			var debug_layer = new LegacyGameInterfaceLayer( "HamstarHelpers: Debug Display",
				debug_layer_draw, InterfaceScaleType.UI );
			layers.Insert( idx, debug_layer );

			var modlock_layer = new LegacyGameInterfaceLayer( "HamstarHelpers: Mod Lock",
				modlock_layer_draw, InterfaceScaleType.UI );
			layers.Insert( idx, modlock_layer );

			if( !this.Config.DisableControlPanel ) {
				var cp_layer = new LegacyGameInterfaceLayer( "HamstarHelpers: Control Panel",
					cp_layer_draw, InterfaceScaleType.UI );
				layers.Insert( idx, cp_layer );
			}
		}


		////////////////

		//public override void UpdateMusic( ref int music ) { //, ref MusicPriority priority
		//	this.MusicHelpers.UpdateMusic();
		//}
	}
}
