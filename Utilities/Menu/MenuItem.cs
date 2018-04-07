using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Utilities.Menu {
	public class MenuItem {
		public static void AddMenuItem( string text, int offset_y, int menu_context, Action my_action ) {
			var mymod = HamstarHelpersMod.Instance;
			var item = new MenuItem( text, offset_y, menu_context, my_action );

			mymod.MenuItemMngr.Items[ item.GetHashCode() ] = item;
		}



		////////////////
		
		public const int MenuTopPos = 250;

		public string MenuText { get; private set; }
		public int OffsetY { get; private set; }
		public int MenuContext { get; private set; }
		public Action MyAction { get; private set; }

		private bool MenuItemHovered = false;
		private float MenuItemScale = 0.8f;



		////////////////

		internal MenuItem( string text, int offset_y, int menu_context, Action my_action ) {
			this.MenuText = text;
			this.OffsetY = offset_y;
			this.MenuContext = menu_context;
			this.MyAction = my_action;
		}


		////////////////

		internal void Draw() {
			if( Main.mouseLeft && this.IsMouseOverMenuItem() ) {
				this.MenuItemScale = 0.8f;

				Main.PlaySound( 11 );

				this.MyAction();
			} else {
				Main.spriteBatch.Begin( SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.UIScaleMatrix );
				this.DrawMenuItem();
				Main.spriteBatch.End();
			}
		}


		internal bool IsMouseOverMenuItem() {
			int rel_offset_y = this.OffsetY < 0 ? Main.screenHeight + this.OffsetY : this.OffsetY;
			
			Vector2 dim = Main.fontDeathText.MeasureString( this.MenuText );
			Vector2 origin = dim * 0.5f;

			float base_pos_x = Main.screenWidth / 2;
			float base_pos_y = rel_offset_y + origin.Y;

			return Main.mouseX >= base_pos_x - ( dim.X * 0.5f ) && Main.mouseX <= base_pos_x + ( dim.X * 0.5f ) &&
					Main.mouseY >= base_pos_y - ( dim.Y * 0.5f ) && Main.mouseY <= base_pos_y + ( dim.Y * 0.5f );
		}


		////////////////

		internal void DrawMenuItem() {
			int rel_offset_y = this.OffsetY < 0 ? Main.screenHeight + this.OffsetY : this.OffsetY;

			byte b = (byte)( ( 255 + Main.tileColor.R * 2 ) / 3 );
			Color color = new Color( (int)b, (int)b, (int)b, 255 );

			Vector2 dim = Main.fontDeathText.MeasureString( this.MenuText );
			Vector2 origin = dim * 0.5f;

			float base_pos_x = Main.screenWidth / 2;
			float base_pos_y = rel_offset_y + origin.Y;

			bool is_selected = Main.mouseX >= base_pos_x - ( dim.X * 0.5f ) && Main.mouseX <= base_pos_x + ( dim.X * 0.5f ) &&
							   Main.mouseY >= base_pos_y - ( dim.Y * 0.5f ) && Main.mouseY <= base_pos_y + ( dim.Y * 0.5f );

			if( is_selected && !this.MenuItemHovered ) {
				Main.PlaySound( 12 );
			}
			this.MenuItemHovered = is_selected;

			if( this.MenuItemHovered ) {
				this.MenuItemScale = this.MenuItemScale < 1f ? this.MenuItemScale + 0.02f : 1f;
			} else {
				this.MenuItemScale = this.MenuItemScale > 0.8f ? this.MenuItemScale - 0.02f : 0.8f;
			}

			for( int i = 0; i < 5; i++ ) {
				Color menu_item_color = i == 4 ? color : Color.Black;

				if( this.MenuItemHovered ) {
					if( i == 4 ) {
						int alpha = 255;
						int red = (int)menu_item_color.R;
						int green = (int)menu_item_color.G;
						int blue = (int)menu_item_color.B;

						float alpha_scale = (float)alpha / 255f;

						red = (int)( (float)red * ( 1f - alpha_scale ) + 255f * alpha_scale );
						green = (int)( (float)green * ( 1f - alpha_scale ) + 215f * alpha_scale );
						blue = (int)( (float)blue * ( 1f - alpha_scale ) );

						menu_item_color = new Color( (byte)red, (byte)green, (byte)blue, (byte)alpha );
					}
				} else {
					menu_item_color *= 0.5f;
				}

				int x_shift = 0;
				int y_shift = 0;

				if( i == 0 ) {
					x_shift = -2;
				}
				if( i == 1 ) {
					x_shift = 2;
				}
				if( i == 2 ) {
					y_shift = -2;
				}
				if( i == 3 ) {
					y_shift = 2;
				}

				float menu_item_scale = this.MenuItemScale;

				Vector2 draw_pos = new Vector2( base_pos_x, base_pos_y );
				draw_pos.X += x_shift;
				draw_pos.Y += y_shift;

				if( Main.netMode == 2 ) {
					menu_item_scale *= 0.5f;
				}

				Main.spriteBatch.DrawString( Main.fontDeathText, this.MenuText, draw_pos, menu_item_color, 0f, origin, menu_item_scale, SpriteEffects.None, 0f );
			}

			Vector2 bonus = Main.DrawThickCursor( false );
			Main.DrawCursor( bonus, false );

			/*GamepadMainMenuHandler.MenuItemPositions.Add( new Vector2( (float)( menu_item_pos_x_base ), (float)( menu_item_pos_y_base + line_height * pos ) + origin.Y ) );
			
			if( GamepadMainMenuHandler.MenuItemPositions.Count == 0 ) {
				Vector2 value2 = new Vector2( (float)Math.Cos( (double)( Main.GlobalTime * 6.28318548f ) ),
					(float)Math.Sin( (double)( Main.GlobalTime * 6.28318548f * 2f ) ) ) * new Vector2( 30f, 15f ) + Vector2.UnitY * 20f;
				UILinkPointNavigator.SetPosition( 2000, new Vector2( (float)Main.screenWidth, (float)Main.screenHeight ) / 2f + value2 );
			}*/
		}
	}



	class MenuItemManager {
		internal IDictionary<int, MenuItem> Items = new Dictionary<int, MenuItem>();

		
		////////////////

		public MenuItemManager() {
			if( !Main.dedServ ) {
				Main.OnPostDraw += this.Draw;
			}
		}

		public void Unload() {
			if( !Main.dedServ ) {
				Main.OnPostDraw -= this.Draw;
			}
		}

		////////////////

		private void Draw( GameTime game_time ) {
			foreach( var kv in this.Items ) {
				if( kv.Value.MenuContext == Main.menuMode ) {
					kv.Value.Draw();
				}
			}
		}
	}
}
