using ForBooks.Data;
using ForBooks.Models;
using ForBooks.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;

namespace UnitTestiranje
{
    [TestClass]
    public class RecenzijaServisTests
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

            _korisnik = new Korisnik(1, "Ime", "Prezime", "korisnik1", "k@test.com", "loz", "061111111");

            _knjiga = new Knjiga(1, "test-isbn", "Test knjiga", DateTime.Now, default, null);

            _db.korisnici.Add(_korisnik);
            _db.knjige.Add(_knjiga);
        }

        [TestMethod]
        public void DodajRecenziju_Uspjesno_DodajeUListuIDodajeUKnjigu()
        {

            string poruka = _servis.DodajRecenziju(1, 1, 5, "Odlicna knjiga");

            Assert.AreEqual("Recenzija je uspješno dodana.", poruka);
            Assert.AreEqual(1, _db.Recenzije.Count);

            var rec = _db.Recenzije[0];
            Assert.AreEqual(1, rec.KorisnikID);
            Assert.AreEqual(1, rec.KnjigaID);
            Assert.AreEqual(5, rec.Ocjena);
            Assert.AreEqual("Odlicna knjiga", rec.Komentar);

            Assert.AreEqual(1, _knjiga.Recenzije.Count);
        }

        [TestMethod]
        public void DodajRecenziju_KorisnikNePostoji_NeDodaje()
        {
            string poruka = _servis.DodajRecenziju(10, 1, 4, "komentar");

            Assert.AreEqual(0, _db.Recenzije.Count);
            StringAssert.Contains(poruka, "Korisnik nije pronađen.");
        }

        [TestMethod]
        public void DodajRecenziju_KnjigaNePostoji_NeDodaje()
        {
            string poruka = _servis.DodajRecenziju(1, 100, 4, "komentar");

            Assert.AreEqual(0, _db.Recenzije.Count);
            StringAssert.Contains(poruka, "Knjiga nije pronađena.");
        }

        [TestMethod]
        public void DodajRecenziju_NeispravnaOcjena_NeDodaje()
        {

            string poruka = _servis.DodajRecenziju(1, 1, 10, "kom");

            Assert.AreEqual(0, _db.Recenzije.Count);
            StringAssert.Contains(poruka, "Ocjena mora biti broj između 1 i 5.");
        }

        [TestMethod]
        public void PrikaziSveRecenzije_PraznaLista_VracaPoruku()
        {
            string output = _servis.PrikaziSveRecenzije();

            StringAssert.Contains(output, "Nema recenzija u bazi.");
        }

        [TestMethod]
        public void PrikaziSveRecenzije_SaPodacima_VracaOpis()
        {
            var rec = new Recenzija(1, _korisnik, _knjiga, 4, "Dobro");
            _db.Recenzije.Add(rec);
            _knjiga.Recenzije.Add(rec);

            string output = _servis.PrikaziSveRecenzije();

            StringAssert.Contains(output, "Test knjiga");
            StringAssert.Contains(output, "4/5");
            StringAssert.Contains(output, "Dobro");
        }

        [TestMethod]
        public void PrikaziProsjecnuOcjenuKnjige_KnjigaNePostoji()
        {
            string output = _servis.PrikaziProsjecnuOcjenuKnjige(5);

            StringAssert.Contains(output, "Knjiga nije pronađena.");
        }

        [TestMethod]
        public void PrikaziProsjecnuOcjenuKnjige_KnjigaBezRecenzija()
        {
            string output = _servis.PrikaziProsjecnuOcjenuKnjige(1);

            StringAssert.Contains(output, "nema recenzija");
        }

        [TestMethod]
        public void PrikaziProsjecnuOcjenuKnjige_RacunaProsjekIspravno()
        {
            var korisnik2 = new Korisnik(2, "C", "D", "u2", "u2@test.com", "loz", "222");
            _db.korisnici.Add(korisnik2);

            var r1 = new Recenzija(1, _korisnik, _knjiga, 4, "ok");
            var r2 = new Recenzija(2, korisnik2, _knjiga, 5, "super");

            _db.Recenzije.Add(r1);
            _db.Recenzije.Add(r2);
            _knjiga.Recenzije.Add(r1);
            _knjiga.Recenzije.Add(r2);

        string output = _servis.PrikaziProsjecnuOcjenuKnjige(1);

    double expectedAverage = 4.50; 

    StringAssert.Contains(output, expectedAverage.ToString("F2", CultureInfo.InvariantCulture));
    StringAssert.Contains(output, "broj recenzija: 2");
        }

        [TestMethod]
        public void ObrisiRecenziju_RecenzijaNePostoji_NeBriseNista()
        {
            var rec = new Recenzija(1, _korisnik, _knjiga, 4, "ok");

            _db.Recenzije.Add(rec);
            _knjiga.Recenzije.Add(rec);

            string poruka = _servis.ObrisiRecenziju(999);

            Assert.AreEqual(1, _db.Recenzije.Count);
            Assert.AreEqual(1, _knjiga.Recenzije.Count);
            StringAssert.Contains(poruka, "ne postoji");
        }

        [TestMethod]
        public void ObrisiRecenziju_Uspjesno_BriseIzBazeIKnjige()
        {
            var rec = new Recenzija(1, _korisnik, _knjiga, 4, "ok");

            _db.Recenzije.Add(rec);
            _knjiga.Recenzije.Add(rec);

            string poruka = _servis.ObrisiRecenziju(1);

            Assert.AreEqual(0, _db.Recenzije.Count);
            Assert.AreEqual(0, _knjiga.Recenzije.Count);
            StringAssert.Contains(poruka, "Recenzija je obrisana.");
        }
    }
}
