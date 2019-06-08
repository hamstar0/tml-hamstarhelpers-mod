using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.IO;
using System.Linq;
using System.Reflection;


namespace HamstarHelpers.Components.Protocol.Stream {
	/// <summary>
	/// Provides a way to automatically ensure order of fields for transmission.
	/// </summary>
	public abstract partial class StreamProtocol {
		internal static bool ValidateConstructor( Type dataType ) {
			ConstructorInfo ctorInfo = dataType.GetConstructor( BindingFlags.Instance | BindingFlags.NonPublic, null,
					new Type[] { }, null );

			if( ctorInfo == null ) {
				return false;
			}
			return !ctorInfo.IsFamily;  // This is so the default ctor can't be inherited; won't mix with non-default ctors
		}


		internal static StreamProtocol CreateInstance( Type mytype ) {
			Object[] args = new object[] { };
			return (StreamProtocol)Activator.CreateInstance( mytype, BindingFlags.NonPublic | BindingFlags.Instance, null, args, null );
		}



		////////////////

		internal int FieldCount { get; private set; } = -1;
		internal IOrderedEnumerable<FieldInfo> OrderedFields {
			get {
				if( this._OrderedFields == null ) {
					Type mytype = this.GetType();
					FieldInfo[] fields = mytype.GetFields( BindingFlags.Public | BindingFlags.Instance );

					this.FieldCount = fields.Count();
					this._OrderedFields = fields
						.Where( field => !Attribute.IsDefined(field, typeof(ProtocolIgnoreAttribute)) )
						.OrderByDescending( field => field.Name );  //Where( f => f.FieldType.IsPrimitive )
				}
				return this._OrderedFields;
			}
		}

		private IOrderedEnumerable<FieldInfo> _OrderedFields = null;



		////////////////
		
		protected StreamProtocol() { }

		
		////////////////

		protected abstract void OnClone();

		protected void InternalOnClone() { this.OnClone(); }


		////////////////

		/// <summary>
		/// Implements internal low level data reading for packet receipt.
		/// </summary>
		/// <param name="reader">Binary data reader.</param>
		protected virtual void ReadStream( BinaryReader reader ) {
			StreamProtocol.ReadStreamIntoContainer( reader, this );
		}


		////////////////

		/// <summary>
		/// Implements low level stream output for packet output.
		/// </summary>
		/// <param name="writer">Binary data writer.</param>
		protected virtual void WriteStream( BinaryWriter writer ) {
			StreamProtocol.WriteStreamFromContainer( writer, this );
		}
	}
}
