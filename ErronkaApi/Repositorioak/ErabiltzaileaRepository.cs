using ErronkaApi.Modeloak;
using NHibernate;
using NHibernate.Linq;

namespace ErronkaApi.Repositorioak
{
    public class ErabiltzaileaRepository
    {
        private readonly ISessionFactory _sessionFactory;

        public ErabiltzaileaRepository(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public ErabiltzaileaRepository() { }

        public virtual (bool success, string? error, Erabiltzailea? user) Login(string erabiltzailea, string pasahitza)
        {
            try
            {
                using var session = _sessionFactory.OpenSession();

                var user = session.Query<Erabiltzailea>()
                    .FirstOrDefault(e =>
                        e.erabiltzailea == erabiltzailea &&
                        e.pasahitza == pasahitza &&
                        !e.ezabatua);

                if (user == null)
                    return (false, "Erabiltzailea edo pasahitza okerra", null);

                return (true, null, user);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, null);
            }
        }

        public virtual (bool success, string? error, List<Erabiltzailea>? users) LortuAktiboak()
        {
            try
            {
                using var session = _sessionFactory.OpenSession();

                var users = session.Query<Erabiltzailea>()
                    .Where(e => !e.ezabatua)
                    .Where(e => e.rola.id == 2)
                    .OrderBy(e => e.erabiltzailea)
                    .ToList();

                return (true, null, users);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, null);
            }
        }
        public virtual (bool success, string? error, Erabiltzailea? user) SortuZerbitzaria(string erabiltzailea, string emaila, string pasahitza, bool txat)
        {
            try
            {
                erabiltzailea = (erabiltzailea ?? string.Empty).Trim();
                emaila = (emaila ?? string.Empty).Trim();
                pasahitza = (pasahitza ?? string.Empty).Trim();

                if (string.IsNullOrWhiteSpace(erabiltzailea))
                    return (false, "Erabiltzaile izena beharrezkoa da", null);

                if (string.IsNullOrWhiteSpace(pasahitza))
                    return (false, "Pasahitza beharrezkoa da", null);

                using var session = _sessionFactory.OpenSession();
                using var tx = session.BeginTransaction();

                var exists = session.Query<Erabiltzailea>()
                    .Any(e => e.erabiltzailea == erabiltzailea && !e.ezabatua);

                if (exists)
                    return (false, "Erabiltzaile izen hori dagoeneko existitzen da", null);

                var rola = session.Query<Rola>().FirstOrDefault(r => r.id == 2);
                if (rola == null)
                    return (false, "Zerbitzari rola ez da aurkitu", null);

                var user = new Erabiltzailea
                {
                    erabiltzailea = erabiltzailea,
                    emaila = emaila,
                    pasahitza = pasahitza,
                    rola = rola,
                    ezabatua = false,
                    txat = txat
                };

                session.Save(user);
                tx.Commit();

                return (true, null, user);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, null);
            }
        }

        public virtual (bool success, bool exists, string? error, bool txat) LortuTxatBaimena(int erabiltzaileId)
        {
            try
            {
                using var session = _sessionFactory.OpenSession();

                var user = session.Query<Erabiltzailea>()
                    .FirstOrDefault(e =>
                        e.id == erabiltzaileId &&
                        !e.ezabatua);

                if (user == null)
                    return (true, false, null, false);

                return (true, true, null, user.txat);
            }
            catch (Exception ex)
            {
                return (false, false, ex.Message, false);
            }
        }
    }
}
