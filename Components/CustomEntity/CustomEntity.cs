using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity {
	abstract public class CustomEntity : Entity {
		abstract public string DisplayName { get; }

		abstract public Texture2D Texture { get; }

		abstract protected IList<CustomEntityProperty> _OrderedProperties { get; }
		public IReadOnlyList<CustomEntityProperty> OrderedProperties { get; }
		private readonly IDictionary<string, int> PropertiesByName = new Dictionary<string, int>();

		private readonly IList<int> PropertyDataOrder = new List<int>();
		private readonly IDictionary<int, CustomEntityData> PropertyData = new Dictionary<int, CustomEntityData>();



		////////////////

		protected CustomEntity( bool is_this_the_real_life ) : base() {
			foreach( var prop in this._OrderedProperties ) {
				CustomEntityData data = prop.CreateData();

				if( data != null ) {
					int code = prop.GetHashCode();
					this.PropertyDataOrder.Add( code );
					this.PropertyData[ code ] = data;
				}
			}

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

		internal CustomEntityData GetPropertyData( CustomEntityProperty prop ) {
			int hash = prop.GetHashCode();

			if( this.PropertyData.ContainsKey(hash) ) {
				return this.PropertyData[ hash ];
			}
			return null;
		}


		////////////////
		
		internal void SerializeToStream( BinaryWriter writer ) {
			for( int i=0; i<this.PropertyDataOrder.Count; i++ ) {
				int code = this.PropertyDataOrder[ i ];
				this.PropertyData[ code ].Serialize( writer, this );
			}
		}

		internal void DeserializeFromStream( BinaryReader reader ) {
			for( int i = 0; i < this.PropertyDataOrder.Count; i++ ) {
				int code = this.PropertyDataOrder[i];
				this.PropertyData[ code ].Deserialize( reader, this );
			}
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
