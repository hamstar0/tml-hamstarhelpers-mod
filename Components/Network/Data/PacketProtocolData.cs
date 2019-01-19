using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.IO;
using System.Linq;
using System.Reflection;


namespace HamstarHelpers.Components.Network.Data {
	/// <summary>
	/// Provides a way to automatically ensure order of fields for transmission.
	/// </summary>
	public abstract partial class PacketProtocolData {
		internal static bool ValidateConstructor( Type dataType ) {
			ConstructorInfo ctorInfo = dataType.GetConstructor( BindingFlags.Instance | BindingFlags.NonPublic, null,
				new Type[] { typeof( PacketProtocolDataConstructorLock ) }, null );

			if( ctorInfo == null ) {
				ctorInfo = dataType.GetConstructor( BindingFlags.Instance | BindingFlags.NonPublic, null,
					new Type[] { }, null );

				if( ctorInfo == null ) {
					return false;
				}
			} else {
				return ctorInfo.IsFamily;
			}

			return true;
		}



		////////////////

		private IOrderedEnumerable<FieldInfo> _OrderedFields = null;

		internal IOrderedEnumerable<FieldInfo> OrderedFields {
			get {
				if( this._OrderedFields == null ) {
					Type mytype = this.GetType();
					FieldInfo[] fields = mytype.GetFields( BindingFlags.Public | BindingFlags.Instance );

					this._OrderedFields = fields.OrderByDescending( x => x.Name );  //Where( f => f.FieldType.IsPrimitive )
				}
				return this._OrderedFields;
			}
		}



		////////////////

		protected PacketProtocolData( PacketProtocolDataConstructorLock ctorLock ) {
			//if( ctorLock == null ) {
			//	throw new NotImplementedException( "Invalid " + this.GetType().Name + ": Must be factory generated or cloned." );
			//}
		}

		protected PacketProtocolData() { }


		////////////////

		/// <summary>
		/// Implements internal low level data reading for packet receipt.
		/// </summary>
		/// <param name="reader">Binary data reader.</param>
		protected virtual void ReadStream( BinaryReader reader ) {
			PacketProtocolData.ReadStreamIntoContainer( reader, this );
		}


		////////////////

		/// <summary>
		/// Implements low level stream output for packet output.
		/// </summary>
		/// <param name="writer">Binary data writer.</param>
		protected virtual void WriteStream( BinaryWriter writer ) {
			PacketProtocolData.WriteStreamFromContainer( writer, this );
		}
	}
}
