using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Services.Hooks.LoadHooks;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Services.AnimatedTexture {
	class AnimatedTextureManager {
		internal IList<AnimatedTexture> Animations = new List<AnimatedTexture>();

		private Func<bool> OnTickGet;



		////////////////

		internal AnimatedTextureManager() {
			var mymod = ModHelpersMod.Instance;

			if( !Main.dedServ ) {
				this.OnTickGet = Timers.Timers.MainOnTickGet();
				Main.OnTick += AnimatedTextureManager._Update;
			}
		}

		////

		public void OnPostModsLoad() {
			if( !Main.dedServ ) {
				LoadHooks.AddWorldUnloadEachHook( () => {
					this.Animations.Clear();
				} );
				LoadHooks.AddModUnloadHook( () => {
					Main.OnTick -= AnimatedTextureManager._Update;
				} );
			}
		}


		////////////////

		public void AddAnimation( AnimatedTexture animation ) {
			this.Animations.Add( animation );
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
			if( Main.gameMenu ) {
				return;
			}

			foreach( var def in this.Animations ) {
				def.AdvanceFrame();
			}
		}
	}
}
