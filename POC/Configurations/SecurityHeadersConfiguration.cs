using System;
namespace POC.API.Configurations
{
    public static class SecurityHeadersConfiguration
    {
        public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            app.Use(async (context, next) =>
            {
                /*
                 * Restringe o navegador a acessar o site apenas por HTTPS. Isso garante que a conexão não será estabelecida por meio de 
                 * uma conexão HTTP insegura.
                 * 
                 * max-age=<expire-time>: O tempo, em segundos, que o navegador deverá guardar a informação de que o site só pode ser acessado usando HTTPS.
                 * includeSubDomains (Optional): Se especificado, esta regra também será aplicada a todos os subdomínios.
                 */
                context.Response.Headers.Add("Strict-Transport-Security", "max-age=63072000; includeSubDomains");

                /*
                 * O header x-frame-options previne o ataque conhecido como clickjacking, desativando iframes no site. Os iframes podem 
                 * ser usados para carregar sites maliciosos. Esta técnica consiste em enganar o usuário sobre o site do qual ele realmente 
                 * está, através do iframe.
                 * 
                 * deny: Desabilita iframe completamente
                 * sameorigin: Permite apenas iframes do mesmo dominio
                 * allow-from: Permite iframes de um dominio especifico
                 */
                context.Response.Headers.Add("X-Frame-Options", "deny");

                /*
                 * O header X-Content-Type-Options previne o ataque conhecido como MIME Sniff. Ele instrui ao browser validar o MIME type 
                 * indicado no header.
                 * 
                 * Bloquea a solicitação se o tipo solicitado for:
                 * - "style" e o tipo MIME não é "text/css"
                 * - "script" e o tipo MIME não é um tipo MIME JavaScript
                 */
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");

                /*
                 * Habilita o filtro, mas ao invés de remover o código malicioso o browser irá parar de carregar o conteúdo.
                 * 
                 * O ataques de XSS exploram a incapacidade do navegador distinguir os scripts que são do site http://site-confiavel.com.br 
                 * de outros que foram injetados de forma maliciosa pelo atacante. Dessa forma o navegador baixa e executa todo o código, 
                 * sem considerar a fonte. Colocando o header X-XSS-Protection, na resposta do servidor, indica para o browser que ao 
                 * identificar código malicioso deve descarta-lo.
                 * 
                 * 0: Desabilita o filtro de XSS
                 * 1: Habilita o filtro de XSS. Se o browser detectar um código malicioso, irá remover e continuar o carregamento
                 * 1; mode=block: Habilita o filtro, mas ao invés de remover o código malicioso o browser irá parar de carregar o conteúdo.
                 */
                context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");

                //context.Response.Headers.Add("X-Developed-By", "GClaims");

                // Remoção de chaves sensívies para a segurança
                context.Response.Headers.Remove("X-Powered-By");
                context.Response.Headers.Add("Server", "*******");

                await next.Invoke();
            });

            return app;
        }
    }
}

