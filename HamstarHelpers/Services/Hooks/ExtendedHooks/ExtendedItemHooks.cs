﻿using System;
using Terraria;
using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader;


namespace HamstarHelpers.Services.Hooks.ExtendedHooks {
	/// <summary>
	/// Supplies custom tModLoader-like, delegate-based hooks for item-relevant functions not currently available in
	/// tModLoader.
	/// </summary>
	public partial class ExtendedItemHooks : ILoadable {
		/// <summary>
		/// Allows binding actions to NPC loot deaths, with knowledge of loots dropped.
		/// </summary>
		/// <param name="hook"></param>
		public static void AddNPCLootHook( NPCLootDelegate hook ) {
			var eih = TmlHelpers.SafelyGetInstance<ExtendedItemHooks>();

			eih.OnNPCLootHooks.Add( hook );
		}
	}
}
