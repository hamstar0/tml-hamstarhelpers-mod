using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity {
	internal sealed partial class SerializableCustomEntity : CustomEntity {
		public override bool SyncFromClient => false;
		public override bool SyncFromServer => false;


		protected override void WriteStream( BinaryWriter writer ) {
			if( !this.IsInitialized ) {
				//throw new HamstarException( "!ModHelpers.SerializableCustomEntity.WriteStream - Not initialized." );
				throw new HamstarException( "Not initialized." );
			}

			if( Main.netMode != 1 ) {
				this.RefreshOwnerWho();
			}

			CustomEntityCore core = this.Core;
			byte ownerWho = this.OwnerPlayerWho == -1 ? (byte)255 : (byte)this.OwnerPlayerWho;
			
			writer.Write( (ushort)CustomEntityManager.GetIdByTypeName( this.MyTypeName ) );
			writer.Write( (byte)ownerWho );
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
			int typeId =			(ushort)reader.ReadUInt16();
			byte ownerWho =		(byte)reader.ReadByte();
			int who =				(ushort)reader.ReadUInt16();
			string displayName =	(string)reader.ReadString();
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

			Type entType = CustomEntityManager.GetTypeById( typeId );
			if( entType == null ) {
				//throw new HamstarException( "!ModHelpers.CustomEntity.ReadStream - Invalid entity type id " + typeId );
				throw new HamstarException( "Invalid entity type id " + typeId );
			}

			Player plr = ownerWho == (byte)255 ? null : Main.player[ownerWho];

			var myentTemplate = (CustomEntity)CustomEntity.CreateRawUninitialized( entType );
			CustomEntityCore core = myentTemplate.CreateCoreTemplate();
			IList<CustomEntityComponent> components = myentTemplate.CreateComponentsTemplate();

			core.WhoAmI = who;
			core.DisplayName = displayName;
			core.Width = wid;
			core.Height = hei;
			core.Position = pos;
			core.Velocity = vel;
			core.direction = dir;
			
			for( int i = 0; i < components.Count; i++ ) {
				components[i].ReadStreamForwarded( reader );
			}
			
			this.MyTypeName = SerializableCustomEntity.GetTypeName( myentTemplate );
			this.CopyChangesFrom( core, components, plr );
//LogHelpers.Log( "READ "+this );
		}
	}
}
