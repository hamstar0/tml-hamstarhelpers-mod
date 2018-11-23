using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity {
	internal partial class SerializableCustomEntity : CustomEntity {
		protected override void WriteStream( BinaryWriter writer ) {
			if( !this.IsInitialized ) {
				throw new HamstarException( "!ModHelpers.SerializableCustomEntity.WriteStream - Not initialized." );
			}

			if( Main.netMode != 1 ) {
				this.RefreshOwnerWho();
			}

			CustomEntityCore core = this.Core;
			byte owner_who = this.OwnerPlayerWho == -1 ? (byte)255 : (byte)this.OwnerPlayerWho;
			
			writer.Write( (ushort)CustomEntityManager.GetIdByTypeName( this.MyTypeName ) );
			writer.Write( (byte)owner_who );
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

			for( int i = 0; i < this.Components.Count; i++ ) {
				this.Components[i].WriteStreamForwarded( writer );
			}
//LogHelpers.Log( "WRITE "+this.ToString()+" pos:"+ core.position );
		}


		protected override void ReadStream( BinaryReader reader ) {
			int type_id =			(ushort)reader.ReadUInt16();
			byte owner_who =		(byte)reader.ReadByte();
			int who =				(ushort)reader.ReadUInt16();
			string display_name =	(string)reader.ReadString();
			var pos = new Vector2 {
				X =					(float)reader.ReadSingle(),
				Y =					(float)reader.ReadSingle()
			};
			int dir =				(short)reader.ReadInt16();
			int wid =				(ushort)reader.ReadUInt16();
			int hei =				(ushort)reader.ReadUInt16();
			var vel = new Vector2 {
				X =					(float)reader.ReadSingle(),
				Y =					(float)reader.ReadSingle()
			};

			Type ent_type = CustomEntityManager.GetTypeById( type_id );
			if( ent_type == null ) {
				throw new HamstarException( "!ModHelpers.CustomEntity.ReadStream - Invalid entity type id " + type_id );
			}

			Player plr = owner_who == (byte)255 ? null : Main.player[owner_who];

			var myent = (CustomEntity)CustomEntity.CreateRaw( ent_type );

			CustomEntityCore core = myent.CreateCoreTemplate();
			core.WhoAmI = who;
			core.DisplayName = display_name;
			core.Width = wid;
			core.Height = hei;
			core.Position = pos;
			core.direction = dir;
			core.Velocity = vel;
			
			IList<CustomEntityComponent> components = myent.CreateComponentsTemplate();
			for( int i = 0; i < components.Count; i++ ) {
				components[i].ReadStreamForwarded( reader );
			}
//LogHelpers.Log( "READ id: "+ent_type.Name+", core: "+core.ToString()+", components: "+components.Count+")");
			
			this.CopyChangesFrom( core, components, plr );
		}
	}
}
