using Terraria.ModLoader.IO;


namespace HamstarHelpers.Internals.Logic {
	partial class WorldLogic {
		public WorldLogic( HamstarHelpersMod mymod ) { }
		

		////////////////
		
		public void LoadForWorld( HamstarHelpersMod mymod, TagCompound tags ) {
			mymod.WorldHelpers.Load( mymod, tags );
		}

		public void SaveForWorld( HamstarHelpersMod mymod, TagCompound tags ) {
			mymod.WorldHelpers.Save( mymod, tags );
		}
	}
}
