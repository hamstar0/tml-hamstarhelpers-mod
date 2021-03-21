using System;
using Terraria.ModLoader.Config.UI;
using HamstarHelpers.Classes.UI.Elements;
using HamstarHelpers.Classes.UI.Theme;


namespace HamstarHelpers.Classes.UI.ModConfig {
	/// <summary>
	/// Implements a ModConfig widget for inputting float values via. text input or slider.
	/// </summary>
	public class FloatInputElement : FloatElement {
		private UITextInputPanel InputElem;



		////////////////

		/// @private
		public override void OnBind() {
			base.OnBind();

			//this.TextDisplayFunction = () => this.labelAttribute?.Label ?? this.memberInfo.Name + ": " + this.GetValue();

			////

			this.InputElem = new UITextInputPanel( UITheme.Vanilla, "Enter decimal" );
			this.InputElem.Opacity = 0.2f;
			this.InputElem.Width.Set( 184f, 0f );
			this.InputElem.Height.Set( 32f, 0f );
			this.InputElem.Left.Set( -92f, 0.5f );
			//this.InputElem.SetPadding( 2f );
			this.InputElem.OnMouseOver += (_, __) => {
				//this.InputElem.Opacity = 1f;
				this.InputElem.Show();
			};
			this.InputElem.OnMouseOut += (_, __) => {
				//this.InputElem.Opacity = 0.2f;
				this.InputElem.Hide();
			};
			this.InputElem.OnUnfocus += () => {
				float val;
				if( float.TryParse( this.InputElem.GetText(), out val ) ) {
					base.SetObject( val );
				} else {
					this.InputElem.SetText( this.GetObject() + "" );
				}
			};
			this.Append( this.InputElem );

			////

			this.InputElem.SetText( this.GetObject().ToString() );
		}


		////////////////

		/// <summary>
		/// Sets the config item's value represented by this widget.
		/// </summary>
		/// <param name="value"></param>
		protected override void SetObject( object value ) {
			base.SetObject( value );
			this.InputElem.SetText( value + "" );
		}

		/// <summary>
		/// Sets the config item's value represented by this widget.
		/// </summary>
		/// <param name="val"></param>
		public void SetFloatValue( float val ) {
			this.SetObject( val );
		}

		////////////////

		/// <summary>
		/// Gets the config value this widget represents.
		/// </summary>
		/// <returns></returns>
		public float GetFloatValue() {
			return (float)this.GetObject();
		}
	}
}
