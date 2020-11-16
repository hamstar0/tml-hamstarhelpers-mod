using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET;
using HamstarHelpers.Helpers.TModLoader;


namespace HamstarHelpers.Classes.Loadable {
	class LoadableManager {
		private ISet<ILoadable> Loadables = new HashSet<ILoadable>();



		////////////////

		public LoadableManager() { }


		public void OnModsLoad() {
			Type iLoadableType = typeof( ILoadable );
			IEnumerable<Assembly> asses = ModLoader.Mods
				.SafeSelect( mod => mod.Code )
				.SafeWhere( code => code != null );

			foreach( Assembly ass in asses ) {
				foreach( Type classType in ass.GetTypes() ) {
					try {
						if( !classType.IsClass || classType.IsAbstract ) {
							continue;
						}

						if( !typeof(ILoadable).IsAssignableFrom(classType) ) {
							continue;
						}

						var loadable = TmlHelpers.SafelyGetInstanceForType(classType) as ILoadable;
						if( loadable == null ) {
							continue;
						}

						this.Loadables.Add( loadable );
					} catch { }
				}
			}
			
			foreach( ILoadable loadable in this.Loadables ) {
				loadable.OnModsLoad();
				//object _;
				//if( !ReflectionHelpers.RunMethod( loadable.GetType(), loadable, "OnModsLoad", new object[] { }, out _ ) ) {
				//	throw new ModHelpersException( "Could not call OnModsLoad for "+loadable.GetType() );
				//}
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
