using System.Threading.Tasks;
using ProAgil.Domain;

namespace ProAgil.Repository
{
    public interface IProAgilRepository
    {
         void Add<T>(T entity) where T : class;
         void Update<T>(T entity) where T : class;
         void Delete<T>(T entity) where T : class;
         Task<bool> SaveChangesAsync();
        //Eventos
         Task<Evento[]> GetAllEventoAsyncByTema(string Tema, bool includePalestrantes = false);
         Task<Evento[]> GetAllEventoAsync(bool includePalestrantes);
         Task<Evento> GetEventoAsyncByID(int EventoID, bool includePalestrantes = false);
        //Palestrantes
         Task<Palestrante[]> GetAllPalestranteAsyncByName(string name, bool includeEventos = false);         
         Task<Palestrante> GetPalestranteAsync(int PalestranteID, bool includeEventos = false);
    }
}