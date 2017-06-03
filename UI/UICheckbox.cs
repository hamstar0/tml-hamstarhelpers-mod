using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using System;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.UI {
	public class UICheckbox : UIText {  // Blatantly lifted from Jopo's mod
		public static Texture2D CheckboxTexture { get; private set; }
		public static Texture2D CheckmarkTexture { get; private set; }

		static UICheckbox() {
			UICheckbox.CheckboxTexture = null;
			UICheckbox.CheckmarkTexture = null;
		}


		public event Action OnSelectedChanged = null;
		public float Order = 0f;
		public bool Clickable = true;
		public string Tooltip = "";

		private bool _selected = false;
		public bool Selected {
			get { return this._selected; }
			set {
				if( value != this._selected ) {
					this._selected = value;
					if( this.OnSelectedChanged != null ) {
						this.OnSelectedChanged.Invoke();
					}
				}
			}
		}



		public UICheckbox( string text, string tooltip, bool clickable = true, float textScale = 1, bool large = false ) : base( text, textScale, large ) {
			if( Main.netMode != 2 && UICheckbox.CheckboxTexture == null || UICheckbox.CheckmarkTexture == null ) {
				var mymod = (HamstarHelpersMod)ModLoader.GetMod( "HamstarHelpers" );
				UICheckbox.CheckboxTexture = mymod.GetTexture( "UI/checkBox" );
				UICheckbox.CheckmarkTexture = mymod.GetTexture( "UI/checkMark" );
			}

			this.Tooltip = tooltip;
			this.Clickable = clickable;

			this.SetText( "   " + text );
			this.Recalculate();
		}

		public override void Click( UIMouseEvent evt ) {
			if( this.Clickable ) {
				this.Selected = !this.Selected;
			}
			this.Recalculate();
		}

		protected override void DrawSelf( SpriteBatch sb ) {
			CalculatedStyle inner_pos = base.GetInnerDimensions();
			Vector2 pos = new Vector2( inner_pos.X, inner_pos.Y - 5 );

			sb.Draw( UICheckbox.CheckboxTexture, pos, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f );
			if( this.Selected ) {
				sb.Draw( UICheckbox.CheckmarkTexture, pos, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f );
			}

			base.DrawSelf( sb );

			if( this.IsMouseHovering && this.Tooltip.Length > 0 ) {
				Main.toolTip = new Item();
				Main.hoverItemName = this.Tooltip;
			}
		}

		public override int CompareTo( object obj ) {
			UICheckbox other = obj as UICheckbox;
			return this.Order.CompareTo( other.Order );
		}
	}
}

