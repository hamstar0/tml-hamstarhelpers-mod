using HamstarHelpers.Services.Promises;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity {
	public class CustomEntityManager {
		private readonly IList<CustomEntity> Entities = new List<CustomEntity>();
		private readonly IDictionary<string, ISet<int>> EntitiesByName = new Dictionary<string, ISet<int>>();


		////////////////

		internal CustomEntityManager() {
			Main.OnTick += CustomEntityManager._Update;

			Promises.AddWorldUnloadEachPromise( () => {
				this.Entities.Clear();
				this.EntitiesByName.Clear();
			} );
		}

		~CustomEntityManager() {
			Main.OnTick -= CustomEntityManager._Update;
		}


		////////////////

		public CustomEntity this[ int idx ] {
			get {
				return this.Entities[ idx ];
			}


			set {
				string old_name = this.Entities[idx] == null ? null : this.Entities[idx].GetType().Name;

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

				this.Entities[ idx ] = value;
				value.whoAmI = idx;
			}
		}


		////////////////

		public ISet<CustomEntity> GetByName( string name ) {
			ISet<int> ent_idxs;
			bool found = this.EntitiesByName.TryGetValue( name, out ent_idxs );

			return new HashSet<CustomEntity>( ent_idxs.Select( i => this.Entities[i] ) );
		}

		public void Add( CustomEntity ent ) {
			int idx = this.Entities.Count;

			this.Entities.Add( ent );
			this[ idx ] = ent;

			ent.whoAmI = idx;
		}


		////////////////

		private static void _Update() { // <- Just in case references are doing something funky...
			HamstarHelpersMod mymod = HamstarHelpersMod.Instance;
			if( mymod == null ) { return; }

			mymod.CustomEntMngr.Update();
		}

		internal void Update() {
			int ent_count = this.Entities.Count;

			for( int i=0; i<ent_count; i++ ) {
				this.Entities[i]?.Update();
			}
		}


		////////////////

		internal void DrawAll( SpriteBatch sb ) {
			foreach( var ent in this.Entities ) {
				ent.Draw( sb );
			}
		}
	}
}
