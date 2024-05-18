using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pisti2
{
    public enum Sembol
    {
        Kupa,
        Karo,
        Sinek,
        Maca
    }

    public enum Numara
    {
        İki,
        Üç,
        Dört,
        Beş,
        Altı,
        Yedi,
        Sekiz,
        Dokuz,
        On,
        Vale,
        Kız,
        Kral,
        As
    }

    public class Kart
    {
        public Sembol Sembol { get; }
        public Numara Numara { get; }

        public Kart(Sembol sembol, Numara numara)
        {
            Sembol = sembol;
            Numara = numara;
        }

        public override string ToString()
        {
            return $"{Sembol} {Numara}";
        }
    }

    public class Destesi
    {
        public List<Kart> kartlar;
        public Random rastgele = new Random();

        public Destesi()
        {
            kartlar = new List<Kart>();
            foreach (Sembol sembol in Enum.GetValues(typeof(Sembol)))
            {
                foreach (Numara numara in Enum.GetValues(typeof(Numara)))
                {
                    kartlar.Add(new Kart(sembol, numara));
                }
            }
        }

        public void Karistir(int defa)
        {
            for (int i = 0; i < defa; i++)
            {
                int n = kartlar.Count;
                while (n > 1)
                {
                    n--;
                    int k = rastgele.Next(n + 1);
                    Kart deger = kartlar[k];
                    kartlar[k] = kartlar[n];
                    kartlar[n] = deger;
                }
            }
        }

        public Kart KartCek()
        {
            if (kartlar.Count > 0)
            {
                Kart cekilenKart = kartlar[0];
                kartlar.RemoveAt(0);
                return cekilenKart;
            }
            return null;
        }
    }

    public class Oyuncu
    {
        public string Ad { get; }
        public int DogumGunu { get; }
        public List<Kart> El { get; }

        public Oyuncu(string ad, int dogumGunu)
        {
            Ad = ad;
            DogumGunu = dogumGunu;
            El = new List<Kart>();
        }
    }

    public class PistiOyunu
    {
        public Oyuncu Oyuncu1 { get; }
        public Oyuncu Oyuncu2 { get; }
        public List<Kart> MasaKartlari { get; }

        public PistiOyunu(Oyuncu oyuncu1, Oyuncu oyuncu2)
        {
            Oyuncu1 = oyuncu1;
            Oyuncu2 = oyuncu2;
            MasaKartlari = new List<Kart>();
        }

        public void Oyna()
        {
            Destesi destesi = new Destesi();
            destesi.Karistir(Math.Max(Oyuncu1.DogumGunu, Oyuncu2.DogumGunu));
            for (int i = 0; i < 4; i++)
            {
                Oyuncu1.El.Add(destesi.KartCek());
                Oyuncu2.El.Add(destesi.KartCek());
            }

            MasaKartlari.Add(destesi.KartCek());
        }
    }

    public class Pisti
    {
        public Oyuncu Oyuncu1 { get; }
        public Oyuncu Oyuncu2 { get; }
        public List<Kart> MasaKartlari { get; private set; }
        public int Oyuncu1Skoru { get; private set; }
        public int Oyuncu2Skoru { get; private set; }

        public Pisti(Oyuncu oyuncu1, Oyuncu oyuncu2)
        {
            Oyuncu1 = oyuncu1;
            Oyuncu2 = oyuncu2;
            MasaKartlari = new List<Kart>();
            Oyuncu1Skoru = 0;
            Oyuncu2Skoru = 0;
        }

        private bool PistiMi(Kart oyuncuKarti, Kart masaKarti)
        {
            return oyuncuKarti.Numara == masaKarti.Numara;
        }

        public void Oyna()
        {
            Destesi destesi = new Destesi();
            destesi.Karistir(Oyuncu1.DogumGunu);
            destesi.Karistir(Oyuncu2.DogumGunu);

            for (int i = 0; i < 4; i++)
            {
                Oyuncu1.El.Add(destesi.KartCek());
                Oyuncu2.El.Add(destesi.KartCek());
            }

            MasaKartlari.Add(destesi.KartCek());

            int simdikiOyuncu = 1;

            while (destesi.kartlar.Count > 0 || Oyuncu1.El.Count > 0 || Oyuncu2.El.Count > 0)
            {
                if (simdikiOyuncu == 1)
                {
                    Kart oyuncuKarti = Oyuncu1.El[0];
                    Oyuncu1.El.RemoveAt(0);

                    Console.WriteLine($"{Oyuncu1.Ad} kart çekiyor: {oyuncuKarti}");

                    bool eslesme = false;

                    for (int i = MasaKartlari.Count - 1; i >= 0; i--)
                    {
                        if (PistiMi(oyuncuKarti, MasaKartlari[i]))
                        {
                            eslesme = true;
                            Oyuncu1Skoru += 10;
                            MasaKartlari.RemoveAt(i);
                            break;
                        }
                    }

                    if (!eslesme)
                    {
                        MasaKartlari.Add(oyuncuKarti);
                    }
                }
                else
                {
                    Kart oyuncuKarti = Oyuncu2.El[0];
                    Oyuncu2.El.RemoveAt(0);

                    Console.WriteLine($"{Oyuncu2.Ad} kart çekiyor: {oyuncuKarti}");

                    bool eslesme = false;
                    for (int i = MasaKartlari.Count - 1; i >= 0; i--)
                    {
                        if (PistiMi(oyuncuKarti, MasaKartlari[i]))
                        {
                            eslesme = true;
                            Oyuncu2Skoru += 10;
                            MasaKartlari.RemoveAt(i);
                            break;
                        }
                    }

                    if (!eslesme)
                    {
                        MasaKartlari.Add(oyuncuKarti);
                    }
                }

                simdikiOyuncu = 3 - simdikiOyuncu;

                if (Oyuncu1.El.Count == 0 && Oyuncu2.El.Count == 0)
                {
                    for (int i = 0; i < 4 && destesi.kartlar.Count > 0; i++)
                    {
                        Oyuncu1.El.Add(destesi.KartCek());
                        Oyuncu2.El.Add(destesi.KartCek());
                    }
                }
            }

            Console.WriteLine($"Oyun sona erdi. {Oyuncu1.Ad} : {Oyuncu1Skoru} - {Oyuncu2.Ad}: {Oyuncu2Skoru}");

            if (Oyuncu1Skoru > Oyuncu2Skoru)
            {
                Console.WriteLine($"{Oyuncu1.Ad}  kazandı!");
            }
            else if (Oyuncu1Skoru < Oyuncu2Skoru)
            {
                Console.WriteLine($"{Oyuncu2.Ad}  kazandı!");
            }
            else
            {
                Console.WriteLine("Oyun berabere!");
            }
        }
    }

        internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Oyuncu 1 İsim: ");
            string oyuncu1Ad = Console.ReadLine();

            Console.Write("Oyuncu 1 Doğum Günü: ");
            int oyuncu1DogumGunu = int.Parse(Console.ReadLine());

            Console.Write("Oyuncu 2 İsim: ");
            string oyuncu2Ad = Console.ReadLine();

            Console.Write("Oyuncu 2 Doğum Günü: ");
            int oyuncu2DogumGunu = int.Parse(Console.ReadLine());

            Oyuncu oyuncu1 = new Oyuncu(oyuncu1Ad, oyuncu1DogumGunu);
            Oyuncu oyuncu2 = new Oyuncu(oyuncu2Ad, oyuncu2DogumGunu);

            Pisti oyun = new Pisti(oyuncu1, oyuncu2);
            oyun.Oyna();

            Console.WriteLine("Çıkmak için herhangi bir tuşa basın...");
            Console.ReadKey();
        }
    }
}
