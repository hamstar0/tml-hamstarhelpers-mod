using HamstarHelpers.Helpers;
using HamstarHelpers.ItemHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace HamstarHelpers.MiscHelpers {
	public static partial class MiscHelpers {
		[System.Obsolete( "use ItemAttributeHelpers.GetRarityColor", true )]
		public static Color GetRarityColor( int rarity ) {
			return ItemAttributeHelpers.GetRarityColor( rarity );
		}
	}




	[System.Obsolete( "use DotNetHelpers.DotNetHelpers", true )]
	public static class DotNetHelpers {
		public static string DictToString( IDictionary<object, object> dict ) {
			return global::HamstarHelpers.DotNetHelpers.DotNetHelpers.DictToString( dict );
		}
	}



	[System.Obsolete( "use DebugHelpers.DebugHelpers", true )]
	public static class DebugHelpers {
		public static bool Once {
			get { return global::HamstarHelpers.DebugHelpers.DebugHelpers.Once; }
			set { global::HamstarHelpers.DebugHelpers.DebugHelpers.Once = value; }
		}
		public static int OnceInAWhile {
			get { return global::HamstarHelpers.DebugHelpers.DebugHelpers.OnceInAWhile; }
			set { global::HamstarHelpers.DebugHelpers.DebugHelpers.OnceInAWhile = value; }
		}
		public static IDictionary<string, string> Display {
			get { return global::HamstarHelpers.DebugHelpers.DebugHelpers.Display; }
			set { global::HamstarHelpers.DebugHelpers.DebugHelpers.Display = value; }
		}
		public static void MsgOnce( string msg ) {
			global::HamstarHelpers.DebugHelpers.DebugHelpers.MsgOnce( msg );
		}
		public static void MsgOnceInAWhile( string msg ) {
			global::HamstarHelpers.DebugHelpers.DebugHelpers.MsgOnceInAWhile( msg );
		}
		public static void PrintToBatch( SpriteBatch sb ) {
			global::HamstarHelpers.DebugHelpers.DebugHelpers.PrintToBatch( sb );
		}
		public static void Log( string msg ) {
			global::HamstarHelpers.DebugHelpers.DebugHelpers.Log( msg );
		}
	}
}
