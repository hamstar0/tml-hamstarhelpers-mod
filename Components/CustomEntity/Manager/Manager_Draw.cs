using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.XnaHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity {
	public partial class CustomEntityManager {
		private static void _DrawPostDrawAll( GameTime _ ) {
			var mymod = ModHelpersMod.Instance;
			if( mymod == null ) { return; }
			var entMngr = mymod.CustomEntMngr;
			if( entMngr == null ) { return; }
			if( entMngr.WorldEntitiesByIndexes.Count == 0 ) { return; }

			var sb = Main.spriteBatch;
			bool isBegun;
			if( !XnaHelpers.IsMainSpriteBatchBegun( out isBegun ) ) { return; }

			if( !isBegun ) {
				sb.Begin( SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.instance.Rasterizer, null, Main.BackgroundViewMatrix.TransformationMatrix );
			}
			entMngr.DrawPostDrawAll( sb );
			if( !isBegun ) {
				sb.End();
			}
		}


		internal void DrawOverlayAll( SpriteBatch sb ) {
			CustomEntity[] ents;
			lock( CustomEntityManager.MyLock ) {
				ents = this.WorldEntitiesByIndexes.Values.ToArray();
			}

			foreach( CustomEntity ent in ents ) {
				var drawComp = ent.GetComponentByType<DrawsInGameEntityComponent>();
				if( drawComp == null ) { continue; }

				Effect fx = drawComp.GetFx( ent );

				if( fx != null ) {
					sb.End();
					sb.Begin( SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.instance.Rasterizer, fx, Main.Transform );
					//fx.CurrentTechnique.Passes[0].Apply();
				}

				drawComp.DrawOverlay( sb, ent );

				if( fx != null ) {
					sb.End();
					sb.Begin( SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.instance.Rasterizer, null, Main.Transform );
				}
			}
		}

		internal void DrawPostTilesAll( SpriteBatch sb ) {
			CustomEntity[] ents;
			lock( CustomEntityManager.MyLock ) {
				ents = this.WorldEntitiesByIndexes.Values.ToArray();
			}

			foreach( CustomEntity ent in ents ) {
				var drawComp = ent.GetComponentByType<DrawsInGameEntityComponent>();
				if( drawComp != null ) {
					drawComp.DrawPostTiles( sb, ent );
				}
			}
		}


		private void DrawPostDrawAll( SpriteBatch sb ) {
			CustomEntity[] ents;
			lock( CustomEntityManager.MyLock ) {
				ents = this.WorldEntitiesByIndexes.Values.ToArray();
			}

			foreach( CustomEntity ent in ents ) {
				var drawComp = ent.GetComponentByType<DrawsInGameEntityComponent>();
				if( drawComp != null ) {
					drawComp.DrawPostDraw( Main.spriteBatch, ent );
				}
			}
		}
	}
}
