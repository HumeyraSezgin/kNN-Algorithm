using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace knnProject
{
    public class Para
    {

        private double varyans;
        private double carpıklık;
        private double basıklık;
        private double entropi;
        private double gercekmi;
        private double distance;
        public double Varyans
        {
            get { return varyans; }
            set { value = varyans; }

        }
        public double Carpıklık
        {
            get { return carpıklık; }
            set { value = carpıklık; }
        }
        public double Basıklık
        {
            get { return basıklık; }
            set { value = basıklık; }
        }
        public double Entropi
        {
            get { return entropi; }
            set { value = entropi; }
        }
        public double Gercekmi
        {
            get { return gercekmi; }
            set { value = gercekmi; }
        }
        public double Distance
        {
            get { return distance; }
            set { value = distance; }

        }
        public Para()
        {

        }
        public Para(double v, double c, double b, double e) //Girdi olarak alınan paranın kolayca tutulabilmesi için yazılan constructor
        {
            this.varyans = v;
            this.carpıklık = c;
            this.basıklık = b;
            this.entropi = e;
        }
        public Para(double v, double c, double b, double e, double g) //Paranın gerçeklik değeri ile birlikte alınması için yazılan constructor
        {
            this.varyans = v;
            this.carpıklık = c;
            this.basıklık = b;
            this.entropi = e;
            this.gercekmi = g;
        }

        public Para(double v, double c, double b, double e, double g, double d) //Paranın kontrol paraları ile olan uzaklığının tutulabilmesi için yazılan constructor
        {
            this.varyans = v;
            this.carpıklık = c;
            this.basıklık = b;
            this.entropi = e;
            this.gercekmi = g;
            this.distance = d;
        }
    }
    class Program
    {
        public static Para[] dosyaOku() //dosya okunuyor
        {
            Para[] paralar = new Para[1372]; //okunan dosyadaki verilerin yazdırılacağı para nesnesi oluşturuluyor
            int count = 0;
            if (File.Exists("data_banknote_authentication.txt")) //dosya okuma kontrolü
            {
                Console.WriteLine("file is exist!");
            }
            else
            {
                Console.WriteLine("file isn't exist!");
            }
            string[] lines = File.ReadAllLines("data_banknote_authentication.txt"); //dosyanın tüm satırları okunuyor
            foreach (string line in lines)
            {

                string[] properties = line.Split(','); //her satırdaki değerleri , gördüğü yerde ayırıyor. Çünkü textte virgüllerin arasında özellikler belirtiliyor
                double[] props = new double[properties.Length];
                for (int i = 0; i < 5; i++) //okunan değeri double olarak algılamayı sağlıyor
                {
                    properties[i] = properties[i].Replace(".", ",");
                    props[i] = Double.Parse(properties[i]);
                }
                Para newPara = new Para(props[0], props[1], props[2], props[3], props[4]); //her satırdan yeni para oluşturuluyor
                paralar[count] = newPara;
                count++;
            }
            //Console.WriteLine(paralar[0].Varyans);   kontrol için
            return paralar;
        }
        public static Para yeniParaGirisi()
        {

            string kullaniciGirisi; //kullanıcıdan bir paranın özelliklerini girmesi isteniyor.
            Console.WriteLine("Lütfen elinizdeki paranın bilgilerini aralarına virgül koyarak sırasıyla varyans,çarpıklık,basıklık,entropi olacak şekilde giriniz(Küsüratı . ile belirtiniz):");
            kullaniciGirisi = Console.ReadLine();
            string[] properties = kullaniciGirisi.Split(','); //kullanıcıdan alınan verileri özelliklere ayırıyor.
            double[] props = new double[properties.Length + 1];
            for (int i = 0; i < properties.Length; i++)
            {
                props[i] = Double.Parse(properties[i].Replace(".", ","));
            }
            Para newPara = new Para(props[0], props[1], props[2], props[3]);
            return newPara;
        }
        static void Main(string[] args)
        {
            Para[] paralar = dosyaOku(); //dosya okunuyor.
            Para yeniPara = yeniParaGirisi(); //kullanıcıdan para girdisi alınıyor
            string l;
            Console.WriteLine("En yakın kaç adet banknotu öğrenmek istiyorsunuz? : "); //en yakın kaç adet banknotun istendiği öğreniliyor
            l = Console.ReadLine();
            int k = Int16.Parse(l);
            //Console.WriteLine(k);
            int textSahteSay = 0; // okunan textteki sahte para sayısı
            int textGercekSay = 0; // okunan textteki gercek para sayısı

            int i = 0;

            Para[] paralarwUzaklık = new Para[1372]; //kullanıcıdan alınan paranın textteki paralar ile yakınlığını bulmak ve bir nesnede tutmak için oluşturuldu
            foreach (Para para in paralar)
            {
                if (paralar[i].Gercekmi == 1) { textGercekSay++; } //texxteki sahte ve gerçek para sayısını bulduruyor
                else { textSahteSay++; }
                double distance = Math.Sqrt(Math.Pow(para.Basıklık - yeniPara.Basıklık, 2) + Math.Pow(para.Carpıklık - yeniPara.Carpıklık, 2) + Math.Pow(para.Entropi - yeniPara.Entropi, 2) + Math.Pow(para.Varyans - yeniPara.Varyans, 2)); // iki para arası uzaklığı hesaplıyor
                paralarwUzaklık[i] = new Para(para.Varyans, para.Carpıklık, para.Basıklık, para.Entropi, para.Gercekmi, distance); // hesaplanan her uzaklığın hangi para ile hesaplandığı ile birlikte tutması için oluşturuldu
                i++;
            }
            Para[] kontrol = new Para[1372]; //uzaklıkları sıralamak için kontrol nesnesi oluşturuldu
            for (i = 0; i < 1372; i++)
            {
                for (int j = i + 1; j < 1372; j++)
                {
                    if (paralarwUzaklık[j].Distance < paralarwUzaklık[i].Distance) //biri diğerinden küçükse yer değişikliğini sağlıyor
                    {
                        kontrol[0] = paralarwUzaklık[i];
                        paralarwUzaklık[i] = paralarwUzaklık[j];
                        paralarwUzaklık[j] = kontrol[0];
                    }
                }
            }

            int sahteParaSay = 0; //gerçek olup olmadığını öğrenmek istediğimiz banknotun en yakınındaki sahte para sayısı ve gerçek para sayısını tutuyorlar
            int gercekParaSay = 0;

            Console.WriteLine("Varyans    Çarpıklık    Basıklık     Entropi     Tür     Uzaklık");
            for (i = 0; i < k; i++) //en yakın k tane parayı yazdırıyor
            {
                Console.WriteLine(string.Format("{0:0.00}", paralarwUzaklık[i].Varyans) + "          " + string.Format("{0:0.00}", paralarwUzaklık[i].Carpıklık) + "        " + string.Format("{0:0.00}", paralarwUzaklık[i].Basıklık) + "        " + string.Format("{0:0.00}", paralarwUzaklık[i].Entropi) + "        " + paralarwUzaklık[i].Gercekmi + "       " + string.Format("{0:0.00}", paralarwUzaklık[i].Distance));
                if (paralarwUzaklık[i].Gercekmi == 1) { gercekParaSay++; }
                else { sahteParaSay++; }
            }
            double tahminiTur; //tahmin edilen türü tutan değişken
            if (sahteParaSay < gercekParaSay) // en yakınındaki sahte para sayısı daha az ise para gerçek 
            {
                tahminiTur = 1;
                Console.WriteLine("Tahmine göre test parası gerçektir.");
            }
            else //değilse sahtedir
            {
                tahminiTur = 0;
                Console.WriteLine("Tahmine göre test parası sahtedir.");
            }
            Console.WriteLine("---------------------------------------------------------------------");

            int a = 0;
            int b = 0;
            Para[] testParalari = new Para[200]; //textten 200 veriyi ayırmak için kullanılıyor
            Para[] testParalariTurleri = new Para[200]; //ayrılan 200 verinin tahminlenen tür ve gerçek türü karşılaştırabilmek için 200 veri Gerçek türleriyle tutuluyor
            Para[] digerParalar = new Para[1172]; //kalan 1172 veriyi tutuyor
            for (i = 0; i < 1372; i++)
            {
                if (i < textSahteSay - 100)
                {
                    digerParalar[b] = new Para(paralar[i].Varyans, paralar[i].Carpıklık, paralar[i].Basıklık, paralar[i].Entropi, paralar[i].Gercekmi); //sahte verilerin son 100 tanesi hariç kalan veri grubuna yazdırıyor
                    b++;
                }
                else if (i > textSahteSay - 101 && i < textSahteSay)//sahte verilerin son 100 tanesi 200 veriye yazdırılıyor
                {
                    testParalari[a] = new Para(paralar[i].Varyans, paralar[i].Carpıklık, paralar[i].Basıklık, paralar[i].Entropi);
                    testParalariTurleri[a] = new Para(paralar[i].Varyans, paralar[i].Carpıklık, paralar[i].Basıklık, paralar[i].Entropi, paralar[i].Gercekmi);
                    a++;
                }

                else if (i < 1372 && i > 1271)//gerçek verilerin son 100 tanesi 200 veriye yazdırılıyor
                {
                    testParalari[a] = new Para(paralar[i].Varyans, paralar[i].Carpıklık, paralar[i].Basıklık, paralar[i].Entropi);
                    testParalariTurleri[a] = new Para(paralar[i].Varyans, paralar[i].Carpıklık, paralar[i].Basıklık, paralar[i].Entropi, paralar[i].Gercekmi);
                    a++;
                }
                else//gerçek verilerin son 100 tanesi hariç kalan veri grubuna yazdırılıyor
                {
                    digerParalar[b] = new Para(paralar[i].Varyans, paralar[i].Carpıklık, paralar[i].Basıklık, paralar[i].Entropi, paralar[i].Gercekmi);
                    b++;
                }
            }
            Console.WriteLine("---------------------------------------------------------------------");
            int basariliTahmin = 0; //başarılı tahmin edilen veri sayısını tutmak için kullanılacak
            Console.WriteLine(" Test verilerinin gerçek sınıfları ile, kNN ile tahminlediğiniz sınıflarının karşılaştırması");
            Console.WriteLine("--------------------------------------------------------------------------------------------");
            for (i = 0; i < testParalari.Length; i++) //200 verinin her birinin 1172 adet veriye olan benzerliği tek tek hesaplanıyor.
            {
                int j = 0;
                while (j < digerParalar.Length)
                {
                    double distance = Math.Sqrt(Math.Pow(digerParalar[j].Basıklık - testParalari[i].Basıklık, 2) + Math.Pow(digerParalar[j].Carpıklık - testParalari[i].Carpıklık, 2) + Math.Pow(digerParalar[j].Entropi - testParalari[i].Entropi, 2) + Math.Pow(digerParalar[j].Varyans - testParalari[i].Varyans, 2)); //uzaklıklar hesaplanıyor
                    digerParalar[i] = new Para(digerParalar[i].Varyans, digerParalar[i].Carpıklık, digerParalar[i].Basıklık, digerParalar[i].Entropi, digerParalar[i].Gercekmi, distance); // kalan veri bilgisine test verisi ile olan uzaklık bilgisi ekleniyor
                    if (j == digerParalar.Length - 1)
                    {


                        Para[] kontrol2 = new Para[1172];//her 1172 veriyi yakınlığına göre sıralıyor
                        for (int n = 0; n < 1172; n++)
                        {
                            for (int m = n + 1; m < 1172; m++)
                            {
                                if (digerParalar[n].Distance < digerParalar[m].Distance)
                                {
                                    kontrol2[0] = digerParalar[n];
                                    digerParalar[n] = digerParalar[m];
                                    digerParalar[m] = kontrol2[0];
                                }
                            }
                        }

                        for (int t = 0; t < k; t++)//en yakın k veri yazdırılıyor
                        {
                            Console.WriteLine(string.Format("{0:0.00}", digerParalar[t].Varyans) + "     " + string.Format("{0:0.00}", digerParalar[t].Carpıklık) + "     " + string.Format("{0:0.00}", digerParalar[t].Basıklık) + "     " + string.Format("{0:0.00}", digerParalar[t].Entropi) + "     " + string.Format("{0:0.00}", distance));
                        }
                        gercekParaSay = 0; //her test verisi için gerçek ve sahte veri sayısını sıfırlamak için yazıldı
                        sahteParaSay = 0;
                        if (paralarwUzaklık[i].Gercekmi == 1) { gercekParaSay++; }//gerçeklik değeri 1 ise çevresinde bulunan gerçek para sayısının tutulduğu değişkeni 1 artırıyor
                        else { sahteParaSay++; }//değilse sahte para sayısının tutulduğu değişkeni 1 artırıyor
                        if (gercekParaSay > sahteParaSay) { tahminiTur = 1; }//çevresindeki gerçek para sayısı fazlaysa tahmini türü 1 yani gerçek oluyor
                        else { tahminiTur = 0; } //değilse tahmini tür 0 yani sahte oluyor

                        Console.WriteLine("Üstte sıralanan paralar  " + string.Format("{0:0.00}", testParalari[i].Varyans) + "      " + string.Format("{0:0.00}", testParalari[i].Carpıklık) + "      " + string.Format("{0:0.00}", testParalari[i].Basıklık) + "      " + string.Format("{0:0.00}", testParalari[i].Entropi) + " parasının en yakın komşularıdır. ");//test edilen veri yazdırılıyor
                        Console.WriteLine("Buna göre tahminlenen tür " + tahminiTur + "  Gerçek tür  " + testParalariTurleri[i].Gercekmi);//test verisinin hem Asıl türü hem Tahminlenen türü yazdırılıyor
                        if (tahminiTur == testParalariTurleri[i].Gercekmi)//tahmini tür ile asıl tür aynı ise başarılı tahmin sayısı artırılıyor
                        {
                            basariliTahmin++;

                        }

                    }
                    j++;
                }

            }
            Console.WriteLine("---------------------------------------------------------------------");
            double basariOrani = (double)basariliTahmin / (double)testParalari.Length;//başarı oranı hesaplanıyor
            Console.WriteLine("Başarı Oranı =  " + basariOrani);//başarı oranı yazdırılıyor

            Console.WriteLine("---------------------------------------------------------------------");
            Console.WriteLine("Text verileri yazdırılıyor.");
            Console.WriteLine("---------------------------------------------------------------------");
            Console.WriteLine("Varyans       Çarpıklık         Basıklık        Entropi            Tür");
            for (i = 0; i < 1372; i++)//textten okunan veriler yazdırılıyor
            {
                Console.WriteLine(string.Format("{0:0.00}", paralar[i].Varyans) + "\t" + "\t" + string.Format("{0:0.00}", paralar[i].Carpıklık) + "\t" + "\t" + string.Format("{0:0.00}", paralar[i].Basıklık) + "\t" + "\t" + string.Format("{0:0.00}", paralar[i].Entropi) + "\t" + "\t" + "    " + paralar[i].Gercekmi);
            }
            Console.ReadKey();
        }
    }
}