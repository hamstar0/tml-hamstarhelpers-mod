using System.Collections.Generic;
using System.Collections.ObjectModel;
using Terraria;


namespace HamstarHelpers.BuffHelpers {
	public class BuffIdentityHelpers {
		public static IReadOnlyDictionary<string, int> NamesToIds {
			get { return HamstarHelpersMod.Instance.BuffIdentityHelpers._NamesToIds; }
		}



		////////////////

		private IDictionary<string, int> __namesToIds = new Dictionary<string, int>();
		private IReadOnlyDictionary<string, int> _NamesToIds = null;


		////////////////

		internal void OnPostSetupContent() {
			this._NamesToIds = new ReadOnlyDictionary<string, int>( this.__namesToIds );

			for( int i = 1; i < Main.buffTexture.Length; i++ ) {
				string name = Lang.GetBuffName( i );
				this.__namesToIds[name] = i;
			}
		}
	}
}
