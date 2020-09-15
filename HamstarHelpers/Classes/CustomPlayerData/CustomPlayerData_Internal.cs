using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Extensions;
using HamstarHelpers.Helpers.DotNET.Reflection;
using HamstarHelpers.Helpers.Players;
using HamstarHelpers.Helpers.TModLoader;


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

				lock( CustomPlayerData.MyLock ) {
					singleton.DataMap.Set2D( playerWho, plrDataType, plrData );
				}

				ReflectionHelpers.RunMethod(
					instance: plrData,
					methodName: "OnEnter",
					args: new object[] { data },
					returnVal: out object _
				);
				plrData.OnEnter( Main.myPlayer == playerWho, data );
			}
		}

		private static void Exit( int playerWho ) {
			if( ModHelpersConfig.Instance.DebugModeHelpersInfo ) {
				LogHelpers.Alert( "Player " + Main.player[playerWho].name + " (" + playerWho + ") exited the game." );
			}

			CustomPlayerData singleton = ModContent.GetInstance<CustomPlayerData>();
			IEnumerable<(Type, CustomPlayerData)> plrDataMap;
			lock( CustomPlayerData.MyLock ) {
				plrDataMap = singleton.DataMap[playerWho].Select( kv => (kv.Key, kv.Value) );
			}

			foreach( (Type plrDataType, CustomPlayerData plrData) in plrDataMap ) {
				object data = plrData.OnExit();

				if( data != null ) {
					CustomPlayerData.SaveFileData( plrData.GetType().Name, PlayerIdentityHelpers.GetUniqueId(), data );
				}
			}
		}


		////////////////

		internal static void UpdateAll() {
			bool isNotMenu = Main.netMode == NetmodeID.Server ? true : !Main.gameMenu;
			var singleton = TmlHelpers.SafelyGetInstance<CustomPlayerData>();
			if( singleton == null ) {
				return;
			}

			Player player;
			
			for( int plrWho = 0; plrWho < Main.maxPlayers; plrWho++ ) {
				player = Main.player[plrWho];

				bool containsKey = false;
				lock( CustomPlayerData.MyLock ) {
					containsKey = singleton.DataMap.ContainsKey( plrWho );
				}

				if( player == null || !player.active ) {
					if( containsKey ) {
						CustomPlayerData.Exit( plrWho );
					}

					continue;
				}

				//bool isInGame = Main.netMode == NetmodeID.Server
				//	? true
				//	: plrWho == Main.myPlayer
				//		? LoadHelpers.IsCurrentPlayerInGame()
				//		: false;
				
				if( isNotMenu ) {
					if( !containsKey ) {
						CustomPlayerData.Enter( plrWho );
					} else {
						IEnumerable<(Type, CustomPlayerData)> plrDataMap;
						lock( CustomPlayerData.MyLock ) {
							plrDataMap = singleton.DataMap[plrWho].Select( kv=>(kv.Key, kv.Value) );
						}

						foreach( (Type plrDataType, CustomPlayerData plrData) in plrDataMap ) {
							plrData.Update();
						}
					}
				} else {
					if( containsKey ) {
						CustomPlayerData.Exit( plrWho );
					}
				}
			}

			if( Main.netMode != NetmodeID.Server && Main.gameMenu ) {
				lock( CustomPlayerData.MyLock ) {
					singleton.DataMap.Clear();
				}
			}
		}
	}
}
