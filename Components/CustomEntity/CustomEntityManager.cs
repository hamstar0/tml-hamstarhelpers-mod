using HamstarHelpers.Services.Promises;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity {
	public class CustomEntityManager {
		public static CustomEntityManager Entities { get { return HamstarHelpersMod.Instance.CustomEntMngr; } }


		////////////////

		private readonly IDictionary<int, CustomEntity> EntitiesToIds = new Dictionary<int, CustomEntity>();
		private readonly IDictionary<string, ISet<int>> EntitiesByName = new Dictionary<string, ISet<int>>();


		////////////////

		internal CustomEntityManager() {
			Main.OnTick += CustomEntityManager._Update;

			Promises.AddWorldUnloadEachPromise( () => {
				this.EntitiesToIds.Clear();
				this.EntitiesByName.Clear();
			} );
		}

		~CustomEntityManager() {
			Main.OnTick -= CustomEntityManager._Update;
		}


		////////////////

		public CustomEntity this[ int idx ] {
			get {
				CustomEntity ent = null;
				this.EntitiesToIds.TryGetValue( idx, out ent );
				return ent;
			}


			set {
				string old_name = this[idx] == null ? null : this[idx].GetType().Name;

				if( old_name != null ) {
					this.EntitiesByName[old_name].Remove( idx );
				}

				if( value != null ) {
					string new_name = value.GetType().Name;

					if( !this.EntitiesByName.ContainsKey(new_name) ) {
						this.EntitiesByName[ new_name ] = new HashSet<int>();
					}
					this.EntitiesByName[ new_name ].Add( idx );
				}

				this.EntitiesToIds[ idx ] = value;
				value.whoAmI = idx;
			}
		}


		////////////////

		public ISet<CustomEntity> GetByName( string name ) {
			ISet<int> ent_idxs;
			bool found = this.EntitiesByName.TryGetValue( name, out ent_idxs );

			return new HashSet<CustomEntity>( ent_idxs.Select( i => this.EntitiesToIds[i] ) );
		}


		public void Add( CustomEntity ent ) {
			int idx = this.EntitiesToIds.Count;
			
			this[ idx ] = ent;
		}


		////////////////

		private static void _Update() { // <- Just in case references are doing something funky...
			HamstarHelpersMod mymod = HamstarHelpersMod.Instance;
			if( mymod == null ) { return; }

			mymod.CustomEntMngr.Update();
		}

		internal void Update() {
			int ent_count = this.EntitiesToIds.Count;

			for( int i=0; i<ent_count; i++ ) {
				this.EntitiesToIds[i]?.Update();
			}
		}


		////////////////

		internal void DrawAll( SpriteBatch sb ) {
			foreach( var kv in this.EntitiesToIds ) {
				kv.Value.Draw( sb );
			}
		}
	}
}
