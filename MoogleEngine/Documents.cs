using Porter2Stemmer;
namespace MoogleEngine;

public class Documents{
    //Titulo de cada documento
    public static List<string> DocTitle = new List<string>();
    //Texto de cada documento
    public static List<string> DocText = new List<string>();
    //Una lista de listas donde la interna tiene las palabras separadas de cada documento
    public static List<List<string>> TokenizedText = new List<List<string>>();
    public static List<Dictionary<string,int>> StemText = new List<Dictionary<string, int>>();
    //Valores TF-IDF de cada palabra de cada documento
    public static List<Dictionary<string,double>> tf_idf = new List<Dictionary<string, double>>();
    public static List<Dictionary<string,double>> tf_idf_stem = new List<Dictionary<string, double>>();

    public static List<Dictionary<string,int>> vocabulary = new List<Dictionary<string,int>>();
    public Documents(){
        //Lista donde cada elemento es un documento representado por las palabras y la cantidad de veces que se repiten
        int indextemp = 0;
        for (int i = 0; i < TokenizedText.Count; i++)
        {
            vocabulary.Add(new Dictionary<string, int>());
            StemText.Add(new Dictionary<string, int>());
            foreach (var word in TokenizedText[i])
            {   //Este itera sobre cada palabra del doc
                //Si la palabra ya la tenemos agregada al dic, le aumentamos la frecuencia
                if (vocabulary[indextemp].ContainsKey(word)){
                    vocabulary[indextemp][word]++;
                }//En caso contrario la agregamos
                else vocabulary[indextemp][word] = 1;
                //Llenamos el diccionario de las palabras stemmizadas
                if (StemText[indextemp].ContainsKey(Stemming(word))){
                    StemText[indextemp][Stemming(word)]++;
                }
                else StemText[indextemp][Stemming(word)] = 1;
            }
            indextemp++;
        }

        
        foreach (var document in vocabulary)
        {
            Dictionary<string,double> dictemp = new Dictionary<string, double>();
            foreach (var word in document)
            {
                if(TF_IDF(word.Value,document.Count,DocTitle.Count,DocsContainsWord(word.Key))>0.02d){
                    dictemp.Add(word.Key,TF_IDF(word.Value,document.Count,DocTitle.Count,DocsContainsWord(word.Key)));
                }
            }
            tf_idf.Add(dictemp);
        }
        foreach (var document in StemText){
            Dictionary<string,double> dictemp = new Dictionary<string, double>();
            foreach (var wordstem in document)
            {
                if(TF_IDF(wordstem.Value,document.Count,DocTitle.Count,DocsContainsSufix(wordstem.Key))>0.02d){
                    dictemp.Add(wordstem.Key,TF_IDF(wordstem.Value,document.Count,DocTitle.Count,DocsContainsSufix(wordstem.Key)));
                }
            }
            tf_idf_stem.Add(dictemp);
        }
        {
            
        }
    }
    public static double TF_IDF(int frec, int words, int docs, int docswithword){
        return ((Math.Log((double)frec+1)/(words))) * (Math.Log(((double)docs+1)/docswithword));
    }

    public static string Stemming(string word)
{
    var stemmer = new EnglishPorter2Stemmer();
    string stemmedWord = "";

    stemmedWord = stemmer.Stem(word).Value;

    return stemmedWord;
}

    public string GetTitle(int pos){
        return DocTitle[pos];
    }
    public string GetText(int pos){
        return DocText[pos];
    }
    public static void LoadDocs(){//Metodo que carga los archivos 
        string[] files = Directory.GetFiles("..", "*.txt",SearchOption.AllDirectories);
        foreach (var item in files)
        {
            //Tomamos el titulo del documento y lo agregamos a su respectiva lista
            string name = Path.GetFileNameWithoutExtension(item);
            DocTitle.Add(name);
            //Ahora hacemos lo mismo para el texto de cada doc
            string content = File.ReadAllText(item);
            DocText.Add(content);
            //Dividimos el texto en cada palabra y lo agregamos a la lista de listas                          
            TokenizedText.Add(content.Replace("\n","").Replace("\r","").ToLower().Split(" @$/#.-:&*+=[]?!(){},'\">_<;%\\".ToCharArray(),StringSplitOptions.RemoveEmptyEntries).ToList());                        
        }
    }
    public static int DocsContainsWord(string word){
        int count = 0;
        foreach (var document in vocabulary)
        {
            if(document.ContainsKey(word)){
                count++;
            }
        }
        return count;
    }
    public static int DocsContainsSufix(string sufix){
        int count = 0;
        foreach (var document in StemText)
        {
            if(document.ContainsKey(sufix)){
                count++;
            }
        }
        return count;
    }
}