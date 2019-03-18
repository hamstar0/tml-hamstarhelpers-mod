using HamstarHelpers.Helpers.DebugHelpers;
using Microsoft.Xna.Framework;
using System;
using Terraria;


namespace HamstarHelpers.Helpers.XnaHelpers {
	public partial class XnaHelpers {
		public static void ScanRectangleWithout( Func<int, int, bool> scanner, Rectangle rect, Rectangle notrect ) {
			int i, j;

			for( i = rect.X; i < ( rect.X + rect.Width ); i++ ) {
				for( j = rect.Y; j < ( rect.Y + rect.Height ); j++ ) {
					if( i > notrect.X && i <= ( notrect.X + notrect.Width ) ) {
						if( j > notrect.Y && j <= ( notrect.Y + notrect.Height ) ) {
							i = notrect.X + notrect.Width;
							if( i >= ( rect.X + rect.Width ) ) { break; }
						}
					}

					if( !scanner( i, j ) ) { return; }
				}
			}
		}

		////////////////

		[Obsolete( "use IsMainSpriteBatchBegun(out bool)" )]
		public static bool IsMainSpriteBatchBegun() {
			return (bool)ModHelpersMod.Instance?.XnaHelpers?.SpriteBatchBegunField?.GetValue( Main.spriteBatch );
		}

		public static bool IsMainSpriteBatchBegun( out bool isBegun ) {
			var mymod = ModHelpersMod.Instance;
			object isBegunRaw = mymod?.XnaHelpers?.SpriteBatchBegunField?.GetValue( Main.spriteBatch );

			if( isBegunRaw != null ) {
				isBegun = (bool)isBegunRaw;
				return true;
			} else {
				isBegun = false;
				return false;
			}
		}

		////////////////

		public static 
	}
}
