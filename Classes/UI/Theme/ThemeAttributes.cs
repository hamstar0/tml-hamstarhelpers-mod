using System;
using Terraria.UI;


namespace HamstarHelpers.Classes.UI.Theme {
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
