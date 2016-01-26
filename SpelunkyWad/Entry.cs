using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpelunkyWad
{
	/// <summary>
	/// Represents an entry in a WAD archive.
	/// </summary>
	public class Entry
	{
		/// <summary>
		/// Creates a new entry.
		/// </summary>
		/// <param name="name">the name</param>
		/// <param name="data">the data</param>
		public Entry(string name, byte[] data)
		{
			this.Name = name;
			this.Data = data;
		}

		/// <summary>
		/// Returns a string representation of the entry.
		/// </summary>
		/// <returns>a string representation</returns>
		public override string ToString()
		{
			return string.Format("Entry (Name: {0}, Data: {1})", this.Name, this.Data);
		}

		/// <summary>
		/// Returns if the entry equals another object.
		/// </summary>
		/// <param name="obj">the object</param>
		/// <returns>if this entry equals</returns>
		public override bool Equals(object obj)
		{
			if (obj is Entry)
			{
				var entry = (Entry) obj;

				return (this.Name == entry.Name && this.Data.Length == entry.Data.Length);
			}
			else
			{
				return base.Equals(obj);
			}
		}

		/// <summary>
		/// Returns the hash code for the entry.
		/// </summary>
		/// <returns>the hash code</returns>
		public override int GetHashCode()
		{
			return (this.Name.GetHashCode() + this.Data.Length);
		}

		/// <summary>
		/// The name of the entry.
		/// </summary>
		public string Name
		{
			get;
			private set;
		}

		/// <summary>
		/// The data for the entry.
		/// </summary>
		public byte[] Data
		{
			get;
			private set;
		}
	}
}
