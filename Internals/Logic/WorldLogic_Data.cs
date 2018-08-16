using Terraria.ModLoader.IO;


namespace HamstarHelpers.Internals.Logic {
	partial class WorldLogic {
		public WorldLogic( ModHelpersMod mymod ) { }
		

		////////////////
		
		public void LoadForWorld( ModHelpersMod mymod, TagCompound tags ) {
			mymod.WorldHelpers.Load( mymod, tags );
		}

		public void SaveForWorld( ModHelpersMod mymod, TagCompound tags ) {
			mymod.WorldHelpers.Save( mymod, tags );
		}
	}
}
