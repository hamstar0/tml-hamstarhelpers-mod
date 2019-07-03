using HamstarHelpers.Components.DataStructures;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.PromisedHooks;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using Terraria;
using Terraria.ModLoader;
using Terraria.Utilities;

using ItemMatcher = System.Func<Terraria.Item, System.Collections.Generic.IDictionary<string, System.Collections.Generic.ISet<int>>, bool>;
using NPCMatcher = System.Func<Terraria.NPC, System.Collections.Generic.IDictionary<string, System.Collections.Generic.ISet<int>>, bool>;
using ProjMatcher = System.Func<Terraria.Projectile, System.Collections.Generic.IDictionary<string, System.Collections.Generic.ISet<int>>, bool>;


namespace HamstarHelpers.Services.EntityGroups {
	public partial class EntityGroups {
		private readonly static object MyLock = new object();

		private readonly static object MyValidatorKey;
		public readonly static PromiseValidator LoadedAllValidator;



		////////////////

		static EntityGroups() {
			EntityGroups.MyValidatorKey = new object();
			EntityGroups.LoadedAllValidator = new PromiseValidator( EntityGroups.MyValidatorKey );
		}



		////////////////

		private bool IsEnabled = false;

		private IReadOnlyDictionary<string, ReadOnlySet<int>> _ItemGroups = null;
		private IReadOnlyDictionary<string, ReadOnlySet<int>> _NPCGroups = null;
		private IReadOnlyDictionary<string, ReadOnlySet<int>> _ProjGroups = null;

		private IReadOnlyDictionary<int, ReadOnlySet<string>> _GroupsPerItem = null;
		private IReadOnlyDictionary<int, ReadOnlySet<string>> _GroupsPerNPC = null;
		private IReadOnlyDictionary<int, ReadOnlySet<string>> _GroupsPerProj = null;

		private IDictionary<string, ReadOnlySet<int>> _RawItemGroups = new Dictionary<string, ReadOnlySet<int>>();
		private IDictionary<string, ReadOnlySet<int>> _RawNPCGroups = new Dictionary<string, ReadOnlySet<int>>();
		private IDictionary<string, ReadOnlySet<int>> _RawProjGroups = new Dictionary<string, ReadOnlySet<int>>();

		private IDictionary<int, ReadOnlySet<string>> _RawGroupsPerItem = new Dictionary<int, ReadOnlySet<string>>();
		private IDictionary<int, ReadOnlySet<string>> _RawGroupsPerNPC = new Dictionary<int, ReadOnlySet<string>>();
		private IDictionary<int, ReadOnlySet<string>> _RawGroupsPerProj = new Dictionary<int, ReadOnlySet<string>>();

		private IList<Tuple<string, string[], ItemMatcher>> CustomItemMatchers = new List<Tuple<string, string[], ItemMatcher>>();
		private IList<Tuple<string, string[], NPCMatcher>> CustomNPCMatchers = new List<Tuple<string, string[], NPCMatcher>>();
		private IList<Tuple<string, string[], ProjMatcher>> CustomProjMatchers = new List<Tuple<string, string[], ProjMatcher>>();

		private IList<Item> ItemPool = null;
		private IList<NPC> NPCPool = null;
		private IList<Projectile> ProjPool = null;



		////////////////

