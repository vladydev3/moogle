using System.Diagnostics;
namespace MoogleEngine;

public static class Moogle
{
    //Para mostrar el tiempo de busqueda en la pagina
    public static TimeSpan time = new TimeSpan();
    //Para la sugerencia
    public static string suggestion = "";

    public static SearchResult QueryProcess(string query) {
        suggestion = ""; //Reiniciamos la sugerencia en caso de que haya más de una búsqueda
        DateTime start = DateTime.Now; //Iniciamos el cronómetro
        
        Query queryprocessed = new Query(query); //Creamos el objeto para el procesamiento de la query
        VectorModel vectorModel = new VectorModel(queryprocessed.queryVector); //Aquí creamos el objeto que se ocupa de rankear los documentos
        
        int resultsCount = vectorModel.Score.Count;
        int aDevolver = Math.Min(10,resultsCount); //Se devolverá el mínimo entre los resultados obtenidos y 10, de esta forma evitamos que se devuelvan demasiados documentos
        //Le damos tamaño al array de snippets y al de los items
        string[] snippet = new string[aDevolver]; 
        SearchItem[] items = new SearchItem[aDevolver];
        //La función de la siguiente variable es poder utilizar el dict queryVector en el método que elabora el snippet
        var temp = queryprocessed.queryVector;
        for (int i = 0; i < aDevolver; i++)
        {
            snippet[i] = Documents.CreateSnippet(vectorModel.Score[i].Item2, temp);
            items[i] = new SearchItem(Documents.DocTitle[vectorModel.Score[i].Item2],snippet[i],(float)vectorModel.Score[i].Item1);
        }
        if(aDevolver > 5) suggestion = ""; //Si hay sufientes documentos no se muestra sugerencia 

        time = DateTime.Now - start; //Se detiene el reloj y se muestra el tiempo en la web
        return new SearchResult(items, suggestion);
    }

}



/*

*/