using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.Services.Promises;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity {
	public class CustomEntityManager : IEnumerable<CustomEntity> {
		public static CustomEntityManager Entities { get { return HamstarHelpersMod.Instance.CustomEntMngr; } }


		////////////////

		private readonly IDictionary<int, CustomEntity> EntitiesToIds = new Dictionary<int, CustomEntity>();
		private readonly IDictionary<string, ISet<int>> EntitiesByComponentName = new Dictionary<string, ISet<int>>();


		////////////////

		internal CustomEntityManager() {
			Main.OnTick += CustomEntityManager._Update;

			// Initialize components:
			IEnumerable<Type> component_types = ReflectionHelpers.GetAllAvailableSubTypes( typeof( CustomEntityComponent ) );
			foreach( var component_type in component_types ) {
				var component = (CustomEntityComponent)Activator.CreateInstance( component_type, new object[] { true } );
				component.StaticInitializeInternalWrapper();
			}

			Promises.AddWorldUnloadEachPromise( () => {
				this.EntitiesToIds.Clear();
				this.EntitiesByComponentName.Clear();
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
				if( value == null ) {
					if( this.EntitiesToIds.ContainsKey( idx ) ) {
						IReadOnlyList<CustomEntityComponent> components = this.EntitiesToIds[idx].ComponentsInOrder;

						foreach( CustomEntityComponent component in components ) {
							string comp_name = component.GetType().Name;

							if( this.EntitiesByComponentName.ContainsKey( comp_name ) ) {
								this.EntitiesByComponentName[comp_name].Remove( idx );
							}
						}

						this.EntitiesToIds.Remove( idx );
					}
				} else {
					foreach( CustomEntityComponent component in value.ComponentsInOrder ) {
						string comp_name = component.GetType().Name;

						if( !this.EntitiesByComponentName.ContainsKey( comp_name ) ) {
							this.EntitiesByComponentName[comp_name] = new HashSet<int>();
						}
						this.EntitiesByComponentName[comp_name].Add( idx );
					}

					value.whoAmI = idx;
					this.EntitiesToIds[idx] = value;
				}
			}
		}
		
		public IEnumerator<CustomEntity> GetEnumerator() {
			return this.EntitiesToIds.Values.GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator() {
			return this.EntitiesToIds.Values.GetEnumerator();
		}


		////////////////

		public ISet<T> GetByType<T>() where T : CustomEntity {
			ISet<int> ent_idxs;

			if( !this.EntitiesByComponentName.TryGetValue( typeof(T).Name, out ent_idxs ) ) {
				return new HashSet<T>();
			}
			
			return new HashSet<T>( ent_idxs.Select( i => (T)this.EntitiesToIds[i] ) );
		}

		////////////////

		public int Add( CustomEntity ent ) {
			int idx = this.EntitiesToIds.Count;
			
			this[ idx ] = ent;

			return idx;
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
			foreach( CustomEntity ent in this.EntitiesToIds.Values ) {
				var draw_comp = (DrawsEntityComponent)ent.GetComponentByType<DrawsEntityComponent>();
				if( draw_comp != null ) {
					draw_comp.Draw( sb, ent );
				}
			}
		}
	}
}
