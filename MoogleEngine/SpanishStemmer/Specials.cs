using System.Collections.Generic;

namespace SpanishStemmer
{
    public static class Specials
    {
        public static List<char> Vocales = new List<char>() { 'a', 'e', 'i', 'o', 'u' };

        public static List<char> VocalesAcentuadas = new List<char>() { 
            'á', 'é', 'í', 'ó', 'ú',
            'à', 'è', 'ì', 'ò', 'ù',
            'ä', 'ë', 'ï', 'ö', 'ü',
            'Á', 'É', 'Í', 'Ó', 'Ú',
            'À', 'È', 'Ì', 'Ò', 'Ù',
            'Ä', 'Ë', 'Ï', 'Ö', 'Ü'
        };

        public static List<string> Step0 = new List<string>() {
            "me", "se", "sela", "selo", "selas", "selos", "la", "le", "lo", "las", "les", "los", "nos"
        };

        public static List<string> AfterStep0 = new List<string>() {
            "iéndo", "ándo", "ár", "ér", "ír", "ando", "iendo", "ar", "er", "ir", "yendo"
        };

        public static List<string> Step1_1 = new List<string>() {
            "anza", "anzas", "ico", "ica", "icos", "icas", "ismo", "ismos", "able", "ables", "ible", "ibles", "ista", "istas", "oso", "osa", "osos", "osas", "amiento", "amientos", "imiento", "imientos"
        };

        public static List<string> Step1_2 = new List<string>() {
            "adora", "ador", "ación", "adoras", "adores", "aciones", "ante", "antes", "ancia", "ancias"
        };

        public static List<string> Step1_3 = new List<string>() {
            "logía", "logías"
        };

        public static List<string> Step1_4 = new List<string>() {
            "ución", "uciones"
        };

        public static List<string> Step1_5 = new List<string>() {
            "encia", "encias"
        };

        public static List<string> Step1_6 = new List<string>() {
            "amente"
        };

        public static List<string> Step1_7 = new List<string>() {
            "mente"
        };

        public static List<string> Step1_8 = new List<string>() {
            "idad", "idades"
        };

        public static List<string> Step1_9 = new List<string>() {
            "iva", "ivo", "ivas", "ivos"
        };

        public static List<string> Step2_a = new List<string>() {
            "yeron", "yendo", "yamos", "yais", "yan", "yen", "yas", "yes", "ya", "ye", "yo", "yó"
        };

        public static List<string> Step2_b1 = new List<string>() {
            "en", "es", "éis", "emos"
        };

        public static List<string> Step2_b2 = new List<string>() {
            "arían", "arías", "arán", "arás", "aríais", "aría", "aréis", "aríamos", "aremos", "ará", 
            "aré", "erían", "erías", "erán", "erás", "eríais", "ería", "eréis", "eríamos", "eremos", 
            "erá", "eré", "irían", "irías", "irán", "irás", "iríais", "iría", "iréis", "iríamos", 
            "iremos", "irá", "iré", "aba", "ada", "ida", "ía", "ara", "iera", "ad", "ed", "id", "ase",
            "iese", "aste", "iste", "an", "aban", "ían", "aran", "ieran", "asen", "iesen", "aron", 
            "ieron", "ado", "ido", "ando", "iendo", "ió", "ar", "er", "ir", "as", "abas", "adas", 
            "idas", "ías", "aras", "ieras", "ases", "ieses", "ís", "áis", "abais", "íais", "arais", 
            "ierais", "aseis", "ieseis", "asteis", "isteis", "ados", "idos", "amos", "ábamos", "íamos", 
            "imos", "áramos", "iéramos", "iésemos", "ásemos"
        };

        public static List<string> Step3_1 = new List<string>() {
            "os", "a", "o", "á", "í", "ó"
        };

        public static List<string> Step3_2 = new List<string>() {
            "e", "é"
        };

