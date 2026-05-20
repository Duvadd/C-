using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

class Kiadas
{
    public int Ev { get; set; }
    public int Negyedev { get; set; }
    public string Eredet { get; set; }
    public string Leiras { get; set; }
    public int Peldanyszam { get; set; }

    public Kiadas(string sor)
    {
        string[] adatok = sor.Split(';');

        Ev = int.Parse(adatok[0]);
        Negyedev = int.Parse(adatok[1]);
        Eredet = adatok[2];
        Leiras = adatok[3];
        Peldanyszam = int.Parse(adatok[4]);
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Ékezetes kiírás és beolvasás miatt
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        // 1. feladat
        List<Kiadas> kiadasok = new List<Kiadas>();

        foreach (string sor in File.ReadAllLines("kiadas.txt", Encoding.UTF8))
        {
            kiadasok.Add(new Kiadas(sor));
        }

        // 2. feladat
        Console.WriteLine("2. feladat:");
        Console.Write("Szerző: ");
        string szerzo = Console.ReadLine();

        int szerzoDb = 0;

        foreach (Kiadas k in kiadasok)
        {
            if (k.Leiras.Contains(szerzo))
            {
                szerzoDb++;
            }
        }

        if (szerzoDb == 0)
        {
            Console.WriteLine("Nem adtak ki");
        }
        else
        {
            Console.WriteLine($"{szerzoDb} könyvkiadás");
        }

        // 3. feladat
        Console.WriteLine("3. feladat:");

        int maxPeldany = kiadasok[0].Peldanyszam;

        foreach (Kiadas k in kiadasok)
        {
            if (k.Peldanyszam > maxPeldany)
            {
                maxPeldany = k.Peldanyszam;
            }
        }

        int maxDb = 0;

        foreach (Kiadas k in kiadasok)
        {
            if (k.Peldanyszam == maxPeldany)
            {
                maxDb++;
            }
        }

        Console.WriteLine($"Legnagyobb példányszám: {maxPeldany}, előfordult {maxDb} alkalommal");

        // 4. feladat
        Console.WriteLine("4. feladat:");

        int index = 0;

        while (!(kiadasok[index].Eredet == "kf" && kiadasok[index].Peldanyszam >= 40000))
        {
            index++;
        }

        Kiadas elso = kiadasok[index];

        Console.WriteLine($"{elso.Ev}/{elso.Negyedev}. {elso.Leiras}");

        // 5. feladat
        Console.WriteLine("5. feladat:");

        Dictionary<int, int> magyarKiadas = new Dictionary<int, int>();
        Dictionary<int, int> magyarPeldany = new Dictionary<int, int>();
        Dictionary<int, int> kulfoldiKiadas = new Dictionary<int, int>();
        Dictionary<int, int> kulfoldiPeldany = new Dictionary<int, int>();

        foreach (Kiadas k in kiadasok)
        {
            if (!magyarKiadas.ContainsKey(k.Ev))
            {
                magyarKiadas[k.Ev] = 0;
                magyarPeldany[k.Ev] = 0;
                kulfoldiKiadas[k.Ev] = 0;
                kulfoldiPeldany[k.Ev] = 0;
            }

            if (k.Eredet == "ma")
            {
                magyarKiadas[k.Ev]++;
                magyarPeldany[k.Ev] += k.Peldanyszam;
            }
            else
            {
                kulfoldiKiadas[k.Ev]++;
                kulfoldiPeldany[k.Ev] += k.Peldanyszam;
            }
        }

        Console.WriteLine("Év\tMagyar kiadás\tMagyar példányszám\tKülföldi kiadás\tKülföldi példányszám");

        foreach (int ev in magyarKiadas.Keys.OrderBy(x => x))
        {
            Console.WriteLine($"{ev}\t{magyarKiadas[ev]}\t{magyarPeldany[ev]}\t{kulfoldiKiadas[ev]}\t{kulfoldiPeldany[ev]}");
        }

        // 5/b. feladat - tabla.html
        using (StreamWriter sw = new StreamWriter("tabla.html", false, Encoding.UTF8))
        {
            sw.WriteLine("<table>");
            sw.WriteLine("<tr><th>Év</th><th>Magyar kiadás</th><th>Magyar példányszám</th><th>Külföldi kiadás</th><th>Külföldi példányszám</th></tr>");

            foreach (int ev in magyarKiadas.Keys.OrderBy(x => x))
            {
                sw.WriteLine($"<tr><td>{ev}</td><td>{magyarKiadas[ev]}</td><td>{magyarPeldany[ev]}</td><td>{kulfoldiKiadas[ev]}</td><td>{kulfoldiPeldany[ev]}</td></tr>");
            }

            sw.WriteLine("</table>");
        }

        // 6. feladat
        Console.WriteLine("6. feladat:");
        Console.WriteLine("Legalább kétszer, nagyobb példányszámban újra kiadott könyvek:");

        Dictionary<string, List<int>> konyvek = new Dictionary<string, List<int>>();

        foreach (Kiadas k in kiadasok)
        {
            if (!konyvek.ContainsKey(k.Leiras))
            {
                konyvek[k.Leiras] = new List<int>();
            }

            konyvek[k.Leiras].Add(k.Peldanyszam);
        }

        foreach (var konyv in konyvek)
        {
            string leiras = konyv.Key;
            List<int> peldanyok = konyv.Value;

            int elsoKiadasPeldanyszama = peldanyok[0];

            int nagyobbUjrakiadasDb = 0;

            for (int i = 1; i < peldanyok.Count; i++)
            {
                if (peldanyok[i] > elsoKiadasPeldanyszama)
                {
                    nagyobbUjrakiadasDb++;
                }
            }

            if (nagyobbUjrakiadasDb >= 2)
            {
                Console.WriteLine(leiras);
            }
        }
    }
}