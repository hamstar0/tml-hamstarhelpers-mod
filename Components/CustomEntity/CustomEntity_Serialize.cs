using HamstarHelpers.Components.CustomEntity.Templates;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace HamstarHelpers.Components.CustomEntity {
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
			ContractResolver = new CustomEntityCoreContractResolver(),
			Converters = new List<JsonConverter> { new CustomEntityConverter() }
		};


		////////////////
		
		protected override void ReadStream( BinaryReader reader ) {
			CustomEntity new_ent = CustomEntityTemplateManager.CreateEntityByID( (ushort)reader.ReadUInt16() );

			new_ent.Core.whoAmI = (ushort)reader.ReadUInt16();
			new_ent.Core.DisplayName = (string)reader.ReadString();
			new_ent.Core.position = new Vector2 {
				X = (float)reader.ReadSingle(),
				Y = (float)reader.ReadSingle()
			};
			new_ent.Core.direction = (short)reader.ReadInt16();
			new_ent.Core.width = (ushort)reader.ReadUInt16();
			new_ent.Core.height = (ushort)reader.ReadUInt16();
			new_ent.Core.velocity = new Vector2 {
				X = (float)reader.ReadSingle(),
				Y = (float)reader.ReadSingle()
			};
//LogHelpers.Log( "READ id: "+this.ID+", name: "+core.DisplayName+", who: "+core.whoAmI+", total templates: "+ CustomEntityTemplates.TotalEntityTemplates());
//LogHelpers.Log( "READ2 new_ent: "+(new_ent==null?"null":"not null")+", component count: "+(new_ent==null?"null2":""+new_ent.Components.Count) );
			
			for( int i = 0; i < new_ent.Components.Count; i++ ) {
				new_ent.Components[i].ReadStreamForwarded( reader );
			}
			
//LogHelpers.Log( "READ "+new_ent.ToString()+" pos:"+new_ent.Core.position );
			this.CopyChangesFrom( new_ent );
		}


		protected override void WriteStream( BinaryWriter writer ) {
			CustomEntityCore core = this.Core;

			writer.Write( (ushort)this.ID );
//LogHelpers.Log( "WRITE id: "+this.ID+", name: "+core.DisplayName+", templates: "+ CustomEntityTemplates.TotalEntityTemplates());
//LogHelpers.Log( "WRITE2 who: "+core.whoAmI+", component count: "+this.Components.Count );

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
//LogHelpers.Log( "WRITE "+this.ToString()+" pos:"+ core.position );
		}
	}
}
