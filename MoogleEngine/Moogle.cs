using System.Diagnostics;
namespace MoogleEngine;

public static class Moogle
{
    //Para mostrar el tiempo de busqueda en la pagina
    public static TimeSpan time = new TimeSpan();
    //Para la sugerencia
    public static string suggestion = "";

    public static SearchResult QueryProcess(string query) {
        suggestion = ""; //Reiniciamos suggestion
        DateTime start = DateTime.Now;
        Stopwatch crono = new Stopwatch();
        crono.Start();
        Query queryprocessed = new Query(query);
        crono.Stop();
        Console.WriteLine("query: " +(double)crono.ElapsedMilliseconds / 1000);
        
        crono.Start();
        VectorModel vectorModel = new VectorModel(queryprocessed.queryVector);
        crono.Stop();
        Console.WriteLine("modelo vectorial: " +(double)crono.ElapsedMilliseconds / 1000);
        crono.Start();
        int resultsCount = vectorModel.Score.Count;
        int aDevolver = Math.Min(10,resultsCount);
        string[] snippet = new string[aDevolver];
        SearchItem[] items = new SearchItem[aDevolver];
        
        for (int i = 0; i < aDevolver; i++)
        {
            snippet[i] = Documents.CreateSnippet(vectorModel.Score[i].Item2, queryprocessed.queryVector.Keys.ToArray());
            items[i] = new SearchItem(Documents.DocTitle[vectorModel.Score[i].Item2],snippet[i],(float)vectorModel.Score[i].Item1);
        }
        time = DateTime.Now - start;
        crono.Stop();
        Console.WriteLine("lo demas: " +(double)crono.ElapsedMilliseconds / 1000);
        return new SearchResult(items, suggestion);
    }

}



/*

*/