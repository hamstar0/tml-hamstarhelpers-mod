using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using System;
using Terraria;


namespace HamstarHelpers.UIHelpers.Elements {
	[Obsolete( "HamstarHelpers.Components.UI.Elements.UICheckbox", true )]
	public class UICheckbox : UIText {
		[Obsolete( "HamstarHelpers.Components.UI.Elements.UICheckbox", true )]
		public static Texture2D CheckboxTexture { get; private set; }
		[Obsolete( "HamstarHelpers.Components.UI.Elements.UICheckbox", true )]
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

		[Obsolete( "HamstarHelpers.Components.UI.Elements.UICheckbox", true )]
		public event Action OnSelectedChanged = null;
		[Obsolete( "HamstarHelpers.Components.UI.Elements.UICheckbox", true )]
		public float Order = 0f;
		[Obsolete( "HamstarHelpers.Components.UI.Elements.UICheckbox", true )]
		public bool IsClickable = true;
		[Obsolete( "HamstarHelpers.Components.UI.Elements.UICheckbox", true )]
		public string Title = "";

		private bool _selected = false;
		[Obsolete( "HamstarHelpers.Components.UI.Elements.UICheckbox", true )]
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

		[Obsolete( "HamstarHelpers.Components.UI.Elements.UICheckbox", true )]
		public UICheckbox( string label, string title, bool is_clickable = true, float text_scale = 1, bool large = false ) : base( label, text_scale, large ) {
			if( Main.netMode != 2 && UICheckbox.CheckboxTexture == null || UICheckbox.CheckmarkTexture == null ) {
				UICheckbox.LoadTextures();
			}

			this.Title = title;
			this.IsClickable = is_clickable;

			this.SetText( "   " + label );
			this.Recalculate();
		}

		[Obsolete( "HamstarHelpers.Components.UI.Elements.UICheckbox", true )]
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

		[Obsolete( "HamstarHelpers.Components.UI.Elements.UICheckbox", true )]
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

