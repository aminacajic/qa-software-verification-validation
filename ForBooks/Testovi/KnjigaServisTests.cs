using ForBooks.Data;
using ForBooks.Enums;
using ForBooks.Models;
using ForBooks.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace UnitTestiranje
{
    [TestClass]
    public class KnjigaServisTests
    {
        private FakeDbClass _db = null!;
        private KnjigaServis _servis = null!;
        private Korisnik _testKorisnik = null!;

        [TestInitialize]
        [MemberNotNull(nameof(_db), nameof(_servis), nameof(_testKorisnik))]
        public void Setup()
        {
            _db = new FakeDbClass();
            _servis = new KnjigaServis(_db);

            // Inicijalizacija test korisnika za preporuke
            _testKorisnik = new Korisnik(
                id: 100,
                ime: "Test",
                prezime: "User",
                username: "testuser",
                email: "test@a.com",
                lozinka: "123",
                brojTelefona: "061234567"
            );
            _db.korisnici.Add(_testKorisnik);
        }

        // Pomoćna funkcija za kreiranje autora 
        private Autor CreateAutor(string ime, string prezime, string datumRodjenja)
        {
            var datum = DateTime.ParseExact(datumRodjenja, "dd.MM.yyyy", null);
            return new Autor(0, ime, prezime, datum); 
        }

        // Dodaj knjigu

        [TestMethod]
        public void DodajKnjigu_DodajeKnjiguUSistem_IspravniPodaci()
        {
            var autori = new List<Autor> { CreateAutor("Paja", "Patak", "01.01.2000") };
            var datum = DateTime.ParseExact("01.01.2025", "dd.MM.yyyy", null);

            string rezultat = _servis.DodajKnjigu(
                "1234567890",
                "Test Knjiga",
                datum,
                VrstaKnjige.NOVEL,
                "Izdavac Test",
                autori
            );

            Assert.AreEqual("Knjiga 'Test Knjiga' uspješno dodana i sačuvana.", rezultat);
            Assert.AreEqual(1, _db.knjige.Count);
            var knjiga = _db.knjige[0];
            Assert.AreEqual("Test Knjiga", knjiga.NazivKnjige);
            Assert.AreEqual("1234567890", knjiga.ISBN);
            Assert.AreEqual(VrstaKnjige.NOVEL, knjiga.VrstaKnjige);
            Assert.AreEqual(1, knjiga.Autori.Count);
            Assert.AreEqual(1, _db.autori.Count);
            Assert.IsTrue(_db.AddKnjigaCalled);
            Assert.IsTrue(_db.AddAutorCalled);
        }

        [TestMethod]
        public void DodajKnjigu_PrazanISBN_VracaGresku()
        {
            var autori = new List<Autor> { CreateAutor("Ime", "Prezime", "01.01.1980") };
            string rezultat = _servis.DodajKnjigu("", "Naziv Test", DateTime.Now, VrstaKnjige.MEMOIR, "Izdavac", autori);

            StringAssert.Contains(rezultat, "GREŠKA: Neispravan ISBN.");
            Assert.AreEqual(0, _db.knjige.Count);
        }

        [TestMethod]
        public void DodajKnjigu_PrazanNaziv_VracaGresku()
        {
            var autori = new List<Autor> { CreateAutor("Ime", "Prezime", "01.01.1980") };
            string rezultat = _servis.DodajKnjigu("12345", "", DateTime.Now, VrstaKnjige.MEMOIR, "Izdavac", autori);

            StringAssert.Contains(rezultat, "GREŠKA: Neispravan naziv.");
            Assert.AreEqual(0, _db.knjige.Count);
        }

        [TestMethod]
        public void DodajKnjigu_PrazanIzdavac_VracaGresku()
        {
            var autori = new List<Autor> { CreateAutor("Ime", "Prezime", "01.01.1980") };
            string rezultat = _servis.DodajKnjigu("12345", "Naziv", DateTime.Now, VrstaKnjige.MEMOIR, "", autori);

            StringAssert.Contains(rezultat, "GREŠKA: Neispravan izdavač.");
            Assert.AreEqual(0, _db.knjige.Count);
        }

        [TestMethod]
        public void DodajKnjigu_NemaAutora_VracaGresku()
        {
            var autori = new List<Autor>();
            string rezultat = _servis.DodajKnjigu("12345", "Naziv", DateTime.Now, VrstaKnjige.MEMOIR, "Izdavac", autori);

            StringAssert.Contains(rezultat, "GREŠKA: Knjiga mora imati barem jednog autora.");
            Assert.AreEqual(0, _db.knjige.Count);
        }

        // Prikazi sve knjige 

        [TestMethod]
        public void PrikaziSveKnjige_VracaPraznuListu()
        {
            var rezultat = _servis.PrikaziSveKnjige();
            Assert.AreEqual(0, rezultat.Count);
        }

        [TestMethod]
        public void PrikaziSveKnjige_VracaKnjige()
        {
            var izdavac = new Izdavac(1, "Test Izdavac");
            var autor1 = CreateAutor("Ime1", "Prezime1", "01.01.1980");
            var autor2 = CreateAutor("Ime2", "Prezime2", "02.02.1990");
            var knjiga = new Knjiga(1, "123-456", "Test Knjiga", new DateTime(2020, 5, 10), VrstaKnjige.NOVEL, izdavac)
            {
                Autori = new List<Autor> { autor1, autor2 }
            };
            _db.knjige.Add(knjiga);

            var rezultat = _servis.PrikaziSveKnjige();

            Assert.AreEqual(1, rezultat.Count);
            Assert.AreEqual("Test Knjiga", rezultat[0].NazivKnjige);
            Assert.AreEqual(2, rezultat[0].Autori.Count);
        }

        // Azuriraj knjigu

        [TestMethod]
        public void AzurirajKnjigu_AzuriraSvePodatke()
        {
            var izd = new Izdavac(1, "Izdavac1");
            var knjiga = new Knjiga(1, "12345", "Stari Naziv", DateTime.Parse("01.01.2020"), VrstaKnjige.NOVEL, izd);
            _db.knjige.Add(knjiga);
            _db.UpdateKnjigeCalled = false;

            var noviDatum = DateTime.ParseExact("15.03.2021", "dd.MM.yyyy", null);

            string rezultat = _servis.AzurirajKnjigu(
                1,                  
                "Novi Naziv",       
                "67890",            
                noviDatum,          
                "Novi Izdavac"     
            );

            Assert.AreEqual("Knjiga ID:1 uspješno ažurirana i sačuvana.", rezultat);
            Assert.AreEqual("Novi Naziv", knjiga.NazivKnjige);
            Assert.AreEqual("67890", knjiga.ISBN);
            Assert.AreEqual(noviDatum, knjiga.DatumObjavljivanja);
            Assert.AreEqual("Novi Izdavac", knjiga.Izdavac!.NazivIzdavaca);
            Assert.IsTrue(_db.UpdateKnjigeCalled);
        }

        [TestMethod]
        public void AzurirajKnjigu_PreskaceNekaPolja()
        {
            var izd = new Izdavac(1, "Izdavac1");
            var stariDatum = DateTime.Parse("01.01.2020");
            var knjiga = new Knjiga(1, "12345", "Stari Naziv", stariDatum, VrstaKnjige.NOVEL, izd);
            _db.knjige.Add(knjiga);
            _db.UpdateKnjigeCalled = false;

            var noviDatum = DateTime.ParseExact("15.03.2021", "dd.MM.yyyy", null);

            string rezultat = _servis.AzurirajKnjigu(
                1,                 
                "",                 
                "",                 
                noviDatum,         
                ""                  
            );

            Assert.AreEqual("Knjiga ID:1 uspješno ažurirana i sačuvana.", rezultat);
            Assert.AreEqual("Stari Naziv", knjiga.NazivKnjige); 
            Assert.AreEqual("12345", knjiga.ISBN); 
            Assert.AreEqual(noviDatum, knjiga.DatumObjavljivanja); 
            Assert.AreEqual("Izdavac1", knjiga.Izdavac!.NazivIzdavaca); 
            Assert.IsTrue(_db.UpdateKnjigeCalled);
        }

        [TestMethod]
        public void AzurirajKnjigu_KnjigaNePostoji_VracaGresku()
        {
            string rezultat = _servis.AzurirajKnjigu(
                999,
                "Novi Naziv",
                "67890",
                new DateTime(2021, 1, 1),
                "Novi Izdavac"
            );

            StringAssert.Contains(rezultat, "GREŠKA: Knjiga nije pronađena.");
            Assert.IsFalse(_db.UpdateKnjigeCalled);
        }

        // obrisi knjigu

        [TestMethod]
        public void ObrisiKnjigu_KnjigaPostoji_BrisanjeUspjesno()
        {
            var knjiga = new Knjiga(1, "12345", "Naziv za brisanje", DateTime.Parse("01.01.2020"), VrstaKnjige.NOVEL, new Izdavac(1, "Izdavac1"));
            _db.knjige.Add(knjiga);
            _db.UpdateKnjigeCalled = false;

            string rezultat = _servis.ObrisiKnjigu(1);

            Assert.AreEqual(0, _db.knjige.Count);
            StringAssert.Contains(rezultat, "Knjiga 'Naziv za brisanje' obrisana i JSON ažuriran.");
            Assert.IsTrue(_db.UpdateKnjigeCalled);
        }

        [TestMethod]
        public void ObrisiKnjigu_KnjigaNePostoji_VracaGresku()
        {
            string rezultat = _servis.ObrisiKnjigu(999);

            StringAssert.Contains(rezultat, "GREŠKA: Knjiga nije pronađena.");
            Assert.IsFalse(_db.UpdateKnjigeCalled);
        }

        // Obrisi neprimjerene recenzije

        [TestMethod]
        public void ObrisiNeprimjereneRecenzije_UklanjaRecenzije()
        {
            var k = new Knjiga(1, "12345", "Naziv", DateTime.Now, VrstaKnjige.NOVEL, new Izdavac(1, "Izd"));
            var recenzijaZaBrisanje1 = new Recenzija { ID = 1, KnjigaID = 1, Komentar = "Uvredljivo sadrzaj" };
            var recenzijaZaBrisanje2 = new Recenzija { ID = 2, KnjigaID = 1, Komentar = "Drugi Uvredljiv komentar" };
            var recenzijaOK = new Recenzija { ID = 3, KnjigaID = 1, Komentar = "Normalna recenzija" };

            k.Recenzije = new List<Recenzija> { recenzijaZaBrisanje1, recenzijaZaBrisanje2, recenzijaOK };
            _db.Recenzije.AddRange(k.Recenzije); 
            _db.knjige.Add(k);

            string rezultat = _servis.ObrisiNeprimjereneRecenzije(1, "uvredljivo,komentar");

            Assert.AreEqual(1, k.Recenzije.Count);
            Assert.AreEqual(1, _db.Recenzije.Count); 
            Assert.AreEqual("Normalna recenzija", k.Recenzije[0].Komentar);
            StringAssert.Contains(rezultat, "Uklonjeno 2 neprimjerenih recenzija za knjigu 'Naziv'.");
        }

        [TestMethod]
        public void ObrisiNeprimjereneRecenzije_KnjigaNePostoji_VracaGresku()
        {
            string rezultat = _servis.ObrisiNeprimjereneRecenzije(999, "uvredljivo");
            StringAssert.Contains(rezultat, "GREŠKA: Knjiga nije pronađena.");
        }

        [TestMethod]
        public void ObrisiNeprimjereneRecenzije_PrazneKljucneRijeci_VracaPoruku()
        {
            var k = new Knjiga(1, "12345", "Naziv", DateTime.Now, VrstaKnjige.NOVEL, new Izdavac(1, "Izd"));
            k.Recenzije = new List<Recenzija> { new Recenzija { ID = 1, Komentar = "Neki komentar" } };
            _db.knjige.Add(k);

            string rezultat = _servis.ObrisiNeprimjereneRecenzije(1, " ");
            StringAssert.Contains(rezultat, "Ključne riječi su prazne ili sadrže samo razmake. Nema brisanja.");
            Assert.AreEqual(1, k.Recenzije.Count);
        }

        // Sortiraj po datumu

        [TestMethod]
        public void SortirajPoDatumu_RedoslijedIspravan()
        {
            _db.knjige.Add(new Knjiga(1, "123", "Knjiga1", DateTime.Parse("2022-01-01"), VrstaKnjige.NOVEL, new Izdavac(1, "Izd")));
            _db.knjige.Add(new Knjiga(2, "124", "Knjiga2", DateTime.Parse("2020-01-01"), VrstaKnjige.NOVEL, new Izdavac(2, "Izd")));
            _db.knjige.Add(new Knjiga(3, "125", "Knjiga3", DateTime.Parse("2021-01-01"), VrstaKnjige.NOVEL, new Izdavac(3, "Izd")));
            _db.UpdateKnjigeCalled = false;

            string rezultat = _servis.SortirajPoDatumu();

            Assert.AreEqual("Knjige sortirane po datumu (najstarije-najnovije) i sačuvane.", rezultat);
            Assert.AreEqual("Knjiga2", _db.knjige[0].NazivKnjige);
            Assert.AreEqual("Knjiga3", _db.knjige[1].NazivKnjige);
            Assert.AreEqual("Knjiga1", _db.knjige[2].NazivKnjige);
            Assert.IsTrue(_db.UpdateKnjigeCalled);
        }

        // Pretraga knjiga

        [TestMethod]
        public void PretragaKnjiga_PrazanPojam_VracaPraznuListu()
        {
            var rezultat = _servis.PretragaKnjiga("1", "");
            Assert.AreEqual(0, rezultat.Count);
        }

        [TestMethod]
        public void PretragaKnjiga_PretragaPoNaslovu_Pronalazi()
        {
            _db.knjige.Add(new Knjiga(1, "123", "MojNaslov", DateTime.Now, VrstaKnjige.NOVEL, new Izdavac(1, "Izd")));
            _db.knjige.Add(new Knjiga(2, "124", "DrugiNaslov", DateTime.Now, VrstaKnjige.NOVEL, new Izdavac(2, "Izd")));

            var rezultat = _servis.PretragaKnjiga("1", "naslov");

            Assert.AreEqual(2, rezultat.Count);
            Assert.IsTrue(rezultat.Any(k => k.NazivKnjige == "MojNaslov"));
            Assert.IsTrue(rezultat.Any(k => k.NazivKnjige == "DrugiNaslov"));
        }

        [TestMethod]
        public void PretragaKnjiga_PretragaPoAutoru_Pronalazi()
        {
            var autor = CreateAutor("Pero", "Peric", "01.01.1980");
            var k1 = new Knjiga(1, "123", "Naslov1", DateTime.Now, VrstaKnjige.NOVEL, new Izdavac(1, "Izd")) { Autori = new List<Autor> { autor } };
            var k2 = new Knjiga(2, "124", "Naslov2", DateTime.Now, VrstaKnjige.NOVEL, new Izdavac(2, "Izd")) { Autori = new List<Autor> { autor } };
            var k3 = new Knjiga(3, "125", "DrugiAutor", DateTime.Now, VrstaKnjige.NOVEL, new Izdavac(3, "Izd")) { Autori = new List<Autor> { CreateAutor("Mujo", "Mujic", "01.01.1970") } };
            _db.knjige.Add(k1);
            _db.knjige.Add(k2);
            _db.knjige.Add(k3);

            var rezultat = _servis.PretragaKnjiga("2", "pero peric");

            Assert.AreEqual(2, rezultat.Count);
            Assert.IsTrue(rezultat.Any(k => k.NazivKnjige == "Naslov1"));
            Assert.IsTrue(rezultat.Any(k => k.NazivKnjige == "Naslov2"));
        }

        // Preporuka knjiga

        [TestMethod]
        public void PrijaviKorisnika_UspjesnaPrijava()
        {
            var korisnik = _servis.PrijaviKorisnika("test@a.com", "123");
            Assert.IsNotNull(korisnik);
            Assert.AreEqual("test@a.com", korisnik!.Email);
        }

        [TestMethod]
        public void PrijaviKorisnika_NeuspjesnaPrijava()
        {
            var korisnik = _servis.PrijaviKorisnika("nepostoji@a.com", "999");
            Assert.IsNull(korisnik);
        }

        [TestMethod]
        public void PreporuciKnjige_KorisnikBezProcitanih_VracaPraznuListu()
        {
            _db.knjige.Add(new Knjiga(1, "Knjiga1", "ISBN1", DateTime.Today, VrstaKnjige.NOVEL, new Izdavac(1, "Izdavac1")));
            
            var rezultat = _servis.PreporuciKnjige(100);

            Assert.AreEqual(0, rezultat.Count);
        }

        [TestMethod]
        public void PreporuciKnjige_NemaSlicnih_VracaPraznuListu()
        {
            var procitana = new Knjiga(1, "Procitana", "ISBN1", DateTime.Today, VrstaKnjige.NOVEL, new Izdavac(1, "Izdavac1"));
            procitana.Recenzije = new List<Recenzija> { new Recenzija { ID = 1, KorisnikID = 100, Komentar = "Ok" } };
            _db.Recenzije.Add(procitana.Recenzije.First());
            _db.knjige.Add(procitana);

            _db.knjige.Add(new Knjiga(2, "Nepovezana", "ISBN2", DateTime.Parse("01.01.1900"), VrstaKnjige.POETRY, new Izdavac(2, "Izdavac2")));

            var rezultat = _servis.PreporuciKnjige(100);

            Assert.AreEqual(0, rezultat.Count);
        }

        [TestMethod]
        public void PreporuciKnjige_ViseSlicnihKnjiga_PrikazujeNajvisePeto()
        {
            var autor = CreateAutor("Marko", "Markovic", "01.01.1980");

            var procitana = new Knjiga(1, "Procitana", "ISBN1", DateTime.Today.AddYears(-1), VrstaKnjige.NOVEL, new Izdavac(1, "Izdavac1"));
            procitana.Autori = new List<Autor> { autor };
            procitana.Recenzije = new List<Recenzija> { new Recenzija { ID = 1, KorisnikID = 100 } };
            _db.Recenzije.Add(procitana.Recenzije.First());
            _db.knjige.Add(procitana);

            for (int i = 2; i <= 7; i++)
            {
                var k = new Knjiga(i, $"Slicna{i}", $"ISBN{i}", DateTime.Today, VrstaKnjige.NOVEL, new Izdavac(i, $"Izdavac{i}"));
                _db.knjige.Add(k);
            }

            var rezultat = _servis.PreporuciKnjige(100);

            Assert.AreEqual(5, rezultat.Count);
        }
    }
}