using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace HamstarHelpers.Components.CustomEntity {
	internal class CustomEntityContractResolver : DefaultContractResolver {
		protected override IList<JsonProperty> CreateProperties( Type type, MemberSerialization member_serialization ) {
			IList<JsonProperty> properties = base.CreateProperties( type, member_serialization );
			
			properties = properties.Where( (p) => {
				if( p.DeclaringType.Name != "CustomEntityCore" ) { return true; }

				switch( p.PropertyName ) {
				case "whoAmI":
				case "lavaWet":
				case "wetCount":
				case "wet":
				case "honeyWet":
				case "oldVelocity":
				case "oldPosition":
				case "velocity":
				case "active":
				case "oldDirection":
				case "BottomRight":
				case "BottomLeft":
				case "Bottom":
				case "TopRight":
				case "TopLeft":
				case "Top":
				case "Right":
				case "Left":
				case "Hitbox":
				case "Size":
				case "Center":
					return false;
				}

				return true;
			} ).ToList();
			
			return properties;
		}
	}



	
	public partial class CustomEntity : PacketProtocolData {
		internal static JsonSerializerSettings SerializerSettings = new JsonSerializerSettings {
			TypeNameHandling = TypeNameHandling.Auto,
			ContractResolver = new CustomEntityContractResolver()
		};


		////////////////

		internal static CustomEntity Deserialize( string data ) {
			return JsonConvert.DeserializeObject<CustomEntity>( data, CustomEntity.SerializerSettings );
		}

		internal static string Serialize( CustomEntity ent ) {
			return JsonConvert.SerializeObject( ent, CustomEntity.SerializerSettings );
		}


		////////////////

		protected override void ReadStream( BinaryReader reader ) {
			CustomEntityCore core = this.Core;

			this.ID = reader.ReadUInt16();

			core.whoAmI = reader.ReadUInt16();
			core.DisplayName = reader.ReadString();
			core.position = new Vector2 {
				X = reader.ReadSingle(),
				Y = reader.ReadSingle()
			};
			core.direction = reader.ReadInt16();
			core.width = reader.ReadUInt16();
			core.height = reader.ReadUInt16();
			core.velocity = new Vector2 {
				X = reader.ReadSingle(),
				Y = reader.ReadSingle()
			};

			CustomEntity new_ent = CustomEntityManager.Instance.CreateEntityFromTemplate( this.ID );

			for( int i = 0; i < new_ent.Components.Count; i++ ) {
				new_ent.Components[i].ReadStreamForwarded( reader );
			}

			this.CopyFrom( new_ent );
		}

		protected override void WriteStream( BinaryWriter writer ) {
			CustomEntityCore core = this.Core;

			writer.Write( (ushort)this.ID );

			writer.Write( (ushort)core.whoAmI );
			writer.Write( (string)core.DisplayName );
			writer.Write( (float)core.position.X );
			writer.Write( (float)core.position.Y );
			writer.Write( (short)core.direction );
			writer.Write( (ushort)core.width );
			writer.Write( (ushort)core.height );
			writer.Write( (float)core.velocity.X );
			writer.Write( (float)core.velocity.Y );

			for( int i=0; i<this.Components.Count; i++ ) {
				this.Components[i].WriteStreamForwarded( writer );
			}
		}
	}
}
