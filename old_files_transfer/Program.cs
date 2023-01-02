using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

var _ = new SekundaerLiteraturParser("./old_data/sekundaer.xml");

static class Helpers {
    public static XDocument ParseFile(string filepath) {
        var text = File.ReadAllText(filepath, System.Text.Encoding.UTF8);
        text = ReplaceWhiteSpaces(text);
        return XDocument.Parse(text, LoadOptions.PreserveWhitespace);
    }


    public static bool HasClass(string classes, XElement element) {
        if (String.IsNullOrEmpty(classes) || element == null) return false;
        if (HasAttribute("class", element)) return element.Attribute("class")!.Value.Contains(classes);
        return false;
    }

    public static bool HasAttribute(string attributename, XElement element) {
        if (String.IsNullOrEmpty(attributename) || element == null) return false;
        if (!element.HasAttributes) return false;
        if (!element.Attributes(attributename).Any()) return false;
        if (String.IsNullOrWhiteSpace(element.Attribute(attributename)!.Value)) return false;
        return true;
    }

    public static string ReplaceWhiteSpaces(string str) {
        Regex regex = new Regex(@"\s+");
        return regex.Replace(str, " ");
    }
}


class SekundaerLiteraturParser {
    private string filepath;
    private string filename;
    private XDocument document;
    private string? Jahreszahl;
    private string? Name;

    private Dictionary<string, (string, string, string)> ParsedFiles;
    private StringBuilder? CurrentText;

    public SekundaerLiteraturParser(string filepath) {
        this.filepath = filepath;
        this.filename = filepath.Split("/").Last();
        this.ParsedFiles = new Dictionary<string, (string, string, string)>();
        if (Directory.Exists("./output/" + filename)) Directory.Delete("./" + filename, true);
        Directory.CreateDirectory("./output/" + filename);
        this.document = Helpers.ParseFile(filepath);
        this.CurrentText = new StringBuilder();
        foreach (var element in document.Descendants()) {
            if (Helpers.HasClass("Jahreszahl", element)) {
                if (Jahreszahl != null) InsertInto();
                this.Jahreszahl = element.Value.Trim();
            }
            if (Helpers.HasClass("Einzug", element)) { 
                if (CurrentText.Length != 0) {
                    InsertInto();
                }
                if (element.Descendants("b").Any()) {
                    this.Name = element.Descendants("b").First().Value.Trim();
                }
                CurrentText.Append(element.ToString());
            }
            if (Helpers.HasClass("Kommentar", element)) {
                CurrentText.Append("\n\n");
                CurrentText.Append(element.ToString());
            }
        }
        Flush();
    }

    private void InsertInto() {
        var AuthorYear = Jahreszahl + "_" + Name;
        if (!ParsedFiles.ContainsKey(AuthorYear)) {
            ParsedFiles.Add(AuthorYear, (Jahreszahl, Name, CurrentText.ToString()));
        }
        else {
            ParsedFiles[AuthorYear] = (
                Jahreszahl,
                Name, 
                ParsedFiles[AuthorYear].Item3 + "\n\n\n" + CurrentText.ToString()
            );
        }
        
        CurrentText.Clear();
    }

    private void Flush() {
        foreach (var entry in this.ParsedFiles) {
            var sb = new StringBuilder();
            var fn = "./output/" + filename + "/" + entry.Value.Item1 + "_" + entry.Value.Item2 + ".html";
            sb.AppendLine("---");
            sb.AppendLine("Jahr: " + entry.Value.Item1);
            sb.AppendLine("Autor: " + entry.Value.Item2);
            sb.AppendLine("---");
            if (File.Exists(fn)) {
                Console.WriteLine(fn);
            }
            sb.Append(entry.Value.Item3);
            System.IO.File.WriteAllText(fn, sb.ToString());
        }
    }
}