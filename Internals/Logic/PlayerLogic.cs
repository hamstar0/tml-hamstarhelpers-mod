using HamstarHelpers.Components.UI;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Services.Promises;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader.IO;


namespace HamstarHelpers.Internals.Logic {
	partial class PlayerLogic {
		internal readonly static object MyValidatorKey;
		internal readonly static PromiseValidator ServerConnectValidator;


		////////////////

		static PlayerLogic() {
			PlayerLogic.MyValidatorKey = new object();
			PlayerLogic.ServerConnectValidator = new PromiseValidator( PlayerLogic.MyValidatorKey );
		}



		////////////////

		public string PrivateUID { get; private set; }
		public bool HasUID { get; private set; }

		private ISet<int> PermaBuffsById = new HashSet<int>();
		private ISet<int> HasBuffIds = new HashSet<int>();
		private IDictionary<int, int> EquipSlotsToItemTypes = new Dictionary<int, int>();

		private uint TestPing = 0;

		public DialogManager DialogManager = new DialogManager();

		public bool HasSyncedModSettings { get; private set; }
		public bool HasSyncedModData { get; private set; }
		public bool IsSynced { get; private set; }



		////////////////

		public PlayerLogic() {
			this.PrivateUID = Guid.NewGuid().ToString( "D" );
			this.HasUID = false;
			this.HasSyncedModSettings = false;
			this.HasSyncedModData = false;
			this.IsSynced = false;
		}


		////////////////

		public void Load( TagCompound tags ) {
			if( tags.ContainsKey( "uid" ) ) {
				this.PrivateUID = tags.GetString( "uid" );
			}
			if( tags.ContainsKey( "perma_buffs" ) ) {
				var perma_buffs = tags.GetList<int>( "perma_buffs" );
				this.PermaBuffsById = new HashSet<int>( perma_buffs.ToArray() );
			}

			this.HasUID = true;
		}

		public TagCompound Save() {
			var perma_buffs = this.PermaBuffsById.ToArray();

			var tags = new TagCompound {
				{ "uid", this.PrivateUID },
				{ "perma_buffs", perma_buffs }
			};
			return tags;
		}
	}
}
