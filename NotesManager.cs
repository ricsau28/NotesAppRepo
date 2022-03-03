#define DEBUG

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
        public NotesManager() : this("")
        {
            //This should barf shouldn't it??
            //Q: How to funnel users to a desired constructor??

        } //default constructor

        public NotesManager(string notesFolder)
        {
            if (string.IsNullOrEmpty(notesFolder))
                throw new ArgumentException("NotesManager.Ctor: notesFolder is null or empty");

            NotesFolder = notesFolder;
        }
        #endregion

        public List<Note> LoadNotes()
        {
#if DEBUG
            int nDebugCount = 0;
            const int MAX_DEBUG_COUNT = 200;
#endif

            int noteCount = 0;
            List<Note> noteList = new List<Note>();

            DirectoryInfo dirInfo = new DirectoryInfo(NotesFolder);
            FileInfo[] noteFiles = dirInfo.GetFiles("*.note");

            Console.WriteLine("NoteManager.LoadNotes: Found {0} files in {1}\n", noteFiles.Length, NotesFolder);

            foreach (FileInfo noteFile in noteFiles)
            {
                Note note = LoadNoteFromFile(noteFile.FullName);
                noteList.Add(note);
                noteCount++;

                //Console.WriteLine("LoadNotes: Note GUID: {0}\tNote Title:{1}", note.GUID, note.Title);     

#if DEBUG
                if (++nDebugCount >= MAX_DEBUG_COUNT)
                    break;
#endif


            }//end foreach  

            Console.WriteLine("LoadNotes: noteCount: {0}\tnoteList.Length: {1}", noteCount, noteList.Count.ToString());

            return noteList;

        }// end LoadNotes

        public Note LoadNoteFromFile(string noteFileName)
        {
            if (string.IsNullOrEmpty(noteFileName))
                return null;


            string version = String.Empty;
            int num = -1;
            DateTime date = DateTime.MinValue;

            if (noteFileName == "c6065ba5-d09d-45e2-ad60-608f97d25bc5.note")
            {
                System.Diagnostics.Debugger.Break(); //Debian 10 Install
            }

            Note note = new Note(noteFileName);
            //Console.WriteLine("Attempting to parse: {0}\n", noteFileName);
            //note.Notebook = "Unfiled"; //Set default here. It will be changed when extracted from tags

            XmlDocument doc = new XmlDocument();

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
                                note.Title = xmlNode.InnerText.Trim();

                                if (note.Title == "Debian 10 Install" || note.Title == "Amazon")
                                    System.Diagnostics.Debugger.Break();
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
                                /*
                                string noteBook = ParseNotebookFromTags(xmlNode);
                                if (!string.IsNullOrEmpty(noteBook))
                                    note.Notebook = noteBook;
                                break;
                                */
                                note.Notebook = ParseNotebookFromTags(xmlNode);
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
                        tagStr = node.InnerText.Trim();
                        //if (!string.IsNullOrEmpty(tagStr) && tagStr.ToLower().Contains("notebook:"))
                        if (!string.IsNullOrEmpty(tagStr))
                        {
                            //TODO: Test assumption of one notebook per note
                            //NB: I assume that a  note should only belong 
                            //to one notebook so we can return the first one found 

                            //NB: For now, assume that a note can belong to many
                            //notebooks and append them to a StringBuilder

                            //string notebook = NotesAppUtilities.ExtractNotebookFromTag(tagStr);

                            notebookTags.Append(NotesAppUtilities.ExtractNotebookFromTag(tagStr));

                            //if (!string.IsNullOrEmpty(notebook))
                            //notebookTags.Append(notebook);

                            //break;
                        }
                    }
                }
            }// end if

            return notebookTags.ToString();

        }// end method ParseTags


    }// end class

}
