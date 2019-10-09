using HamstarHelpers.Classes.UI.Elements;
using HamstarHelpers.Classes.UI.Theme;
using System;
using Terraria.ModLoader.Config.UI;


namespace HamstarHelpers.Classes.UI.ModConfig {
	/// @private
	public class AccessibleFloatElement : FloatElement {
		/// @private
		public delegate void ChangeEvent( float oldValue );



		////////////////

		/// @private
		public event ChangeEvent OnChange;



		////////////////

		/// @private
		protected override void SetObject( object value ) {
			var oldVal = (float)this.GetObject();

			base.SetObject( value );
			this.OnChange?.Invoke( oldVal );
		}

		////////////////

		/// @private
		public float GetFloatValue() {
			return (float)this.GetObject();
		}

		/// @private
		public void SetFloatValue( float val ) {
			this.SetObject( val );
		}
	}




	/// <summary>
	/// Implements a text field for inputting float values.
	/// </summary>
	public class ConfigFloatInput : ConfigElement {
		private AccessibleFloatElement FloatElem;
		private UITextField InputElem;



		////////////////

		/// @private
		public override void OnBind() {
			base.OnBind();

			this.TextDisplayFunction = () => "blah";

			////

			this.FloatElem = new AccessibleFloatElement();
			this.FloatElem.OnChange += ( _ ) => {
				float val = this.FloatElem.GetFloatValue();
				this.SetObject( val );
				this.InputElem.SetText( val.ToString() );
			};
			this.Append( this.FloatElem );

			this.InputElem = new UITextField( UITheme.Vanilla, "Enter decimal number value" );
			this.InputElem.OnTextChange += ( _, e ) => {
				float val;
				if( float.TryParse( e.Text, out val ) ) {
					this.SetObject( val );
					this.FloatElem.SetFloatValue( val );
				}
			};
			this.Append( this.InputElem );

			////

			this.FloatElem.SetFloatValue( (float)this.GetObject() );
			this.InputElem.SetText( this.GetObject().ToString() );
		}
	}
}
