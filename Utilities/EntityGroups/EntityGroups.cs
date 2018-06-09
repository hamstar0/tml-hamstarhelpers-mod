using HamstarHelpers.DotNetHelpers.DataStructures;
using HamstarHelpers.TmlHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Terraria;


namespace HamstarHelpers.Utilities.EntityGroups {
	public partial class EntityGroups {
		private static object MyLock = new object();


		public static IReadOnlyDictionary<string, ReadOnlySet<int>> ItemGroups {
			get {
				lock( EntityGroups.MyLock ) {
					return HamstarHelpersMod.Instance.EntityGroups._ItemGroups;
				}
			}
		}
		public static IReadOnlyDictionary<string, ReadOnlySet<int>> NPCGroups {
			get {
				lock( EntityGroups.MyLock ) {
					return HamstarHelpersMod.Instance.EntityGroups._NPCGroups;
				}
			}
		}
		public static IReadOnlyDictionary<string, ReadOnlySet<int>> ProjectileGroups {
			get {
				lock( EntityGroups.MyLock ) {
					return HamstarHelpersMod.Instance.EntityGroups._ProjGroups;
				}
			}
		}



		////////////////

		private IReadOnlyDictionary<string, ReadOnlySet<int>> _ItemGroups;
		private IReadOnlyDictionary<string, ReadOnlySet<int>> _NPCGroups;
		private IReadOnlyDictionary<string, ReadOnlySet<int>> _ProjGroups;


		////////////////

		internal EntityGroups() {
			TmlLoadHelpers.AddPostModLoadPromise( () => {
				var item_matchers = new List<KeyValuePair<string, Func<Item, bool>>>();
				var npc_matchers = new List<KeyValuePair<string, Func<NPC, bool>>>();
				var proj_matchers = new List<KeyValuePair<string, Func<Projectile, bool>>>();

				this.DefineItemEquipmentGroups( item_matchers );
				this.DefineItemPlaceablesGroups( item_matchers );

				lock( EntityGroups.MyLock ) {
					this._ItemGroups = new ReadOnlyDictionary<string, ReadOnlySet<int>>( EntityGroups.ComputeGroups( item_matchers ) );
					this._NPCGroups = new ReadOnlyDictionary<string, ReadOnlySet<int>>( EntityGroups.ComputeGroups( npc_matchers ) );
					this._ProjGroups = new ReadOnlyDictionary<string, ReadOnlySet<int>>( EntityGroups.ComputeGroups( proj_matchers ) );
				}
			} );
		}
	}
}
