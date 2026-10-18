using ForBooks.Data;
using ForBooks.Enums;
using ForBooks.Models;
using ForBooks.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace UnitTestiranje
{
    [TestClass]
    public class RecenzijaServisDataDrivenTests
    {
        private FakeDbClass _db;
        private RecenzijaServis _servis;
        private Korisnik _korisnik;
        private Knjiga _knjiga;

        [TestInitialize]
        public void Setup()
        {
            _db = new FakeDbClass();
            _servis = new RecenzijaServis(_db);

            _korisnik = new Korisnik(1, "Ime", "Prezime", "kor", "mail@test.com", "loz", "111");
            _db.korisnici.Add(_korisnik);

            _knjiga = new Knjiga(1, "isbn", "Test knjiga", DateTime.Now, default, null);
            _db.knjige.Add(_knjiga);
        }

        [DataTestMethod]
        [DataRow(5, "Odlična knjiga", 1, "Recenzija je uspješno dodana.")]
        [DataRow(0, "Loša", 0, "Ocjena mora biti broj između 1 i 5.")]
        [DataRow(6, "Previše dobra", 0, "Ocjena mora biti broj između 1 i 5.")]
        public void DodajRecenziju_DataRow_Ocjene(
            double ocjena,
            string komentar,
            int expectedCount,
            string expectedMessage)
        {
            string result = _servis.DodajRecenziju(1, 1, ocjena, komentar);

            Assert.AreEqual(expectedCount, _db.Recenzije.Count);
            StringAssert.Contains(result, expectedMessage);
        }

        public static IEnumerable<object[]> ObrisiRecenzijuData()
        {
            yield return new object[] { 1, 0, "Recenzija je obrisana." };
            yield return new object[] { 999, 1, "Recenzija sa tim ID-jem ne postoji." };
        }

        [DataTestMethod]
        [DynamicData(nameof(ObrisiRecenzijuData), DynamicDataSourceType.Method)]
        public void ObrisiRecenziju_DynamicData(
            int inputId,
            int expectedCountAfter,
            string expectedMessage)
        {
            var rec = new Recenzija(1, _korisnik, _knjiga, 5, "Ok");
            _db.Recenzije.Add(rec);
            _knjiga.Recenzije.Add(rec);

            string result = _servis.ObrisiRecenziju(inputId);

            Assert.AreEqual(expectedCountAfter, _db.Recenzije.Count);
            StringAssert.Contains(result, expectedMessage);
        }

        public static IEnumerable<object[]> ProsjecnaOcjenaFromCsv()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ProsjecneOcjeneData.csv");

            foreach (var line in File.ReadAllLines(path).Skip(1))
            {
                var p = line.Split(',');

                yield return new object[]
                {
                    int.Parse(p[0]),
                    double.Parse(p[1], CultureInfo.InvariantCulture),
                    double.Parse(p[2], CultureInfo.InvariantCulture),
                    double.Parse(p[3], CultureInfo.InvariantCulture)
                };
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(ProsjecnaOcjenaFromCsv), DynamicDataSourceType.Method)]
        public void PrikaziProsjecnuOcjenuKnjige_CsvDynamicData(
            int knjigaId,
            double ocjena1,
            double ocjena2,
            double expectedAverage)
        {

            _db.knjige.Clear();
            _knjiga = new Knjiga(knjigaId, "isbn", $"Knjiga {knjigaId}", DateTime.Now, default, null);
            _db.knjige.Add(_knjiga);

            _db.Recenzije.Add(new Recenzija(1, _korisnik, _knjiga, ocjena1, "prva"));
            _db.Recenzije.Add(new Recenzija(2, _korisnik, _knjiga, ocjena2, "druga"));
            _knjiga.Recenzije.AddRange(_db.Recenzije.Where(r => r.KnjigaID == knjigaId));

            string result = _servis.PrikaziProsjecnuOcjenuKnjige(knjigaId);

            StringAssert.Contains(result, expectedAverage.ToString("F2", CultureInfo.InvariantCulture));
        }
    }
}
