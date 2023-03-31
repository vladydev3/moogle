namespace MoogleEngine;

public static class Moogle
{
    public static SearchResult Query(string query) {
        Documents documents = new Documents();
        
        SearchItem[] items = new SearchItem[3] {
            new SearchItem(documents.GetTitle(0),documents.GetText(0), 0.9f),
            new SearchItem(documents.GetTitle(1),documents.GetText(1), 0.5f),
            new SearchItem(documents.GetTitle(2),documents.GetText(2), 0.4f),
        };
        return new SearchResult(items, query);//este query es el suggestion
    }

    
}
