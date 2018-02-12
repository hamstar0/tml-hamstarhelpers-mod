using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.TmlHelpers {
	[System.Obsolete( "use GlobalNPC", false )]
	public class AltNPCInfo {
		private static IDictionary<string, Type> NpcInfoTypes;
		private static IDictionary<int, IDictionary<string, AltNPCInfo>> NpcInfos;


		////////////////

		static AltNPCInfo() {
			AltNPCInfo.DataInitialize();
		}
		
		internal static void DataInitialize() {
			AltNPCInfo.NpcInfoTypes = new Dictionary<string, Type>();
			AltNPCInfo.NpcInfos = new Dictionary<int, IDictionary<string, AltNPCInfo>>();
			for( int who = 0; who < Main.npc.Length; who++ ) {
				AltNPCInfo.NpcInfos[who] = new Dictionary<string, AltNPCInfo>();
			}
		}



		////////////////

		[System.Obsolete( "use AltNPCInfo.RegisterInfoType<T>()", true )]
		public static void RegisterInfoType( Mod mod, AltNPCInfo info ) {
			string modname = TmlHelpers.GetModUniqueName( mod );
			//AltNPCInfo.NpcInfoModMappedTypes[modname] = info.GetType();
			Type t = info.GetType();
			AltNPCInfo.NpcInfoTypes[ modname ] = t;
		}
		
		public static void RegisterInfoType<T>() where T : AltNPCInfo {
			Type t = typeof( T );
			AltNPCInfo.NpcInfoTypes[ t.ToString() ] = t;
		}

		////////////////

		[System.Obsolete( "use AltNPCInfo.GetNpcInfo<T>( Mod mod, int npc_who )", true )]
		public static AltNPCInfo GetNpcInfo( Mod mod, int npc_who ) {
			if( !AltNPCInfo.NpcInfos.ContainsKey(npc_who) || AltNPCInfo.NpcInfos[npc_who].Count == 0 ) { return null; }

			string modname = TmlHelpers.GetModUniqueName( mod );
			if( !AltNPCInfo.NpcInfos[npc_who].ContainsKey(modname) ) { return null; }

			return AltNPCInfo.NpcInfos[npc_who][modname];	// Dislike this merging convention
		}

		public static T GetNpcInfo<T>( int npc_who ) where T : AltNPCInfo {
			T npc_info = null;
			if( !AltNPCInfo.NpcInfos.ContainsKey( npc_who ) ) { return npc_info; }
			if( AltNPCInfo.NpcInfos[npc_who].Count == 0 ) { return npc_info; }

			string t = typeof(T).ToString();
			if( !AltNPCInfo.NpcInfos[npc_who].ContainsKey(t) ) { return npc_info; }

			npc_info = (T)AltNPCInfo.NpcInfos[npc_who][t];
			return npc_info;
		}


		////////////////

		internal static void UpdateAll() {
			if( AltNPCInfo.NpcInfoTypes.Count == 0 ) { return; }

			var map = AltNPCInfo.NpcInfos;

			for( int who = 0; who < Main.npc.Length; who++ ) {
				NPC npc = Main.npc[who];
				bool is_empty = map[who].Count == 0;

				if( npc == null || !npc.active || npc.type == 0 ) {
					if( !is_empty ) {
						AltNPCInfo.Clear( who );
					}
					continue;
				}

				if( !is_empty ) {
					foreach( AltNPCInfo info in map[who].Values ) {
						if( info.NpcType != npc.type ) {
							is_empty = true;
							break;
						}
					}
					if( is_empty ) {
						AltNPCInfo.Clear( who );
					}
				}

				if( is_empty ) {
					AltNPCInfo.AddAll( npc );
				}

				// Run updates
				foreach( AltNPCInfo info in map[who].Values ) {
					info.Update();
				}
			}
		}

		////////////////

		private static void AddAll( NPC npc ) {
			foreach( var kv in AltNPCInfo.NpcInfoTypes ) {
				AltNPCInfo.Add( npc, kv.Key, kv.Value );
			}
		}

		private static bool Add( NPC npc, string label, Type mytype ) {
			var info = (AltNPCInfo)Activator.CreateInstance( mytype );
			if( !info.CanInitialize( npc ) ) { return false; }

			info.InnerInitialize( npc );

			AltNPCInfo.NpcInfos[npc.whoAmI][label] = info;

			return true;
		}


		private static void Clear( int who ) {
			IDictionary<string, AltNPCInfo> infos = AltNPCInfo.NpcInfos[who];

			foreach( AltNPCInfo info in infos.Values ) {
				info.PostDeath();
			}
			infos.Clear();
		}



		////////////////

		public int NpcWho { get; private set; }
		public int NpcType { get; private set; }

		public NPC Npc { get { return this.NpcWho == -1 ? null : Main.npc[this.NpcWho]; } }


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

		[System.Obsolete( "Re-implement anew via. AltNPCInfo.Update()", true )]
		public virtual void OnTerrainCollide() { }

		public virtual void Update() { }

		public virtual void PostDeath() { }
	}
}
