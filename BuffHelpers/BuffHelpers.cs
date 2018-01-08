using System.Collections.Generic;
using System.Collections.ObjectModel;
using Terraria;


namespace HamstarHelpers.BuffHelpers {
	public class BuffHelpers {
		public static IReadOnlyDictionary<string, int> BuffIdsByName { get {
			return HamstarHelpersMod.Instance.BuffHelpers._BuffIdsByName;
		} }


		////////////////

		public static void AddPermaBuff( Player player, int buff_id ) {
			var modplayer = player.GetModPlayer<HamstarHelpersPlayer>();
			modplayer.PermaBuffsById.Add( buff_id );
		}

		public static void RemovePermaBuff( Player player, int buff_id ) {
			var modplayer = player.GetModPlayer<HamstarHelpersPlayer>();
			if( modplayer.PermaBuffsById.Contains(buff_id) ) {
				modplayer.PermaBuffsById.Remove( buff_id );
			}
		}



		////////////////

		private IDictionary<string, int> __buffIdsByName = new Dictionary<string, int>();
		private IReadOnlyDictionary<string, int> _BuffIdsByName;


		////////////////

		internal BuffHelpers() {
			this._BuffIdsByName = new ReadOnlyDictionary<string, int>( this.__buffIdsByName );
		}

		internal void OnPostSetupContent() {
			for( int i = 0; i < Main.buffTexture.Length; i++ ) {
				this.__buffIdsByName[Lang.GetBuffName( i )] = i;
			}
		}
	}
}
