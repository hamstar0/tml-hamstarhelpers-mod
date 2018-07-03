using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
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

		abstract public IList<CustomEntityProperty> Attributes { get; }


		////////////////

		protected CustomEntity() : base() { }


		////////////////
		
		internal void Update() {
			int attr_count = this.Attributes.Count;

			for( int i=0; i<attr_count; i++ ) {
				this.Attributes[ i ].Update( this );
			}
		}
	}
}
