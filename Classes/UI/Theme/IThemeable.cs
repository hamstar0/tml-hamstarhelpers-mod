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

		/// <summary></summary>
		void Show();

		/// <summary></summary>
		void Hide();
	}
}