		internal EntityGroups() {
			lock( EntityGroups.MyLock ) {
				this._ItemGroups = new ReadOnlyDictionary<string, ReadOnlySet<int>>( this._RawItemGroups );
				this._NPCGroups = new ReadOnlyDictionary<string, ReadOnlySet<int>>( this._RawNPCGroups );
				this._ProjGroups = new ReadOnlyDictionary<string, ReadOnlySet<int>>( this._RawProjGroups );

				this._GroupsPerItem = new ReadOnlyDictionary<int, ReadOnlySet<string>>( this._RawGroupsPerItem );
				this._GroupsPerNPC = new ReadOnlyDictionary<int, ReadOnlySet<string>>( this._RawGroupsPerNPC );
				this._GroupsPerProj = new ReadOnlyDictionary<int, ReadOnlySet<string>>( this._RawGroupsPerProj );
			}
			
			PromisedHooks.PromisedHooks.AddPostModLoadPromise( () => {
				if( !this.IsEnabled ) { return; }

				this.GetItemPool();
				this.GetNPCPool();
				this.GetProjPool();
				
				ThreadPool.QueueUserWorkItem( _ => {
					int _check = 0;

					try {
						IList<Tuple<string, string[], ItemMatcher>> itemMatchers;
						IList<Tuple<string, string[], NPCMatcher>> npcMatchers;
						IList<Tuple<string, string[], ProjMatcher>> projMatchers;

						lock( EntityGroups.MyLock ) {
							itemMatchers = EntityGroups.DefineItemGroups();
							_check++;
							npcMatchers = EntityGroups.DefineNPCGroups();
							_check++;
							projMatchers = EntityGroups.DefineProjectileGroups();
							_check++;
						}

						this.ComputeGroups<Item>( itemMatchers, ref this._RawItemGroups, ref this._RawGroupsPerItem );
						_check++;
						this.ComputeGroups<NPC>( npcMatchers, ref this._RawNPCGroups, ref this._RawGroupsPerNPC );
						_check++;
						this.ComputeGroups<Projectile>( projMatchers, ref this._RawProjGroups, ref this._RawGroupsPerProj );
						_check++;

						this.ComputeGroups<Item>( this.CustomItemMatchers, ref this._RawItemGroups, ref this._RawGroupsPerItem );
						_check++;
						this.ComputeGroups<NPC>( this.CustomNPCMatchers, ref this._RawNPCGroups, ref this._RawGroupsPerNPC );
						_check++;
						this.ComputeGroups<Projectile>( this.CustomProjMatchers, ref this._RawProjGroups, ref this._RawGroupsPerProj );
						_check++;

						lock( EntityGroups.MyLock ) {
							this.CustomItemMatchers = null;
							this.CustomNPCMatchers = null;
							this.CustomProjMatchers = null;
							this.ItemPool = null;
							this.NPCPool = null;
							this.ProjPool = null;
						}
						
						PromisedHooks.PromisedHooks.TriggerValidatedPromise( EntityGroups.LoadedAllValidator, EntityGroups.MyValidatorKey );
						_check++;
					} catch( Exception e ) {
						LogHelpers.Warn( "Initialization failed (at #"+_check+"): "+e.ToString() );
					}
				} );
			} );
		}

		~EntityGroups() {
			lock( EntityGroups.MyLock ) { }
		}


		////////////////

		private IList<T> GetPool<T>() where T : Entity {
			IList<T> list = null;
			
			switch( typeof( T ).Name ) {
			case "Item":
				list = (IList<T>)this.GetItemPool();
				break;
			case "NPC":
				list = (IList<T>)this.GetNPCPool();
				break;
			case "Projectile":
				list = (IList<T>)this.GetProjPool();
				break;
			default:
				throw new NotImplementedException( "Invalid Entity type " + typeof( T ).Name );
			}
			
			return list;
		}

		internal IList<Item> GetItemPool() {
			if( this.ItemPool != null ) { return this.ItemPool; }

			var list = new Item[ ItemLoader.ItemCount ];
			list[0] = null;
			
			for( int i = 1; i < ItemLoader.ItemCount; i++ ) {
				list[i] = new Item();
				list[i].SetDefaults( i, true );
			}

			this.ItemPool = new List<Item>( list );
			return this.ItemPool;
		}

		internal IList<NPC> GetNPCPool() {
			if( this.NPCPool != null ) { return this.NPCPool; }
			
			var list = new NPC[ NPCLoader.NPCCount ];
			list[0] = null;

			for( int i = 1; i < NPCLoader.NPCCount; i++ ) {
				list[i] = new NPC();
				list[i].SetDefaults( i );
			}

			this.NPCPool = new List<NPC>( list );
			return this.NPCPool;
		}

		internal IList<Projectile> GetProjPool() {
			if( this.ProjPool != null ) { return this.ProjPool; }
			
			var list = new Projectile[ ProjectileLoader.ProjectileCount ];
			list[0] = null;

			UnifiedRandom oldRand = Main.rand;
			Main.rand = new UnifiedRandom();
			
			for( int i = 1; i < ProjectileLoader.ProjectileCount; i++ ) {
				list[i] = new Projectile();

				try {
					list[i].SetDefaults( i );
				} catch( Exception e ) {
					LogHelpers.Log( "GetProjPool " + i + " - " + e.ToString() );
				}
			} 

			Main.rand = oldRand;

			this.ProjPool = new List<Projectile>( list );
			return this.ProjPool;
		}
	}
}
