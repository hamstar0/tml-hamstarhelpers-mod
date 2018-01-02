using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.TmlHelpers {
	[System.Obsolete( "use GlobalProjectile", false )]
	public class AltProjectileInfo {
		private static IDictionary<string, Type> ProjInfoTypes;
		private static IDictionary<int, IDictionary<string, AltProjectileInfo>> ProjInfos;


		////////////////

		static AltProjectileInfo() {
			AltProjectileInfo.DataInitialize();
		}

		internal static void DataInitialize() {
			AltProjectileInfo.ProjInfoTypes = new Dictionary<string, Type>();
			AltProjectileInfo.ProjInfos = new Dictionary<int, IDictionary<string, AltProjectileInfo>>();
			for( int who = 0; who < Main.projectile.Length; who++ ) {
				AltProjectileInfo.ProjInfos[who] = new Dictionary<string, AltProjectileInfo>();
			}
		}



		////////////////
		
		public static void RegisterInfoType<T>() where T : AltProjectileInfo {
			Type t = typeof( T );
			AltProjectileInfo.ProjInfoTypes[t.ToString()] = t;
		}

		////////////////
		
		public static T GetProjInfo<T>( int proj_who ) where T : AltProjectileInfo {
			T proj_info = null;
			if( !AltProjectileInfo.ProjInfos.ContainsKey( proj_who ) ) { return proj_info; }
			if( AltProjectileInfo.ProjInfos[proj_who].Count == 0 ) { return proj_info; }

			string t = typeof( T ).ToString();
			if( !AltProjectileInfo.ProjInfos[proj_who].ContainsKey( t ) ) { return proj_info; }

			proj_info = (T)AltProjectileInfo.ProjInfos[proj_who][t];
			return proj_info;
		}

		////////////////

		internal static void UpdateAll() {
			if( AltProjectileInfo.ProjInfoTypes.Count == 0 ) { return; }

			var map = AltProjectileInfo.ProjInfos;

			for( int who = 0; who < Main.projectile.Length; who++ ) {
				Projectile proj = Main.projectile[who];
				bool is_empty = map[who].Count == 0;
				
				if( proj == null || !proj.active || proj.type == 0 ) {
					if( !is_empty ) {
						AltProjectileInfo.Clear( who );
					}
					continue;
				}

				if( !is_empty ) {
					foreach( AltProjectileInfo info in map[who].Values ) {
						if( info.ProjType != proj.type ) {
							is_empty = true;
							break;
						}
					}
					if( is_empty ) {
						AltProjectileInfo.Clear( who );
					}
				}

				if( is_empty ) {
					AltProjectileInfo.AddAll( proj );
				}

				// Run updates
				foreach( AltProjectileInfo info in map[who].Values ) {
					info.Update();
				}
			}
		}

		////////////////

		private static void AddAll( Projectile proj ) {
			foreach( var kv in AltProjectileInfo.ProjInfoTypes ) {
				AltProjectileInfo.Add( proj, kv.Key, kv.Value );
			}
		}

		private static bool Add( Projectile proj, string label, Type mytype ) {
			var info = (AltProjectileInfo)Activator.CreateInstance( mytype );
			if( !info.CanInitialize( proj ) ) { return false; }

			info.InnerInitialize( proj );

			AltProjectileInfo.ProjInfos[proj.whoAmI][label] = info;

			return true;
		}


		private static void Clear( int who ) {
			IDictionary<string, AltProjectileInfo> infos = AltProjectileInfo.ProjInfos[ who ];

			foreach( AltProjectileInfo info in infos.Values ) {
				info.PostDeath();
			}
			infos.Clear();
		}



		////////////////

		public int ProjWho { get; private set; }
		public int ProjType { get; private set; }

		public Projectile Proj { get { return this.ProjWho == -1 ? null : Main.projectile[ this.ProjWho ]; } }


		////////////////

		protected AltProjectileInfo() {
			this.ProjWho = -1;
			this.ProjType = -1;
		}

		private void InnerInitialize( Projectile projectile ) {
			this.ProjWho = projectile.whoAmI;
			this.ProjType = projectile.type;
			this.Initialize( projectile );
		}

		////////////////

		public virtual bool CanInitialize( Projectile projectile ) {
			return false;
		}

		public virtual void Initialize( Projectile projectile ) { }

		public virtual void Update() { }

		public virtual void PostDeath() { }
	}
}
