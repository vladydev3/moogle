namespace MoogleEngine
{
    public static class Levenshtein
    {
        //Algoritmo de Levenshtein para calcular la cercania entre palabras
        //Recursivamente
        public static int DistanciaLev(string a, string b)
        {
            int sucio = int.MaxValue;
            DistanciaLev(a, b, 0, 0, 0, ref sucio);
            return sucio;
        }

        public static void DistanciaLev(string a, string b, int indexA, int indexB, int Cambios, ref int best)
        {
            if (Cambios >= 3) { return; }
            if (Cambios >= best || Cambios >= a.Length / 2) { return; }
            while (indexA <= a.Length - 1 && indexB <= b.Length - 1 && a[indexA] == b[indexB])
            {
                indexA++;
                indexB++;
            }
            if (indexA >= a.Length - 1 || indexB >= b.Length - 1)
            {
                best = Math.Min(Cambios + a.Length - indexA + b.Length - indexB, best);
                return;
            }
            DistanciaLev(a, b, indexA + 1, indexB, Cambios + 1, ref best);
            DistanciaLev(a, b, indexA, indexB + 1, Cambios + 1, ref best);
            DistanciaLev(a, b, indexA + 1, indexB + 1, Cambios + 1, ref best);
        }
    }
}