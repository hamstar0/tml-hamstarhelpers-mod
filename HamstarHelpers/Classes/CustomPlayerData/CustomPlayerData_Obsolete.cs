using System;
using Terraria;
using HamstarHelpers.Classes.Loadable;


namespace HamstarHelpers.Classes.PlayerData {
	/// <summary>
	/// An alternative to ModPlayer for basic per-player, per-game data storage and Update use.
	/// </summary>
	public partial class CustomPlayerData : ILoadable {
		/// @private
		[Obsolete("use `OnEnter(bool, object)`", true)]
		protected virtual void OnEnter( object data ) { }
	}
}
