using HamstarHelpers.Services.Hooks.LoadHooks;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Services.AnimatedTexture {
	class AnimatedTextureManager {
		internal IList<AnimatedTexture> Defs = new List<AnimatedTexture>();
		private Func<bool> OnTickGet;



		////////////////

		internal AnimatedTextureManager() {
			var mymod = ModHelpersMod.Instance;

			if( !Main.dedServ ) {
				this.OnTickGet = Timers.Timers.MainOnTickGet();
				Main.OnTick += AnimatedTextureManager._Update;
			}
		}

		internal void OnPostSetupContent() {
			if( !Main.dedServ ) {
				LoadHooks.AddModUnloadHook( () => {
					Main.OnTick -= AnimatedTextureManager._Update;
				} );
			}
		}


		////////////////

		public void AddAnimation( AnimatedTexture animation ) {
			this.Defs.Add( animation );
		}


		////////////////

		private static void _Update() {
			var mymod = ModHelpersMod.Instance;
			if( mymod == null || mymod.AnimatedTextures == null ) { return; }

			if( mymod.AnimatedTextures.OnTickGet() ) {
				mymod.AnimatedTextures.Update();
			}
		}


		internal void Update() {
			foreach( var def in this.Defs ) {
				def.AdvanceFrame();
			}
		}
	}
}
