using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;


namespace ProjetoRastreador.Web.Filtros
{
    public class FiltroExcessao : IExceptionFilter
    {

        public void OnException(ExceptionContext context)
        {
            context.Result = new ContentResult
            {
                Content = "Foi encontrado um Erro no Software :( - "+ context.Exception.Message,
            };
        }
    }
}
