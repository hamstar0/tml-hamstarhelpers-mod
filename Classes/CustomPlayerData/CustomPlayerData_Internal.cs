using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Extensions;
using HamstarHelpers.Helpers.DotNET.Reflection;
using HamstarHelpers.Helpers.Players;
using HamstarHelpers.Helpers.TModLoader;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Classes.PlayerData {
	/// <summary>
	/// An alternative to ModPlayer for basic per-player, per-game data storage and Update use.
	/// </summary>
	public partial class CustomPlayerData : ILoadable {
		private static void Enter( int playerWho ) {
			Player player = Main.player[playerWho];

			if( ModHelpersConfig.Instance.DebugModeHelpersInfo ) {
				LogHelpers.Alert( "Player "+player.name+" ("+playerWho+") entered the game." );
			}

			CustomPlayerData singleton = ModContent.GetInstance<CustomPlayerData>();
			IEnumerable<Type> plrDataTypes = ReflectionHelpers.GetAllAvailableSubTypesFromMods( typeof( CustomPlayerData ) );
			string uid = PlayerIdentityHelpers.GetUniqueId( player );

			foreach( Type plrDataType in plrDataTypes ) {
				object data = CustomPlayerData.LoadFileData( plrDataType.Name, uid );
				var plrData = (CustomPlayerData)Activator.CreateInstance(
					plrDataType,
					BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
					null,
					new object[] { },
					null );
				plrData.PlayerWho = playerWho;

				singleton.DataMap.Set2D( playerWho, plrDataType, plrData );

				plrData.OnEnter( data );
			}
		}

		private static void Exit( int playerWho ) {
			if( ModHelpersConfig.Instance.DebugModeHelpersInfo ) {
				LogHelpers.Alert( "Player " + Main.player[playerWho].name + " (" + playerWho + ") exited the game." );
			}

			CustomPlayerData singleton = ModContent.GetInstance<CustomPlayerData>();

			foreach( (Type plrDataType, CustomPlayerData plrData) in singleton.DataMap[playerWho] ) {
				object data = plrData.OnExit();
				if( data != null ) {
					CustomPlayerData.SaveFileData( plrData.GetType().Name, PlayerIdentityHelpers.GetUniqueId(), data );
				}
			}
		}


		////////////////

		private static void UpdateAll() {
			bool isNotMenu = Main.netMode == 2 ? true : !Main.gameMenu;
			var singleton = ModContent.GetInstance<CustomPlayerData>();
			Player player;
			
			for( int plrWho = 0; plrWho < Main.maxPlayers; plrWho++ ) {
				player = Main.player[plrWho];

				if( player == null || !player.active ) {
					if( singleton.DataMap.ContainsKey( plrWho ) ) {
						CustomPlayerData.Exit( plrWho );
					}

					continue;
				}

				//bool isInGame = Main.netMode == 2
				//	? true
				//	: plrWho == Main.myPlayer
				//		? LoadHelpers.IsCurrentPlayerInGame()
				//		: false;
				
				if( isNotMenu ) {
					if( !singleton.DataMap.ContainsKey(plrWho) ) {
						CustomPlayerData.Enter( plrWho );
					} else {
						foreach( (Type plrDataType, CustomPlayerData plrData) in singleton.DataMap[plrWho] ) {
							plrData.Update();
						}
					}
				} else {
					if( singleton.DataMap.ContainsKey( plrWho ) ) {
						CustomPlayerData.Exit( plrWho );
					}
				}
			}

			if( Main.netMode != 2 && Main.gameMenu ) {
				singleton.DataMap.Clear();
			}
		}
	}
}
