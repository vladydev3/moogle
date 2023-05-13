using System.Text.RegularExpressions;
using System.Text;
using SpanishStemmer;
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
    //Matriz TF-IDF
    public static Dictionary<string, double>[] tf_idf_Matrix = new Dictionary<string, double>[0];
    public static string[][] words = new string[0][];
    public static void LoadDocs()
    {
        string[] files = Directory.GetFiles(Path.Join("..", "Content"));
        //Le damos el tamaño correspondiente a los arrays necesarios
        DocTitle = new string[files.Length];
        DocText = new string[files.Length];
        wordsinText = new HashSet<string>[files.Length];
        words = new string[files.Length][];

        stemwordFrec = new Dictionary<string, int>[DocText.Length];
        tf_idf_Matrix = new Dictionary<string, double>[DocText.Length];

        Parallel.ForEach(files, (file, state, index) =>
        {
            //Tomamos el titulo del documento y lo agregamos a su respectiva lista
            string name = Path.GetFileNameWithoutExtension(file);
            DocTitle[index] = name;

            //Ahora hacemos lo mismo para el texto de cada doc 
            Encoding encoding = Encoding.UTF8; // UTF-8 para manejar correctamente caracteres acentuados
            using (StreamReader reader = new StreamReader(file, encoding))
            {
                string content = reader.ReadToEnd();
                DocText[index] = content;
                string strippedContent = Regex.Replace(content, @"[\p{P}\p{S}]+", " "); // Eliminar signos de puntuación
                words[index] = strippedContent.ToLower().Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                wordsinText[index] = words[index].ToHashSet(); //Almacenamos las palabras únicas de cada documento
                string[] stemmedwords = StemmingText(words[index]); //Le aplicamos Stemming a las palabras
                //Ordenar las palabras del texto para contar luego las repetidas
                Array.Sort(stemmedwords);
                //Agrupar y contar las palabras repetidas
                var groupedWords = stemmedwords.GroupBy(w => w);
                Dictionary<string, int> wordCounts = groupedWords.ToDictionary(g => g.Key, g => g.Count());
                //Crear un nuevo hashset a partir del diccionario de counts
                HashSet<string> wordSet = new HashSet<string>(wordCounts.Keys);
                //Agregar el hashset al array de documentos
                stemwordFrec[index] = new Dictionary<string, int>(wordSet.Select(w => new KeyValuePair<string, int>(w, wordCounts[w])));
            }
        });
        IDFCorpus();
        CreateTF_IDF_Matrix();
    }

    private static void IDFCorpus()
    {
        int N = stemwordFrec.Length; //Cantidad de documentos

        Dictionary<string, int> docCount = new Dictionary<string, int>(); //Para contar la cantidad de documentos que contienen a un término

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

        // Calcula el IDF de cada palabra
        foreach (var word in docCount.Keys)
        {
            idf[word] = Math.Log10(N / (double)docCount[word]);
        }
    }
    public static double TF(int frec, int mostCommonWord)
    {
        return ((double)frec / (double)mostCommonWord);
    }

    private static void CreateTF_IDF_Matrix()
    {
        tf_idf_Matrix = new Dictionary<string, double>[stemwordFrec.Length];

        Parallel.ForEach(stemwordFrec, (document, state, i) =>
        {
            tf_idf_Matrix[i] = new Dictionary<string, double>();
            int mostCommon = document.Values.Max(); //Guardamos la palabra que más se repite en el documento

            foreach (var word in document)
            {
                if (idf.ContainsKey(word.Key))
                {
                    if (!tf_idf_Matrix[i].ContainsKey(word.Key))
                    {
                        tf_idf_Matrix[i].Add(word.Key, TF(word.Value, mostCommon) * idf[word.Key]);
                    }
                }
            }
            tf_idf_Matrix[i] = NormalizeVector(tf_idf_Matrix[i]); //Normalizamos el vector
        });
    }
    public static Dictionary<string, double> NormalizeVector(Dictionary<string, double> vector)
    {
        double sumOfSquares = Math.Sqrt(vector.Values.Sum(x => x * x));
        foreach (var row in vector)
        {
            vector[row.Key] = row.Value / sumOfSquares;
        }
        return vector;
    }
    public static string Stemming(string word)
    {
        Stemmer stemmer = new Stemmer();
        return stemmer.Execute(word);
    }
    public static string[] StemmingText(string[] words)
    {
        string[] wordsStem = new string[words.Length];
        Stemmer stemmer = new Stemmer();
        for (int i = 0; i < words.Length; i++)
        {
            wordsStem[i] = stemmer.Execute(words[i]);
        }
        return wordsStem;
    }
    public static string CreateSnippet(int indexDoc, Dictionary<string, double> query)//Este metodo elabora el snippet 
    {
        string text = DocText[indexDoc];
        int middle = -1;
        int SnipLeng = 500;

        foreach (var word in query)
        {
            if (word.Value < 0.05) continue; //Esto para evitar que arme el snippet alrededor de alguna stopword 
            middle = text.IndexOf(word.Key); //Buscamos el índice de la palabra en el texto

            if (middle >= 0)
            {
                if (text.Length <= SnipLeng) //Si el texto tiene menos de 500 caracteres se devuelve el texto entero
                    return text;
                if (text.Length - middle <= SnipLeng)
                    return text.Substring(middle, text.Length - middle);

                if (middle < 250)
                    return text.Substring(0, SnipLeng);
                else
                    return text.Substring(middle - 250, SnipLeng);
            }
        }
        return text.Substring(0, 500);
    }
}