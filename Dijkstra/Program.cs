using System;
using System.Collections.Generic;
using System.Linq;
namespace Dijkstra
{
    class Program
    {
        static void Main(string[] args)
        {
            Graf graf = new Graf();
            List<Rute> rute = graf.FindKortestRute("Esbjerg", "Thisted");

            int afstand = 0;
            Console.WriteLine($"Den korteste rute fra {rute[0].Fra.Bynavn} til {rute[rute.Count-1].Til.Bynavn}");
            foreach (Rute item in rute)
            {
                Console.WriteLine($"{item.Fra.Bynavn} ---> {item.Til.Bynavn}");
                afstand += item.Afstand;
            }
            Console.WriteLine();
            Console.WriteLine($"Afstanden er {afstand}");
        }

        
    }

    class Graf
    {

        private List<By> byer = new List<By>();

        public List<By> Byer
        {
            get { return byer; }
            set { byer = value; }
        }

        //Instancierer alle byer og ruter og lægger dem i listen "byer".
        public Graf()
        {
            //Alt data i grafen
            By b1 = new By("Aalborg");
            By b2 = new By("Randers");
            By b3 = new By("Herning");
            By b4 = new By("Aarhus");
            By b5 = new By("Horsens");
            By b6 = new By("Esbjerg");
            By b7 = new By("Kolding");
            By b8 = new By("Holstebro");
            By b9 = new By("Thisted");
            By b10 = new By("Vejle");
            By b11 = new By("Hjørring");
            By b12 = new By("Viborg");
            By b13 = new By("Silkeborg");

            By.BeggeVeje(b1, b9, 92);
            By.BeggeVeje(b1, b2, 92);
            By.BeggeVeje(b9, b8, 84);
            By.BeggeVeje(b2, b4, 39);
            By.BeggeVeje(b4, b5, 51);
            By.BeggeVeje(b3, b5, 81);
            By.BeggeVeje(b3, b10, 74);
            By.BeggeVeje(b3, b6, 88);
            By.BeggeVeje(b10, b7, 28);
            By.BeggeVeje(b6, b10, 84);
            By.BeggeVeje(b6, b7, 71);
            By.BeggeVeje(b10, b5, 30);
            By.BeggeVeje(b3, b8, 29);
            By.BeggeVeje(b1, b11, 53);
            By.BeggeVeje(b11, b9, 121);
            By.BeggeVeje(b2, b12, 45);
            By.BeggeVeje(b12, b3, 48);
            By.BeggeVeje(b12, b8, 51);
            By.BeggeVeje(b12, b1, 80);
            By.BeggeVeje(b12, b13, 36);
            By.BeggeVeje(b13, b4, 45);
            By.BeggeVeje(b13, b3, 44);
            By.BeggeVeje(b13, b5, 49);

            Byer.Add(b1);
            Byer.Add(b2);
            Byer.Add(b3);
            Byer.Add(b4);
            Byer.Add(b5);
            Byer.Add(b6);
            Byer.Add(b7);
            Byer.Add(b8);
            Byer.Add(b9);
            Byer.Add(b10);
            Byer.Add(b11);
            Byer.Add(b12);
            Byer.Add(b13);
        }


        public List<Rute> FindKortestRute(string fra, string til)
        {
            By kiggerPå = null;
            By starterFra = byer.Find(p => p.Bynavn == fra);
            By lederEfter = byer.Find(p => p.Bynavn == til);
            List<By> byListe = new List<By>();
            byListe.Add(starterFra);
            

            foreach (By by in byer)
            {

                //Hvis der er fundet en rute til byen der ledes efter, fjernes alle byer fra listen, hvis rute i forvejen er længere end den allerede fundne rute.
                if (lederEfter.KortesteRuteAfstand() != 0)
                {
                    byListe.RemoveAll(b => b.KortesteRuteAfstand() > lederEfter.KortesteRuteAfstand());
                }

                //Hvis der ikke er flere byer på gennemløbslisten, løbes algoritmen ikke igennem mere.
                if (byListe.Count > 0 ) {

                    //Listen sorteres de efter afstand til byen fra start.
                    //Byen med korteste rute, er byen der løbes igennem næste gang.
                    byListe.OrderBy(b => b.KortesteRute);
                    kiggerPå = byListe[0];
                    kiggerPå.Besøgt = true;
                    byListe.Remove(kiggerPå);
                
                

                    foreach (Rute rute in kiggerPå.Ruter)
                    {
                        //Hvis rutes til-by ikke er på gennemløbslisten og ikke er besøgt, tilføjes den til.
                        if (!rute.Til.Besøgt && !byListe.Contains(rute.Til))
                        {
                            byListe.Add(rute.Til);
                        }

                        //Hvis en by ikke har en kortesteRute er afstanden 0.
                        //Er afstanden 0 eller hvis rutens fra-byens kortesteRute + rutens afstand er kortere end rutens til-by, sættes den korteste rute til byen til den sammen som byen vi kommer fra + den rute der føre til den nye by 

                        if (rute.Til.KortesteRuteAfstand() == 0 || (rute.Fra.KortesteRuteAfstand() + rute.Afstand) < rute.Til.KortesteRuteAfstand())
                        {
                            rute.Til.KortesteRute = new List<Rute>();
                            foreach (Rute r in rute.Fra.KortesteRute)
                            {
                                rute.Til.KortesteRute.Add(r);
                            }
                            rute.Til.KortesteRute.Add(rute);

                        }
                    }
                } else
                {
                    //Stopper foreach-loopet hvis gennemløbslisten er tom.
                    break;
                }
            }

            //Returnere den korteste rute til byen man leder efter.
            return lederEfter.KortesteRute;
        }
    }

    class By
    {
        private string bynavn;

        public string Bynavn
        {
            get { return bynavn; }
            set { bynavn = value; }
        }

        private List<Rute> ruter = new List<Rute>();

        public List<Rute> Ruter
        {
            get { return ruter; }
            set { ruter = value; }
        }

        private bool besøgt = false;

        public bool Besøgt
        {
            get { return besøgt; }
            set { besøgt = value; }
        }


        public void TilføjRute(Rute rute)
        {
            ruter.Add(rute);
        }

        private List<Rute> kortesteRute = new List<Rute>();

        public List<Rute> KortesteRute
        {
            get { return kortesteRute; }
            set { kortesteRute = value; }
        }

        //Returnerer den samlede afstand for hele ruten til byen.
        public int KortesteRuteAfstand()
        {
            int afstand = 0;
            foreach (Rute rute in kortesteRute)
            {
                afstand += rute.Afstand;
            }

            return afstand;
        }

        public By(string bynavn)
        {
            this.bynavn = bynavn;
        }

        //Sætter ruten på begge byer.
        public static void BeggeVeje(By til, By fra, int afstand)
        {
            Rute r1 = new Rute(til, fra, afstand);
            fra.TilføjRute(r1);

            Rute r2 = new Rute(fra, til, afstand);
            til.TilføjRute(r2);
        }

    }

    class Rute
    {
        public Rute()
        {
            fra = null;
            til = null;
            afstand = 0;
        }

        public Rute(By til, By fra, int afstand) : this()
        {
            this.til = til;
            this.fra = fra;
            this.afstand = afstand;
        }
        
        private By fra;

        public By Fra
        {
            get { return fra; }
            set { fra = value; }
        }

        private By til;

        public By Til
        {
            get { return til; }
            set { til = value; }
        }

        private int afstand;

        public int Afstand
        {
            get { return afstand; }
            set { afstand = value; }
        }
    }
}
