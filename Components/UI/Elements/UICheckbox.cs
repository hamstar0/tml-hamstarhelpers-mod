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
			UICheckbox.CheckboxTexture = HamstarHelpersMod.Instance.GetTexture( "Components/UI/Elements/check_box" );
			UICheckbox.CheckmarkTexture = HamstarHelpersMod.Instance.GetTexture( "Components/UI/Elements/check_mark" );
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

		public UICheckbox( string label, string title, bool is_clickable = true, float text_scale = 1, bool large = false ) : base( label, text_scale, large ) {
			if( Main.netMode != 2 && UICheckbox.CheckboxTexture == null || UICheckbox.CheckmarkTexture == null ) {
				UICheckbox.LoadTextures();
			}

			this.Title = title;
			this.IsClickable = is_clickable;

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
			CalculatedStyle inner_pos = base.GetInnerDimensions();
			Vector2 pos = new Vector2( inner_pos.X, inner_pos.Y - 5 );

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

