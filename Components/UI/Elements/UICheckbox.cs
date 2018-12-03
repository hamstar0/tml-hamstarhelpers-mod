using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using System;
using Terraria;


namespace HamstarHelpers.Components.UI.Elements {
	public class UICheckbox : UIText {
		public static Texture2D CheckboxTexture { get; private set; }
		public static Texture2D CheckmarkTexture { get; private set; }


		////////////////

		static UICheckbox() {
			UICheckbox.CheckboxTexture = null;
			UICheckbox.CheckmarkTexture = null;
		}

		internal static void LoadTextures() {
			UICheckbox.CheckboxTexture = ModHelpersMod.Instance.GetTexture( "Components/UI/Elements/check_box" );
			UICheckbox.CheckmarkTexture = ModHelpersMod.Instance.GetTexture( "Components/UI/Elements/check_mark" );
		}


		////////////////

		public event Action OnSelectedChanged = null;
		public float Order = 0f;
		public bool IsClickable = true;
		public string Title = "";

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


		////////////////

		public UICheckbox( string label, string title, bool isClickable = true, float textScale = 1, bool large = false )
				: base( label, textScale, large ) {
			if( Main.netMode != 2 && UICheckbox.CheckboxTexture == null || UICheckbox.CheckmarkTexture == null ) {
				UICheckbox.LoadTextures();
			}

			this.Title = title;
			this.IsClickable = isClickable;

			this.SetText( "   " + label );
			this.Recalculate();
		}

		public override void Click( UIMouseEvent evt ) {
			if( this.IsClickable ) {
				this.Selected = !this.Selected;
			}
			this.Recalculate();
		}


		////////////////
		
		protected override void DrawSelf( SpriteBatch sb ) {
			CalculatedStyle innerPos = base.GetInnerDimensions();
			Vector2 pos = new Vector2( innerPos.X, innerPos.Y - 5 );

			sb.Draw( UICheckbox.CheckboxTexture, pos, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f );
			if( this.Selected ) {
				sb.Draw( UICheckbox.CheckmarkTexture, pos, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f );
			}

			base.DrawSelf( sb );

			if( this.IsMouseHovering && this.Title.Length > 0 ) {
				Main.HoverItem = new Item();
				Main.hoverItemName = this.Title;
			}
		}


		////////////////

		public override int CompareTo( object obj ) {
			try {
				UICheckbox other = obj as UICheckbox;
				return this.Order.CompareTo( other.Order );
			} catch( Exception ) {
				return 0;
			}
		}
	}
}

