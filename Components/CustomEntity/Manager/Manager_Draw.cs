using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.XnaHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
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
			
			bool __;
			XnaHelpers.DrawBatch(
				(sb) => {
					var mymod2 = ModHelpersMod.Instance;
					mymod2.CustomEntMngr.DrawPostDrawAll( sb );
				},
				SpriteSortMode.Deferred,
				BlendState.AlphaBlend,
				Main.DefaultSamplerState,
				DepthStencilState.None,
				Main.instance.Rasterizer,
				null,
				Main.BackgroundViewMatrix.TransformationMatrix,
				out __
			);
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
