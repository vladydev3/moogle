namespace MoogleEngine;

public class VectorModel
{
    //Para guardar el Score
    public List<(double,int)> Score = new List<(double, int)>();
    private static Dictionary<string,double> query = new Dictionary<string, double>(); 
    public VectorModel(Dictionary<string,double> queryVector)
    {
        query = queryVector;
        for (int i = 0; i < Documents.tf_idf_Matrix.Length; i++)
        {
            //Si el documento no es relevante, pasa a la otra iteración
            if (!RelevantDoc(Documents.tf_idf_Matrix[i], i)) continue;
            double similarity = CosineSimilarity(Documents.tf_idf_Matrix[i], queryVector);
            foreach (var word in Query.splitText)
            {
                //Si el documento contiene además de la raíz de la palabra, la palabra exacta a la que ingresó el usuario, se le multiplica * 10 el valor de score
                if(Documents.wordsinText[i].Contains(word))
                {
                    similarity *= 10;
                }
            }
            if (similarity>0) Score.Add((similarity,i));
        }
        Score = Score.OrderByDescending(x => x.Item1).ToList();
    }
    static double CosineSimilarity(Dictionary<string,double> vector1, Dictionary<string,double> vector2)
    {
        double dotProduct = DotProduct(vector1, vector2);
        double magnitude1 = Math.Sqrt(DotProduct(vector1, vector1));
        double magnitude2 = Math.Sqrt(DotProduct(vector2, vector2));

        return dotProduct / (magnitude1 * magnitude2);
    }

    static double DotProduct(Dictionary<string,double> vector1, Dictionary<string,double> vector2)
    {   //Producto escalar
        double dotProduct = 0;

        foreach (var word in vector2)
        {
            if (vector1.ContainsKey(word.Key)){    
                dotProduct += word.Value * vector1[word.Key];
            }
        }
        return dotProduct;
    }
    private static bool RelevantDoc(Dictionary<string,double> docTFIDF, int i){
        //Comprueba si el documento es relevante para la query
        string[] querykeys = query.Keys.ToArray();
        if (Query.requireoperator.Item1){
            for (int j = 0; j < Query.requireoperator.Item2.Count; j++)
            {
                if(!docTFIDF.ContainsKey(querykeys[Query.requireoperator.Item2[j]])) return false;
            }
        }
        if (Query.excludeoperator.Item1){
            for (int j = 0; j < Query.excludeoperator.Item2.Count; j++)
            {
                if(docTFIDF.ContainsKey(querykeys[Query.excludeoperator.Item2[j]])) return false;
            }
        }

        if(!Query.requireoperator.Item1 && !Query.excludeoperator.Item1) {
            foreach (KeyValuePair<string, double> word in query)
            {
                //Si contiene al menos una palabra de la query y esta tiene tf-idf mayor que 0.05 (o sea no es una stopword) el documento es válido
                if(docTFIDF.ContainsKey(word.Key) && word.Value > 0.05) return true;
            }       
            return false;
        }
        return true;
    }
}