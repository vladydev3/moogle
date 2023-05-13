using System.Text.RegularExpressions;
namespace MoogleEngine;

class Query
{
    //Texto de la query
    private string text = "";
    public static string[] splitText = new string[0];
    //Diccionario que contiene las palabras de la query y los valores de tf-idf
    public Dictionary<string, double> queryVector = new Dictionary<string, double>();
    //Lista para almacenar el operador de importancia
    static List<int> k = new List<int>();
    //Variables de los operadores
    public static (bool, List<int>) requireoperator = (false, new List<int>());
    public static (bool, List<int>) excludeoperator = (false, new List<int>());
    public static (bool, (int, int)) proximityoperator;
    public Query(string query)
    {
        queryVector = new Dictionary<string, double>();
        requireoperator = (false, new List<int>());
        excludeoperator = (false, new List<int>());
        text = query;
        //Se separa el texto
        splitText = query.ToLower().Split(" ?,;".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        AnyOperator(splitText); //Busca si hay algún operador
        for (int i = 0; i < splitText.Length; i++)
        {
            //Se le aplica stemming a las palabras
            string word = Documents.Stemming(splitText[i]);
            //Buscar el operador de importancia
            word = ImportanceOperator(word);
            if (!Documents.idf.ContainsKey(word))
            {   //Si una palabra no aparece en el corpus se busca la similar y se almacena en suggestion
                string similar = SimilarWord(splitText[i]);
                if (!String.IsNullOrEmpty(similar)) Moogle.suggestion += similar + " ";
            }
            //Si la palabra ya la tenemos agregada al dic, le aumentamos la frecuencia
            else if (queryVector.ContainsKey(word))
            {
                queryVector[word]++;
            }//En caso contrario la agregamos
            else queryVector[word] = 1;
        }
        QueryProcessed();
    }
    //Buscar palabras mas parecidas a la que no aparece en la query
    public static string SimilarWord(string NotFound)
    {
        string similar = "";
        int shortestDistance = int.MaxValue;
        foreach (var diccionarios in Documents.wordsinText)
        {
            foreach (var word in diccionarios)
            {
                int distance = Levenshtein.DistanciaLev(NotFound, word);
                if (distance < shortestDistance)
                {   //Se guarda la menor distancia entre la palabra no encontrada y las palabras del corpus
                    shortestDistance = distance;
                    similar = word;
                }
            }
        }
        if (String.IsNullOrEmpty(similar)) return "";
        return similar;
    }

    private void QueryProcessed()
    {
        Dictionary<string, double> temp = new Dictionary<string, double>(queryVector.Count);
        int i = 0;
        foreach (var word in queryVector)
        {
            double tfidf = 0;
            tfidf = Documents.TF((int)word.Value, queryVector.Count) * Documents.idf[word.Key] * Math.Pow(2, k[i++]);//Para agregarle el operador de importancia
            temp.Add(word.Key, tfidf);
        }
        queryVector = Documents.NormalizeVector(temp);
    }

    private static void AnyOperator(string[] query)
    {
        int pos = Array.IndexOf(query,"~");
        if(pos != -1){
            proximityoperator.Item1 = true;
            proximityoperator.Item2 = (pos-1,pos);
            var temp = query.ToList();
            temp.Remove("~");
            splitText = temp.ToArray();
            query = splitText;
        }
        else proximityoperator.Item1 = false;
        for (int i = 0; i < query.Length; i++)
        {
            if (query[i].Contains("^"))
            {
                requireoperator.Item1 = true;
                requireoperator.Item2.Add(i);
                splitText[i] = splitText[i].Replace("^", "");
            }
            if (query[i].Contains("!"))
            {
                excludeoperator.Item1 = true;
                excludeoperator.Item2.Add(i);
                splitText[i] = splitText[i].Replace("!", "");
            }
        }
    }
    private string ImportanceOperator(string queryWord)
    {
        k.Add(queryWord.Count(c => c == '*'));
        return queryWord.Replace("*", "");
    }
}

