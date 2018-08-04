using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Helpers.DebugHelpers;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers {
	partial class HamstarHelpersMod : Mod {
		private void DrawMiniMapForAll( SpriteBatch sb ) {
			ISet<CustomEntity> ents = CustomEntityManager.GetByComponentType<DrawsOnMapEntityComponent>();

			foreach( var ent in ents ) {
				var map_comp = ent.GetComponentByType<DrawsOnMapEntityComponent>();

				if( Main.mapStyle == 1 ) {
					map_comp.DrawMiniMap( sb, ent );
				} else {
					map_comp.DrawOverlayMap( sb, ent );
				}
			}
		}


		private void DrawFullMapForAll( SpriteBatch sb ) {
			ISet<CustomEntity> ents = CustomEntityManager.GetByComponentType<DrawsOnMapEntityComponent>();

			foreach( var ent in ents ) {
				var map_comp = ent.GetComponentByType<DrawsOnMapEntityComponent>();

				map_comp.DrawFullscreenMap( sb, ent );
			}
		}
	}
}
