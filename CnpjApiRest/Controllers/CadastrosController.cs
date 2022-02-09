using CnpjApiRest.Models;
using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;


namespace Sistemas.Controllers
{


    public class CadastrosController : Controller
    {
        
       
        public ActionResult Empresa()
        {
            return View(new CnpjApiRest.Models.ApiRestCnpj());
        }
        public async Task<ActionResult> BuscarCnpjApiAsync(string searchcnpj)
        {
            string dados = "";
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"https://www.receitaws.com.br/v1/cnpj/" + searchcnpj);
                var rest = await client.GetAsync("");
                dados = await rest.Content.ReadAsStringAsync();

                if (dados == "Too many requests, please try again later.")
                {
                    TempData["errocnpj"] = "É possível consultar 3 CNPJ por minuto aguarde!";
                    return View("Empresa", new CnpjApiRest.Models.ApiRestCnpj());
                }
            }

            var serializer = JsonConvert.DeserializeObject<ApiRestCnpj>(dados);

            foreach (var item in serializer.atividade_principal)
            {
               
                serializer.descnaeprincipal = item.text;
                serializer.cnaeprincipal = item.code;

            }

            if (serializer.status == "ERROR")
            {
                TempData["errocnpj"] = "Verifique o CNPJ digitado!";
            }

            return View("Empresa", serializer);

        }


    }
}
