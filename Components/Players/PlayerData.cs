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

		public static T GetData<T>( int player_who ) where T : PlayerData {
			var datas = ModHelpersMod.Instance.PlayerDataMngr.Data[ player_who ];
			Type t = typeof( T );
			PlayerData data = null;

			datas.TryGetValue( t.GUID, out data );
			return (T)data;
		}


		////////////////

		internal static void LoadAll( int player_who, TagCompound tags ) {
			var datas = ModHelpersMod.Instance.PlayerDataMngr.Data[ player_who ];

			foreach( var data in datas.Values ) {
				string tags_name = data.GetType().Name;
				if( !tags.ContainsKey(tags_name) ) { continue; }

				var new_tags = tags.GetCompound( tags_name );
				data.Load( new_tags );
			}
		}

		internal static void SaveAll( int player_who, TagCompound tags ) {
			var datas = ModHelpersMod.Instance.PlayerDataMngr.Data[ player_who ];

			foreach( var data in datas.Values ) {
				string tags_name = data.GetType().Name;
				var new_tags = new TagCompound();

				data.Save( new_tags );

				tags[ tags_name ] = new_tags;
			}
		}



		////////////////

		internal int MyPlayer = -1;
		public Player Player => Main.player[ this.MyPlayer ];



		////////////////
		
		abstract protected void Load( TagCompound tags );
		
		abstract protected void Save( TagCompound tags );
	}
}
