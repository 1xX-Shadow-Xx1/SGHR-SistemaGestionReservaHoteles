using SGHR.Web.Models.Sesion;

namespace SGHR.Web.Data
{
    public class HttpSesion
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpSesion(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void SaveSesion(SesionLoginModel sesion)
        {
            var session = _httpContextAccessor.HttpContext.Session;

            session.SetInt32("UserId", sesion.IdUser);
            session.SetInt32("SesionId", sesion.Idsesion);
            session.SetString("UserName", sesion.UserName);
            session.SetString("UserRole", sesion.RolUser.ToString());
        }
    }

}
