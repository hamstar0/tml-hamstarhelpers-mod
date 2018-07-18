using HamstarHelpers.Services.Promises;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity {
	public class CustomEntityManager {
		public static CustomEntityManager Entities { get { return HamstarHelpersMod.Instance.CustomEntMngr; } }


		////////////////

		private readonly IDictionary<int, CustomEntity> EntitiesToIds = new Dictionary<int, CustomEntity>();
		private readonly IDictionary<string, ISet<int>> EntitiesByTypeName = new Dictionary<string, ISet<int>>();


		////////////////

		internal CustomEntityManager() {
			Main.OnTick += CustomEntityManager._Update;

			Promises.AddWorldUnloadEachPromise( () => {
				this.EntitiesToIds.Clear();
				this.EntitiesByTypeName.Clear();
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
				string old_name = this[ idx ]?.GetType().Name;

				if( old_name != null ) {
					this.EntitiesByTypeName[ old_name ].Remove( idx );
				}

				if( value != null ) {
					string new_name = value.GetType().Name;

					if( !this.EntitiesByTypeName.ContainsKey(new_name) ) {
						this.EntitiesByTypeName[ new_name ] = new HashSet<int>();
					}
					this.EntitiesByTypeName[ new_name ].Add( idx );

					value.whoAmI = idx;
					this.EntitiesToIds[idx] = value;
				} else {
					this.EntitiesToIds.Remove( idx );
				}
			}
		}


		////////////////

		public ISet<T> GetByType<T>() where T : CustomEntity {
			ISet<int> ent_idxs;

			if( !this.EntitiesByTypeName.TryGetValue( typeof(T).Name, out ent_idxs ) ) {
				return new HashSet<T>();
			}
			
			return new HashSet<T>( ent_idxs.Select( i => (T)this.EntitiesToIds[i] ) );
		}

		////////////////

		public void Add( CustomEntity ent ) {
			int idx = this.EntitiesToIds.Count;
			
			this[ idx ] = ent;
		}

		public void Remove( CustomEntity ent ) {
			this[ ent.whoAmI ] = null;
		}
		public void Remove( int idx ) {
			this[ idx ] = null;
		}


		////////////////

		private static void _Update() { // <- Just in case references are doing something funky...
			HamstarHelpersMod mymod = HamstarHelpersMod.Instance;
			if( mymod == null ) { return; }

			mymod.CustomEntMngr.Update();
		}

		internal void Update() {
			foreach( CustomEntity ent in this.EntitiesToIds.Values.ToArray() ) {
				ent.Update();
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
