using HamstarHelpers.Helpers.Debug;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;


namespace HamstarHelpers.Helpers.XNA {
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

		public static bool DrawBatch( Action<SpriteBatch> draw, out bool isBegun, bool forceDraw=true ) {
			if( !XnaHelpers.IsMainSpriteBatchBegun( out isBegun ) ) {
				return false; // take no chances
			}

			if( !isBegun ) {
				Main.spriteBatch.Begin();

				try {
					draw( Main.spriteBatch );
				} catch( Exception e ) {
					LogHelpers.WarnOnce( e.ToString() );
				}
				
				Main.spriteBatch.End();
			} else {
				if( forceDraw ) {
					LogHelpers.WarnOnce( DebugHelpers.GetCurrentContext( 2 ) + " - SpriteBatch already begun. Drawing anyway..." );
					try {
						draw( Main.spriteBatch );
					} catch( Exception e ) {
						LogHelpers.WarnOnce( e.ToString() );
					}
				}
			}
			
			return true;
		}


		public static bool DrawBatch( Action<SpriteBatch> draw,
				SpriteSortMode sortMode,
				BlendState blendState,
				SamplerState samplerState,
				DepthStencilState depthStencilState,
				RasterizerState rasterizerState,
				Effect effect,
				Matrix transformMatrix,
				out bool isBegun,
				bool forceBeginAnew = false,
				bool forceDraw = true ) {
			if( !XnaHelpers.IsMainSpriteBatchBegun( out isBegun ) ) {
				return false; // take no chances
			}

			if( !isBegun ) {
				Main.spriteBatch.Begin( sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, transformMatrix );

				try {
					draw( Main.spriteBatch );
				} catch( Exception e ) {
					LogHelpers.WarnOnce( e.ToString() );
				}

				Main.spriteBatch.End();
			} else {
				if( forceDraw ) {
					LogHelpers.WarnOnce( DebugHelpers.GetCurrentContext( 2 ) + " - SpriteBatch already begun. Drawing anyway..." );
					try {
						draw( Main.spriteBatch );
					} catch( Exception e ) {
						LogHelpers.WarnOnce( e.ToString() );
					}
				}
			}

			return true;
		}
	}
}
