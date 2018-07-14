using HamstarHelpers.Internals.NetProtocols;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity {
	abstract public class CustomEntity : Entity {
		abstract public string DisplayName { get; }

		abstract public Texture2D Texture { get; }

		private IDictionary<string, int> PropertiesByName = new Dictionary<string, int>();
		abstract protected IList<CustomEntityProperty> _OrderedProperties { get; }
		public IReadOnlyList<CustomEntityProperty> OrderedProperties { get; private set; }

		private IList<int> _PropertyDataOrder = new List<int>();
		private IDictionary<int, CustomEntityPropertyData> _PropertyData = new Dictionary<int, CustomEntityPropertyData>();

		public IReadOnlyList<int> PropertyDataOrder { get; private set; }
		public IReadOnlyDictionary<int, CustomEntityPropertyData> PropertyData { get; private set; }



		////////////////

		protected CustomEntity( bool is_this_the_real_life ) : base() {
			foreach( var prop in this._OrderedProperties ) {
				CustomEntityPropertyData data = prop.CreateData();

				if( data != null ) {
					int code = prop.GetHashCode();
					this._PropertyDataOrder.Add( code );
					this._PropertyData[ code ] = data;
				}
			}

			this.OrderedProperties = new ReadOnlyCollection<CustomEntityProperty>( this._OrderedProperties );
			this.PropertyDataOrder = new ReadOnlyCollection<int>( this._PropertyDataOrder );
			this.PropertyData = new ReadOnlyDictionary<int, CustomEntityPropertyData>( this._PropertyData );
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

		internal CustomEntityPropertyData GetPropertyData( CustomEntityProperty prop ) {
			int hash = prop.GetHashCode();

			if( this._PropertyData.ContainsKey(hash) ) {
				return this._PropertyData[ hash ];
			}
			return null;
		}


		////////////////

		public void Sync() {
			if( Main.netMode != 2 ) { throw new Exception("Server only"); }
			CustomEntityProtocol.SendToClients( this );
		}

		internal void SetData( IList<CustomEntityPropertyData> data_list ) {
			int i = 0;
			
			foreach( int code in this._PropertyData.Keys.ToArray() ) {
				CustomEntityPropertyData data = data_list[i++];
				CustomEntityPropertyData old_data = this._PropertyData[ code ];

				if( data.GetType().Name != old_data.GetType().Name ) {
					throw new Exception( "Custom entity data mismatch." );
				}

				this._PropertyData[ code ] = data;
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
