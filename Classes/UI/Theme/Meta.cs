using System;
using Terraria.UI;


namespace HamstarHelpers.Classes.UI.Theme {
	/// <summary>
	/// Interface for all elements that support themes intrinsically.
	/// </summary>
	public interface IThemeable {
		/// <summary>
		/// An alternative to the normal `Append` method to apply theming to appended element.
		/// </summary>
		/// <param name="element"></param>
		void AppendThemed( UIElement element );
		/// <summary></summary>
		void RefreshTheme();
		/// <summary>
		/// Refreshes theme for contained elements.
		/// </summary>
		/// <param name="element"></param>
		void RefreshThemeForChild( UIElement element );
		/// <summary></summary>
		/// <param name="theme"></param>
		void SetTheme( UITheme theme );
	}




	/// <summary>
	/// Indicates an element is meant to have a theme.
	/// </summary>
	[AttributeUsage( AttributeTargets.Field | AttributeTargets.Property,
		AllowMultiple = false,
		Inherited = true )]
	public abstract class ThemedAttrbute : Attribute { }




	/// <summary>
	/// Indicates an element is meant to have a theme as if a panel.
	/// </summary>
	[AttributeUsage( AttributeTargets.Field | AttributeTargets.Property,
		AllowMultiple = false,
		Inherited = true )]
	public class PanelThemeAttribute : ThemedAttrbute { }




	/// <summary>
	/// Indicates an element is meant to have a theme as if a text element.
	/// </summary>
	[AttributeUsage( AttributeTargets.Field | AttributeTargets.Property,
		AllowMultiple = false,
		Inherited = true )]
	public class TextThemeAttribute : ThemedAttrbute { }




	/// <summary>
	/// Indicates an element is meant to have a theme as if a list container.
	/// </summary>
	[AttributeUsage( AttributeTargets.Field | AttributeTargets.Property,
		AllowMultiple = false,
		Inherited = true )]
	public class ListContainerThemeAttribute : ThemedAttrbute { }




	/// <summary>
	/// Indicates an element is meant to have a theme as if a list item.
	/// </summary>
	[AttributeUsage( AttributeTargets.Field | AttributeTargets.Property,
		AllowMultiple = false,
		Inherited = true )]
	public class ListItemThemeAttribute : ThemedAttrbute { }




	/// <summary>
	/// Indicates an element is meant to have a theme as if a button.
	/// </summary>
	[AttributeUsage( AttributeTargets.Field | AttributeTargets.Property,
		AllowMultiple = false,
		Inherited = true )]
	public class ButtonThemeAttribute : ThemedAttrbute { }




	/// <summary>
	/// Indicates an element is meant to have a theme as if a text input.
	/// </summary>
	[AttributeUsage( AttributeTargets.Field | AttributeTargets.Property,
		AllowMultiple = false,
		Inherited = true )]
	public class InputThemeAttribute : ThemedAttrbute { }
}
