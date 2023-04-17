namespace MoogleEngine;

public static class Moogle
{
    public static SearchResult QueryProcess(string query) {
        Documents documents = new Documents();
        Query queryprocessed = new Query(query);
        string suggestion = "";
        
        SearchItem[] items = new SearchItem[3] {
            new SearchItem(documents.GetTitle(0),documents.GetText(0), 0.7f),
            new SearchItem(documents.GetTitle(1),documents.GetText(1), 0.5f),
            new SearchItem(documents.GetTitle(2),documents.GetText(2), 0.4f),
        };

        bool queryOK = true;
        for (int i = 0; i < Query.splitText.Count; i++)
        {
            if(Documents.DocsContainsWord(Query.splitText[i])==0){
                queryOK = false;
            }
        }
        if(!queryOK){
            foreach (var item in Query.QueryVoc)
            {
                suggestion += item.Key + " ";
            }
        }
        return new SearchResult(items, suggestion);
    }

}
