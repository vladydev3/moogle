using System.Text.RegularExpressions;
using System.Text;
namespace MoogleEngine;

public class Documents
{
    //Titulo de cada documento
    public static string[] DocTitle = new string[0];
    //Texto de cada documento
    public static string[] DocText = new string[0];
    //Vocabulario donde se guardan las palabras con stemming y la cantidad de veces que aparecen en el texto
    public static Dictionary<string, int>[] stemwordFrec = new Dictionary<string, int>[0];
    //Variable solo para tener las palabras unicas sin stemming de cada doc
    public static HashSet<string>[] wordsinText = new HashSet<string>[0];
    //Valores IDF de cada palabra del corpus
    public static Dictionary<string, double> idf = new Dictionary<string, double>();
    //Valores TF-IDF de cada palabra de cada documento
    public static Dictionary<string, double>[] tf_idf = new Dictionary<string, double>[0];

    public static void LoadDocs()
    {
        string[] files = Directory.GetFiles(Path.Join("..", "Content"));
        DocTitle = new string[files.Length];
        DocText = new string[files.Length];
        wordsinText = new HashSet<string>[files.Length];

        stemwordFrec = new Dictionary<string, int>[DocText.Length];
        tf_idf = new Dictionary<string, double>[DocText.Length];
        
        Parallel.ForEach(files, (file, state, index) =>
        {
            //Tomamos el titulo del documento y lo agregamos a su respectiva lista
            string name = Path.GetFileNameWithoutExtension(file);
            DocTitle[index] = name;
            
            //Ahora hacemos lo mismo para el texto de cada doc
            Encoding encoding = Encoding.UTF8; // UTF-8 para manejar correctamente caracteres acentuados
            using (StreamReader reader = new StreamReader(file,encoding))
            {
                string content = reader.ReadToEnd();
                DocText[index] = content;
                string strippedContent = Regex.Replace(content, @"[\p{P}\p{S}]+", " "); // Eliminar signos de puntuación
                string[] words = strippedContent.ToLower().Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                wordsinText[index] = words.ToHashSet();
                string[] stemwords = StemmingText(words);
                //Ordenar las palabras del texto para contar luego las repetidas
                Array.Sort(stemwords);
                //Agrupar y contar las palabras repetidas
                var groupedWords = stemwords.GroupBy(w => w);
                Dictionary<string, int> wordCounts = groupedWords.ToDictionary(g => g.Key, g => g.Count());
                //Crear un nuevo hashset a partir del diccionario de counts
                HashSet<string> wordSet = new HashSet<string>(wordCounts.Keys);
                //Agregar el hashset al array de documentos
                stemwordFrec[index] = new Dictionary<string, int>(wordSet.Select(w => new KeyValuePair<string, int>(w, wordCounts[w])));
            }
        });
        IDFCorpus();
        CreateTF_IDF();
    }

private static void IDFCorpus()
{
    int N = stemwordFrec.Length; //Cantidad de documentos
    int maxDocs = (int)Math.Ceiling(N * 0.5); //Umbral para descartar palabras muy comunes

    Dictionary<string, int> docCount = new Dictionary<string, int>();

    // Recorre todas las palabras únicas y cuenta en cuántos documentos están
    foreach (var doc in stemwordFrec)
    {
        HashSet<string> uniqueWords = new HashSet<string>(doc.Keys);
        
        foreach (var word in uniqueWords)
        {
            if (!docCount.ContainsKey(word))
            {
                docCount[word] = 0;
            }
            
            docCount[word]++;
        }
    }

    // Calcula el IDF de cada palabra y descarta las muy comunes
    foreach (var word in docCount.Keys)
    {
        if (docCount[word] < maxDocs)
        {
            idf[word] = Math.Log10(N / (double)docCount[word]);
        }
    }
}
    public static double TF(int frec, int mostCommonWord)
    {
        return ((double)frec / (double)mostCommonWord);
    }

    public static void CreateTF_IDF()
    {
        tf_idf = new Dictionary<string, double>[stemwordFrec.Length];

        Parallel.ForEach(stemwordFrec, (document, state, i) =>
        {
            tf_idf[i] = new Dictionary<string, double>();
            int mostCommon = document.Values.Max();

            foreach (var word in document)
            {
                if (idf.ContainsKey(word.Key))
                {
                    if (!tf_idf[i].ContainsKey(word.Key))
                    {
                        tf_idf[i].Add(word.Key, TF(word.Value, mostCommon) * idf[word.Key]);
                    }
                }
            }
            tf_idf[i] = NormalizeVector(tf_idf[i]);
        });
    }
    public static Dictionary<string,double> NormalizeVector(Dictionary<string,double> vector){
        double sumOfSquares = Math.Sqrt(vector.Values.Sum(x => x * x));
        foreach (var row in vector)
        {
            vector[row.Key] = row.Value / sumOfSquares;
        }
        return vector;
    }
    public static string Stemming(string word){
        return SpanishStemmer.Stemmer(word);
    }
    public static string[] StemmingText(string[] words){
        string[] wordsStem = new string[words.Length];
        for(int i=0;i<words.Length;i++){
            wordsStem[i] = SpanishStemmer.Stemmer(words[i]);
        }
        return wordsStem;
    }
    public static string CreateSnippet(int indexDoc, string[] query)//Este metodo coge el snip 
    {
        string text = DocText[indexDoc];
        int middle = -1;
        int SnipLeng = 500;

        foreach (string word in query)
        {
            middle = text.IndexOf(word);
            if (middle >= 0)
            {
                if (text.Length <= SnipLeng)
                    return text;
                if (text.Length - middle <= SnipLeng)
                    return text.Substring(middle, text.Length - middle);

                if (middle < 250)
                    return text.Substring(0, SnipLeng);
                else
                    return text.Substring(middle - 250, SnipLeng);
            }
        }
        return text.Substring(0,500);
    }
}