using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
				if( p.DeclaringType.Name != "Entity" ) { return true; }

				switch( p.PropertyName ) {
				//case "whoAmI":
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



	internal class CustomEntityConverter : JsonConverter {
		public override bool CanWrite {
			get { return false; }
		}

		public override bool CanConvert( Type obj_type ) {
			return obj_type == typeof( CustomEntity );
		}


		public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer ) {
			IEnumerable<Type> all_comp_types = ReflectionHelpers.GetAllAvailableSubTypes( typeof( CustomEntityComponent ) );
			IDictionary<string, Type> all_comp_type_map = all_comp_types.ToDictionary( t => t.Name, t => t );

			JObject jo = JObject.Load( reader );

			CustomEntityCore core = jo["Core"].ToObject<CustomEntityCore>();
			IList<CustomEntityComponent> components = new List<CustomEntityComponent>();
			int i;

			string[] comp_names = jo["ComponentNames"].ToObject<string[]>();
			Type[] comp_types = new Type[ comp_names.Length ];

			for( i=0; i<comp_names.Length; i++ ) {
				if( !all_comp_type_map.ContainsKey( comp_names[i] ) ) {
					return null;
				}
				comp_types[i] = all_comp_type_map[ comp_names[i] ];
			}
			
			i = 0;
			foreach( JObject obj in jo["Components"] ) {
				Type comp_type = comp_types[i];
				object comp = obj.ToObject( comp_type, serializer );

				components.Add( (CustomEntityComponent)comp );
				i++;
			}

			return new CustomEntity( core, components );
		}


		public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer ) {
			throw new NotImplementedException();
		}
	}




	public partial class CustomEntity : PacketProtocolData {
		internal static JsonSerializerSettings SerializerSettings = new JsonSerializerSettings {
			TypeNameHandling = TypeNameHandling.None,
			ContractResolver = new CustomEntityContractResolver(),
			Converters = new List<JsonConverter> { new CustomEntityConverter() }
		};


		////////////////
		
		protected override void ReadStream( BinaryReader reader ) {
			var core = new CustomEntityCore( "", 1, 1 );
			this.Core = core;

			this.ID = (ushort)reader.ReadUInt16();

			core.whoAmI = (ushort)reader.ReadUInt16();
			core.DisplayName = (string)reader.ReadString();
			core.position = new Vector2 {
				X = (float)reader.ReadSingle(),
				Y = (float)reader.ReadSingle()
			};
			core.direction = (short)reader.ReadInt16();
			core.width = (ushort)reader.ReadUInt16();
			core.height = (ushort)reader.ReadUInt16();
			core.velocity = new Vector2 {
				X = (float)reader.ReadSingle(),
				Y = (float)reader.ReadSingle()
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
