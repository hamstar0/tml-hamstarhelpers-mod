using HamstarHelpers.TmlHelpers;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Utilities.EntityGroups {
	public partial class EntityGroups {
		public static IDictionary<string, ISet<int>> ItemGroups {
			get {
				return HamstarHelpersMod.Instance.EntityGroups._ItemGroups;
			}
		}
		public static IDictionary<string, ISet<int>> NPCGroups {
			get {
				return HamstarHelpersMod.Instance.EntityGroups._NPCGroups;
			}
		}
		public static IDictionary<string, ISet<int>> ProjectileGroups {
			get {
				return HamstarHelpersMod.Instance.EntityGroups._ProjGroups;
			}
		}



		////////////////

		private IDictionary<string, ISet<int>> _ItemGroups = new Dictionary<string, ISet<int>>();
		private IDictionary<string, ISet<int>> _NPCGroups = new Dictionary<string, ISet<int>>();
		private IDictionary<string, ISet<int>> _ProjGroups = new Dictionary<string, ISet<int>>();

		private IDictionary<string, Func<Item, bool>> ItemMatchers = new Dictionary<string, Func<Item, bool>>();
		private IDictionary<string, Func<NPC, bool>> NPCMatchers = new Dictionary<string, Func<NPC, bool>>();
		private IDictionary<string, Func<Projectile, bool>> ProjMatchers = new Dictionary<string, Func<Projectile, bool>>();


		////////////////

		internal EntityGroups() {
			this.DefinePrimaryItemGroups();

			TmlLoadHelpers.AddPostModLoadPromise( () => {
				this.ComputeItemGroups();
				this.ComputeNPCGroups();
				this.ComputeProjectileGroups();
			} );
		}
	}
}
