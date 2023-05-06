namespace MoogleEngine;

class Query{
    //Texto de la query
    private string text = "";
    public static string[] splitText = new string[0];
    //Pequeño diccionario que contiene las palabras de la query y los valores de tf-idf
    public Dictionary<string,double> queryVector = new Dictionary<string, double>();

    //Para saber si se hace algun cambio en la query
    public bool change = false;
    public Query(string query){
        queryVector = new Dictionary<string, double>();
        text = query;
        //SearchOperator(text);
        //Separamos el texto
        splitText = query.ToLower().Split(" ?~!*^,;".ToCharArray(),StringSplitOptions.RemoveEmptyEntries);
        for(int i=0;i<splitText.Length;i++)
        {
            string word = SpanishStemmer.Stemmer(splitText[i]);
            if(!Documents.idf.ContainsKey(word)){
                //string similar = SimilarWord(splitText[i]);
                //if(!String.IsNullOrEmpty(similar)) Moogle.suggestion += similar + " ";
            }         
            //Si la palabra ya la tenemos agregada al dic, le aumentamos la frecuencia
            else if (queryVector.ContainsKey(word)){
                queryVector[word]++;
            }//En caso contrario la agregamos
            else queryVector[word] = 1;
        }
        QueryProcessed();
        SearchOperator(query);
    }
    //Buscar palabras mas parecidas a la que no aparece en la query
    public static string SimilarWord(string NotFound){
        string similar = "";
        foreach (var diccionarios in Documents.wordsinText)
        {
            foreach (var word in diccionarios)
            {
                if(Levenshtein.DistanciaLev(NotFound, word)<(NotFound.Length/2) && Documents.idf.ContainsKey(word)){
                    similar = word;
                    break;
                }
            }
        }
        if(String.IsNullOrEmpty(similar)) return "";
        return similar;
    }

    private static int LevenshteinDistance(string s, string t)
    {
        int n = s.Length;
        int m = t.Length;
        int[,] d = new int[n + 1, m + 1];
        
        // Verify arguments.
        if (n == 0)
        {
            return m;
        }
        
        if (m == 0)
        {
            return n;
        }
        
        // Initialize arrays.
        for (int i = 0; i <= n; d[i, 0] = i++)
        {
        }
        
        for (int j = 0; j <= m; d[0, j] = j++)
        {
        }
        
        // Begin looping.
        for (int i = 1; i <= n; i++)
        {
            for (int j = 1; j <= m; j++)
            {
                // Compute cost.
                int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                d[i, j] = Math.Min(
                Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                d[i - 1, j - 1] + cost);
            }
        }
        // Return cost.
        return d[n, m];
    }

    private void QueryProcessed(){
        Dictionary<string,double> temp = new Dictionary<string, double>(queryVector.Count);
        foreach (var word in queryVector)
        {            
            double tfidf = 0;
            tfidf = Documents.TF((int)word.Value, queryVector.Count) * Documents.idf[word.Key];
            temp.Add(word.Key,tfidf);
        }
        queryVector = Documents.NormalizeVector(temp);
    }
    static void SearchOperator(string query){
    }

    
}

