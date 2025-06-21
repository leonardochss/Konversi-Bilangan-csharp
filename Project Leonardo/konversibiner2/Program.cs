using System;
using System.Collections.Generic;

namespace konversibiner2
{
    class Program
    {
        static void Main()
        {
            bool lanjutProgram = true;

            {
                Console.Clear();
                Console.WriteLine("---- Menu Utama ----");
                Console.WriteLine("1. Konversi Bilangan");
                Console.WriteLine("2. Kalkulator Operator");
                Console.Write("Pilih menu (1/2): ");

                string pilihan = Console.ReadLine();

                switch (pilihan)
                {
                    case "1":
                        MenuKonversi();
                        break;
                    case "2":
                        MenuOperator();
                        break;
                    case "0":
                        lanjutProgram = false;
                        break;
                    default:
                        Console.WriteLine("Pilihan tidak valid!");
                        break;
                }

                if (lanjutProgram)
                {
                    Console.Write("\nTekan ENTER untuk kembali ke menu...");
                    Console.ReadLine();
                }
            }

            Console.WriteLine("\nProgram selesai. Terima kasih!");
        }

        // === MENU KONVERSI ===
        static void MenuKonversi()
        {
            Console.Clear();
            Console.WriteLine("=== KONVERSI BILANGAN ===");
            try
            {
                Angka angka = InputAngka("yang akan dikonversi");
                TampilkanHasil(angka.NilaiDesimal);
            }
            catch (FormatException)
            {
                Console.WriteLine("Input tidak valid! Pastikan format dan basis benar.");
            }
        }

        // === MENU OPERATOR ===
        static void MenuOperator()
        {
            bool lanjut = true;

            while (lanjut)
            {
                Console.Clear();
                Console.WriteLine("=== Kalkulator Operator ===");

                try
                {
                    Angka angka1 = InputAngka("pertama");

                    Console.Write("\nPilih operator (+, -, *, /): ");
                    string op = Console.ReadLine();

                    Angka angka2 = InputAngka("kedua");

                    Console.WriteLine("\nAngka pertama: " + angka1.Deskripsi + " = " + angka1.NilaiDesimal + " (desimal)");
                    Console.WriteLine("Angka kedua  : " + angka2.Deskripsi + " = " + angka2.NilaiDesimal + " (desimal)");

                    int hasil = 0;

                    switch (op)
                    {
                        case "+":
                            hasil = angka1.NilaiDesimal + angka2.NilaiDesimal;
                            break;
                        case "-":
                            hasil = angka1.NilaiDesimal - angka2.NilaiDesimal;
                            break;
                        case "*":
                            hasil = angka1.NilaiDesimal * angka2.NilaiDesimal;
                            break;
                        case "/":
                            if (angka2.NilaiDesimal == 0)
                            {
                                Console.WriteLine("Error: Pembagian dengan nol!");
                                continue;
                            }
                            hasil = angka1.NilaiDesimal / angka2.NilaiDesimal;
                            break;
                        default:
                            Console.WriteLine("Operator tidak dikenali!");
                            continue;
                    }

                    Console.WriteLine("\nOperasi: " + angka1.NilaiDesimal + " " + op + " " + angka2.NilaiDesimal + " = " + hasil);
                    TampilkanHasil(hasil);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Input tidak valid! Pastikan angka dan basis sesuai.");
                }

                Console.Write("\nIngin melakukan operasi lagi? (y/n): ");
                lanjut = Console.ReadLine().ToLower() == "y";
            }
        }

        static Angka InputAngka(string urutan)
        {
            Console.WriteLine($"\nMasukkan angka {urutan}:");
            Console.WriteLine("1. Biner");
            Console.WriteLine("2. Oktal");
            Console.WriteLine("3. Desimal");
            Console.WriteLine("4. Heksadesimal");
            Console.Write("Pilih basis (1-4): ");

            int basisPilihan = int.Parse(Console.ReadLine());
            int basis = 10;
            string basisStr = "";

            switch (basisPilihan)
            {
                case 1:
                    basis = 2;
                    basisStr = "biner";
                    break;
                case 2:
                    basis = 8;
                    basisStr = "oktal";
                    break;
                case 3:
                    basis = 10;
                    basisStr = "desimal";
                    break;
                case 4:
                    basis = 16;
                    basisStr = "heksadesimal";
                    break;
                default:
                    throw new FormatException("Basis tidak valid");
            }

            Console.Write("Masukkan angkanya: ");
            string angkaInput = Console.ReadLine().Replace(" ", ""); // Menghapus spasi

            int desimal;

            if (basis == 10)
            {
                // Desimal boleh negatif
                desimal = int.Parse(angkaInput);
            }
            else
            {
                // Biner, Oktal, Heksa: TIDAK boleh negatif
                if (angkaInput.StartsWith("-"))
                {
                    throw new FormatException("Input tidak boleh negatif untuk basis non-desimal.");
                }

                desimal = Convert.ToInt32(angkaInput, basis);
            }

            string deskripsi = angkaInput + " (" + basisStr + ")";
            return new Angka(desimal, deskripsi);
        }


        static void TampilkanHasil(int hasil)
        {

            Console.WriteLine("\n=== Hasil Konversi ===");
            Console.WriteLine("Desimal     : " + hasil);

            // Representasi Biner
            string biner;
            if (hasil >= 0)
                biner = Convert.ToString(hasil, 2).PadLeft(8, '0');
            else
                biner = Convert.ToString(hasil & 0xFF, 2).PadLeft(8, '0'); // 2's complement 8-bit

            Console.WriteLine("Biner       : " + FormatBiner(biner));

            // Oktal dan Heksadesimal - gunakan langsung hasil, agar negatif tetap tampil sebagai negatif
            string oktal = (hasil < 0 ? "-" : "") + Convert.ToString(Math.Abs(hasil), 8);
            string heksa = (hasil < 0 ? "-" : "") + Convert.ToString(Math.Abs(hasil), 16).ToUpper();

            Console.WriteLine("Oktal       : " + oktal);
            Console.WriteLine("Heksadesimal: " + heksa);
        }


        // Fungsi Format Biner 4-bit group
        static string FormatBiner(string biner)
        {
            List<string> grup = new List<string>();

            for (int i = 0; i < biner.Length; i += 4)
            {
                grup.Add(biner.Substring(i, Math.Min(4, biner.Length - i)));
            }

            return string.Join(" ", grup);
        }

    }

    class Angka
    {
        public int NilaiDesimal { get; set; }
        public string Deskripsi { get; set; }

        public Angka(int nilai, string deskripsi)
        {
            NilaiDesimal = nilai;
            Deskripsi = deskripsi;
        }
    }
}