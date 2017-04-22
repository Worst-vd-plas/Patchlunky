using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpelunkyWad
{
    /// <summary>
    /// Represents a WAD archive.
    /// </summary>
    public class Archive
    {
        /// <summary>
        /// Creates a new WAD archive.
        /// </summary>
        /// <param name="wadFile">the WAD file</param>
        /// <param name="wixFile">the WIX file</param>
        public Archive(string wadFile, string wixFile)
        {
            this.WadFile = wadFile;
            this.WixFile = wixFile;

            this.Groups = new List<Group>();
        }

        /// <summary>
        /// Creates a new WAD archive.
        /// </summary>
        /// <param name="wadFile">the WAD file</param>
        public Archive(string wadFile) : this(wadFile, wadFile + ".wix")
        {
        }

        /// <summary>
        /// Returns a string representation of the archive.
        /// </summary>
        /// <returns>a string representation</returns>
        public override string ToString()
        {
            return string.Format("Archive (WAD File: {0}, WIX File: {1}, Groups: {2})", this.WadFile, this.WixFile, this.Groups);
        }

        /// <summary>
        /// Loads this WAD archive from file.
        /// </summary>
        public void Load()
        {
            //context
            var groups = new List<Group>();
            Group group = null;

            using (FileStream wadStream = File.Open(this.WadFile, FileMode.Open))
            using (StreamReader wixStreamReader = new StreamReader(File.Open(this.WixFile, FileMode.Open)))
            {
                while (!wixStreamReader.EndOfStream)
                {
                    //read
                    var line = wixStreamReader.ReadLine();
                    var parts = line.Split(' ');
                    var identifier = parts[0];

                    switch (identifier)
                    {
                        case "!group":
                            {
                                if (group != null)
                                {
                                    //add
                                    groups.Add(group);
                                }

                                //group
                                var name = parts[1];

                                //create
                                group = new Group(name);

                                break;
                            }
                        default:
                            {
                                //entry
                                var name = parts[0];
                                var offset = int.Parse(parts[1]);
                                var length = int.Parse(parts[2]);

                                //data
                                var data = new byte[length];
                                wadStream.Seek(offset, SeekOrigin.Begin);
                                wadStream.Read(data, 0, length);

                                //create
                                var entry = new Entry(name, data);
                                group.Entries.Add(entry);

                                break;
                            }
                    }
                }

                if (group != null)
                {
                    //add last
                    groups.Add(group);
                }
            }

            //set
            this.Groups = groups;
        }

        /// <summary>
        /// Saves this WAD archive to file.
        /// </summary>
        public void Save()
        {
            //offset
            var offset = 0;
            var written = new Dictionary<Entry, int>();

            using (FileStream wadStream = File.Open(this.WadFile, FileMode.Create))
            using (StreamWriter wixStreamWriter = new StreamWriter(File.Open(this.WixFile, FileMode.Create)))
            {
                foreach (var group in this.Groups)
                {
                    //group
                    wixStreamWriter.WriteLine($"!group {@group.Name}");

                    foreach (var entry in group.Entries)
                    {
                        //entry
                        wixStreamWriter.WriteLine($"{entry.Name} {offset} {entry.Data.Length}");
                        wadStream.Write(entry.Data, 0, entry.Data.Length);

                        //increment
                        written[entry] = offset;
                        offset += entry.Data.Length;
                    }
                }
            }
        }

        /// <summary>
        /// The location of the WAD file for the archive.
        /// </summary>
        public string WadFile
        {
            get;
            set;
        }

        /// <summary>
        /// The location of the WIX file for the archive.
        /// </summary>
        public string WixFile
        {
            get;
            set;
        }

        /// <summary>
        /// The groups in the archive.
        /// </summary>
        public List<Group> Groups
        {
            get;
            set;
        }
    }
}