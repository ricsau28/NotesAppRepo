using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;

namespace NotesApp
{

    public class NotesManager
    {
        //private string notesFolder = string.Empty;

        #region Properties
        public string NotesFolder { get; set; }
        #endregion

        #region Constructors
        public NotesManager() { } //default constructor

        public NotesManager(string notesFolder)
        {
            NotesFolder = notesFolder;
        }
        #endregion

        public List<Note> LoadNotes()
        {
            if (string.IsNullOrEmpty(NotesFolder))
                return null;

            int nDebugCount = 0;
            List<Note> noteList = new List<Note>();

            DirectoryInfo dirInfo = new DirectoryInfo(NotesFolder);
            FileInfo[] noteFiles = dirInfo.GetFiles("*.note");

            Console.WriteLine("NoteManager.LoadNotes: Found {0} files in {1}\n", noteFiles.Length, NotesFolder);

            foreach (FileInfo noteFile in noteFiles)
            {
                Note note = LoadNoteFromFile(noteFile.FullName);
                noteList.Add(note);

                Console.WriteLine("Note GUID: {0}\tNote Title:{1}", note.GUID, note.Title);

                nDebugCount++;

                /*
                if (nDebugCount >= 10)
                    break;
                */
            }//end foreach         

            return noteList;

        }// end LoadNotes

        public Note LoadNoteFromFile(string noteFileName)
        {
            if (string.IsNullOrEmpty(noteFileName))
                return null;


            string version = String.Empty;
            int num = -1;
            DateTime date = DateTime.MinValue;
            Note note = null;

            XmlDocument doc = new XmlDocument();


            //Console.WriteLine("Attempting to parse: {0}\n", noteFileName);

            note = new Note(noteFileName);

            doc.Load(noteFileName);

            XmlNodeList elemList = doc.GetElementsByTagName("*");

            foreach (XmlNode xmlNode in elemList)
            {
                switch (xmlNode.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (xmlNode.Name)
                        {
                            case "note":
                                note.Version = xmlNode.Attributes["version"].Value; //.GetAttribute("version");
                                break;

                            case "title":
                                note.Title = xmlNode.InnerText;
                                break;

                            case "note-content":
                                note.Content = xmlNode.InnerText;
                                note.NoteVersion = xmlNode.Attributes["version"].Value;
                                break;

                            case "text":
                                note.Text = xmlNode.InnerXml;
                                /* XmlNode? node = doc.ReadNode(xmlNode);
                                if (node != null)
                                    note.Text = node.InnerText;
                                else
                                    note.Text = xmlNode.ReadInnerXml(); */
                                break;

                            case "last-change-date":
                                if (DateTime.TryParse(xmlNode.InnerText, out date))
                                    note.ChangeDate = date;
                                else
                                    note.ChangeDate = DateTime.Now;
                                break;

                            case "last-metadata-change-date":
                                if (DateTime.TryParse(xmlNode.InnerText, out date))
                                    note.MetadataChangeDate = date;
                                else
                                    note.MetadataChangeDate = DateTime.Now;
                                break;

                            case "create-date":
                                if (DateTime.TryParse(xmlNode.InnerText, out date))
                                    note.CreateDate = date;
                                else
                                    note.CreateDate = DateTime.Now;
                                break;

                            case "cursor-position":
                                if (int.TryParse(xmlNode.InnerText, out num))
                                    note.CursorPosition = num;
                                break;

                            case "selection-bound-position":
                                if (int.TryParse(xmlNode.InnerText, out num))
                                    note.SelectionBoundPosition = num;
                                break;

                            case "width":
                                if (int.TryParse(xmlNode.InnerText, out num))
                                    note.Width = num;
                                break;

                            case "height":
                                if (int.TryParse(xmlNode.InnerText, out num))
                                    note.Height = num;
                                break;

                            case "x":
                                if (int.TryParse(xmlNode.InnerText, out num))
                                    note.X = num;
                                break;

                            case "y":
                                if (int.TryParse(xmlNode.InnerText, out num))
                                    note.Y = num;
                                break;

                            case "tags":
                                //See Tomboy's Note.cs
                                string noteBook = ParseNotebookFromTags(xmlNode);
                                if (!string.IsNullOrEmpty(noteBook))
                                    note.Notebook = noteBook;
                                break;

                        }//end switch

                        break;

                }// end switch            

            }// *** END FOREACH ***

            return note;

        }// end method


        // <summary>
        // Parse the tags from the <tags> element
        // See Note.Archiver ParseTags
        // </summary>
        //List<string> ParseNotebookFromTags (XmlNode tagsNode)
        string ParseNotebookFromTags(XmlNode tagsNode)
        {
            //List<string> tags = new List<string> ();

            /* foreach (XmlNode node in tagNodes.SelectNodes ("//tag")) {
				tags.Add (node.InnerText);
			} */

            StringBuilder notebookTags = new StringBuilder();
            string tagStr = string.Empty;

            if (tagsNode.HasChildNodes)
            {

                foreach (XmlNode node in tagsNode.ChildNodes)
                {
                    if (node.Name == "tag")
                    {
                        tagStr = node.InnerText;
                        //if (!string.IsNullOrEmpty(tagStr) && tagStr.ToLower().Contains("notebook:"))
                        if (!string.IsNullOrEmpty(tagStr))
                        {
                            //TODO: Test assumption of one notebook per note
                            //NB: I assume that a  note should only belong 
                            //to one notebook so we can return the first one found 

                            //NB: For now, assume that a note can belong to many
                            //notebooks and append them to a StringBuilder
                            string notebook = NotesAppUtilities.ExtractNotebookFromTag(tagStr);

                            if (!string.IsNullOrEmpty(notebook))
                                notebookTags.Append(notebook);
                        }
                    }
                }
            }// end if

            return notebookTags.ToString();

        }// end method ParseTags


    }// end class

}
