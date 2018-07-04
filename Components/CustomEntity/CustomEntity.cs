using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity {
	abstract public class CustomEntityProperty {
		public virtual void Update( CustomEntity ent ) { }
	}
	//IsItem,
	//IsPlayerHostile,
	//IsFriendlyNpcHostile,
	//IsPvpHostile,
	//IsPlayerTarget,
	//IsPvpTarget,
	//IsFiendlyNpcTarget,
	//IsHostileNpcTarget,
	//IsCapturable,
	//TakesHits,
	//TakesDamage,
	//TakesKnockback,
	//RespectsTerrain

	//abstract public class CustomEntityAttributeMovement { }
	//SeeksTarget,
	//IsGravityBound,
	//IsRailBound,
	//IsRopeBound,
	//Floats,
	//Flies,
	//Crawls,
	//Swims

	//abstract public class CustomEntityAttributeBehavior { }
	//SeeksTarget,
	//AvoidsTarget,
	//Wanders,
	//AlwaysAimsAtTarget

	
	
	
	abstract public class CustomEntity : Entity {
		abstract public string DisplayName { get; }

		abstract public Texture2D Texture { get; }
		
		abstract protected IList<CustomEntityProperty> _OrderedProperties { get; }
		public IReadOnlyList<CustomEntityProperty> OrderedProperties { get; }

		private readonly IDictionary<string, int> PropertiesByName = new Dictionary<string, int>();



		////////////////

		protected CustomEntity() : base() {
			this.OrderedProperties = new ReadOnlyCollection<CustomEntityProperty>( this._OrderedProperties );
		}


		////////////////

		public CustomEntityProperty GetPropertyByName( string name ) {
			int prop_count = this.OrderedProperties.Count;

			if( this.PropertiesByName.Count != prop_count ) {
				this.PropertiesByName.Clear();

				for( int i = 0; i < prop_count; i++ ) {
					string prop_name = this.OrderedProperties[i].GetType().Name;
					this.PropertiesByName[prop_name] = i;
				}
			}

			int idx;

			if( this.PropertiesByName.TryGetValue(name, out idx ) ) {
				return this.OrderedProperties[ idx ];
			}
			return null;
		}


		////////////////

		internal void Update() {
			int prop_count = this.OrderedProperties.Count;
			
			for( int i=0; i<prop_count; i++ ) {
				this.OrderedProperties[ i ].Update( this );
			}
		}
	}
}
