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
			var sb = Main.spriteBatch;
			bool is_begun = XnaHelpers.IsMainSpriteBatchBegun();

			if( !is_begun ) {
				sb.Begin( SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.instance.Rasterizer, null, Main.BackgroundViewMatrix.TransformationMatrix );
			}
			ModHelpersMod.Instance?.CustomEntMngr?.PostDrawAll( sb );
			if( !is_begun ) {
				sb.End();
			}
		}


		internal void PreDrawAll( SpriteBatch sb ) {
			CustomEntity[] ents;
			lock( CustomEntityManager.MyLock ) {
				ents = this.EntitiesByIndexes.Values.ToArray();
			}

			foreach( CustomEntity ent in ents ) {
				var draw_comp = ent.GetComponentByType<DrawsInGameEntityComponent>();
				if( draw_comp != null ) {
					draw_comp.PreDraw( Main.spriteBatch, ent );
				}
			}
		}

		internal void DrawAll( SpriteBatch sb ) {
			CustomEntity[] ents;
			lock( CustomEntityManager.MyLock ) {
				ents = this.EntitiesByIndexes.Values.ToArray();
			}

			foreach( CustomEntity ent in ents ) {
				var draw_comp = ent.GetComponentByType<DrawsInGameEntityComponent>();
				if( draw_comp != null ) {
					draw_comp.Draw( sb, ent );
				}
			}
		}


		private void PostDrawAll( SpriteBatch sb ) {
			CustomEntity[] ents;
			lock( CustomEntityManager.MyLock ) {
				ents = this.EntitiesByIndexes.Values.ToArray();
			}

			foreach( CustomEntity ent in ents ) {
				var draw_comp = ent.GetComponentByType<DrawsInGameEntityComponent>();
				if( draw_comp != null ) {
					draw_comp.PostDraw( Main.spriteBatch, ent );
				}
			}
		}
	}
}
