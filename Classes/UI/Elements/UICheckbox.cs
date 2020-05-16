using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.UI;
using HamstarHelpers.Classes.UI.Theme;


namespace HamstarHelpers.Classes.UI.Elements {
	/// <summary>
	/// Defines a UI checkbox (toggleable button) element.
	/// </summary>
	public class UICheckbox : UIThemedText {
		/// <summary></summary>
		public Texture2D CheckboxTexture;
		/// <summary></summary>
		public Texture2D CheckmarkTexture;

		////

		/// <summary>
		/// Hooks changes to the button state.
		/// </summary>
		public event Action OnSelectedChanged = null;

		/// <summary>
		/// Allows defining a custom sort order value (for putting in an ordered list).
		/// </summary>
		public float Order = 0f;

		/// <summary>
		/// Enables mouse interactivity.
		/// </summary>
		public bool IsClickable = true;

		/// <summary>
		/// Mouse hover popup label.
		/// </summary>
		public string Title = "";

		private bool _selected = false;
		/// <summary>
		/// Indicates if the box is "checked".
		/// </summary>
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

		/// <param name="theme">Appearance style.</param>
		/// <param name="label">Display text next to checkbox control.</param>
		/// <param name="title">Mouse hover popup label.</param>
		/// <param name="isClickable">Enables mouse interactivity.</param>
		/// <param name="textScale">Multiplies label text size.</param>
		/// <param name="large">Uses 'large' label text style.</param>
		public UICheckbox( UITheme theme,
				string label,
				string title,
				bool isClickable = true,
				float textScale = 1,
				bool large = false )
				: base( theme, true, label, textScale, large ) {
			if( Main.netMode != NetmodeID.Server ) {
				this.CheckboxTexture = ModHelpersMod.Instance.GetTexture( "Classes/UI/Elements/check_box" );
				this.CheckmarkTexture = ModHelpersMod.Instance.GetTexture( "Classes/UI/Elements/check_mark" );
			}

			this.Title = title;
			this.IsClickable = isClickable;

			this.SetText( "   " + label );
			this.Recalculate();
		}

		/// <summary>
		/// Called on click. Can be called manually.
		/// </summary>
		/// <param name="evt">Unused.</param>
		public override void Click( UIMouseEvent evt ) {
			if( this.IsClickable ) {
				this.Selected = !this.Selected;
			}
			this.Recalculate();
		}


		////////////////
		
		/// @private
		protected override void DrawSelf( SpriteBatch sb ) {
			CalculatedStyle innerPos = base.GetInnerDimensions();
			Vector2 pos = new Vector2( innerPos.X, innerPos.Y - 5 );

			sb.Draw( this.CheckboxTexture, pos, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f );
			if( this.Selected ) {
				sb.Draw( this.CheckmarkTexture, pos, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f );
			}

			base.DrawSelf( sb );

			if( this.IsMouseHovering && this.Title.Length > 0 ) {
				Main.HoverItem = new Item();
				Main.hoverItemName = this.Title;
			}
		}


		////////////////

		/// <summary>
		/// Decides sort order in a list.
		/// </summary>
		/// <param name="obj">Object to compare rank to.</param>
		/// <returns>Value representing greater-than or less-than sortion status relative to the given comparison object.</returns>
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

