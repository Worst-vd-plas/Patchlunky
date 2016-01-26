using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpelunkyWad
{
	/// <summary>
	/// Represents a group of entries.
	/// </summary>
	public class Group
	{
		/// <summary>
		/// Creates a new group.
		/// </summary>
		/// <param name="name">the name</param>
		/// <param name="entries">the entries</param>
		public Group(string name, List<Entry> entries)
		{
			this.Name = name;
			this.Entries = entries;
		}

		/// <summary>
		/// Creates a new group.
		/// </summary>
		/// <param name="name">the entries</param>
		public Group(string name) : this(name, new List<Entry>())
		{

		}

		/// <summary>
		/// Returns a string representation of the group.
		/// </summary>
		/// <returns>a string representation</returns>
		public override string ToString()
		{
			return string.Format("Group (Name: {0}, Entries: {1})", this.Name, this.Entries);
		}

		/// <summary>
		/// Returns if the entry equals another object.
		/// </summary>
		/// <param name="obj">the object</param>
		/// <returns>if the entry equals</returns>
		public override bool Equals(object obj)
		{
			if (obj is Group)
			{
				var group = (Group) obj;

				return (this.Name == group.Name && this.Entries == group.Entries);
			}
			else
			{
				return base.Equals(obj);
			}
		}

		/// <summary>
		/// Returns the hash code for the group.
		/// </summary>
		/// <returns>the hash code</returns>
		public override int GetHashCode()
		{
			return (this.Name.GetHashCode() + this.Entries.GetHashCode());
		}

		/// <summary>
		/// The name of the group.
		/// </summary>
		public string Name
		{
			get;
			private set;
		}

		/// <summary>
		/// The entries in the group.
		/// </summary>
		public List<Entry> Entries
		{
			get;
			private set;
		}
	}
}
