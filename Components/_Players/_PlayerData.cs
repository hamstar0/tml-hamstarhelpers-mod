/*using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader.IO;


namespace HamstarHelpers.Components.Players {
	internal class PlayerDataManager {
		internal readonly IDictionary<Guid, PlayerData>[] Data;


		////////////////

		internal PlayerDataManager() {
			IEnumerable<Type> subclasses = ReflectionHelpers.GetAllAvailableSubTypes( typeof( PlayerData ) );

			this.Data = new IDictionary<Guid, PlayerData>[ Main.player.Length ];

			for( int i = 0; i < Main.player.Length; i++ ) {
				this.Data[i] = new Dictionary<Guid, PlayerData>();
			}

			foreach( var subclass in subclasses ) {
				for( int i = 0; i < Main.player.Length; i++ ) {
					var data = (PlayerData)Activator.CreateInstance( subclass );
					data.MyPlayer = i;

					this.Data[ i ][ subclass.GUID ] = data;
				}
			}
		}
	}



	abstract public partial class PlayerData {
		public static T GetData<T>( Player player ) where T : PlayerData {
			return PlayerData.GetData<T>( player.whoAmI );
		}

		public static T GetData<T>( int playerWho ) where T : PlayerData {
			var datas = ModHelpersMod.Instance.PlayerDataMngr.Data[ playerWho ];
			Type t = typeof( T );
			PlayerData data = null;

			datas.TryGetValue( t.GUID, out data );
			return (T)data;
		}


		////////////////

		internal static void LoadAll( int playerWho, TagCompound tags ) {
			Player player = Main.player[ playerWho ];
			if( player == null || !player.active ) {
				LogHelpers.Log( "PlayerData.LoadAll - Player id'd "+playerWho+" could not load their data." );
				return;
			}

			
			var datas = ModHelpersMod.Instance.PlayerDataMngr.Data[ playerWho ];

			foreach( var data in datas.Values ) {
				string tagsName = data.GetType().Name;
				if( !tags.ContainsKey(tagsName) ) { continue; }

				var newTags = tags.GetCompound( tagsName );
				data.Load( newTags );
			}
		}

		internal static void SaveAll( int playerWho, TagCompound tags ) {
			var datas = ModHelpersMod.Instance.PlayerDataMngr.Data[ playerWho ];

			foreach( var data in datas.Values ) {
				string tagsName = data.GetType().Name;
				var newTags = new TagCompound();

				data.Save( newTags );

				tags[ tagsName ] = newTags;
			}
		}



		////////////////

		internal int MyPlayer = -1;
		public Player Player => Main.player[ this.MyPlayer ];



		////////////////
		
		abstract protected void Load( TagCompound tags );
		
		abstract protected void Save( TagCompound tags );
	}
}*/
