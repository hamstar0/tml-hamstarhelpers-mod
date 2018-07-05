using Microsoft.Xna.Framework;
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

		protected CustomEntity( bool is_this_the_real_life ) : base() {
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


		////////////////

		public void Draw( SpriteBatch sb ) {
			if( Main.netMode == 2 ) { return; }

			Rectangle world_scr_rect = new Rectangle( (int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth, Main.screenHeight );
			Rectangle my_rect = this.Hitbox;
			if( !my_rect.Intersects( world_scr_rect ) ) { return; }

			if( !this.PreDraw(sb) ) { return; }

			Vector2 scr_scr_pos = this.position - Main.screenPosition;
			Rectangle scr_rect = my_rect;
			scr_rect.X -= world_scr_rect.X;
			scr_rect.Y -= world_scr_rect.Y;

			float scale = 1f;

			sb.Draw( this.Texture, scr_scr_pos, scr_rect, Color.White, 0f, new Vector2(), scale, SpriteEffects.None, 1f );

			this.PostDraw( sb );
		}

		public virtual bool PreDraw( SpriteBatch sb ) { return true; }
		public virtual void PostDraw( SpriteBatch sb ) { }
	}
}
