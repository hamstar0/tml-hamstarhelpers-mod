using HamstarHelpers.Helpers.DotNetHelpers;
using Microsoft.Xna.Framework;
using System;
using System.Reflection;
using Terraria;


namespace HamstarHelpers.Helpers.XnaHelpers {
	public class XnaHelpers {
		public static void ScanRectangleWithout( Func<int, int, bool> scanner, Rectangle rect, Rectangle notrect ) {
			int i, j;

			for( i=rect.X; i<(rect.X+rect.Width); i++ ) {
				for( j=rect.Y; j<(rect.Y+rect.Height); j++ ) {
					if( i > notrect.X && i <= (notrect.X+notrect.Width) ) {
						if( j > notrect.Y && j <= (notrect.Y + notrect.Height) ) {
							i = notrect.X + notrect.Width;
							if( i >= (rect.X+rect.Width) ) { break; }
						}
					}
					
					if( !scanner(i, j) ) { return; }
				}
			}
		}


		public static bool IsMainSpriteBatchBegun() {
			if( ModHelpersMod.Instance?.XnaHelpers?.MainSpriteBatchBegun == null ) {
				return false;
			}
			return (bool)ModHelpersMod.Instance.XnaHelpers.MainSpriteBatchBegun.GetValue( Main.spriteBatch );
		}



		////////////////

		private FieldInfo MainSpriteBatchBegun = null;



		////////////////

		internal XnaHelpers() {
			ReflectionHelpers.GetField( Main.spriteBatch, "_beginCalled",
				BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
				out this.MainSpriteBatchBegun
			);
		}
	}
}
