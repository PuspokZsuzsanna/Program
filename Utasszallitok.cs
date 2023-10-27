using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Utasszallitok
{
    class Sebessegkategoria
    {
        private int Utazosebesseg;
        public string Kategorianev
        {
            get
            {
                if (Utazosebesseg < 500) return "Alacsony sebességű";
                else if (Utazosebesseg < 1000) return "Szubszonikus";
                else if (Utazosebesseg < 1200) return "Transzszonikus";
                else return "Szuperszonikus";
            }
        }
        public Sebessegkategoria(int utazosebesseg)
        {
            Utazosebesseg = utazosebesseg;
        }
    }
    class Repulo
    {
        public string Tipus { get; private set; }
        public int Ev { get; private set; }
        public string Utas { get; private set; }
        public string Szemelyzet { get; private set; }
        public int Utazosebesseg { get; private set; }
        public int Felszallotomeg  { get; private set; }
        public double Fesztav { get; private set; }
        public string SebKategoria { get; private set; }

        public int MaxUtas => Utas.Split('-').Length == 1 ? int.Parse(Utas) : int.Parse(Utas.Split('-')[1]);

        public int MaxSzemelyzet => Szemelyzet.Split('-').Length == 1 ? int.Parse(Szemelyzet) : int.Parse(Szemelyzet.Split('-')[1]);

        public int FesztavLab =>  (int)Math.Round(Fesztav * 3.2808); // tetszőleges kerekítési módszert választhat a vizsgázó

        public int FelszallotomegTonna => (int)Math.Round(Felszallotomeg / 1000.0);

        public Repulo(string adatsor)
        {
            string[] m = adatsor.Split(';');
            Tipus = m[0];
            Ev = int.Parse(m[1]);
            Utas = m[2];
            Szemelyzet = m[3];
            Utazosebesseg = int.Parse(m[4]);
            Felszallotomeg = int.Parse(m[5]);
            Fesztav = double.Parse(m[6]);
            SebKategoria = new Sebessegkategoria(Utazosebesseg).Kategorianev;
        }
    }
    
    class Utasszallitok
    {
        static void Main()
        {
            List<Repulo> repulok = new List<Repulo>();
            foreach (var sor in File.ReadAllLines("utasszallitok.txt").Skip(1))
            {
                repulok.Add(new Repulo(sor));
            }

            Console.WriteLine($"4. feldat: Adatsorok száma: {repulok.Count}");

            Console.WriteLine($"5. feladat: Boeing típusok száma: {repulok.Count(x => x.Tipus.StartsWith("Boeing"))}");

            Console.WriteLine("6. feladat: A legtöbb utast szállító repülőgéptípus");
            Repulo maxUtasRepulo = repulok.First();
            foreach (var r in repulok.Skip(1))
            {
                if (r.MaxUtas > maxUtasRepulo.MaxUtas) maxUtasRepulo = r;
            }
            Console.WriteLine($"\tTípus: {maxUtasRepulo.Tipus}");
            Console.WriteLine($"\tElső felszállás: {maxUtasRepulo.Ev}");
            Console.WriteLine($"\tUtasok száma: {maxUtasRepulo.Utas}");
            Console.WriteLine($"\tSzemélyzet: {maxUtasRepulo.Szemelyzet}");
            Console.WriteLine($"\tUtazósebesség: {maxUtasRepulo.Utazosebesseg}");

            Console.Write("7. feladat:\n\t");
            Dictionary<string, int> katStat = new Dictionary<string, int>();
            katStat.Add("Alacsony sebességű", 0);
            katStat.Add("Szubszonikus", 0);
            katStat.Add("Transzszonikus", 0);
            katStat.Add("Szuperszonikus", 0);
            foreach (var r in repulok)
            {
                katStat[r.SebKategoria]++;
            }
            if (katStat.Values.Contains(0))
            {
                foreach (var k in katStat)
                {
                    if (k.Value == 0) Console.Write($"{k.Key} ");
                }
            } else Console.WriteLine("Minden sebességkategóriából van repülőgéptípus.");


            List<string> ki = new List<string>();
            ki.Add("típus;év;utas;személyzet;utazósebesség;felszállótömeg;fesztáv");
            foreach (var r in repulok)
            {
                ki.Add($"{r.Tipus};{r.Ev};{r.MaxUtas};{r.MaxSzemelyzet};{r.Utazosebesseg};{r.FelszallotomegTonna};{r.FesztavLab}");
            }
            File.WriteAllLines("utasszallitok_new.txt", ki);

            Console.ReadKey(); // Nem a megoldás része
        }
    }
}
