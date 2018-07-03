using Terraria;


namespace HamstarHelpers.Components.CustomEntity {
	public class IsItemEntityProperty : CustomEntityProperty {
		public override void Update( CustomEntity ent ) {
			Player player = Main.LocalPlayer;

			if( ent.Hitbox.Intersects( player.Hitbox ) ) {
			}
		}
	}
}
