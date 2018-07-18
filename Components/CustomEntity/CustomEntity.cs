using HamstarHelpers.Helpers.DebugHelpers;
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
		abstract public int FrameCount { get; }

		private IDictionary<string, int> ComponentsByTypeName = new Dictionary<string, int>();
		abstract protected IList<CustomEntityComponent> _OrderedComponents { get; }
		public IReadOnlyList<CustomEntityComponent> OrderedComponents { get; private set; }

		private IList<int> _ComponentDataOrder = new List<int>();
		private IDictionary<int, CustomEntityComponentData> _ComponentData = new Dictionary<int, CustomEntityComponentData>();

		public IReadOnlyList<int> ComponentDataOrder { get; private set; }
		public IReadOnlyDictionary<int, CustomEntityComponentData> ComponentData { get; private set; }



		////////////////

		protected CustomEntity( bool is_this_the_real_life ) {
			foreach( var prop in this._OrderedComponents ) {
				CustomEntityComponentData data = prop.CreateDataInternalWrapper();

				if( data != null ) {
					int code = prop.GetHashCode();
					this._ComponentDataOrder.Add( code );
					this._ComponentData[code] = data;
				}
			}
			
			this.OrderedComponents = new ReadOnlyCollection<CustomEntityComponent>( this._OrderedComponents );
			this.ComponentDataOrder = new ReadOnlyCollection<int>( this._ComponentDataOrder );
			this.ComponentData = new ReadOnlyDictionary<int, CustomEntityComponentData>( this._ComponentData );
		}


		////////////////
		
		public CustomEntityComponent GetComponentByType<T>() where T : CustomEntityComponent {
			int comp_count = this.OrderedComponents.Count;

			if( this.ComponentsByTypeName.Count != comp_count ) {
				this.ComponentsByTypeName.Clear();

				for( int i = 0; i < comp_count; i++ ) {
					string comp_name = this.OrderedComponents[i].GetType().Name;
					this.ComponentsByTypeName[comp_name] = i;
				}
			}

			int idx;

			if( !this.ComponentsByTypeName.TryGetValue( typeof(T).Name, out idx ) ) {
				return null;
			}
			return this.OrderedComponents[ idx ];
		}

		internal CustomEntityComponentData GetPropertyData( CustomEntityComponent prop ) {
			int hash = prop.GetHashCode();

			if( this._ComponentData.ContainsKey(hash) ) {
				return this._ComponentData[ hash ];
			}
			return null;
		}


		////////////////

		public void Sync() {
			if( Main.netMode != 2 ) { throw new Exception("Server only"); }
			CustomEntityProtocol.SendToClients( this );
		}

		internal void SetData( IList<CustomEntityComponentData> data_list ) {
			int i = 0;
			
			foreach( int code in this._ComponentData.Keys.ToArray() ) {
				CustomEntityComponentData data = data_list[i++];
				CustomEntityComponentData old_data = this._ComponentData[ code ];

				if( data.GetType().Name != old_data.GetType().Name ) {
					throw new Exception( "Custom entity data mismatch." );
				}

				this._ComponentData[ code ] = data;
			}
		}


		////////////////

		internal void Update() {
			int prop_count = this.OrderedComponents.Count;
			
			for( int i=0; i<prop_count; i++ ) {
				this.OrderedComponents[ i ].Update( this );
			}

			if( this.CheckMouseHover() ) {
				this.OnMouseHover();
			}
		}


		////////////////
		
		private bool CheckMouseHover() {
			if( Main.netMode == 2 ) { throw new Exception( "Server cannot OnMouseClick." ); }

			var world_scr_rect = new Rectangle( (int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth, Main.screenHeight );
			Rectangle box = this.Hitbox;
			if( !box.Intersects( world_scr_rect ) ) {
				return false;
			}

			var screen_box = new Rectangle( box.X - world_scr_rect.X, box.Y - world_scr_rect.Y, box.Width, box.Height );

			return screen_box.Contains( Main.mouseX, Main.mouseY );
		}

		public virtual void OnMouseHover() { }


		////////////////

		public void Draw( SpriteBatch sb ) {
			if( Main.netMode == 2 ) { throw new Exception( "Server cannot Draw." ); }

			var world_scr_rect = new Rectangle( (int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth, Main.screenHeight );
			if( !this.Hitbox.Intersects( world_scr_rect ) ) { return; }

			if( !this.PreDraw(sb) ) { return; }

			var scr_scr_pos = this.position - Main.screenPosition;
			var tex_rect = new Rectangle( 0, 0, this.Texture.Width, this.Texture.Height / this.FrameCount );

			float scale = 1f;

			sb.Draw( this.Texture, scr_scr_pos, tex_rect, Color.White, 0f, new Vector2(), scale, SpriteEffects.None, 1f );

			this.PostDraw( sb );
		}


		public virtual bool PreDraw( SpriteBatch sb ) { return true; }
		public virtual void PostDraw( SpriteBatch sb ) { }
	}
}
