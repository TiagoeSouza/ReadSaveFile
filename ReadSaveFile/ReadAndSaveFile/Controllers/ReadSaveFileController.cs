using ReadAndSaveFile.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReadAndSaveFile.Controllers
{
    public class ReadSaveFileController : Controller
    {
        public static string retorno = "";
        public ActionResult Index()
        {
            ViewBag.Retorno = retorno;
            return View();
        }

        public ActionResult UploadFile()
        {
            try
            {
                List<Fornecedor> fornecedores = new List<Fornecedor>();

                if (Request.Files.Count > 0)
                {
                    if (Request.Files[0].ContentLength > 0)
                    {
                        var file = Request.Files[0];
                        using (StreamReader sr = new StreamReader(file.InputStream))
                        {
                            String linha;
                            while ((linha = sr.ReadLine()) != null)
                            {
                                Fornecedor forn = new Fornecedor();
                                List<string> fornSplit = linha.Split('|').ToList();
                                if (fornSplit.Count() > 0)
                                {
                                    forn.NOME = fornSplit[1];
                                }

                                fornecedores.Add(forn);
                            }
                        }

                        if (Request["ACAO"] == "1")
                        {
                            RepositorioFornecedor.insert(fornecedores);
                        }

                        retorno = "Arquivo processado com sucesso.<br/>Total de Fornecedores Lidos: " + fornecedores.Count;
                    }
                    else
                    {
                        retorno = "O arquivo selecionado parece estar vazio";
                    }

                }
                else
                {
                    retorno = "Arquivo não localizado.";
                }
            }
            catch (Exception exp)
            {

                retorno = "Ocorreu um problema ao processar o arquivo selecionado.";
            }

            return RedirectToAction("Index");
        }


        public JsonResult Get()
        {
            List<Fornecedor> fornecedores = RepositorioFornecedor.Get();

            return Json(fornecedores,JsonRequestBehavior.AllowGet);
        }

    }
}