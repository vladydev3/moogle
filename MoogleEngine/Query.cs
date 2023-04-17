using System;
namespace MoogleEngine;

class Query{
    public string text;
    public static List<string> splitText = new List<string>();
    public static Dictionary<string,int> StemmedQuery = new Dictionary<string, int>();
    public static Dictionary<string,int> QueryVoc = new Dictionary<string, int>();
    public static double[]? QueryVector;
    //Palabras que no aparecen en los documentos
    public Query(string query){
        QueryVoc = new Dictionary<string, int>();
        StemmedQuery = new Dictionary<string, int>();
        text = query;
        splitText = query.ToLower().Split(" ~!*^,;".ToCharArray(),StringSplitOptions.RemoveEmptyEntries).ToList();
        foreach (string word in splitText)
        {   //Si no hay docs que contengan una palabra, buscamos la mas similar
            if(Documents.DocsContainsWord(word)==0) {
                string similar = SimilarWord(word);
                if (QueryVoc.ContainsKey(similar)){
                    QueryVoc[similar]++;
                }
                else QueryVoc[similar] = 1;
            }
            //Si la palabra ya la tenemos agregada al dic, le aumentamos la frecuencia
            else if (QueryVoc.ContainsKey(word)){
                QueryVoc[word]++;
            }//En caso contrario la agregamos
            else QueryVoc[word] = 1;
            //Hacemos lo mismo para los sufijos
            if(Documents.DocsContainsSufix(Documents.Stemming(word))==0){
                string similarSufix = SimilarSufix(Documents.Stemming(word));
                if (StemmedQuery.ContainsKey(similarSufix)){
                    StemmedQuery[similarSufix]++;
                }
                else StemmedQuery[similarSufix] = 1;
            }
            else if(StemmedQuery.ContainsKey(Documents.Stemming(word))){
                StemmedQuery[Documents.Stemming(word)]++;
            }
            else StemmedQuery[Documents.Stemming(word)] = 1;
        }
        QueryProcessed();
    }
    //Buscar palabra mas parecida a la que no aparece en la query
    public static string SimilarWord(string NotFound){
        string similar = "";
        double similarTFIDF = 0;
        int distance = int.MaxValue;
        foreach (var diccionarios in Documents.tf_idf)
        {
            foreach (var word in diccionarios)
            {
                if(LevenshteinDistance(NotFound, word.Key)<distance){
                    distance = LevenshteinDistance(NotFound, word.Key);
                    similar = word.Key;
                    similarTFIDF = word.Value;
                }
                else if(LevenshteinDistance(NotFound, word.Key)==distance){
                    if(similarTFIDF<word.Value){
                        similar = word.Key;
                        similarTFIDF = word.Value;
                    }
                }
            }
        }
        
        return similar;
    }

    public static string SimilarSufix(string NotFound){
        string similarSufix = "";
        double similarTFIDF = 0;
        int distance = int.MaxValue;
        foreach (var diccionarios in Documents.tf_idf_stem)
        {
            foreach (var word in diccionarios)
            {
                if(LevenshteinDistance(NotFound, word.Key)<distance){
                    distance = LevenshteinDistance(NotFound, word.Key);
                    similarSufix = word.Key;
                    similarTFIDF = word.Value;
                }
                else if(LevenshteinDistance(NotFound, word.Key)==distance){
                    if(similarTFIDF<word.Value){
                        similarSufix = word.Key;
                        similarTFIDF = word.Value;
                    }
                }
            }
        }
        
        return similarSufix;
    }

    public static int LevenshteinDistance(string s, string t)
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

    public static void QueryProcessed(){
        List<double> temp = new List<double>();
        foreach (var word in QueryVoc)
        {            
            double tfidf = Documents.TF_IDF(word.Value,QueryVoc.Count,Documents.DocText.Count,Documents.DocsContainsWord(word.Key));
            if(tfidf>0.09d){
                temp.Add(tfidf);
            }
        }
        QueryVector = temp.ToArray();
    }
    
}

