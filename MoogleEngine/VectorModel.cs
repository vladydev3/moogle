namespace MoogleEngine;

public class VectorModel
{
    public List<(double,int)> Score = new List<(double, int)>();

    public VectorModel(Dictionary<string,double> queryVector)
    {
        for (int i = 0; i < Documents.tf_idf.Length; i++)
        {
            int count = 0;
            foreach (KeyValuePair<string, double> queryword in queryVector)
            {
                if(Documents.tf_idf[i].ContainsKey(queryword.Key)) count++;
            }
            if(count==0) continue;
            double similarity = CosineSimilarity(Documents.tf_idf[i], queryVector);
            foreach (var word in Query.splitText)
            {
                if(Documents.wordsinText[i].Contains(word))
                {
                    similarity += 10;
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
    {
        double dotProduct = 0;

        foreach (var word in vector2)
        {
            if (vector1.ContainsKey(word.Key)){    
                dotProduct += word.Value * vector1[word.Key];
            }
        }
        return dotProduct;
    }
}