using System;
using System.Collections.Generic;
using ForBooks.Data;
using ForBooks.Models;
using System.Globalization;

namespace ForBooks.Services
{
    public class RecenzijaServis
    {
        private readonly DbClass _db;

        public RecenzijaServis(DbClass db)
        {
            _db = db;
        }

        public string DodajRecenziju(int korisnikId, int knjigaId, double ocjena, string komentar)
        {
            Korisnik korisnik = null;
            foreach (var k in _db.korisnici)
            {
                if (k.ID == korisnikId)
                {
                    korisnik = k;
                    break;
                }
            }
            if (korisnik == null)
                return "Korisnik nije pronađen.";
            Knjiga knjiga = null;
            foreach (var k in _db.knjige)
            {
                if (k.ID == knjigaId)
                {
                    knjiga = k;
                    break;
                }
            }
            if (knjiga == null)
                return "Knjiga nije pronađena.";

            if (ocjena < 1 || ocjena > 5)
                return "Ocjena mora biti broj između 1 i 5.";
            int noviId = 1;
            foreach (var r in _db.Recenzije)
            {
                if (r.ID >= noviId)
                    noviId = r.ID + 1;
            }
            var rec = new Recenzija
            {
                ID = noviId,
                KorisnikID = korisnikId,
                KnjigaID = knjigaId,
                Korisnik = korisnik,
                Knjiga = knjiga,
                Ocjena = ocjena,
                Komentar = komentar
            };
            _db.Recenzije.Add(rec);
            knjiga.Recenzije.Add(rec);
            _db.UpdateRecenzije();

            return "Recenzija je uspješno dodana.";
        }

        public string PrikaziSveRecenzije()
        {
            if (_db.Recenzije.Count == 0)
                return "Nema recenzija u bazi.";

            string rezultat = "";

            foreach (var r in _db.Recenzije)
            {
                string ime = r.Korisnik?.Ime ?? "?";
                string knjigaNaziv = r.Knjiga?.NazivKnjige ?? "?";

                rezultat +=
                    $"{r.ID}. {ime} - '{knjigaNaziv}' | Ocjena: {r.Ocjena}/5{Environment.NewLine}" +
                    $"   Komentar: {r.Komentar}{Environment.NewLine}";
            }

            return rezultat.TrimEnd();
        }

        public string PrikaziProsjecnuOcjenuKnjige(int knjigaId)
        {
            Knjiga knjiga = null;
            foreach (var k in _db.knjige)
            {
                if (k.ID == knjigaId)
                {
                    knjiga = k;
                    break;
                }
            }
            if (knjiga == null)
                return "Knjiga nije pronađena.";

            List<Recenzija> recenzijeKnjige = new List<Recenzija>();

            foreach (var r in _db.Recenzije)
            {
                if (r.KnjigaID == knjigaId)
                    recenzijeKnjige.Add(r);
            }

            if (recenzijeKnjige.Count == 0)
                return $"Knjiga '{knjiga.NazivKnjige}' nema recenzija.";

            double suma = 0;
            foreach (var r in recenzijeKnjige)
                suma += r.Ocjena;
                
        double prosjek = suma / recenzijeKnjige.Count;

            return string.Format(
                CultureInfo.InvariantCulture,
                "Prosječna ocjena za '{0}' je {1:F2}/5 (broj recenzija: {2}).",
                knjiga.NazivKnjige, 
                prosjek, 
                recenzijeKnjige.Count
            );
        }

        public string ObrisiRecenziju(int recenzijaId)
        {
            Recenzija target = null;
            foreach (var r in _db.Recenzije)
            {
                if (r.ID == recenzijaId)
                {
                    target = r;
                    break;
                }
            }

            if (target == null)
                return "Recenzija sa tim ID-jem ne postoji.";
            if (target.Knjiga != null)
            {
                for (int i = 0; i < target.Knjiga.Recenzije.Count; i++)
                {
                    if (target.Knjiga.Recenzije[i].ID == recenzijaId)
                    {
                        target.Knjiga.Recenzije.RemoveAt(i);
                        break;
                    }
                }
            }
            for (int i = 0; i < _db.Recenzije.Count; i++)
            {
                if (_db.Recenzije[i].ID == recenzijaId)
                {
                    _db.Recenzije.RemoveAt(i);
                    break;
                }
            }
            _db.UpdateRecenzije();

            return "Recenzija je obrisana.";
        }
    }
}
