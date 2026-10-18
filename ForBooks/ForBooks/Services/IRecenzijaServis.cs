using ForBooks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForBooks.Services
{
    public interface IRecenzijaService
    {
        void DodajRecenziju(Recenzija recenzija);
        List<Recenzija> GetRecenzijeZaKnjigu(int knjigaId);
        double IzracunajProsjekOcjena(int knjigaId);
        List<Knjiga> PreporuciKnjige(Korisnik korisnik);
    }
}
