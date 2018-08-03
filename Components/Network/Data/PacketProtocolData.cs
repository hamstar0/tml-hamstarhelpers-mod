using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.IO;
using System.Linq;
using System.Reflection;


namespace HamstarHelpers.Components.Network.Data {
	/// <summary>
	/// Provides a way to automatically ensure order of fields for transmission.
	/// </summary>
	public partial class PacketProtocolData {
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
			PacketProtocolData.WriteStreamIntoContainer( writer, this );
		}
	}
}
