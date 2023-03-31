using System.Linq;
using System.Text.RegularExpressions;
namespace MoogleEngine;

public class Documents{
    //Title of each doc
    public List<string> DocTitle = new List<string>();
    //Text of each doc
    public List<string> DocText = new List<string>();

    public long DocCount;
    public string GetTitle(int pos){
        return DocTitle[pos];
    }
    public string GetText(int pos){
        return DocText[pos];
    }
    public Documents(string docs_directory = "/home/vlady/universidad/moogle/moogle/Content/20news"){
        string[] files = Directory.GetFiles(docs_directory, "*.txt",SearchOption.AllDirectories);
        foreach (var item in files)
        {
            string name = Path.GetFileNameWithoutExtension(item);
            DocTitle.Add(name);
        }
        foreach (var item in files)
        {
            string content = File.ReadAllText(item);
            DocText.Add(content);
        }
        DocCount = DocTitle.Count;
    }
    
    
    
}