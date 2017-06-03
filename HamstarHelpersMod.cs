using HamstarHelpers.DisplayHelpers;
using HamstarHelpers.MiscHelpers;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;


namespace HamstarHelpers {
	public class HamstarHelpersMod : Mod {
		public HamstarHelpersMod() {
			this.Properties = new ModProperties() {
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};
		}
		
		public override void PostDrawInterface( SpriteBatch sb ) {
			PlayerMessage.DrawPlayerLabels( sb );
			SimpleMessage.DrawMessage( sb );

			DebugHelpers.PrintToBatch( sb );
			DebugHelpers.Once = false;
			DebugHelpers.OnceInAWhile--;
		}
	}
}
