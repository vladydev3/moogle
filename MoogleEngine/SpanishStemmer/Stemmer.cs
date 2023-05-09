using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpanishStemmer
{
    public class Stemmer
    {
        public string Execute(string word)
        {
            return Execute(word, true);
        }

        public string Execute(string word, bool useStopWords)
        {
            string result = word;

            if (!useStopWords && Specials.stop_words.IndexOf(word) < 0)
            {
                if (word.Length >= 3)
                {
                    StringBuilder sb = new StringBuilder(word.ToLower());

                    if (sb[0] == '\'') sb.Remove(0, 1);

                    int r1 = 0, r2 = 0, rv = 0;
                    computeR1R2RV(sb, ref r1, ref r2, ref rv);

                    step0(sb, rv);
                    int cont = sb.Length;
                    step1(sb, r1, r2);

                    if (sb.Length == cont)
                    {
                        step2a(sb, rv);
                        if (sb.Length == cont)
                        {
                            step2b(sb, rv);
                        }
                    }
                    step3(sb, rv);
                    removeAcutes(sb);

                    result = sb.ToString().ToLower();
                }
            }

            return result;
        }

        private void computeR1R2RV(StringBuilder sb, ref int r1, ref int r2, ref int rv)
        {
            r1 = sb.Length;
            r2 = sb.Length;
            rv = sb.Length;

            //R1
            for (int i = 1; i < sb.Length; ++i)
            {
                if ((!isVowel(sb[i])) && (isVowel(sb[i - 1])))
                {
                    r1 = i + 1;
                    break;
                }
            }

            //R2
            for (int i = r1 + 1; i < sb.Length; ++i)
            {
                if ((!isVowel(sb[i])) && (isVowel(sb[i - 1])))
                {
                    r2 = i + 1;
                    break;
                }
            }

            //RV
            if (sb.Length >= 2)
            {
                if (!isVowel(sb[1]))
                {
                    for (int i = 1; i < sb.Length; ++i)
                    {
                        if (isVowel(sb[i]))
                        {
                            rv = sb.Length > i ? i + 1 : sb.Length;
                            break;
                        }
                    }
                }
                else
                {
                    if (isVowel(sb[0]) && isVowel(sb[1]))
                    {
                        for (int i = 1; i < sb.Length; ++i)
                        {
                            if (!isVowel(sb[i]))
                            {
                                rv = sb.Length > i ? i + 1 : sb.Length;
                                break;
                            }
                        }
                    }
                    else
                    {
                        rv = sb.Length >= 3 ? 3 : sb.Length;
                    }
                }
            }
        }

        private bool isVowel(char c)
        {
            return Specials.Vocales.IndexOf(c) >= 0;
        }

        private void step0(StringBuilder sb, int rv)
        {
            int index = -1;

            for (int i = 5; i > 1 && index < 0; --i)
            {
                if (sb.Length >= i)
                {
                    //Busco el indice del sufijo
                    index = Specials.Step0.LastIndexOf(sb.ToString(sb.Length - i, i));

                    //Si lo he encontrado...
                    if (index >= 0)
                    {
                        string aux = Specials.Step0[index];

                        //busco el indice de la palabra a la que debe preceder
                        int index_after = Specials.AfterStep0.LastIndexOf(aux);

                        //Si encuentro la palabra a la que debe preceder...
                        if (index_after >= 0)
                        {
                            string palabra = Specials.AfterStep0[index_after];

                            //Compruebo si esa palabra precede, efectivamente, al sufijo
                            if (sb.ToString(0, index).Substring(0, index_after).Length + palabra.Length == sb.ToString(0, index).Length)
                            {
                                if (Specials.AfterStep0[index_after] == "yendo" && sb[index_after - 1] == 'u' && index_after >= rv)
                                {
                                    sb.Remove(sb.Length - index, index);
                                }
                                else
                                {
                                    sb.Remove(sb.Length - index, index);
                                    for (int j = index_after; j < sb.Length; j++)
                                        sb[j] = Specials.EliminaAcento(sb[j]);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void step1(StringBuilder sb, int r1, int r2)
        {
            int posicion = -1;
            int coleccion = -1;
            string encontrada = "";
            string buscar = sb.ToString();

            foreach (string s in Specials.Step1_1)
            {
                int index = buscar.LastIndexOf(s);
                if (index >= 0)
                {
                    string palabra = buscar.Substring(index);
                    int aux = -1;

                    aux = Specials.Step1_1.LastIndexOf(palabra);
                    if (aux >= 0 && Specials.Step1_1[aux].Length > encontrada.Length)
                    {
                        encontrada =  Specials.Step1_1[aux];
                        posicion = index;
                        coleccion = 1;
                    }
                }
            }

            foreach (string s in Specials.Step1_2)
            {
                int index = buscar.LastIndexOf(s);
                if (index >= 0)
                {
                    string palabra = buscar.Substring(index);
                    int aux = -1;

                    aux = Specials.Step1_2.LastIndexOf(palabra);
                    if (aux >= 0 && Specials.Step1_2[aux].Length > encontrada.Length)
                    {
                        encontrada = Specials.Step1_2[aux];
                        posicion = index;
                        coleccion = 2;
                    }
                }
            }

            foreach (string s in Specials.Step1_3)
            {
                int index = buscar.LastIndexOf(s);
                if (index >= 0)
                {
                    string palabra = buscar.Substring(index);
                    int aux = -1;

                    aux = Specials.Step1_3.LastIndexOf(palabra);
                    if (aux >= 0 && Specials.Step1_3[aux].Length > encontrada.Length)
                    {
                        encontrada = Specials.Step1_3[aux];
                        posicion = index;
                        coleccion = 3;
                    }
                }
            }

            foreach (string s in Specials.Step1_4)
            {
                int index = buscar.LastIndexOf(s);
                if (index >= 0)
                {
                    string palabra = buscar.Substring(index);
                    int aux = -1;

                    aux = Specials.Step1_4.LastIndexOf(palabra);
                    if (aux >= 0 && Specials.Step1_4[aux].Length > encontrada.Length)
                    {
                        encontrada = Specials.Step1_4[aux];
                        posicion = index;
                        coleccion = 4;
                    }
                }
            }

            foreach (string s in Specials.Step1_5)
            {
                int index = buscar.LastIndexOf(s);
                if (index >= 0)
                {
                    string palabra = buscar.Substring(index);
                    int aux = -1;

                    aux = Specials.Step1_5.LastIndexOf(palabra);
                    if (aux >= 0 && Specials.Step1_5[aux].Length > encontrada.Length)
                    {
                        encontrada = Specials.Step1_5[aux];
                        posicion = index;
                        coleccion = 5;
                    }
                }
            }

            foreach (string s in Specials.Step1_6)
            {
                int index = buscar.LastIndexOf(s);
                if (index >= 0)
                {
                    string palabra = buscar.Substring(index);
                    int aux = -1;

                    aux = Specials.Step1_6.LastIndexOf(palabra);
                    if (aux >= 0 && Specials.Step1_6[aux].Length > encontrada.Length)
                    {
                        encontrada = Specials.Step1_6[aux];
                        posicion = index;
                        coleccion = 6;
                    }
                }
            }

            foreach (string s in Specials.Step1_7)
            {
                int index = buscar.LastIndexOf(s);
                if (index >= 0)
                {
                    string palabra = buscar.Substring(index);
                    int aux = -1;

                    aux = Specials.Step1_7.LastIndexOf(palabra);
                    if (aux >= 0 && Specials.Step1_7[aux].Length > encontrada.Length)
                    {
                        encontrada = Specials.Step1_7[aux];
                        posicion = index;
                        coleccion = 7;
                    }
                }
            }

            foreach (string s in Specials.Step1_8)
            {
                int index = buscar.LastIndexOf(s);
                if (index >= 0)
                {
                    string palabra = buscar.Substring(index);
                    int aux = -1;

                    aux = Specials.Step1_8.LastIndexOf(palabra);
                    if (aux >= 0 && Specials.Step1_8[aux].Length > encontrada.Length)
                    {
                        encontrada = Specials.Step1_8[aux];
                        posicion = index;
                        coleccion = 8;
                    }
                }
            }

            foreach (string s in Specials.Step1_9)
            {
                int index = buscar.LastIndexOf(s);
                if (index >= 0)
                {
                    string palabra = buscar.Substring(index);
                    int aux = -1;

                    aux = Specials.Step1_9.LastIndexOf(palabra);
                    if (aux >= 0 && Specials.Step1_9[aux].Length > encontrada.Length)
                    {
                        encontrada = Specials.Step1_9[aux];
                        posicion = index;
                        coleccion = 9;
                    }
                }
            }

            if (posicion >= 0)
            {
                switch (coleccion)
                {
                    case 1:
                        if (posicion >= r2)
                            sb.Remove(posicion, sb.Length - posicion);
                        break;
                    case 2:
                        if (posicion >= r2)
                            sb.Remove(posicion, sb.Length - posicion);
                        break;
                    case 3:
                        if (posicion >= r2)
                        {
                            sb.Remove(posicion, sb.Length - posicion);
                            sb.Append("log");
                        }
                        break;
                    case 4:
                        if (posicion >= r2)
                        {
                            sb.Remove(posicion, sb.Length - posicion);
                            sb.Append("u");
                        }
                        break;
                    case 5:
                        if (posicion >= r2)
                        {
                            sb.Remove(posicion, sb.Length - posicion);
                            sb.Append("ente");
                        }
                        break;
                    case 6:
                        if (posicion >= r1)
                            sb.Remove(posicion, sb.Length - posicion);
                        else
                        {
                            string aux = sb.ToString(0, posicion);
                            if (aux.Substring(0, aux.Length - 2) == "iv" &&
                                aux.Substring(0, aux.Length - 2) == "oc" &&
                                aux.Substring(0, aux.Length - 2) == "ic" &&
                                aux.Substring(0, aux.Length - 2) == "ad" && posicion >= r2)
                            {
                                sb.Remove(posicion, sb.Length - posicion);
                            }
                        }
                        break;
                    case 7:
                    case 8:
                    case 9:
                        if (posicion >= r2)
                        {
                            sb.Remove(posicion, sb.Length - posicion);
                        }
                        break;
                }
            }
        }

        private void step2a(StringBuilder sb, int rv)
        {
            int index = -1;

            //Busco el indice del sufijo
            index = Specials.Step2_a.IndexOf(sb.ToString());

            if (index >= rv && sb.ToString().Substring(sb.Length - index - 1, 1) == "u")
            {
                sb.Remove(sb.Length - index, index);
            }
        }

        private void step2b(StringBuilder sb, int rv)
        {
            string seleccionado = "";
            int pos = -1;
            int index = -1;

            foreach(string s in Specials.Step2_b1)
            {
                index = sb.ToString().LastIndexOf(s);
                if (index >= 0) // && Specials.Step2_b1[index].Length > seleccionado.Length)
                {
                    string palabra = sb.ToString().Substring(index);
                    int aux = index;

                    index = Specials.Step2_b1.LastIndexOf(palabra);
                    if (index >= 0)
                    {
                        seleccionado = Specials.Step2_b1[index];
                        pos = aux;
                    }
                }
            }

            if (pos >= rv && sb.ToString(sb.Length - pos - 2, pos) == "gu")
                sb.Remove(pos - 1, sb.Length - pos + 1);

            pos = -1;
            index = -1;
            seleccionado = "";

            foreach (string s in Specials.Step2_b2)
            {
                index = sb.ToString().LastIndexOf(s);
                if (index >= 0)
                {
                    string palabra = sb.ToString().Substring(index);
                    int aux = index;

                    index = Specials.Step2_b2.LastIndexOf(palabra);
                    if (index >= 0)
                    {
                        seleccionado = Specials.Step2_b2[index];
                        pos = aux;
                    }
                }
            }

            if (pos >= rv)
                sb.Remove(pos, sb.Length - pos);
        }

        private void step3(StringBuilder sb, int rv)
        {
            string seleccionado = "";
            int pos = -1;
            int index = -1;

            foreach (string s in Specials.Step3_1)
            {
                index = sb.ToString().LastIndexOf(s);
                if (index >= 0) // && Specials.Step3_1[index].Length > seleccionado.Length)
                {
                    string palabra = sb.ToString().Substring(index);
                    int aux = index;

                    index = Specials.Step3_1.LastIndexOf(palabra);
                    if (index >= 0)
                    {
                        seleccionado = Specials.Step3_1[index];
                        pos = aux;
                    }
                }
            }

            if (pos >= rv)
                sb.Remove(pos, sb.Length - pos);

            pos = -1;
            index = -1;
            seleccionado = "";

            foreach (string s in Specials.Step3_2)
            {
                index = sb.ToString().LastIndexOf(s);
                if (index >= 0)
                {
                    string palabra = sb.ToString().Substring(index);
                    int aux = index;

                    index = Specials.Step3_2.LastIndexOf(palabra);
                    if (index >= 0)
                    {
                        seleccionado = Specials.Step3_2[index];
                        pos = index;
                    }
                }
            }

            if (pos >= 0 && sb.ToString(sb.Length - pos - 2, pos) == "gu" && pos - 1 >= rv)
                sb.Remove(pos - 1, sb.Length - pos + 1);
        }

        private void removeAcutes(StringBuilder sb)
        {
            for(int i = 0; i < sb.Length; ++i)
            {
                char c = sb[i];
                sb[i] = Specials.EliminaAcento(c);
            }
        }
    }
}