        public static List<string> stop_words = new List<string>() {
            "de", "la", "que", "el", "en", "y", "a", "los", "del", "se", "las", "por", "un", "para", "con", 
            "no", "una", "su", "al", "es", "lo", "como", "más", "pero", "sus", "le", "ya", "o", "fue", "este", 
            "ha", "sí", "porque", "esta", "son", "entre", "está", "cuando", "muy", "sin", "sobre", "ser", 
            "tiene", "también", "me", "hasta", "hay", "donde", "han", "quien", "están", "estado", "desde", 
            "todo", "nos", "durante", "estados", "todos", "uno", "les", "ni", "contra", "otros", "fueron", 
            "ese", "eso", "había", "ante", "ellos", "e", "esto", "mí", "antes", "algunos", "qué", "unos", 
            "yo", "otro", "otras", "otra", "él", "tanto", "esa", "estos", "mucho", "quienes", "nada", "muchos", 
            "cual", "sea", "poco", "ella", "estar", "haber", "estas", "estaba", "estamos", "algunas", "algo", 
            "nosotros", "mi", "mis", "tú", "te", "ti", "tu", "tus", "ellas", "nosotras", "vosotros", "vosotras", 
            "os", "mío", "mía", "míos", "mías", "tuyo", "tuya", "tuyos", "tuyas", "suyo", "suya", "suyos", "suyas", 
            "nuestro", "nuestra", "nuestros", "nuestras", "vuestro", "vuestra", "vuestros", "vuestras", "esos", 
            "esas", "estoy", "estás", "está", "estamos", "estáis", "están", "esté", "estés", "estemos", "estéis", 
            "estén", "estaré", "estarás", "estará", "estaremos", "estaréis", "estarán", "estaría", "estarías", 
            "estaríamos", "estaríais", "estarían", "estaba", "estabas", "estábamos", "estabais", "estaban", 
            "estuve", "estuviste", "estuvo", "estuvimos", "estuvisteis", "estuvieron", "estuviera", "estuvieras", 
            "estuviéramos", "estuvierais", "estuvieran", "estuviese", "estuvieses", "estuviésemos", "estuvieseis", 
            "estuviesen", "estando", "estado", "estada", "estados", "estadas", "estad", "he", "has", "ha", "hemos", 
            "habéis", "han", "haya", "hayas", "hayamos", "hayáis", "hayan", "habré", "habrás", "habrá", "habremos", 
            "habréis", "habrán", "habría", "habrías", "habríamos", "habríais", "habrían", "había", "habías", 
            "habíamos", "habíais", "habían", "hube", "hubiste", "hubo", "hubimos", "hubisteis", "hubieron", 
            "hubiera", "hubieras", "hubiéramos", "hubierais", "hubieran", "hubiese", "hubieses", "hubiésemos", 
            "hubieseis", "hubiesen", "habiendo", "habido", "habida", "habidos", "habidas", "soy", "eres", "es", 
            "somos", "sois", "son", "sea", "seas", "seamos", "seáis", "sean", "seré", "serás", "será", "seremos", 
            "seréis", "serán", "sería", "serías", "seríamos", "seríais", "serían", "era", "eras", "éramos", "erais", 
            "eran", "fui", "fuiste", "fue", "fuimos", "fuisteis", "fueron", "fuera", "fueras", "fuéramos", "fuerais", 
            "fueran", "fuese", "fueses", "fuésemos", "fueseis", "fuesen", "siendo", "sido", "sed", "tengo", "tienes", 
            "tiene", "tenemos", "tenéis", "tienen", "tenga", "tengas", "tengamos", "tengáis", "tengan", "tendré", 
            "tendrás", "tendrá", "tendremos", "tendréis", "tendrán", "tendría", "tendrías", "tendríamos", "tendríais", 
            "tendrían", "tenía", "tenías", "teníamos", "teníais", "tenían", "tuve", "tuviste", "tuvo", "tuvimos", 
            "tuvisteis", "tuvieron", "tuviera", "tuvieras", "tuviéramos", "tuvierais", "tuvieran", "tuviese", "tuvieses", 
            "tuviésemos", "tuvieseis", "tuviesen", "teniendo", "tenido", "tenida", "tenidos", "tenidas", "tened"
        };

        public static char EliminaAcento(char c)
        {
            char res = c;

            switch (c)
            {
                case 'á':
                case 'à':
                case 'ä':
                    res = 'a';
                    break;
                case 'é':
                case 'è':
                case 'ë':
                    res = 'e';
                    break;
                case 'í':
                case 'ì':
                case 'ï':
                    res = 'i';
                    break;
                case 'ó':
                case 'ò':
                case 'ö':
                    res = 'o';
                    break;
                case 'ú':
                case 'ù':
                case 'ü':
                    res = 'u';
                    break;
            }

            return res;
        }
    }
}