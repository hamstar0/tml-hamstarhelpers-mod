using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.UI.Elements;
using HamstarHelpers.Helpers.DotNET.Reflection;
using System.Reflection;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;


namespace HamstarHelpers.Classes.UI.Theme {
	/// <summary>
	/// Defines a theme to use for common UI color and layout settings. Default values are a Mod Helpers custom theme.
	/// </summary>
	public partial class UITheme {
		/// <returns>Clone of the current object.</returns>
		public UITheme Clone() {
			return (UITheme)this.MemberwiseClone();
		}

		/// <summary>
		/// Copies internal fields into another.
		/// </summary>
		/// <param name="newTheme">Theme to copy from.</param>
		public void Switch( UITheme newTheme ) {
			foreach( FieldInfo field in typeof(UITheme).GetFields() ) {
				field.SetValue( this, field.GetValue( newTheme ) );
			}
		}


		////////////////

		/// <summary>
		/// Applies the current them to the given element, as the element specified. Does not assume anything about
		/// the element to apply theming.
		/// </summary>
		/// <param name="element"></param>
		/// <returns>`false` if the element does not specify how it will be themed.</returns>
		public bool Apply( UIElement element ) {
			foreach( CustomAttributeData attr in element.GetType().CustomAttributes ) {
				if( !attr.AttributeType.IsSubclassOf(typeof(ThemedAttrbute)) ) {
					continue;
				}
				
				if( attr.AttributeType == typeof( PanelThemeAttribute ) ) {
					if( element is UIPanel ) {
						this.ApplyPanel( (UIPanel)element );
						return true;
					}
				} else if( attr.AttributeType == typeof( TextThemeAttribute ) ) {
					if( element is UIText ) {
						this.ApplyText( (UIText)element );
						return true;
					}
				} else if( attr.AttributeType == typeof(ListContainerThemeAttribute) ) {
					if( element is UIPanel ) {
						this.ApplyListContainer( (UIPanel)element );
						return true;
					}
				} else if( attr.AttributeType == typeof(ListItemThemeAttribute) ) {
					if( element is UIPanel ) {
						this.ApplyListItem( (UIPanel)element );
						return true;
					}
				} else if( attr.AttributeType == typeof(ButtonThemeAttribute) ) {
					if( element is UITextPanel<string> ) {
						this.ApplyButton( (UITextPanel<string>)element );
						return true;
					}
				} else if( attr.AttributeType == typeof(InputThemeAttribute) ) {
					if( element is UITextInputPanel ) {
						this.ApplyInput( (UITextInputPanel)element );
						return true;
					}
				}

				throw new ModHelpersException( "Invalid theme Attribute "+attr.AttributeType.Name+" for "+element.GetType().Name );
			}

			if( element is IThemeable ) {
				((IThemeable)element).SetTheme( this );
				return true;
			}

			return false;
		}


		/// <summary>
		/// Attempts to style an element by its type. No additional context is implied (e.g. UIList is not
		/// treated as a list, as only it's inner "container" element is styled).
		/// </summary>
		/// <param name="element"></param>
		public virtual void ApplyByType( UIElement element ) {
			if( element is UITextPanel<string> ) {
				this.ApplyButton( (UITextPanel<string>)element );
			} else if( element is UITextInputPanel ) {
				this.ApplyInput( (UITextInputPanel)element );
			} else if( element is UIText ) {
				this.ApplyText( (UIText)element );
			} else if( element is UIPanel ) {
				this.ApplyPanel( (UIPanel)element );
			}
		}


		////////////////

		/// <summary>
		/// Applies standard panel theming to a UI panel.
		/// </summary>
		/// <param name="panel"></param>
		public virtual void ApplyPanel( UIPanel panel ) {
			panel.BackgroundColor = this.MainBgColor;
			panel.BorderColor = this.MainEdgeColor;
		}

		/// <summary>
		/// Applies standard "header" panel theming to a UI panel.
		/// </summary>
		/// <param name="panel"></param>
		public virtual void ApplyHeader( UIPanel panel ) {
			panel.BackgroundColor = this.HeadBgColor;
			panel.BorderColor = this.HeadEdgeColor;
		}

		////////////////

		/// <summary>
		/// Applies standard text color to a UI text element.
		/// </summary>
		/// <param name="elem"></param>
		public virtual void ApplyText( UIText elem ) {
			elem.TextColor = this.MainTextColor;
		}

		////////////////

