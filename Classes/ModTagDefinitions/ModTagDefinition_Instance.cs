using HamstarHelpers.Helpers.Debug;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace HamstarHelpers.Classes.ModTagDefinitions {
	/// <summary></summary>
	public enum TagFlavor {
		/// <summary></summary>
		Specification,
		/// <summary></summary>
		Important,
		/// <summary></summary>
		TechnicallyImportant,
		/// <summary></summary>
		NonStandard,
		/// <summary></summary>
		Risky,
		/// <summary></summary>
		Problematic,
		/// <summary></summary>
		Broken
	}




	public partial class ModTagDefinition {
		public readonly static IReadOnlyDictionary<TagFlavor, Color> TagFlavorColors =
			new ReadOnlyDictionary<TagFlavor, Color>(
				new Dictionary<TagFlavor, Color> {
					{ TagFlavor.Specification, Color.Silver },
					{ TagFlavor.Important, Color.Blue },
					{ TagFlavor.TechnicallyImportant, Color.SkyBlue },
					{ TagFlavor.NonStandard, Color.Yellow },
					{ TagFlavor.Risky, Color.Purple },
					{ TagFlavor.Problematic, Color.SlateGray },
					{ TagFlavor.Broken, Color.Red },
				}
			);

		/// <summary>
		/// Matches a color with a given mod tag. Used to represent tags that should be emphasized.
		/// </summary>
		/// <param name="tag">Mod tag</param>
		/// <returns>Color of tag.</returns>
		public static Color GetTagColor( string tag ) {
			switch( tag ) {
			// Important tags:
			case "MP Compatible":
				return Color.Blue;
			case "Needs New World":
			case "Needs New Player":
				return Color.SkyBlue;
			// Negative tags:
			case "May Lag":
			case "Cheat-like":
				return Color.Yellow;
			case "Non-functional":
				return Color.Red;
			case "Misleading Info":
			case "Buggy":
				return Color.Purple;
			case "Unimaginative":
			case "Low Effort":
			case "Unoriginal Content":
				return Color.Tomato;
			case "Unmaintained":
			case "Unfinished":
				return Color.SlateGray;
			default:
				return Color.Silver;
			}
		}



		////////////////

		public string Tag { get; private set; }
		public string Category { get; private set; }
		public string Description { get; private set; }
		public TagFlavor Flavor { get; private set; }
		public ISet<string> ForcesTags { get; private set; }
		public ISet<string> ExcludesOnAdd { get; private set; }



		////////////////

		public ModTagDefinition( string tag,
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

		public Color GetColor() {
			return ModTagDefinition.TagFlavorColors[ this.Flavor ];
		}
	}
}
