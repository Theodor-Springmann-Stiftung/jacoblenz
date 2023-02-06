using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;


var _ = new NeueSekLitParser("./old_data/sekn.xml");


static class Helpers {
    public static XDocument ParseFile(string filepath) {
        var text = File.ReadAllText(filepath, System.Text.Encoding.UTF8);
        text = ReplaceWhiteSpaces(text, " ");
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

    public static string ReplaceWhiteSpaces(string str, string replacement) {
        Regex regex = new Regex(@"\s+");
        return regex.Replace(str, replacement);
    }
}


class SekundaerLiteraturParser {
    private string filepath;
    private string filename;
    private XDocument document;
    private string? Jahreszahl;
    private string? Name;

    private List<(string Jahr, string Name, string Text, string Sort)> ParsedFiles;
    private StringBuilder? CurrentText;

    public SekundaerLiteraturParser(string filepath) {
        this.filepath = filepath;
        this.filename = filepath.Split("/").Last();
        this.ParsedFiles = new List<(string Jahr, string Name, string Text, string Sort)>();
        if (Directory.Exists("./output/" + filename)) Directory.Delete("./output/" + filename, true);
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
        var similarentries = ParsedFiles.Where(x => x.Jahr == Jahreszahl && x.Name == Name).ToList();
        if (!similarentries.Any()) {
            ParsedFiles.Add((Jahreszahl, Name, CurrentText.ToString(), "1"));
        }
        else {
            var fe = similarentries[0];
            fe.Sort = "1";
            ParsedFiles.Add((
                Jahreszahl,
                Name, 
                CurrentText.ToString(),
                (similarentries.Count() + 1).ToString()
            ));
        }
        
        CurrentText.Clear();
    }

    private void Flush() {
        foreach (var entry in this.ParsedFiles) {
            var sb = new StringBuilder();
            var fn = "./output/" + filename + "/";
            if (String.IsNullOrWhiteSpace(entry.Sort)) fn += entry.Jahr + "_" + entry.Name + ".html";
            else fn += entry.Jahr + "_" + entry.Name + "_" + entry.Sort + ".html";
            sb.AppendLine("---");
            sb.AppendLine("Jahr: " + entry.Jahr);
            sb.AppendLine("Autor: " + entry.Name);
            if (!String.IsNullOrWhiteSpace(entry.Sort)) sb.AppendLine("Sort: " + entry.Sort);
            sb.AppendLine("---");
            if (File.Exists(fn)) {
                Console.WriteLine("Überschreibt: " + fn);
            }
            sb.Append(entry.Text);
            System.IO.File.WriteAllText(fn, sb.ToString());
        }
    }
}

class Parser {
    private string filepath;
    private string catname;
    private string filename;
    private XDocument document;
    private string? Jahreszahl;

    private List<(string Jahr, string Name, string Text, string Sort)> ParsedFiles;
    private StringBuilder? CurrentText;

    public Parser(string filepath, string catname) {
        this.filepath = filepath;
        this.catname = catname;
        this.filename = filepath.Split("/").Last();
        this.ParsedFiles = new List<(string Jahr, string Name, string Text, string Sort)>();
        if (Directory.Exists("./output/" + filename)) Directory.Delete("./output/" + filename, true);
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
        var similarentries = ParsedFiles.Where(x => x.Jahr == Jahreszahl).ToList();
        if (!similarentries.Any()) {
            ParsedFiles.Add((Jahreszahl, string.Empty, CurrentText.ToString(), "1"));
        }
        else {
            ParsedFiles.Add((
                Jahreszahl,
                string.Empty, 
                CurrentText.ToString(),
                (similarentries.Count() + 1).ToString()
            ));
        }
        
        CurrentText.Clear();
    }

    private void Flush() {
        foreach (var entry in this.ParsedFiles) {
            var sb = new StringBuilder();
            var fn = "./output/" + filename + "/";
            fn += Helpers.ReplaceWhiteSpaces(entry.Jahr, "-") + "_" + entry.Sort + ".html";
            sb.AppendLine("---");
            sb.AppendLine(this.catname + ": " + entry.Jahr);
            if (!String.IsNullOrWhiteSpace(entry.Sort)) sb.AppendLine("Sort: " + entry.Sort);
            sb.AppendLine("---");
            if (File.Exists(fn)) {
                Console.WriteLine("Überschreibt: " + fn);
            }
            sb.Append(entry.Text);
            System.IO.File.WriteAllText(fn, sb.ToString());
        }
    }
}


class NeueSekLitParser {
    private string filepath;
    private string filename;
    private XDocument document;
    private string? Jahreszahl;
    private string? Name;

    private List<(string Jahr, string Name, string Text, string Sort)> ParsedFiles;
    private StringBuilder? CurrentText;

    public NeueSekLitParser(string filepath) {
        this.filepath = filepath;
        this.filename = filepath.Split("/").Last();
        this.ParsedFiles = new List<(string Jahr, string Name, string Text, string Sort)>();
        if (Directory.Exists("./output/" + filename)) Directory.Delete("./output/" + filename, true);
        Directory.CreateDirectory("./output/" + filename);
        this.document = Helpers.ParseFile(filepath);
        this.CurrentText = new StringBuilder();
        foreach (var element in document.Descendants()) {
            if (element.Name == "h1") {
                if (Jahreszahl != null) InsertInto();
                this.Jahreszahl = element.Value.Trim();
            }
            if (Helpers.HasClass("Einzug", element)) { 
                if (CurrentText.Length != 0) {
                    InsertInto();
                }
                if (element.Descendants("b").Any()) {
                    char[] totrim = { '[', ']', ':', ',' };
                    var name = element.Descendants("b").First().Value.Trim();
                    this.Name = name.TrimEnd(totrim).TrimStart(totrim);

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
        var similarentries = ParsedFiles.Where(x => x.Jahr == Jahreszahl && x.Name == Name).ToList();
        if (!similarentries.Any()) {
            ParsedFiles.Add((Jahreszahl, Name, CurrentText.ToString(), "1"));
        }
        else {
            var fe = similarentries[0];
            fe.Sort = "1";
            ParsedFiles.Add((
                Jahreszahl,
                Name, 
                CurrentText.ToString(),
                (similarentries.Count() + 1).ToString()
            ));
        }
        
        CurrentText.Clear();
    }

    private void Flush() {
        foreach (var entry in this.ParsedFiles) {
            var sb = new StringBuilder();
            var fn = "./output/" + filename + "/";
            if (String.IsNullOrWhiteSpace(entry.Sort)) fn += entry.Jahr + "_" + entry.Name + ".html";
            else fn += entry.Jahr + "_" + entry.Name + "_" + entry.Sort + ".html";
            sb.AppendLine("---");
            sb.AppendLine("Jahr: " + entry.Jahr);
            sb.AppendLine("Autor: " + entry.Name);
            if (!String.IsNullOrWhiteSpace(entry.Sort)) sb.AppendLine("Sort: " + entry.Sort);
            sb.AppendLine("---");
            if (File.Exists(fn)) {
                Console.WriteLine("Überschreibt: " + fn);
            }
            sb.Append(entry.Text);
            System.IO.File.WriteAllText(fn, sb.ToString());
        }
    }
}