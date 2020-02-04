using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.XNA;
using HamstarHelpers.Services.Timers;


namespace HamstarHelpers.Classes.UI.Menu {
	/// <summary>
	/// A clickable menu text item. Does not affect positions of other existing vanilla menu items, and is entirely
	/// light-weight.
	/// </summary>
	public class MenuItem {
		/// <summary>
		/// Adds a menu item.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="offsetY">Vertical pixel position. `MenuItem.MenuTopPosition` is the usual base line for menu
		/// items.</param>
		/// <param name="menuContext">Which `Main.menuMode` this belongs to.</param>
		/// <param name="myAction">Action to perform on click.</param>
		public static void AddMenuItem( string text, int offsetY, int menuContext, Action myAction ) {
			var mymod = ModHelpersMod.Instance;
			var item = new MenuItem( text, offsetY, menuContext, myAction );
			string key = text + "." + offsetY + "." + menuContext;
			
			mymod.MenuItemMngr.Items[ key ] = item;
		}



		////////////////

		/// <summary>
		/// The usual base line position for menu items.
		/// </summary>
		public const int MenuTopPosition = 250;

		/// <summary></summary>
		public string MenuText { get; private set; }
		/// <summary>Offset from top of screen.</summary>
		public int OffsetY { get; private set; }
		/// <summary>Which `Main.menuMode` this belongs to.</summary>
		public int MenuContext { get; private set; }
		/// <summary>Action to perform on click.</summary>
		public Action MyAction { get; private set; }

		private bool MenuItemHovered = false;
		private float MenuItemScale = 0.8f;



		////////////////

		internal MenuItem( string text, int offsetY, int menuContext, Action myAction ) {
			this.MenuText = text;
			this.OffsetY = offsetY;
			this.MenuContext = menuContext;
			this.MyAction = myAction;
		}


		////////////////

		internal void Draw() {
			if( Main.mouseLeft && this.IsMouseOverMenuItem() ) {
				this.MenuItemScale = 0.8f;

				Main.PlaySound( 11 );

				Timers.RunNow( () => {
					this.MyAction();
				} );
			} else {
				bool _;
				XNAHelpers.DrawBatch( (sb) => this.DrawMenuItem(),
					SpriteSortMode.Deferred,
					BlendState.AlphaBlend,
					SamplerState.LinearClamp,
					DepthStencilState.None,
					RasterizerState.CullCounterClockwise,
					null,
					Main.UIScaleMatrix,
					out _
				);
			}
		}


		internal bool IsMouseOverMenuItem() {
			int relOffsetY = this.OffsetY < 0 ? Main.screenHeight + this.OffsetY : this.OffsetY;
			
			Vector2 dim = Main.fontDeathText.MeasureString( this.MenuText );
			Vector2 origin = dim * 0.5f;

			float basePosX = Main.screenWidth / 2;
			float basePosY = relOffsetY + origin.Y;

			return Main.mouseX >= basePosX - ( dim.X * 0.5f ) && Main.mouseX <= basePosX + ( dim.X * 0.5f ) &&
					Main.mouseY >= basePosY - ( dim.Y * 0.5f ) && Main.mouseY <= basePosY + ( dim.Y * 0.5f );
		}


		////////////////

		internal void DrawMenuItem() {
			int relOffsetY = this.OffsetY < 0 ? Main.screenHeight + this.OffsetY : this.OffsetY;

			byte b = (byte)( ( 255 + Main.tileColor.R * 2 ) / 3 );
			Color color = new Color( (int)b, (int)b, (int)b, 255 );

			Vector2 dim = Main.fontDeathText.MeasureString( this.MenuText );
			Vector2 origin = dim * 0.5f;

			float basePosX = Main.screenWidth / 2;
			float basePosY = relOffsetY + origin.Y;

			bool isSelected = Main.mouseX >= basePosX - ( dim.X * 0.5f ) && Main.mouseX <= basePosX + ( dim.X * 0.5f ) &&
							   Main.mouseY >= basePosY - ( dim.Y * 0.5f ) && Main.mouseY <= basePosY + ( dim.Y * 0.5f );

			if( isSelected && !this.MenuItemHovered ) {
				Main.PlaySound( 12 );
			}
			this.MenuItemHovered = isSelected;

			if( this.MenuItemHovered ) {
				this.MenuItemScale = this.MenuItemScale < 1f ? this.MenuItemScale + 0.02f : 1f;
			} else {
				this.MenuItemScale = this.MenuItemScale > 0.8f ? this.MenuItemScale - 0.02f : 0.8f;
			}

			for( int i = 0; i < 5; i++ ) {
				Color menuItemColor = i == 4 ? color : Color.Black;

				if( this.MenuItemHovered ) {
					if( i == 4 ) {
						int alpha = 255;
						int red = (int)menuItemColor.R;
						int green = (int)menuItemColor.G;
						int blue = (int)menuItemColor.B;

						float alphaScale = (float)alpha / 255f;

						red = (int)( (float)red * ( 1f - alphaScale ) + 255f * alphaScale );
						green = (int)( (float)green * ( 1f - alphaScale ) + 215f * alphaScale );
						blue = (int)( (float)blue * ( 1f - alphaScale ) );

						menuItemColor = new Color( (byte)red, (byte)green, (byte)blue, (byte)alpha );
					}
				} else {
					menuItemColor *= 0.5f;
				}

				int xShift = 0;
				int yShift = 0;

				if( i == 0 ) {
					xShift = -2;
				}
				if( i == 1 ) {
					xShift = 2;
				}
				if( i == 2 ) {
					yShift = -2;
				}
				if( i == 3 ) {
					yShift = 2;
				}

				float menuItemScale = this.MenuItemScale;

				Vector2 drawPos = new Vector2( basePosX, basePosY );
				drawPos.X += xShift;
				drawPos.Y += yShift;

				if( Main.netMode == 2 ) {
					menuItemScale *= 0.5f;
				}

				Main.spriteBatch.DrawString( Main.fontDeathText, this.MenuText, drawPos, menuItemColor, 0f, origin, menuItemScale, SpriteEffects.None, 0f );
			}

			Vector2 bonus = Main.DrawThickCursor( false );
			Main.DrawCursor( bonus, false );

			/*GamepadMainMenuHandler.MenuItemPositions.Add( new Vector2( (float)( menuItemPosXBase ), (float)( menuItemPoYBase + lineHeight * pos ) + origin.Y ) );
			
			if( GamepadMainMenuHandler.MenuItemPositions.Count == 0 ) {
				Vector2 value2 = new Vector2( (float)Math.Cos( (double)( Main.GlobalTime * 6.28318548f ) ),
					(float)Math.Sin( (double)( Main.GlobalTime * 6.28318548f * 2f ) ) ) * new Vector2( 30f, 15f ) + Vector2.UnitY * 20f;
				UILinkPointNavigator.SetPosition( 2000, new Vector2( (float)Main.screenWidth, (float)Main.screenHeight ) / 2f + value2 );
			}*/
		}
	}
}
