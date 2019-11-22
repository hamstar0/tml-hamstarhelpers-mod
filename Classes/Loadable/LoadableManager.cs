using HamstarHelpers.Helpers.DotNET;
using HamstarHelpers.Helpers.DotNET.Reflection;
using HamstarHelpers.Helpers.TModLoader;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;


namespace HamstarHelpers.Classes.Loadable {
	class LoadableManager {
		private ISet<ILoadable> Loadables = new HashSet<ILoadable>();



		////////////////

		public LoadableManager() { }


		public void OnModsLoad() {
			IEnumerable<Assembly> asses = ModLoader.Mods
				.SafeSelect( mod => mod.GetType().Assembly );

			foreach( Assembly ass in asses ) {
				foreach( Type classType in ass.GetTypes() ) {
					try {
						Type iloadableType = classType.GetInterface( "ILoadable" );
						if( iloadableType == null ) {
							continue;
						}

						var loadable = (ILoadable)TmlHelpers.SafelyGetInstance( classType );
						if( loadable == null ) {
							continue;
						}

						this.Loadables.Add( loadable );
					} catch { }
				}
			}

			foreach( ILoadable loadable in this.Loadables ) {
				object _;
				ReflectionHelpers.RunMethod( loadable.GetType(), loadable, "OnModsLoad", new object[] { }, out _ );
			}
		}


		public void OnPostModsLoad() {
			foreach( ILoadable loadable in this.Loadables ) {
				loadable.OnPostModsLoad();
			}
		}

		public void OnModsUnload() {
			foreach( ILoadable loadable in this.Loadables ) {
				loadable.OnModsUnload();
			}
		}
	}
}
