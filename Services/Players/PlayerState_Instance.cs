using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Services.Players {
	public partial class PlayerState {
		internal IDictionary<string, Action<Player, int>> BuffExpireHooks = new Dictionary<string, Action<Player, int>>();
		internal IDictionary<string, Action<Player, int, Item>> ArmorEquipHooks = new Dictionary<string, Action<Player, int, Item>>();
		internal IDictionary<string, Action<Player, int, int>> ArmorUnequipHooks = new Dictionary<string, Action<Player, int, int>>();
	}
}
