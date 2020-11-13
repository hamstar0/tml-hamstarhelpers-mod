using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Reflection;
using System;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;
using Terraria.Utilities;


namespace HamstarHelpers.Helpers.TModLoader {
	/// <summary>
	/// Assorted static "helper" functions pertaining to tModLoader.
	/// </summary>
	public static partial class TmlHelpers {
		private static UnifiedRandom BackupRand = null;



		////////////////

		/// <summary>
		/// Gets the singleton instance of a given class type. If no such instance exists, one is created and registered.
		/// Warning: Avoid calling ContentInstance.Register(...) for this class. Use caution with tModLoader's singletons.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T SafelyGetInstance<T>() where T : class {
			lock( TmlHelpers.MyLock ) {
				T instance = ModContent.GetInstance<T>();
				if( instance != null ) {
					return instance;
				}

				instance = (T)Activator.CreateInstance(
					typeof( T ),
					BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
					null,
					new object[] { },
					null
				);
				if( instance == null ) {
					throw new ModHelpersException( "Could not generate singleton for " + typeof( T ).Name );
				}

				ContentInstance.Register( instance );

				return instance;
			}
		}

		/// <summary>
		/// Gets the singleton instance of a given class type. If no such instance exists, one is created and registered.
		/// Note: Avoid calling ContentInstance.Register(...) for this class after.
		/// </summary>
		/// <param name="classType"></param>
		/// <returns></returns>
		public static object SafelyGetInstanceForType( Type classType ) {
			lock( TmlHelpers.MyLock ) {
				MethodInfo method = typeof( TmlHelpers ).GetMethod( "SafelyGetInstance" );
				MethodInfo genericMethod = method.MakeGenericMethod( classType );

				object rawInstance = genericMethod.Invoke( null, new object[] { } );
				if( rawInstance == null ) {
					throw new ModHelpersException( "Could not get ModContent singleton of "+classType.Name );
				}

				return rawInstance;
			}
		}


		////////////////

		private static void ForceSetupPlayer( Player player ) {
			ModPlayer[] modPlayers;

			if( !ReflectionHelpers.Get( player, "modPlayers", out modPlayers ) || modPlayers.Length == 0 ) {
				MethodInfo setupPlayerMethod = typeof( PlayerHooks ).GetMethod( "SetupPlayer", ReflectionHelpers.MostAccess );
				if( setupPlayerMethod == null ) {
					throw new ModHelpersException( "Could not run SetupPlayer for " + ( player?.name ?? "null player" ) );
				}

				setupPlayerMethod.Invoke( null, new object[] { player } );
			}
		}

		////////////////

		/// <summary>
		/// Provides an alternative to `Player.GetModPlayer(...)` to ensure the given player is properly loaded. Addresses some
		/// confusing types of errors.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="mod"></param>
		/// <param name="modPlayerName"></param>
		/// <returns></returns>
		public static ModPlayer SafelyGetModPlayer( Player player, Mod mod, string modPlayerName ) {    // Solely for Main.LocalPlayer?
			TmlHelpers.ForceSetupPlayer( player );
			return player.GetModPlayer( mod, modPlayerName );
		}

		/// <summary>
		/// Provides an alternative to `Player.GetModPlayer(...)` to ensure the given player is properly loaded. Addresses some
		/// confusing types of errors.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="player"></param>
		/// <returns></returns>
		public static T SafelyGetModPlayer<T>( Player player ) where T : ModPlayer {
			TmlHelpers.ForceSetupPlayer( player );
			return player.GetModPlayer<T>();
		}


		////////////////

		/// <summary>
		/// Ensures a valid randomizer. Typically returns `Main.rand`.
		/// </summary>
		/// <returns></returns>
		public static UnifiedRandom SafelyGetRand() {
			if( Main.rand != null ) {
				return Main.rand;
			}
			if( TmlHelpers.BackupRand == null ) {
				TmlHelpers.BackupRand = new UnifiedRandom( (int)DateTime.UtcNow.ToFileTime() );
			}
			return TmlHelpers.BackupRand;
		}
	}
}
