using Terraria.ModLoader.IO;


namespace HamstarHelpers.Internals.Logic {
	partial class WorldLogic {
		public WorldLogic() { }
		

		////////////////
		
		public void LoadForWorld( TagCompound tags ) {
			var mymod = ModHelpersMod.Instance;
			mymod.WorldStateHelpers.Load( tags );
		}

		public void SaveForWorld( TagCompound tags ) {
			var mymod = ModHelpersMod.Instance;
			mymod.WorldStateHelpers.Save( tags );
		}
	}
}
