using HamstarHelpers.Helpers.DebugHelpers;
using Microsoft.Xna.Framework;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity.Components {
	abstract public class IsClickableEntityComponent : CustomEntityComponent {
		private void UpdateMe( CustomEntity ent ) {
			if( this.CheckMouseHover( ent ) ) {
				this.OnMouseHover( ent );
			}
		}

		public override void UpdateSingle( CustomEntity ent ) {
			this.UpdateMe( ent );
		}
		public override void UpdateClient( CustomEntity ent ) {
			this.UpdateMe( ent );
		}

		////////////////

		protected abstract void OnMouseHover( CustomEntity ent );


		////////////////

		private bool CheckMouseHover( CustomEntity ent ) {
			Entity core = ent.Core;
			var world_scr_rect = new Rectangle( (int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth, Main.screenHeight );
			Rectangle box = core.Hitbox;
			if( !box.Intersects( world_scr_rect ) ) {
				return false;
			}

			var screen_box = new Rectangle( box.X - world_scr_rect.X, box.Y - world_scr_rect.Y, box.Width, box.Height );

			return screen_box.Contains( Main.mouseX, Main.mouseY );
		}
	}
}
