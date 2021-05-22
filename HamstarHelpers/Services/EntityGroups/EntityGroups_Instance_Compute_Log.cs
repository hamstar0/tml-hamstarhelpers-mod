using HamstarHelpers.Classes.DataStructures;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Libraries.DotNET;
using HamstarHelpers.Libraries.Items.Attributes;
using HamstarHelpers.Libraries.NPCs.Attributes;
using HamstarHelpers.Libraries.Projectiles.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Services.EntityGroups {
	/// <summary>
	/// Supplies collections of named entity groups based on traits shared between entities. Groups are either items, NPCs,
	/// or projectiles. Must be enabled on mod load to be used (note: collections may require memory).
	/// </summary>
	public partial class EntityGroups {
		private static void LogGroup( Type type, string groupName, IReadOnlySet<int> entIds ) {
			switch( groupName ) {
			case "Any Item":
			case "Any NPC":
			case "Any Projectile":
				return;
			}

			IList<string> entNames = entIds.SafeSelect(
				(itemType) => {
					switch( type.Name ) {
					case "Item":
						return ItemAttributeLibraries.GetQualifiedName( itemType );
					case "NPC":
						return NPCAttributeLibraries.GetQualifiedName( itemType );
					case "Projectile":
						return ProjectileAttributeLibraries.GetQualifiedName( itemType );
					default:
						throw new NotImplementedException();
					}
				}
			).ToList();

			var entNameChunks = new List<string>();
			var chunk = new List<string>();

			for( int i=0; i<entNames.Count; i++ ) {
				chunk.Add( entNames[i] );

				if( chunk.Count >= 10 ) {
					entNameChunks.Add( string.Join(", ", chunk) );
					chunk.Clear();
				}
			}

			if( chunk.Count > 0 ) {
				entNameChunks.Add( string.Join( ", ", chunk ) );
			}

			ModHelpersMod.Instance.Logger.Info( "\"" + groupName + "\" (" + type.Name + ") - "
				+ ( entIds.Count > 0 ? "[\n  " : "[" )
				+ string.Join( ",\n  ", entNameChunks )
				+ ( entIds.Count > 0 ? "\n]" : "]" )
			);
		}
	}
}
