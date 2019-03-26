using System;
using System.Collections.Generic;
using Terraria.GameInput;
using Terraria.ModLoader;


namespace HamstarHelpers.Services.CustomHotkeys {
	public partial class CustomHotkeys {
		private readonly ModHotKey Key1;
		private readonly ModHotKey Key2;

		private readonly IDictionary<string, Action> Key1Actions = new Dictionary<string, Action>();
		private readonly IDictionary<string, Action> Key2Actions = new Dictionary<string, Action>();



		////////////////

		internal CustomHotkeys() {
			this.Key1 = ModHelpersMod.Instance.RegisterHotKey( "Custom Hotkey 1", "K" );
			this.Key2 = ModHelpersMod.Instance.RegisterHotKey( "Custom Hotkey 2", "L" );
		}

		////////////////

		public void ProcessTriggers( TriggersSet triggersSet ) {
			if( this.Key1.JustPressed ) {
				foreach( Action act in this.Key1Actions.Values ) {
					act();
				}
			}
			if( this.Key2.JustPressed ) {
				foreach( Action act in this.Key2Actions.Values ) {
					act();
				}
			}
		}
	}
}
