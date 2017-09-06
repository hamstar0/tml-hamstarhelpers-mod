using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.TmlHelpers {
	public class AltNPCInfo {
		private static IDictionary<string, AltNPCInfo> NpcInfoCopies;
		internal static IDictionary<int, IDictionary<string, AltNPCInfo>> NpcInfos;

		static AltNPCInfo() {
			AltNPCInfo.NpcInfoCopies = new Dictionary<string, AltNPCInfo>();
			AltNPCInfo.NpcInfos = new Dictionary<int, IDictionary<string, AltNPCInfo>>();
			for( int who = 0; who < Main.npc.Length; who++ ) {
				AltNPCInfo.NpcInfos[who] = new Dictionary<string, AltNPCInfo>();
			}
		}


		////////////////

		public static void RegisterInfoType( Mod mod, AltNPCInfo info ) {
			string modname = TmlHelpers.GetModUniqueName( mod );
			AltNPCInfo.NpcInfoCopies[modname] = info;
		}

		public static AltNPCInfo GetNpcInfo( Mod mod, int npc_who ) {
			if( !AltNPCInfo.NpcInfos.ContainsKey(npc_who) || AltNPCInfo.NpcInfos[npc_who].Count == 0 ) { return null; }

			string modname = TmlHelpers.GetModUniqueName( mod );
			if( !AltNPCInfo.NpcInfos[npc_who].ContainsKey(modname) ) { return null; }

			return AltNPCInfo.NpcInfos[npc_who][modname];
		}

		////////////////

		internal static void UpdateAll() {
			for( int who = 0; who < Main.npc.Length; who++ ) {
				NPC npc = Main.npc[who];
				if( npc == null || !npc.active ) {
					if( AltNPCInfo.NpcInfos[who].Count > 0 ) { AltNPCInfo.NpcInfos[who].Clear(); }
					continue;
				}

				bool is_empty = AltNPCInfo.NpcInfos[who].Count == 0;
				
				if( !is_empty ) {
					foreach( string name in AltNPCInfo.NpcInfos[who].Keys ) {
						if( AltNPCInfo.NpcInfos[who][name].NpcType != npc.type ) {
							is_empty = true;
							break;
						}
					}
					if( is_empty ) { AltNPCInfo.NpcInfos[who].Clear(); }
				}

				if( is_empty ) {
					foreach( var kv in AltNPCInfo.NpcInfoCopies ) {
						AltNPCInfo base_info = kv.Value;
						if( !base_info.CanInitialize( npc ) ) { continue; }

						var info = (AltNPCInfo)Activator.CreateInstance( base_info.GetType() );
						info.InnerInitialize( npc );
						
						AltNPCInfo.NpcInfos[who][kv.Key] = info;
					}
				}
			}
		}



		////////////////

		public int NpcWho { get; private set; }
		public int NpcType { get; private set; }

		public NPC Npc { get { return this.NpcWho == -1 ? null : Main.npc[this.NpcWho]; } }

		internal bool IsColliding = false;

		////////////////

		protected AltNPCInfo() {
			this.NpcWho = -1;
			this.NpcType = -1;
		}

		private void InnerInitialize( NPC npc ) {
			this.NpcWho = npc.whoAmI;
			this.NpcType = npc.type;
			this.Initialize( npc );
		}

		////////////////

		public virtual bool CanInitialize( NPC npc ) {
			return false;
		}

		public virtual void Initialize( NPC npc ) { }

		public virtual void OnTerrainCollide() { }
	}
}
