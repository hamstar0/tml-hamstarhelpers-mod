using Terraria;


namespace HamstarHelpers.Components.CustomEntity {
	public class IsItemEntityProperty : CustomEntityProperty {
		public override void Update( CustomEntity ent ) {
			Main.LocalPlayer.getRect();
		}
	}
}
