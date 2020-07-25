using HamstarHelpers.Helpers.Debug;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;


namespace HamstarHelpers.Helpers.XNA {
	/// <summary>
	/// Assorted static "helper" functions pertaining to XNA. 
	/// </summary>
	public partial class XNAHelpers {
		/// <summary>
		/// "Scans" a rectangle with a provided custom function, returning on the function reporting success. Skips over
		/// another given rectangle, if overlapping.
		/// </summary>
		/// <param name="scanner"></param>
		/// <param name="rect"></param>
		/// <param name="notRect"></param>
		public static void ScanRectangleWithout( Func<int, int, bool> scanner, Rectangle rect, Rectangle? notRect ) {
			int i, j;
			Rectangle nRect = notRect.HasValue ? notRect.Value : default(Rectangle);

			for( i = rect.X; i < (rect.X + rect.Width); i++ ) {
				for( j = rect.Y; j < (rect.Y + rect.Height); j++ ) {
					if( notRect.HasValue ) {
						if( i > nRect.X && i <= (nRect.X + nRect.Width) ) {
							if( j > nRect.Y && j <= (nRect.Y + nRect.Height) ) {
								j = nRect.Y + nRect.Height;

								if( j >= (rect.Y + rect.Height) ) {
									break;
								}
							}
						}
					}

					if( !scanner( i, j ) ) { return; }
				}
			}
		}

		////////////////

		/// <summary>
		/// Reports if the given SpriteBatch has begun drawing.
		/// </summary>
		/// <param name="sb"></param>
		/// <param name="isBegun"></param>
		/// <returns>If `false`, could not determine one way or the other.</returns>
		public static bool IsSpriteBatchBegun( SpriteBatch sb, out bool isBegun ) {
			var mymod = ModHelpersMod.Instance;
			object isBegunRaw = mymod?.XnaHelpers?.SpriteBatchBegunField?.GetValue( sb );

			if( isBegunRaw != null ) {
				isBegun = (bool)isBegunRaw;
				return true;
			} else {
				isBegun = false;
				return false;
			}
		}

		/// <summary>
		/// Reports if `Main.SpriteBatch` has begun drawing.
		/// </summary>
		/// <param name="isBegun"></param>
		/// <returns>If `false`, could not determine one way or the other.</returns>
		public static bool IsMainSpriteBatchBegun( out bool isBegun ) {
			return XNAHelpers.IsSpriteBatchBegun( Main.spriteBatch, out isBegun );
		}


		////////////////

		/// <summary>
		/// Draws to the main SpriteBatch by way of a callback. Attempts to resolve when to draw based on the SpriteBatch's
		/// `Begun()` state.
		/// </summary>
		/// <param name="draw"></param>
		/// <param name="isBegun">Indicates that the SpriteBatch was already `Begun()`.</param>
		/// <param name="forceDraw">Forces drawing even when the SpriteBatch is already `Begun()`.</param>
		/// <returns>`true` if no issues occurred with the drawing.</returns>
		public static bool DrawBatch( Action<SpriteBatch> draw, out bool isBegun, bool forceDraw=true ) {
			if( !XNAHelpers.IsMainSpriteBatchBegun( out isBegun ) ) {
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


		/// <summary>
		/// Draws to the main SpriteBatch by way of a callback. Attempts to resolve when to draw based on the SpriteBatch's
		/// `Begun()` state. If the SpriteBatch needs to be begun anew, the given set of relevant parameters will be applied.
		/// </summary>
		/// <param name="draw"></param>
		/// <param name="sortMode"></param>
		/// <param name="blendState"></param>
		/// <param name="samplerState"></param>
		/// <param name="depthStencilState"></param>
		/// <param name="rasterizerState"></param>
		/// <param name="effect"></param>
		/// <param name="transformMatrix"></param>
		/// <param name="isBegun">Indicates that the SpriteBatch was already `Begun()`.</param>
		/// <param name="forceBeginAnew">Forces the SpriteBatch to `Begin()`.</param>
		/// <param name="forceDraw">Forces drawing even wehn the SpriteBatch is already `Begun()`.</param>
		/// <returns></returns>
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
			if( !XNAHelpers.IsMainSpriteBatchBegun( out isBegun ) ) {
				return false; // take no chances
			}

			if( isBegun && forceBeginAnew ) {
				isBegun = false;
				Main.spriteBatch.End();
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
