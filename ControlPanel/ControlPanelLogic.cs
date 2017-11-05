using HamstarHelpers.TmlHelpers;
using System.Collections.Generic;
using Terraria.ModLoader;


namespace HamstarHelpers.ControlPanel {
	class ControlPanelLogic {
		public Mod CurrentMod = null;


		////////////////

		public ControlPanelLogic() { }

		////////////////

		public ISet<Mod> GetMods() {
			ISet<Mod> mods = new HashSet<Mod>();	// TODO: Implement ordered set

			mods.Add( HamstarHelpersMod.Instance );

			foreach( var mod in ExtendedModManager.ExtendedMods ) {
				if( mod == HamstarHelpersMod.Instance || mod.File == null ) { continue; }
				mods.Add( mod );
			}

			foreach( var mod in ModLoader.LoadedMods ) {
				if( mods.Contains( mod ) || mod.File == null ) { continue; }
				mods.Add( mod );
			}

			return mods;
		}

		////////////////

		public void SetCurrentMod( Mod mod ) {
			this.CurrentMod = mod;
		}
	}
}