		/// <summary>
		/// Applies standard text input theming to a UI text field.
		/// </summary>
		/// <param name="panel"></param>
		public virtual void ApplyInput( UITextInputPanel panel ) {
			panel.BackgroundColor = this.InputBgColor;
			panel.BorderColor = this.InputEdgeColor;
			panel.TextColor = this.InputTextColor;
		}

		/// <summary>
		/// Applies standard text input theming to a UI text area.
		/// </summary>
		/// <param name="panel"></param>
		public virtual void ApplyInput( UITextInputAreaPanel panel ) {
			panel.BackgroundColor = this.InputBgColor;
			panel.BorderColor = this.InputEdgeColor;
			panel.TextColor = this.InputTextColor;
		}

		/// <summary>
		/// Applies standard text input theming to any UI panel that accepts text colors.
		/// </summary>
		/// <param name="panel"></param>
		public virtual void ApplyInput( UIPanel panel ) {
			panel.BackgroundColor = this.InputBgColor;
			panel.BorderColor = this.InputEdgeColor;
			ReflectionHelpers.Set( panel, "TextColor", this.InputTextColor );
		}

		////

		/// <summary>
		/// Applies standard disabled text input theming to a UI text area.
		/// </summary>
		/// <param name="panel"></param>
		public virtual void ApplyInputDisable( UITextInputAreaPanel panel ) {
			panel.BackgroundColor = this.InputBgDisabledColor;
			panel.BorderColor = this.InputEdgeDisabledColor;
			panel.TextColor = this.InputTextDisabledColor;
		}

		/// <summary>
		/// Applies standard disabled text input theming to a UI text area.
		/// </summary>
		/// <param name="panel"></param>
		public virtual void ApplyInputDisable( UIPanel panel ) {
			panel.BackgroundColor = this.InputBgDisabledColor;
			panel.BorderColor = this.InputEdgeDisabledColor;
			ReflectionHelpers.Set( panel, "TextColor", this.InputTextDisabledColor );
		}

		////////////////

		/// <summary>
		/// Applies standard button theming to a UI text panel button.
		/// </summary>
		/// <param name="panel"></param>
		public virtual void ApplyButton<T>( UITextPanel<T> panel ) {
			panel.BackgroundColor = this.ButtonBgColor;
			panel.BorderColor = this.ButtonEdgeColor;
			panel.TextColor = this.ButtonTextColor;
		}

		/// <summary>
		/// Applies standard 'lit' button theming to a UI text panel button.
		/// </summary>
		/// <param name="panel"></param>
		public virtual void ApplyButtonLit<T>( UITextPanel<T> panel ) {
			panel.BackgroundColor = this.ButtonBgLitColor;
			panel.BorderColor = this.ButtonEdgeLitColor;
			panel.TextColor = this.ButtonTextLitColor;
		}

		/// <summary>
		/// Applies standard disabled button theming to a UI text panel button.
		/// </summary>
		/// <param name="panel"></param>
		public virtual void ApplyButtonDisable<T>( UITextPanel<T> panel ) {
			panel.BackgroundColor = this.ButtonBgDisabledColor;
			panel.BorderColor = this.ButtonEdgeDisabledColor;
			panel.TextColor = this.ButtonTextDisabledColor;
		}

		////////////////

		/// <summary>
		/// Applies standard list container theming to a UI panel.
		/// </summary>
		/// <param name="panel"></param>
		public virtual void ApplyListContainer( UIPanel panel ) {
			panel.BackgroundColor = this.ListBgColor;
			panel.BorderColor = this.ListEdgeColor;
		}

		/// <summary>
		/// Applies standard list item theming to a UI panel.
		/// </summary>
		/// <param name="panel"></param>
		public virtual void ApplyListItem( UIPanel panel ) {
			panel.BackgroundColor = this.ListItemBgColor;
			panel.BorderColor = this.ListItemEdgeColor;
		}

		/// <summary>
		/// Applies standard 'lit' list item theming to a UI panel.
		/// </summary>
		/// <param name="panel"></param>
		public virtual void ApplyListItemLit( UIPanel panel ) {
			panel.BackgroundColor = this.ListItemBgLitColor;
			panel.BorderColor = this.ListItemEdgeLitColor;
		}

		/// <summary>
		/// Applies standard 'selected' list item theming to a UI panel.
		/// </summary>
		/// <param name="panel"></param>
		public virtual void ApplyListItemSelected( UIPanel panel ) {
			panel.BackgroundColor = this.ListItemBgSelectedColor;
			panel.BorderColor = this.ListItemEdgeSelectedColor;
		}
	}
}
