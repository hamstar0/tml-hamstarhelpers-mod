using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using HamstarHelpers.Helpers.Debug;


namespace HamstarHelpers.Services.ModTagDefinitions {
	/// <summary></summary>
	public enum TagFlavor {
		/// <summary>Normal descriptive tag.</summary>
		Mundane,
		/// <summary>Technical descriptive tag.</summary>
		Specification,
		/// <summary>Describes a focal aspect of a mod.</summary>
		Emphasis,
		/// <summary>An important-to-note specification.</summary>
		Important,
		/// <summary>A technically-important specification; not a general description.</summary>
		TechnicallyImportant,
		/// <summary>Buyer beware.</summary>
		Alert,
		/// <summary>Even more buyer beware.</summary>
		Warning,
		/// <summary>Something is lacking.</summary>
		Deficient,
		/// <summary>Something doesn't work right.</summary>
		Broken,
		/// <summary>Subjectively bad.</summary>
		IllFavored
	}




	/// <summary>
	/// Describes a basic attribute of a given mod. Meant to be combined in sets to create a comprehensive
	/// categorical description of a mod.
	/// </summary>
	public partial class ModTagDefinition {
		/// <summary>
		/// Map of mod tag "flavors" to corresponding hues.
		/// </summary>
		public readonly static IReadOnlyDictionary<TagFlavor, Color> TagFlavorColors =
			new ReadOnlyDictionary<TagFlavor, Color>(
				new Dictionary<TagFlavor, Color> {
					{ TagFlavor.Mundane, Color.Silver * 0.8f },
					{ TagFlavor.Specification, Color.Silver },
					{ TagFlavor.Emphasis, Color.Silver * 1.1f },
					{ TagFlavor.Important, Color.Blue },
					{ TagFlavor.TechnicallyImportant, Color.Lerp(Color.Blue, Color.White, 0.5f) },
					{ TagFlavor.Alert, Color.Yellow },
					{ TagFlavor.Warning, Color.Purple },
					{ TagFlavor.Deficient, Color.SlateGray },
					{ TagFlavor.Broken, Color.Red },
					{ TagFlavor.IllFavored, Color.Tomato },
				}
			);



		////////////////

		/// <summary>
		/// Descriptive tag.
		/// </summary>
		public string Tag { get; private set; }
		/// <summary>
		/// Category for the tag.
		/// </summary>
		public string Category { get; private set; }
		/// <summary>
		/// Human-readable description of the given tag.
		/// </summary>
		public string Description { get; private set; }
		/// <summary>
		/// Describes the "flavor" or practical signicance of the given tag.
		/// </summary>
		public TagFlavor Flavor { get; private set; }
		/// <summary>
		/// Tags that are forced to apply in tandem with the current.
		/// </summary>
		public ISet<string> ForcesTags { get; private set; }
		/// <summary>
		/// Tags that are mutually exclusive with the current.
		/// </summary>
		public ISet<string> ExcludesOnAdd { get; private set; }



		////////////////

		/// <summary></summary>
		/// <param name="tag"></param>
		/// <param name="category"></param>
		/// <param name="description"></param>
		/// <param name="flavor"></param>
		/// <param name="forcesTag"></param>
		/// <param name="excludesOnAdd"></param>
		private ModTagDefinition( string tag,
				string category,
				string description,
				TagFlavor flavor,
				ISet<string> forcesTag = null,
				ISet<string> excludesOnAdd = null ) {
			this.Tag = tag;
			this.Category = category;
			this.Description = description;
			this.Flavor = flavor;
			this.ForcesTags = forcesTag ?? new HashSet<string>();
			this.ExcludesOnAdd = excludesOnAdd ?? new HashSet<string>();
		}


		////////////////

		/// <summary>
		/// Gets the "flavor" color of the current tag.
		/// </summary>
		/// <returns></returns>
		public Color GetFlavorColor() {
			return ModTagDefinition.TagFlavorColors[ this.Flavor ];
		}
	}
}
