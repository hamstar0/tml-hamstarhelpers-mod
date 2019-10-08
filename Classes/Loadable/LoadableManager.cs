using HamstarHelpers.Helpers.DotNET;
using HamstarHelpers.Helpers.DotNET.Reflection;
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

						ILoadable loadable = this.LoadLoadableSingleton( classType );
						if( loadable == null ) {
							continue;
						}

						this.Loadables.Add( loadable );
					} catch { }
				}
			}

			foreach( ILoadable loadable in this.Loadables ) {
				loadable.OnModsLoad();
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


		////////////////

		private ILoadable LoadLoadableSingleton( Type loadableClassType ) {
			MethodInfo method = typeof( ModContent ).GetMethod( "GetInstance" );
			MethodInfo genericMethod = method.MakeGenericMethod( loadableClassType );
			object rawInstance = genericMethod?.Invoke( null, new object[] { } );

			if( rawInstance != null ) {
				return (ILoadable)rawInstance;
			}

			var loadable = (ILoadable)Activator.CreateInstance( loadableClassType, ReflectionHelpers.MostAccess );
			ContentInstance.Register( loadable );

			return loadable;
		}
	}
}
