using ForBooks.Data;
using ForBooks.Enums;
using ForBooks.Models;
using ForBooks.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis; 
using System.IO;
using System.Linq;
using System.Text.Json;

namespace UnitTestiranje
{
    public class ObrisiRecenzijeTestModel
    {
        public string Description { get; set; } = null!;
        public string Keywords { get; set; } = null!;
        public List<ReviewData> Reviews { get; set; } = null!;
        public int ExpectedRemovedCount { get; set; }

        public class ReviewData
        {
            public string Komentar { get; set; } = null!;
            public double Ocjena { get; set; }
        }
    }

    [TestClass]
    public class KnjigaServisDataDrivenTests
    {
        private FakeDbClass db = null!;
        private KnjigaServis servis = null!;

        [TestInitialize]
        [MemberNotNull(nameof(db), nameof(servis))] 
        public void Setup()
        {
            (db, servis) = CreateServis();
        }

        private (FakeDbClass db, KnjigaServis servis) CreateServis()
        {
            var db = new FakeDbClass();
            var servis = new KnjigaServis(db);
            return (db, servis);
        }

        // Metoda koja čita podatke iz eksterne JSON datoteke
        public static IEnumerable<object[]> ObrisiNeprimjereneRecenzijeTestDataFromJson()
        {
            var fileName = "ObrisiRecenzijeTestData.json";
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

            if (!File.Exists(path))
            {
                throw new FileNotFoundException(
                    $"Test datoteka nije pronađena: {path}. Molimo provjerite .csproj konfiguraciju.", 
                    path);
            }

            string jsonString = File.ReadAllText(path);
            
            var data = JsonSerializer.Deserialize<List<ObrisiRecenzijeTestModel>>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            
            return data!
                .Select(testCase => new object[] { testCase })
                .ToList();
        }

        [DataTestMethod]
        [DynamicData(nameof(ObrisiNeprimjereneRecenzijeTestDataFromJson), DynamicDataSourceType.Method)]
        [TestCategory("DataDrivenTest")]
        public void ObrisiNeprimjereneRecenzije_DynamicDataJson_ShouldRemoveCorrectCount(ObrisiRecenzijeTestModel testCase)
        {
            int knjigaId = 1;
            int recenzijaIdCounter = 1;

            // Kreiranje izolovanog okruženja za test (FakeDb)
            var knjiga = new Knjiga(knjigaId, "isbn", $"Knjiga za test - {testCase.Description}", DateTime.Now, VrstaKnjige.NOVEL, null!)
            {
                Recenzije = new List<Recenzija>()
            };
            db.knjige.Add(knjiga); 

            // Popunjavanje FakeDb-a recenzijama iz JSON-a
            foreach (var rData in testCase.Reviews)
            {
                var korisnik = new Korisnik(recenzijaIdCounter, "Ime", "Prezime", $"kor{recenzijaIdCounter}", $"k{recenzijaIdCounter}@test.com", "loz", "111");
                db.korisnici.Add(korisnik); 
                
                var rec = new Recenzija(recenzijaIdCounter++, korisnik, knjiga, rData.Ocjena, rData.Komentar);
                knjiga.Recenzije.Add(rec);
                db.Recenzije.Add(rec); 
            }

            int initialCountInKnjiga = knjiga.Recenzije.Count;
            int initialCountInDb = db.Recenzije.Count;

            servis.ObrisiNeprimjereneRecenzije(knjigaId, testCase.Keywords);

            int finalCountInKnjiga = knjiga.Recenzije.Count;
            int actualRemovedFromKnjiga = initialCountInKnjiga - finalCountInKnjiga;
            
            // Provjera je li ispravan broj obrisan iz liste recenzija knjige
            Assert.AreEqual(testCase.ExpectedRemovedCount, actualRemovedFromKnjiga, 
                $"Test '{testCase.Description}' - Broj obrisanih iz Knjige nije tačan.");
            
            // Provjera je li isti broj obrisan i iz glavne baze (FakeDb)
            int expectedFinalDbCount = initialCountInDb - testCase.ExpectedRemovedCount;
            Assert.AreEqual(expectedFinalDbCount, db.Recenzije.Count, 
                $"Test '{testCase.Description}' - Broj preostalih recenzija u Db-u nije tačan.");

            foreach (var recenzija in knjiga.Recenzije)
            {
                var keywords = testCase.Keywords.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var keyword in keywords)
                {
                    bool sadrziKljucnuRijec = recenzija.Komentar.ToLower().Contains(keyword.ToLower());
                    
                    Assert.IsFalse(sadrziKljucnuRijec, 
                        $"Test '{testCase.Description}' nije uspio. Preostala recenzija sadrži zabranjenu ključnu reč '{keyword}'.");
                }
            }
        }
        
        [DataTestMethod]
        [DataRow(1, "", 0, "Prazne ključne riječi - ne bi trebalo obrisati ništa")]
        [DataRow(1, "   ", 0, "Samo whitespace ključne riječi - ne bi trebalo obrisati ništa")]
        [DataRow(1, "ne-postojeci-tekst", 0, "Nepostojeće ključne riječi - ne bi trebalo obrisati ništa")]
        [TestCategory("DataDrivenTest")]
        public void ObrisiNeprimjereneRecenzije_DataRow_EdgeCases(int knjigaId, string keywords, int expectedRemovedCount, string description)
        {
            var knjiga = new Knjiga(knjigaId, "isbn", "Knjiga za test", DateTime.Now, VrstaKnjige.NOVEL, null!)
            {
                Recenzije = new List<Recenzija>()
            };
            db.knjige.Add(knjiga);
            
            knjiga.Recenzije.Add(new Recenzija(1, new Korisnik(1,"A","B","c","d","e","f"), knjiga, 5, "ok komentar 1"));
            knjiga.Recenzije.Add(new Recenzija(2, new Korisnik(2,"A","B","c","d","e","f"), knjiga, 4, "ok komentar 2"));
            knjiga.Recenzije.Add(new Recenzija(3, new Korisnik(3,"A","B","c","d","e","f"), knjiga, 3, "ok komentar 3"));
            
            db.Recenzije.AddRange(knjiga.Recenzije);
            db.korisnici.Add(new Korisnik(1,"A","B","c","d","e","f"));
            db.korisnici.Add(new Korisnik(2,"A","B","c","d","e","f"));
            db.korisnici.Add(new Korisnik(3,"A","B","c","d","e","f"));

            int initialCount = knjiga.Recenzije.Count;
            servis.ObrisiNeprimjereneRecenzije(knjigaId, keywords);

            int finalCount = knjiga.Recenzije.Count;
            int actualRemoved = initialCount - finalCount;
            
            Assert.AreEqual(expectedRemovedCount, actualRemoved, 
                $"Test '{description}' nije uspio. Očekivano obrisano: {expectedRemovedCount}, Stvarno obrisano: {actualRemoved}.");
        }
    }
}