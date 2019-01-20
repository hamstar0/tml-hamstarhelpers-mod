using HamstarHelpers.Helpers.DebugHelpers;
using Microsoft.Xna.Framework;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity.Components {
	public abstract class IsClickableEntityComponent : CustomEntityComponent {
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
			var worldScrRect = new Rectangle( (int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth, Main.screenHeight );
			Rectangle box = core.Hitbox;

			if( !box.Intersects( worldScrRect ) ) {
				return false;
			}

			var screenBox = new Rectangle( box.X - worldScrRect.X, box.Y - worldScrRect.Y, box.Width, box.Height );

			return screenBox.Contains( Main.mouseX, Main.mouseY );
		}
	}
}
