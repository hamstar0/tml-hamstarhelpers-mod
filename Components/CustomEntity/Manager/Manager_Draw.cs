using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.XnaHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity {
	public partial class CustomEntityManager {
		private static void _PostDrawAll( GameTime _ ) {
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
			entMngr.PostDrawAll( sb );
			if( !isBegun ) {
				sb.End();
			}
		}


		internal void PreDrawAll( SpriteBatch sb ) {
			CustomEntity[] ents;
			lock( CustomEntityManager.MyLock ) {
				ents = this.WorldEntitiesByIndexes.Values.ToArray();
			}

			foreach( CustomEntity ent in ents ) {
				var drawComp = ent.GetComponentByType<DrawsInGameEntityComponent>();
				if( drawComp != null ) {
					drawComp.PreDraw( sb, ent );
				}
			}
		}

		internal void DrawAll( SpriteBatch sb ) {
			CustomEntity[] ents;
			lock( CustomEntityManager.MyLock ) {
				ents = this.WorldEntitiesByIndexes.Values.ToArray();
			}

			foreach( CustomEntity ent in ents ) {
				var drawComp = ent.GetComponentByType<DrawsInGameEntityComponent>();
				if( drawComp != null ) {
					drawComp.Draw( sb, ent );
				}
			}
		}


		private void PostDrawAll( SpriteBatch sb ) {
			CustomEntity[] ents;
			lock( CustomEntityManager.MyLock ) {
				ents = this.WorldEntitiesByIndexes.Values.ToArray();
			}

			foreach( CustomEntity ent in ents ) {
				var drawComp = ent.GetComponentByType<DrawsInGameEntityComponent>();
				if( drawComp != null ) {
					drawComp.PostDraw( Main.spriteBatch, ent );
				}
			}
		}
	}
}
