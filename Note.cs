using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;

namespace NotesApp
{

    //See Tomboy.NoteData class
    public class Note
    {
        string uri;
        string version;
        string guid;
        string title;
        string text;
        string content;
        string note_version;
        DateTime create_date;
        DateTime change_date;
        DateTime metadata_change_date;
        Dictionary<string, Tag> tags;

        int cursor_pos, selection_bound_pos;
        int width, height, x, y;
        bool open_on_startup;

        #region Properties
        public string Version
        {
            get { return version; }
            set { version = value; }
        }

        public string GUID
        {
            get { return guid; }
            set { guid = value; }
        }

        public string Notebook
        {
            get;
            set;
        }

        public string Uri
        {
            get
            {
                return uri;
            }
        }

        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
            }
        }

        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
            }
        }

        public string NoteVersion
        {
            get { return note_version; }
            set { note_version = value; }
        }
        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        public DateTime CreateDate
        {
            get
            {
                return create_date;
            }
            set
            {
                create_date = value;
            }
        }

        /// <summary>
        /// Indicates the last time note content data changed.
        /// Does not include tag/notebook changes (see MetadataChangeDate).
        /// </summary>
        public DateTime ChangeDate
        {
            get
            {
                return change_date;
            }
            set
            {
                change_date = value;
                metadata_change_date = value;
            }
        }

        /// <summary>
        /// Indicates the last time non-content note data changed.
        /// This currently only applies to tags/notebooks.
        /// </summary>
        public DateTime MetadataChangeDate
        {
            get
            {
                return metadata_change_date;
            }
            set
            {
                metadata_change_date = value;
            }
        }


        // FIXME: the next six attributes don't belong here (the data
        // model), but belong into the view; for now they are kept here
        // for backwards compatibility

        public int CursorPosition
        {
            get
            {
                return cursor_pos;
            }
            set
            {
                cursor_pos = value;
            }
        }

        public int SelectionBoundPosition
        {
            get
            {
                return selection_bound_pos;
            }
            set
            {
                selection_bound_pos = value;
            }
        }

        public int Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }

        public int Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
            }
        }

        public int X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }

        public Dictionary<string, Tag> Tags
        {
            get
            {
                return tags;
            }
        }

        public bool IsOpenOnStartup
        {
            get
            {
                return open_on_startup;
            }
            set
            {
                open_on_startup = value;
            }
        }

        #endregion

        #region Constructors

        /*
        public Note(string uri)
        {
            this.uri = uri;
            //this.guid = uri.Substring(0, uri.IndexOf(".note"));
            this.guid = Path.GetFileNameWithoutExtension(this.uri);

            x = -1;
            y = -1;
            selection_bound_pos = -1;

            tags = new Dictionary<string, Tag>();

            create_date = DateTime.MinValue;
            change_date = DateTime.MinValue;
            metadata_change_date = DateTime.MinValue;

        }
        */

        public Note(string uri, string nameOfNotebook = "Unfiled")
        {
            Notebook = nameOfNotebook;

            this.uri = uri;
            //this.guid = uri.Substring(0, uri.IndexOf(".note"));
            this.guid = Path.GetFileNameWithoutExtension(this.uri);

            x = -1;
            y = -1;
            selection_bound_pos = -1;

            tags = new Dictionary<string, Tag>();

            create_date = DateTime.MinValue;
            change_date = DateTime.MinValue;
            metadata_change_date = DateTime.MinValue;
        }
        #endregion

        /*
        public override string ToString()
        {

            return string.Format("Note uri: {0}\n" +
                                 "Note GUID: {1}\n" +
                                 "Note title: {2}\n" +
                                 "Note version: {3}\n" +
                                 "Note created: {4}\n" +
                                 "Note last-change-date: {5}\n" +
                                 "Note text: {6}\n" +
                                 "Note content version: {7}\n" +
                                 "Note content: {8}\n" +
                                 "Note notebook: {9}\n",
                                this.uri,
                                this.guid,
                                this.title,
                                this.version,
                                this.create_date.ToLongDateString(),
                                this.change_date.ToLongDateString(),
                                this.text,
                                this.note_version,
                                this.content,
                                this.Notebook);

        }*/

        public override string ToString()
        {

            return string.Format("Note uri: {0}\n" +
                                 "Note GUID: {1}\n" +
                                 "Note title: {2}\n" +
                                 "Note version: {3}\n" +
                                 "Note created: {4}\n" +
                                 "Note last-change-date: {5}\n" +
                                 "Note content version: {6}\n" +
                                 "Note notebook: {7}\n",
                                this.uri,
                                this.guid,
                                this.title,
                                this.version,
                                this.create_date.ToLongDateString(),
                                this.change_date.ToLongDateString(),
                                this.note_version,
                                this.Notebook);

        }


    }


}